/**
 * 路径处理工具
 */

using System.IO;
using UnityEngine;
namespace XrCode
{
    public class PathUtil
    {
        /// <summary>
        /// 资源完整路径的前缀
        /// </summary>
        public const string ASSETPATHPREFIX = "Assets/AssetBundleLocal";
        public const string ResourcesPath = "AssetBundleLocal";
        private const string CONFIGDATAPATH = "Data/ConfData.bytes";

        /// <summary>
        /// 获得资源的完整路径
        /// </summary>
        public static string GetFullAssetPath(string assetPath)
        {
            string path = "";
            if (!AppConfig.LoadAssetWithServer)
            {
                if (!AppConfig.LoadAssetWithResources)
                {
                    path = StringUtil.PathConcat(ASSETPATHPREFIX, assetPath);
                }
                else
                {
                    path = StringUtil.PathConcat(ResourcesPath, assetPath);
                }
                D.Log($"[FullPath]: path {path}");
            }
            else
            {
                //path = StringUtil.Concat(AppConfig.ServerResourceURL, $"StreamingAssets/AssetBundles/assets/assetbundlelocal/{assetPath.ToLower()}.assetbundle");
                path = StringUtil.Concat(AppConfig.ServerResourceURL, $"StreamingAssets/AssetBundles/Assets/AssetBundleLocal/{assetPath}.assetbundle");
                D.Log($"[assetPath]: path {assetPath}");
                D.Log($"[FullPath]: path {path}");
            }
            return path;
        }
        public static string GetLocalConfigPath()
        {
#if UNITY_EDITOR

            //return GetLocalFilePath(CONFIGDATAPATH);

#elif UNITY_WEBGL

        D.Error($"[ConfPath]: begin");
        string path = StringUtil.Concat(AppConfig.ServerResourceURL, $"StreamingAssets/{CONFIGDATAPATH}");

        D.Error($"[ConfPath]: path {path}");

        if (!File.Exists(path))
        {
            D.Error(" 路径不存在 ");
        }
        return path;
#endif
            return GetLocalFilePath(CONFIGDATAPATH);
        }

        /// <summary>
        /// 获得本地资源文件路径
        /// 热更新资源会写入到PresistentData目录下
        /// 本地资源先查询PresistentData目录，没有则返回StreamingAssets目录下路径
        /// </summary>
        public static string GetLocalFilePath(string filePath, bool www = false)
        {
            return CheckPresistentDataFileExsits(filePath) ? GetPresistentDataFilePath(filePath) : GetStreamingAssetsFilePath(filePath, www);
        }

        /// <summary>
        /// 获得StreamingAssets下的资源文件路径
        /// </summary>
        public static string GetStreamingAssetsFilePath(string filePath, bool www = false)
        {
            var streamingAssetsPath = Application.streamingAssetsPath;
            if (AppConfig.LoadAssetWithServer)
            {
                streamingAssetsPath = GetServerFileURL("StreamingAssets");
            }
#if UNITY_EDITOR
            if (www)
            {
                // Mac环境编辑模式下，用www加载StreamingAssets资源要加前缀
                streamingAssetsPath = StringUtil.Concat("file://", Application.streamingAssetsPath);
            }

#elif UNITY_IOS
        if (www)
        {
            // IOS用www加载StreamingAssets资源要加前缀
            streamingAssetsPath = StringUtil.Concat("file://", Application.streamingAssetsPath);
        }
#elif UNITY_WEBGL
            streamingAssetsPath = GetServerFileURL("StreamingAssets");
#endif
            string path = StringUtil.PathConcat(streamingAssetsPath, filePath);
            return path;
        }

        /// <summary>
        /// 获得PresistentData下的资源文件路径
        /// </summary>
        public static string GetPresistentDataFilePath(string filePath)
        {
            return StringUtil.PathConcat(Application.persistentDataPath, filePath);
        }

        /// <summary>
        /// 检测PresistentData下的资源文件路径
        /// </summary>
        public static bool CheckPresistentDataFileExsits(string filePath)
        {
            var path = GetPresistentDataFilePath(filePath);
            return FileUtil.Exists(path);
        }

        /// <summary>
        /// 获取下载资源路径
        /// </summary>
        public static string GetServerFileURL(string filePath)
        {
            return StringUtil.Concat(AppConfig.ServerResourceURL, filePath);
        }

        /// <summary>
        /// 获得本地AssetBundle路径
        /// </summary>
        public static string GetLocalAssetBundleFilePath(string assetBundleName)
        {
            var assetBundlesPath = StringUtil.PathConcat(AssetBundleConfig.AssetBundlesFolder, assetBundleName);
            return GetLocalFilePath(assetBundlesPath);
        }
        public static string GetLoaclAssetWithResour(string assetBundleName)
        {
            var assetBundlesPath = StringUtil.PathConcat(AssetBundleConfig.AssetBundlesFolder, assetBundleName);
            if (assetBundlesPath.EndsWith(".assetbundle"))
            {
                assetBundlesPath = assetBundlesPath.Replace(".assetbundle", "");
            }
            return $"{assetBundlesPath}";
        }
    }
}