/**
 * AB包异步加载器
 */

using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

namespace XrCode
{

    public class AssetBundleLoader : IDisposable
    {
        /// <summary>
        /// 加载的AB包名
        /// </summary>
        public string AssetBundleName { set; get; }
        /// <summary>
        /// 是否正在加载
        /// </summary>
        public bool IsLoading { get; private set; }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <returns></returns>
        public IEnumerator AsyncLoadAssetBundle1()
        {
            IsLoading = true;
            var path = PathUtil.GetLocalAssetBundleFilePath(AssetBundleName);
            var abCreateRequest = AssetBundle.LoadFromFileAsync(path);
            D.Log("[AssetBundle] 开始加载 {0}", path);
            yield return abCreateRequest;
            D.Log("[AssetBundle] 加载完成 {0}", path);
            AssetBundleMod.Instance.LoadAssetBundleFinished(this, abCreateRequest.assetBundle);
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <returns></returns>
        public IEnumerator AsyncLoadAssetBundle()
        {
            IsLoading = true;
            var path = PathUtil.GetLocalAssetBundleFilePath(AssetBundleName);
#if UNITY_IOS
            string url = Path.Combine(Application.streamingAssetsPath, path);
            path = "file://" + url;
#endif
            string localFilePath = "";
            if (AppConfig.LoadAssetWithServer)//从CDN下载资源，保存到本地是，把AppConfig.ServerResourceURL替换为null
            {
                localFilePath = path.Replace(AppConfig.ServerResourceURL + "StreamingAssets/", "");
            }
            // 如果现在是从CDN下载资源,先检查本地是否存在
            if (AppConfig.LoadAssetWithServer && GameSDKManger.Instance.CheckFileExists(localFilePath))
            {
                D.Log("本地存在" + localFilePath + " 开始加载");
                AssetBundle bundle = null;
                GameSDKManger.Instance.ReadFileSync(localFilePath, delegate (byte[] bytes)
                 {
                     if (bytes != null && bytes.Length > 0)
                     {
                         try
                         {
                             // 使用字节数组加载 AssetBundle
                             bundle = AssetBundle.LoadFromMemory(bytes);
                             if (bundle != null)
                             {
                                 D.Error("AssetBundle 从本地加载成功");
                             }
                             else
                             {
                                 D.Error("AssetBundle 加载失败");
                             }
                         }
                         catch (Exception e)
                         {
                             D.Error($"加载 AssetBundle 过程中出错: {e.Message}");
                         }
                     }
                     else
                     {
                         D.Error("读取的字节数组为空或无效");
                     }
                 });
                yield return new WaitForEndOfFrame();
                if (bundle != null)
                {
                    AssetBundleMod.Instance.LoadAssetBundleFinished(this, bundle);
                }
            }
            else
            {
                // 使用UnityWebRequest下载 path是本地StreamingAssets目录或者CDN路径都可以
                using (UnityWebRequest request = UnityWebRequest.Get(path))
                {
                    D.Log("[AssetBundle] 开始加载 {0}", path);
                    yield return request.SendWebRequest();
                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        //如果资源是从CDN下载的，则保存一份到本地
                        if (AppConfig.LoadAssetWithServer)
                        {
                            // 保存文件
                            string dirPath = Regex.Replace(localFilePath, @"/[^/]+$", "");
                            if (!GameSDKManger.Instance.CheckDirectoryExists(dirPath))
                            {
                                D.Log("路径不存在，开始创建" + dirPath);
                                GameSDKManger.Instance.CreateDirectoryMkdir(dirPath);
                            }
                            GameSDKManger.Instance.WriteFileSync(localFilePath, request.downloadHandler.data);
                        }
                        // 加载 AssetBundle
                        AssetBundle bundle = AssetBundle.LoadFromMemory(request.downloadHandler.data);
                        if (bundle != null)
                        {
                            D.Log("[AssetBundle] 加载完成 {0}", path);
                            AssetBundleMod.Instance.LoadAssetBundleFinished(this, bundle);
                        }
                        else
                        {
                            D.Error("AssetBundle 加载失败");
                            AssetBundleMod.Instance.LoadAssetBundleFinished(this, null);
                        }
                    }
                    else
                    {
                        AssetBundleMod.Instance.LoadAssetBundleFinished(this, null);
                        D.Log("[AssetBundle] eee 加载失败  {0}", path);
                    }
                }
            }

        }

        /// <summary>
        /// 销毁重置
        /// </summary>
        public void Dispose()
        {
            IsLoading = false;
            AssetBundleName = string.Empty;
        }
    }
}