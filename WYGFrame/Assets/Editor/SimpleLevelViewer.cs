using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

public class SimpleLevelViewer : EditorWindow
{
    private int currentLevel = 0;
    private int cols = 35; // 宽度固定35
    private int rows = 45; // 高度默认45，可根据关卡数据调整
    private int[,] gridData; // 动态大小网格，存储步骤编号
    private Vector2 scrollPosition;
    private Vector2 stepsScrollPosition;
    
    // 步骤数据
    private List<StepData> steps = new List<StepData>();
    private int selectedStepIndex = 0; // 当前选中的步骤索引
    
    // 10个颜色：索引0-9
    private readonly Color[] gameColors = new Color[]
    {
        new Color(1f, 0.7f, 0.3f, 1f),  // 0: 橙色
        new Color(0.5f, 0f, 0.5f, 1f),  // 1: 紫色
        new Color(1f, 0.6f, 0.7f, 1f),  // 2: 粉色
        new Color(1f, 0f, 0f, 1f),      // 3: 红色
        new Color(1f, 1f, 0f, 1f),      // 4: 黄色
        new Color(0f, 1f, 1f, 1f),      // 5: 青色
        new Color(0f, 0f, 1f, 1f),      // 6: 蓝色
        new Color(0f, 1f, 0f, 1f),      // 7: 绿色
        new Color(0.5f, 0.8f, 1f, 1f),  // 8: 天蓝色
        new Color(0.6f, 0.3f, 0.1f, 1f) // 9: 棕色
    };
    
    [System.Serializable]
    public class StepData
    {
        public int stepNumber;
        public int colorIndex;
        public List<Vector2Int> positions;
        
        public StepData(int stepNumber, int colorIndex)
        {
            this.stepNumber = stepNumber;
            this.colorIndex = colorIndex;
            this.positions = new List<Vector2Int>();
        }
    }
    
    // 用于加载和保存的数据结构
    public class LevelDataForLoad
    {
        public int r;
        public int c;
        public StepDataJson[] s;
    }
    
    public class StepDataJson
    {
        public int s;
        public int c;
        [JsonConverter(typeof(PositionConverter))]
        public List<int[]> p;  // 改为List<int[]>以匹配游戏格式
    }
    
    // 自定义转换器：处理p字段的字符串和数组两种格式
    public class PositionConverter : JsonConverter
    {
        public override bool CanConvert(System.Type objectType)
        {
            return objectType == typeof(List<int[]>);
        }

        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new List<int[]>();
            
            if (reader.TokenType == JsonToken.String)
            {
                // 字符串格式："[[x,y],...]" -> 解析为List<int[]>
                string posStr = reader.Value as string;
                var coordPattern = @"\[(\d+),(\d+)\]";
                var matches = System.Text.RegularExpressions.Regex.Matches(posStr, coordPattern);
                
                foreach (System.Text.RegularExpressions.Match match in matches)
                {
                    int x = int.Parse(match.Groups[1].Value);
                    int y = int.Parse(match.Groups[2].Value);
                    result.Add(new int[] { x, y });
                }
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                // 已经是数组格式：[[x,y],...] -> 直接反序列化
                result = serializer.Deserialize<List<int[]>>(reader);
            }
            
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // 保存时写为数组格式（游戏可以直接读取）
            var positions = value as List<int[]>;
            serializer.Serialize(writer, positions);
        }
    }
    
    [MenuItem("Tools/Simple Level Viewer")]
    public static void ShowWindow()
    {
        var window = GetWindow<SimpleLevelViewer>("关卡编辑器");
        // 初始化网格
        if (window.gridData == null)
        {
            window.gridData = new int[window.cols, window.rows];
        }
        // 初始化默认步骤
        if (window.steps.Count == 0)
        {
            window.steps.Add(new StepData(1, 0));
            window.selectedStepIndex = 0;
            window.UpdateGridDisplay();
        }
    }
    
    void OnGUI()
    {
        // 顶部：加载和保存按钮
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("关卡编号:", GUILayout.Width(60));
        currentLevel = EditorGUILayout.IntField(currentLevel, GUILayout.Width(100));
        
        if (GUILayout.Button("加载关卡", GUILayout.Width(80)))
        {
            LoadLevel(currentLevel);
        }
        
        if (GUILayout.Button("保存关卡", GUILayout.Width(80)))
        {
            SaveLevel(currentLevel);
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        // 中部：步骤列表
        EditorGUILayout.LabelField("步骤列表:");
        stepsScrollPosition = EditorGUILayout.BeginScrollView(stepsScrollPosition, GUILayout.Height(150));
        
        for (int i = 0; i < steps.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            
            // 步骤按钮
            Color originalColor = GUI.backgroundColor;
            if (i == selectedStepIndex)
            {
                GUI.backgroundColor = Color.green;
            }
            
            if (GUILayout.Button($"步骤 {steps[i].stepNumber}", GUILayout.Width(80)))
            {
                selectedStepIndex = i;
            }
            
            GUI.backgroundColor = originalColor;
            
            // 颜色选择
            EditorGUILayout.LabelField($"颜色:", GUILayout.Width(40));
            int newColorIndex = EditorGUILayout.IntSlider(steps[i].colorIndex, 0, 9, GUILayout.Width(200));
            if (newColorIndex != steps[i].colorIndex)
            {
                steps[i].colorIndex = newColorIndex;
                UpdateGridDisplay();
            }
            
            // 颜色预览
            GUI.backgroundColor = gameColors[steps[i].colorIndex];
            GUILayout.Box("", GUILayout.Width(30), GUILayout.Height(20));
            GUI.backgroundColor = originalColor;
            
            EditorGUILayout.LabelField($"格子数: {steps[i].positions.Count}", GUILayout.Width(80));
            
            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.EndScrollView();
        
        // 添加/删除步骤按钮
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("添加步骤", GUILayout.Width(80)))
        {
            AddStep();
        }
        
        if (GUILayout.Button("删除步骤", GUILayout.Width(80)))
        {
            DeleteStep();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        // 底部：网格编辑区
        EditorGUILayout.LabelField($"网格编辑区 ({cols}x{rows}) - 左键添加 / 右键删除:");
        
        // 绘制网格
        float cellSize = 15f;
        float startX = 20f;
        float startY = 380f;
        
        // 绘制网格背景
        Rect gridBackground = new Rect(startX - 1, startY - 1, cols * cellSize + 1, rows * cellSize + 1);
        EditorGUI.DrawRect(gridBackground, Color.black);
        
        // 绘制网格
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                // 翻转Y轴：游戏y=0在底部，显示y=0在顶部
                int displayY = (rows - 1) - y;
                
                Rect cellRect = new Rect(
                    startX + x * cellSize,
                    startY + displayY * cellSize,
                    cellSize - 1,
                    cellSize - 1
                );
                
                // 获取颜色
                int stepIndex = gridData[x, y];
                Color cellColor;
                
                if (stepIndex >= 0 && stepIndex < steps.Count)
                {
                    cellColor = gameColors[steps[stepIndex].colorIndex];
                }
                else
                {
                    cellColor = new Color(0.95f, 0.95f, 0.95f, 1f); // 空白格子浅灰色
                }
                
                // 绘制格子
                var outline = new Color(0f, 0f, 0f, 0.2f);
                Handles.DrawSolidRectangleWithOutline(cellRect, cellColor, outline);
                
                // 处理鼠标点击
                Event e = Event.current;
                if (cellRect.Contains(e.mousePosition))
                {
                    if (e.type == EventType.MouseDown)
                    {
                        if (e.button == 0) // 左键
                        {
                            AddPositionToCurrentStep(x, y);
                            e.Use();
                        }
                        else if (e.button == 1) // 右键
                        {
                            RemovePositionFromCurrentStep(x, y);
                            e.Use();
                        }
                    }
                }
            }
        }
    }
    
    void LoadLevel(int level)
    {
        string filePath = $"Assets/Game/Levels/json/lv_{level}.json";
        
        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"关卡文件不存在，创建默认配置: {filePath}");
            CreateDefaultLevel(level);
            return;
        }
        
        try
        {
            // 清空数据
            steps.Clear();
            
            // 读取JSON
            string jsonContent = File.ReadAllText(filePath);
            
            // 使用Newtonsoft.Json解析
            var levelData = JsonConvert.DeserializeObject<LevelDataForLoad>(jsonContent);
            
            if (levelData == null || levelData.s == null)
            {
                Debug.LogError($"关卡数据为空");
                steps.Add(new StepData(1, 0));
                selectedStepIndex = 0;
                UpdateGridDisplay();
                return;
            }
            
            Debug.Log($"加载关卡{level}，找到{levelData.s.Length}个步骤");
            
            // 解析每个步骤并找出最大Y坐标
            int maxY = 0;
            foreach (var stepJson in levelData.s)
            {
                var step = new StepData(stepJson.s, stepJson.c);
                
                // p现在是List<int[]>格式
                if (stepJson.p != null)
                {
                    foreach (var pos in stepJson.p)
                    {
                        if (pos != null && pos.Length >= 2)
                        {
                            step.positions.Add(new Vector2Int(pos[0], pos[1]));
                            maxY = Mathf.Max(maxY, pos[1]);
                        }
                    }
                }
                
                steps.Add(step);
            }
            
            // 使用关卡配置的行数，如果没有则根据数据计算
            if (levelData.r > 0)
            {
                rows = levelData.r;
            }
            else
            {
                // 根据最大Y值+1（因为是0-based索引）
                rows = Mathf.Max(maxY + 1, 35);
            }
            gridData = new int[cols, rows];
            Debug.Log($"关卡行数: {rows}, 最大Y坐标: {maxY}");
            
            // 确保至少有一个步骤
            if (steps.Count == 0)
            {
                steps.Add(new StepData(1, 0));
            }
            
            selectedStepIndex = 0;
            UpdateGridDisplay();
            Debug.Log($"关卡{level}加载完成，共{steps.Count}个步骤");
            Repaint();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"加载关卡失败: {e.Message}\n{e.StackTrace}");
            steps.Clear();
            steps.Add(new StepData(1, 0));
            selectedStepIndex = 0;
            UpdateGridDisplay();
        }
    }
    
    void CreateDefaultLevel(int level)
    {
        // 创建默认关卡（一个空步骤）
        steps.Clear();
        steps.Add(new StepData(1, 0));
        selectedStepIndex = 0;
        
        // 重置为默认大小
        rows = 45;
        gridData = new int[cols, rows];
        UpdateGridDisplay();
        
        // 保存到文件
        SaveLevel(level);
    }
    
    void SaveLevel(int level)
    {
        string filePath = $"Assets/Game/Levels/json/lv_{level}.json";
        
        // 确保目录存在
        string directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        // 构建JSON数据
        var levelData = new LevelDataForLoad
        {
            r = rows,  // 使用实际行数
            c = cols,  // 宽度固定35
            s = steps.Select(step => new StepDataJson
            {
                s = step.stepNumber,
                c = step.colorIndex,
                p = step.positions.Select(pos => new int[] { pos.x, pos.y }).ToList()
            }).ToArray()
        };
        
        // 使用Newtonsoft.Json序列化，带缩进
        string jsonContent = JsonConvert.SerializeObject(levelData, Formatting.Indented);
        File.WriteAllText(filePath, jsonContent);
        
        Debug.Log($"关卡{level}保存成功: {filePath}，大小: {cols}x{rows}");
        UnityEditor.AssetDatabase.Refresh();
    }
    
    void AddStep()
    {
        int nextStepNumber = steps.Count > 0 ? steps[steps.Count - 1].stepNumber + 1 : 1;
        steps.Add(new StepData(nextStepNumber, 0));
        selectedStepIndex = steps.Count - 1;
        Debug.Log($"添加步骤{nextStepNumber}");
    }
    
    void DeleteStep()
    {
        if (steps.Count <= 1)
        {
            Debug.LogWarning("至少需要保留一个步骤");
            return;
        }
        
        // 删除最后一个步骤
        int lastIndex = steps.Count - 1;
        Debug.Log($"删除步骤{steps[lastIndex].stepNumber}");
        steps.RemoveAt(lastIndex);
        
        // 调整选中索引
        if (selectedStepIndex >= steps.Count)
        {
            selectedStepIndex = steps.Count - 1;
        }
        
        UpdateGridDisplay();
    }
    
    void AddPositionToCurrentStep(int x, int y)
    {
        if (steps.Count == 0) return;
        
        var pos = new Vector2Int(x, y);
        var currentStep = steps[selectedStepIndex];
        
        // 从其他步骤中移除这个位置
        foreach (var step in steps)
        {
            if (step != currentStep)
            {
                step.positions.Remove(pos);
            }
        }
        
        // 添加到当前步骤
        if (!currentStep.positions.Contains(pos))
        {
            currentStep.positions.Add(pos);
        }
        
        UpdateGridDisplay();
        Repaint();
    }
    
    void RemovePositionFromCurrentStep(int x, int y)
    {
        if (steps.Count == 0) return;
        
        var pos = new Vector2Int(x, y);
        var currentStep = steps[selectedStepIndex];
        
        currentStep.positions.Remove(pos);
        
        UpdateGridDisplay();
        Repaint();
    }
    
    void UpdateGridDisplay()
    {
        // 确保gridData已初始化
        if (gridData == null || gridData.GetLength(0) != cols || gridData.GetLength(1) != rows)
        {
            gridData = new int[cols, rows];
        }
        
        // 清空网格
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                gridData[x, y] = -1;
            }
        }
        
        // 按步骤绘制
        for (int i = 0; i < steps.Count; i++)
        {
            foreach (var pos in steps[i].positions)
            {
                if (pos.x >= 0 && pos.x < cols && pos.y >= 0 && pos.y < rows)
                {
                    gridData[pos.x, pos.y] = i; // 存储步骤索引
                }
            }
        }
    }
}

