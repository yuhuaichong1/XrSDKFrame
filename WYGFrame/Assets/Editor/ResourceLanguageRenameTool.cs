using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class CardViewChineseFinder : EditorWindow
{
    private Vector2 scrollPosition;
    private List<ResourceInfo> foundResources = new List<ResourceInfo>();
    private string searchPath = "Assets";
    private int itemsPerRow = 3;
    private float cardWidth = 180;
    private float cardHeight = 80;

    [MenuItem("Tools/中文资源查找器(卡片版)")]
    public static void ShowWindow()
    {
        var window = GetWindow<CardViewChineseFinder>("中文资源查找器");
        window.minSize = new Vector2(600, 400);
    }

    private void OnGUI()
    {
        DrawToolbar();

        if (foundResources.Count == 0)
        {
            DrawEmptyState();
            return;
        }

        DrawResourceGrid();
    }

    private void DrawToolbar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        // 搜索路径
        GUILayout.Label("路径:", GUILayout.Width(35));
        searchPath = EditorGUILayout.TextField(searchPath, GUILayout.Width(200));

        if (GUILayout.Button("扫描", GUILayout.Width(60)))
        {
            ScanResources();
        }

        GUILayout.FlexibleSpace();

        // 统计信息
        GUILayout.Label($"找到 {foundResources.Count} 个资源");

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);
    }

    private void DrawEmptyState()
    {
        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginVertical(GUILayout.Width(300));

        GUILayout.Label("🎯 中文资源查找器", EditorStyles.boldLabel, GUILayout.Height(30));
        GUILayout.Space(10);

        EditorGUILayout.HelpBox("点击\"扫描\"按钮来查找项目中所有包含中文名称的资源和文件夹", MessageType.Info);

        GUILayout.Space(20);

        if (GUILayout.Button("开始扫描", GUILayout.Height(40)))
        {
            ScanResources();
        }

        EditorGUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
    }

    private void DrawResourceGrid()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        int currentIndex = 0;

        while (currentIndex < foundResources.Count)
        {
            EditorGUILayout.BeginHorizontal();

            for (int i = 0; i < itemsPerRow && currentIndex < foundResources.Count; i++)
            {
                DrawResourceCard(foundResources[currentIndex]);
                currentIndex++;
            }

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawResourceCard(ResourceInfo resource)
    {
        EditorGUILayout.BeginVertical("Box", GUILayout.Width(cardWidth), GUILayout.Height(cardHeight));

        // 图标和名称
        EditorGUILayout.BeginHorizontal();
        if (resource.icon != null)
        {
            GUILayout.Label(resource.icon, GUILayout.Width(32), GUILayout.Height(32));
        }

        EditorGUILayout.BeginVertical();
        GUILayout.Label(resource.name, EditorStyles.boldLabel, GUILayout.Height(16));

        // 路径（缩短显示）
        string shortPath = resource.path;
        if (shortPath.Length > 25)
        {
            shortPath = "..." + shortPath.Substring(shortPath.Length - 25);
        }
        GUILayout.Label(shortPath, EditorStyles.miniLabel, GUILayout.Height(12));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        // 操作按钮
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("选中", EditorStyles.miniButton))
        {
            SelectResource(resource);
        }

        if (GUILayout.Button("定位", EditorStyles.miniButton))
        {
            PingResource(resource);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    private void SelectResource(ResourceInfo resource)
    {
        UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(resource.path);
        if (obj != null)
        {
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
        }
    }

    private void PingResource(ResourceInfo resource)
    {
        UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(resource.path);
        if (obj != null)
        {
            EditorGUIUtility.PingObject(obj);
        }
    }

    private void ScanResources()
    {
        foundResources.Clear();

        if (!Directory.Exists(searchPath))
        {
            EditorUtility.DisplayDialog("错误", "指定的路径不存在！", "确定");
            return;
        }

        // 扫描文件
        ScanChineseFiles();

        // 扫描文件夹
        ScanChineseFolders();

        // 按类型和名称排序
        foundResources = foundResources
            .OrderBy(r => r.isFolder ? 0 : 1)
            .ThenBy(r => r.name)
            .ToList();
    }

    private void ScanChineseFiles()
    {
        string[] allFiles = Directory.GetFiles(searchPath, "*.*", SearchOption.AllDirectories)
            .Where(file => !file.EndsWith(".meta"))
            .ToArray();

        foreach (string filePath in allFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string relativePath = filePath.Replace("\\", "/");

            if (ContainsChinese(fileName))
            {
                Texture2D icon = AssetDatabase.GetCachedIcon(relativePath) as Texture2D;
                foundResources.Add(new ResourceInfo
                {
                    name = fileName,
                    path = relativePath,
                    icon = icon,
                    isFolder = false
                });
            }
        }
    }

    private void ScanChineseFolders()
    {
        string[] allFolders = Directory.GetDirectories(searchPath, "*", SearchOption.AllDirectories);
        foreach (string folderPath in allFolders)
        {
            string folderName = Path.GetFileName(folderPath);
            string relativePath = folderPath.Replace("\\", "/");

            if (ContainsChinese(folderName))
            {
                foundResources.Add(new ResourceInfo
                {
                    name = folderName,
                    path = relativePath,
                    icon = EditorGUIUtility.FindTexture("Folder Icon"),
                    isFolder = true
                });
            }
        }
    }

    private bool ContainsChinese(string text)
    {
        if (string.IsNullOrEmpty(text))
            return false;

        return Regex.IsMatch(text, @"[\u4e00-\u9fa5]");
    }

    [System.Serializable]
    private class ResourceInfo
    {
        public string name;
        public string path;
        public Texture icon;
        public bool isFolder;
    }
}