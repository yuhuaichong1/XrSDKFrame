using UnityEngine;
using UnityEditor;

public class EditorSetting : ScriptableObject
{
    public bool enableOpElementHierarchy = true;

    public string uiPrefabRootDir = "AssetBundleLocal/Prefabs/UI";  

    // UI类生成根目录（相对于 Application.dataPath）
    // 将在相对路径下创建对应基类。
    // 将在相对路径下创建快捷类。创建后应自行改名（避免覆盖）。
    public string generatedBaseUIRootDir = "Scripts/Game/GeneratedBaseUI";
    public string generatedTempUIRootDir = "Scripts/Game/Modules";

    private const string kAssetPath = "Assets/Tools/XrCode/Editor/EditorSetting.asset";

    private static EditorSetting sm_Instance = null;
    public static EditorSetting Instance
    {
        get
        {
            if (sm_Instance == null)
            {
                sm_Instance = AssetDatabase.LoadAssetAtPath<EditorSetting>(kAssetPath);
#if UNITY_EDITOR
                if (sm_Instance == null)
                {
                    sm_Instance = CreateInstance<EditorSetting>();
                    AssetDatabase.CreateAsset(sm_Instance, kAssetPath);
                }
#else
                    Debug.Assert(sm_Instance != null);
#endif
            }
            return sm_Instance;
        }
    }

#if UNITY_EDITOR
    [MenuItem("XrCode/EditorSetting", false, 999)]
    public static void Select()
    {
        Debug.Log("Application.dataPath: " + Application.dataPath);
        Selection.activeObject = Instance;
    }
#endif
}
