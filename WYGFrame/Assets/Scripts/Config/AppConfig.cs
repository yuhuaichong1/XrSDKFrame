

using System;

public static class AppConfig
{
    public static bool UseAssetBundle = true; // 是否使用 AssetBundle
    public static bool CheckVersionUpdate = false; // 是否检查版本更新
    public static bool LoadAssetWithServer = false; // 是否从服务器加载资源
    public static bool LoadAssetWithResources = false; // 是否从 Resources 加载资源
    public static  string ServerResourceURL = "http://49.233.134.149/ab/"; // 服务器资源地址
}