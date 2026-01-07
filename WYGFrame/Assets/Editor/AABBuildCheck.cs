using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using XrCode;

public class AABBuildCheck : Editor, IPreprocessBuildWithReport
{
    string GMName = "GM";
    string DebugConsoleName = "IngameDebugConsole";
    string GameManagerName = "Game";
    string DefinePath = "Scripts/_Game/GameDefines.cs";
    string URLCode = "URL = ";
    string DebugCode = "ifDebug = ";
    string SkipAdCode = "ifSkipAD = ";

    string curURL;

    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        if(report.summary.platform == BuildTarget.Android)
        {
            bool ifAAB = EditorUserBuildSettings.buildAppBundle;
            if (ifAAB)
            {
                SetGMFalse();
                SetDCFalse();
                SetGameFalse();
                SetDebugFalse();
                ShowOtherMsg();
            }
        }
    }

    #region GM物体的隐藏

    /// <summary>
    /// 取消GM物体的显示
    /// </summary>
    private void SetGMFalse()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (!currentScene.IsValid() || !currentScene.isLoaded)
        {
            Debug.LogWarning("[AAB打包检测] 当前无有效激活场景，跳过GM物体检测");
            return;
        }

        GameObject[] rootObjects = currentScene.GetRootGameObjects();
        GameObject gmObj = null;
        foreach (GameObject rootObj in rootObjects)
        {
            // 先判断根物体是否是GM
            if (rootObj.name == GMName)
            {
                gmObj = rootObj;
                break;
            }

            // 递归查找子物体中的GM
            gmObj = FindObjInChildren(rootObj, GMName);
            if (gmObj != null)
            {
                break;
            }
        }

        // 处理找到的GM物体
        if (gmObj != null)
        {
            ProcessGmObject(gmObj, currentScene.name);
        }
        else
        {
            Debug.Log($"[AAB打包检测] 当前场景「{currentScene.name}」中未找到GM物体，无需处理");
        }
    }

    /// <summary>
    /// 递归查找子物体中的目标物体
    /// </summary>
    /// <param name="parentObj">父物体</param>
    /// <returns>找到的GM物体，未找到返回null</returns>
    private GameObject FindObjInChildren(GameObject parentObj, string checkName)
    {
        foreach (Transform child in parentObj.transform)
        {
            if (child.gameObject.name == checkName)
            {
                return child.gameObject;
            }

            // 递归查找孙子物体
            GameObject gmInGrandChild = FindObjInChildren(child.gameObject, checkName);
            if (gmInGrandChild != null)
            {
                return gmInGrandChild;
            }
        }
        return null;
    }

    /// <summary>
    /// 处理GM物体（检测激活状态并禁用）
    /// </summary>
    /// <param name="gmObj">GM物体</param>
    /// <param name="sceneName">所属场景名</param>
    private void ProcessGmObject(GameObject gmObj, string sceneName)
    {
        if (gmObj.activeSelf)
        {
            gmObj.SetActive(false);
            EditorSceneManager.MarkSceneDirty(gmObj.scene);
            Debug.LogError($"[AAB打包检测] 当前场景「{sceneName}」中的GM物体「{GMName}」已被自动禁用");
        }
        else
        {
            Debug.LogError($"[AAB打包检测] 当前场景「{sceneName}」中的GM物体「{GMName}」已为禁用状态");
        }
    }

    #endregion

    #region IngameDebugConsole物体的隐藏

    /// <summary>
    /// 取消IngameDebugConsole物体的显示
    /// </summary>
    private void SetDCFalse()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (!currentScene.IsValid() || !currentScene.isLoaded)
        {
            Debug.LogWarning("[AAB打包检测] 当前无有效激活场景，跳过DebugConsoleName物体检测");
            return;
        }

        GameObject[] rootObjects = currentScene.GetRootGameObjects();
        GameObject dcObj = null;
        foreach (GameObject rootObj in rootObjects)
        {
            // 先判断根物体是否是GM
            if (rootObj.name == DebugConsoleName)
            {
                dcObj = rootObj;
                break;
            }

            // 递归查找子物体中的GM
            dcObj = FindObjInChildren(rootObj, DebugConsoleName);
            if (dcObj != null)
            {
                break;
            }
        }

        // 处理找到的GM物体
        if (dcObj != null)
        {
            ProcessDcObject(dcObj, currentScene.name);
        }
        else
        {
            Debug.Log($"[AAB打包检测] 当前场景「{currentScene.name}」中未找到DebugConsoleName物体，无需处理");
        }
    }

    /// <summary>
    /// 处理IngameDebugConsole物体（检测激活状态并禁用）
    /// </summary>
    /// <param name="dcObj">IngameDebugConsole物体</param>
    /// <param name="sceneName">所属场景名</param>
    private void ProcessDcObject(GameObject dcObj, string sceneName)
    {
        if (dcObj.activeSelf)
        {
            dcObj.SetActive(false);
            EditorSceneManager.MarkSceneDirty(dcObj.scene);
            Debug.LogError($"[AAB打包检测] 当前场景「{sceneName}」中的IngameDebugConsole物体「{DebugConsoleName}」已被自动禁用");
        }
        else
        {
            Debug.LogError($"[AAB打包检测] 当前场景「{sceneName}」中的IngameDebugConsole物体「{DebugConsoleName}」已为禁用状态");
        }
    }

    #endregion

    #region Game物体的设置

    /// <summary>
    /// 设置Game物体中的值
    /// </summary>
    public void SetGameFalse()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (!currentScene.IsValid() || !currentScene.isLoaded)
        {
            Debug.LogWarning("[AAB打包检测] 当前无有效激活场景，跳过Game物体检测");
            return;
        }

        GameObject[] rootObjects = currentScene.GetRootGameObjects();
        GameObject gameObj = null;
        foreach (GameObject rootObj in rootObjects)
        {
            // 先判断根物体是否是GM
            if (rootObj.name == GameManagerName)
            {
                gameObj = rootObj;
                break;
            }

            // 递归查找子物体中的GM
            gameObj = FindObjInChildren(rootObj, GameManagerName);
            if (gameObj != null)
            {
                break;
            }
        }

        // 处理找到的GM物体
        if (gameObj != null)
        {
            ProcessGameObject(gameObj, currentScene.name);
        }
        else
        {
            Debug.Log($"[AAB打包检测] 当前场景「{currentScene.name}」中未找到Game物体！");
        }
    }

    /// <summary>
    /// 处理Game脚本（开启后台网络检测）
    /// </summary>
    /// <param name="gameObj">Game物体</param>
    /// <param name="sceneName">所属场景名</param>
    private void ProcessGameObject(GameObject gameObj, string sceneName)
    {
        Game game = gameObj.GetComponent<Game>();
        if (game != null)
        {
            if(!game.ifCheckNetwork)
            {
                game.ifCheckNetwork = true;
                EditorSceneManager.MarkSceneDirty(gameObj.scene);
                Debug.LogError($"[AAB打包检测] 当前场景「{sceneName}」中的Game物体「{GameManagerName}」开启网络检测");
            }
            else
            {
                Debug.LogError($"[AAB打包检测] 当前场景「{sceneName}」中的Game物体「{GameManagerName}」已经开启网络检测");
            }

        }
        else
        {
            Debug.LogError($"[AAB打包检测] 当前场景「{sceneName}」中的Game物体「{GameManagerName}」未找到对应的Game脚本！");
        }
    }

    #endregion

    #region Debug、SkipAd变量的关闭

    /// <summary>
    /// 获取完整脚本路径并执行
    /// </summary>
    private void SetDebugFalse()
    {
        string scriptPath = Path.Combine(Application.dataPath, DefinePath);
        if (!File.Exists(scriptPath))
        {
            Debug.LogError("未找到脚本文件：" + scriptPath);
            return;
        }

        ModifyScriptCode(scriptPath);
    }

    /// <summary>
    /// 修改对应代码
    /// </summary>
    /// <param name="scriptPath">完整代码路径</param>
    private void ModifyScriptCode(string scriptPath)
    {
        string[] allLines = File.ReadAllLines(scriptPath, Encoding.UTF8);
        bool isDebugModified = false;
        bool isSkipAdModified = false;
        bool isUrlGet = false;

        for (int i = 0; i < allLines.Length; i++)
        {
            string line = allLines[i];
            string lineWithoutComment = line.Split(new[] { "//" }, StringSplitOptions.None)[0];
            if(lineWithoutComment.Trim().Contains($"{URLCode}"))
            {
                Match match = Regex.Match(lineWithoutComment.Trim(), @"URL\s*=\s*[""']([^""']+)[""'];?"); ;
                if (match.Success)
                {
                    curURL = match.Groups[1].Value;
                    isUrlGet = true;
                }
                continue;
            }
            if (lineWithoutComment.Trim().Contains($"{DebugCode}true"))
            {
                allLines[i] = line.Replace($"{DebugCode}true", $"{DebugCode}false");
                isDebugModified = true;
                continue;
            }
            if(lineWithoutComment.Trim().Contains($"{SkipAdCode}true"))
            {
                allLines[i] = line.Replace($"{SkipAdCode}true", $"{SkipAdCode}false");
                isSkipAdModified = true;
                continue;
            }

            if(isDebugModified && isSkipAdModified && isUrlGet)
            {
                break;
            }
        }

        if (isDebugModified)
        {
            Debug.LogError($"已将「{DebugCode} = true;」修改为「{DebugCode} = false;」");
        }
        else
        {
            Debug.LogError($"{DefinePath}中未找到Debug开关变量：「{DebugCode}true;」 或其已经修改为 false");
        }

        if (isSkipAdModified)
        {
            Debug.LogError($"已将「{SkipAdCode} = true;」修改为「{SkipAdCode} = false;」");
        }
        else
        {
            Debug.LogError($"{SkipAdCode}中未找到SkipAd开关变量：「{SkipAdCode}true;」 或其已经修改为 false");
        }

        if(isDebugModified || isSkipAdModified)
        {
            File.WriteAllLines(scriptPath, allLines, Encoding.UTF8);
            AssetDatabase.Refresh();
        }
    }

    #endregion

    #region 显示其他信息

    public void ShowOtherMsg()
    {
        Debug.LogError($"URL：{curURL}");
        Debug.LogError($"CompanyName：{PlayerSettings.companyName}");
        Debug.LogError($"ProductName：{PlayerSettings.productName}");
        Debug.LogError($"Version：{PlayerSettings.bundleVersion}");
        Debug.LogError($"PackageName：{PlayerSettings.applicationIdentifier}");
#if UNITY_ANDROID
        Debug.LogError($"BundleVersionCode：{PlayerSettings.Android.bundleVersionCode}");
        string keystoreName = PlayerSettings.Android.keystoreName;
        Debug.Log($"Keystore: {Path.GetFileName(keystoreName)}");
#elif UNITY_IOS
        Debug.LogError($"BundleVersionCode：{PlayerSettings.iOS.buildNumber}");
#endif
    }

    #endregion
}
