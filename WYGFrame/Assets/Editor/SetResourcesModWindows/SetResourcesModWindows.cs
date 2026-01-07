using System.IO;
using UnityEditor;
using UnityEngine;

namespace XrCode
{
    public class AppConfigEditorWindow : EditorWindow
    {
        private bool useAssetBundle;
        private bool checkVersionUpdate;
        private bool loadAssetWithServer;
        private bool loadAssetWithResources;
        private string serverResourceURL = "http://49.233.134.149/ab/"; // 默认服务器地址

        [MenuItem("Tools/设置资源加载方式")]
        static void OpenWindow()
        {
            GetWindow<AppConfigEditorWindow>("设置资源加载方式");
        }

        private void OnEnable()
        {
            // 初始化窗口时读取静态类字段的值
            useAssetBundle = AppConfig.UseAssetBundle;
            checkVersionUpdate = AppConfig.CheckVersionUpdate;
            loadAssetWithServer = AppConfig.LoadAssetWithServer;
            loadAssetWithResources = AppConfig.LoadAssetWithResources;
            serverResourceURL = AppConfig.ServerResourceURL; // 读取服务器地址
        }

        private void OnGUI()
        {
            GUILayout.Label("配置选项", EditorStyles.boldLabel);

            // 设置字段的 UI
            useAssetBundle = EditorGUILayout.Toggle("使用 AssetBundle:", useAssetBundle);
            checkVersionUpdate = EditorGUILayout.Toggle("检测版本更新:", checkVersionUpdate);
            loadAssetWithServer = EditorGUILayout.Toggle("从服务器加载资源:", loadAssetWithServer);
            // 如果用户尝试勾选“从服务器加载资源”，但没有勾选“使用 AssetBundle”，则撤销勾选
            //if (loadAssetWithServer && !useAssetBundle)
            //{
            //    loadAssetWithServer = false;
            //    EditorUtility.DisplayDialog("错误", "必须勾选使用 AssetBundle才能从服务器加载资源。", "确定");
            //}
            GUILayout.Label("使用 Resources 加载资源，则使用 AssetBundle 和从服务器加载资源，不管如何设置都为 false", GUILayout.Width(700));
            loadAssetWithResources = EditorGUILayout.Toggle("从 Resources 加载资源", loadAssetWithResources);
            //if (loadAssetWithResources)
            //{
            //    useAssetBundle = false;
            //    loadAssetWithServer = false;
            //}
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("服务器资源地址:", GUILayout.Width(150)); // 标签宽度
            serverResourceURL = EditorGUILayout.TextField(serverResourceURL, GUILayout.Width(400)); // 输入框宽度
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);
            GUILayout.Label("提示：");
            GUILayout.Label("勾选从服务器加载资源，必须勾选使用 AssetBundle");
            GUILayout.Label("勾选使用 AssetBundle，如果未勾选从服务器加载资源，资源需bulid后放到StreamingAssets目录");
            GUILayout.Label("未勾选使用 AssetBundle，加载资源方式为AssetDatabase，资源放在Asset文件即可");
            GUILayout.Label("配置表文件（LuBan导出Data文件夹），Resources加载需要放置在Resources文件夹，其余放置在StreamingAssets目录。");
            if (GUILayout.Button("保存设置"))
            {
                SaveSettings();
            }
        }

        private void SaveSettings()
        {
            // 设置静态类字段的值
            AppConfig.UseAssetBundle = useAssetBundle;
            AppConfig.CheckVersionUpdate = checkVersionUpdate;
            AppConfig.LoadAssetWithServer = loadAssetWithServer;
            AppConfig.LoadAssetWithResources = loadAssetWithResources;
            AppConfig.ServerResourceURL = serverResourceURL; // 保存服务器地址
                                                             // 保存到文件
            SaveToFile();

            // 刷新编辑器
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("设置已保存", "配置信息已成功保存！", "确定");
        }

        private void SaveToFile()
        {
            string path = "Assets/Scripts/Config/AppConfig.cs";
            string content = @"

using System;

public static class AppConfig
{
    public static bool UseAssetBundle = " + useAssetBundle.ToString().ToLower() + @"; // 是否使用 AssetBundle
    public static bool CheckVersionUpdate = " + checkVersionUpdate.ToString().ToLower() + @"; // 是否检查版本更新
    public static bool LoadAssetWithServer = " + loadAssetWithServer.ToString().ToLower() + @"; // 是否从服务器加载资源
    public static bool LoadAssetWithResources = " + loadAssetWithResources.ToString().ToLower() + @"; // 是否从 Resources 加载资源
    public static  string ServerResourceURL = """ + serverResourceURL + @"""; // 服务器资源地址
}";

            File.WriteAllText(path, content);
        }
    }

}