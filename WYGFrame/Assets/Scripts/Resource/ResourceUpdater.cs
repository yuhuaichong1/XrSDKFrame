/**
 * 热更新资源下载器
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System;
using Newtonsoft.Json;
namespace XrCode
{
    public class ResourceUpdater : MonoSingleton<ResourceUpdater>, ILoad
    {
        private const string VERSIONCONFIGFILE = "VersionConfig.json";
        /// <summary>
        /// 本地版本配置信息
        /// </summary>
        private VersionConfig mLocalVersionConfig;
        /// <summary>
        /// 服务器版本配置信息
        /// </summary>
        private VersionConfig mServerVersionConfig;
        /// <summary>
        /// 需要热更新下载的AssetBundle队列
        /// </summary>
        private Queue<string> mNeedDownLoadQueue;
        //
        /// <summary>
        /// 热更新检测加载结束回调委托
        /// </summary>
        public System.Action OnFinished;

        public void Load()
        {
            mNeedDownLoadQueue = new Queue<string>();
        }
        /// <summary>
        /// 开始更新
        /// </summary>
        public void Startup()
        {
            StartCoroutine(CheckUpdate());
            //Finished();
        }

        /// <summary>
        /// 检查更新，包括大版本换包和热更新
        /// </summary>
        private IEnumerator CheckUpdate()
        {
            // 加载并初始化版本信息文件
            yield return InitVersion();

            // 没有版本对比文件，直接进入游戏
            if (mLocalVersionConfig == null || mServerVersionConfig == null)
            {
                Finished();
                yield break;
            }

            // 检测版本配置文件
            if (true || CheckVersion(mLocalVersionConfig.Version, mServerVersionConfig.Version))
            {
                // 对比版本资源
                yield return CompareVersion();
            }

            if (mNeedDownLoadQueue.Count == 0)
            {
                Finished();
            }
            else
            {
                DownLoadResource();
            }
        }

        /// <summary>
        /// 初始化本地版本
        /// </summary>
        IEnumerator InitVersion()
        {

            if (!GameSDKManger.Instance.CheckFileExists(VERSIONCONFIGFILE))//本地没有VersionConfig
            {
                D.Log("本地没有VersionConfig");
                mLocalVersionConfig = new VersionConfig() { Version = "1.-1" };//默认版本为1.-1
            }
            else
            {
                GameSDKManger.Instance.ReadFileSync(VERSIONCONFIGFILE, delegate (string str)
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.Converters.Add(new Hash128Converter());
                    mLocalVersionConfig = JsonConvert.DeserializeObject<VersionConfig>(str, settings);
                });
            }
            string serverVersionConfigPath = PathUtil.GetServerFileURL("StreamingAssets/" + VERSIONCONFIGFILE);
            UnityWebRequest webReq = new UnityWebRequest(serverVersionConfigPath);
            webReq.downloadHandler = new DownloadHandlerBuffer();
            webReq.SetRequestHeader("Content-Type", "application/json");
            yield return webReq.SendWebRequest();
            mServerVersionConfig = string.IsNullOrEmpty(webReq.error) ? JsonUtility.FromJson<VersionConfig>(webReq.downloadHandler.text) : mLocalVersionConfig;
            webReq.Dispose();
            //var localVersionConfigPath = PathUtil.GetLocalFilePath(VERSIONCONFIGFILE, true);//加载本地版本
            //UnityWebRequest webReq = new UnityWebRequest(localVersionConfigPath);
            //webReq.downloadHandler = new DownloadHandlerBuffer();
            //webReq.SetRequestHeader("Content-Type", "application/json");
            //yield return webReq.SendWebRequest();
            //if (webReq.isDone)
            //{
            //    D.Error(" 加载本地版本完成 ");//下载资源列表
            //}
            //mLocalVersionConfig = JsonUtility.FromJson<VersionConfig>(webReq.downloadHandler.text);
            //webReq.Dispose();
            //var serverVersionConfigPath = PathUtil.GetServerFileURL(VERSIONCONFIGFILE);
            //webReq = new UnityWebRequest(serverVersionConfigPath);
            //webReq.downloadHandler = new DownloadHandlerBuffer();
            //webReq.SetRequestHeader("Content-Type", "application/json");
            //yield return webReq.SendWebRequest();
            //mServerVersionConfig = string.IsNullOrEmpty(webReq.error) ? JsonUtility.FromJson<VersionConfig>(webReq.downloadHandler.text) : mLocalVersionConfig;
            //webReq.Dispose();
        }

        /// <summary>
        /// 更新配置文件
        /// </summary>
        private void UpdateVersionConfig()
        {
            //var path = PathUtil.GetPresistentDataFilePath(VERSIONCONFIGFILE);
            //var text = JsonUtility.ToJson(mServerVersionConfig);
            //FileUtil.WriteAllText(path, text);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new Hash128Converter());
            var text = JsonConvert.SerializeObject(mServerVersionConfig, settings);
            GameSDKManger.Instance.WriteFileSync(VERSIONCONFIGFILE, text);
        }

        /// <summary>
        /// 版本对比
        /// </summary>
        private bool CheckVersion(string sourceVersion, string targetVersion)
        {
            string[] sourceVers = sourceVersion.Split('.');
            string[] targetVers = targetVersion.Split('.');
            try
            {
                int sV0 = int.Parse(sourceVers[0]);
                int tV0 = int.Parse(targetVers[0]);
                int sV1 = int.Parse(sourceVers[1]);
                int tV1 = int.Parse(targetVers[1]);
                // 大版本更新
                if (tV0 > sV0)
                {
                    D.Log("New Version");
                    return false;
                }
                // 热更新
                if (tV0 == sV0 && tV1 > sV1)
                {
                    D.Log("Update Res ...");
                    return true;
                }
            }
            catch (System.Exception e)
            {
                D.Error(e.Message);
            }
            return false;
        }

        /// <summary>
        /// 比较版本资源
        /// </summary>
        /// <returns></returns>
        IEnumerator CompareVersion()
        {
            Dictionary<string, Hash128> localAllAssetBundlesDict = new Dictionary<string, Hash128>();
            string manifestAssetBundlePath = StringUtil.PathConcat(AssetBundleConfig.AssetBundlesFolder, AssetBundleConfig.AssetBundlesFolder);
            if (!GameSDKManger.Instance.CheckFileExists(manifestAssetBundlePath))
            {
                GameSDKManger.Instance.CreateDirectoryMkdir(AssetBundleConfig.AssetBundlesFolder);
                if (mLocalVersionConfig.assetMD5 != null)
                {
                    foreach (var item in mLocalVersionConfig.assetMD5)
                    {
                        localAllAssetBundlesDict.Add(item.Key, item.Value);
                    }
                }
            }
            else
            {
                GameSDKManger.Instance.ReadFileSync(manifestAssetBundlePath, delegate (byte[] bytes)
                {
                    AssetBundle bundle = AssetBundle.LoadFromMemory(bytes);
                    var localManifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                    List<string> localAllAssetBundles = new List<string>(localManifest.GetAllAssetBundles());
                    foreach (var abName in localAllAssetBundles)
                    {
                        localAllAssetBundlesDict.Add(abName, localManifest.GetAssetBundleHash(abName));
                        CheckLocalMd5(localAllAssetBundlesDict, abName);//检查本地保存的md5值，避免重复下载
                    }
                    bundle.Unload(true);
                });
            }
            string serverManifestPath = PathUtil.GetServerFileURL("StreamingAssets/" + manifestAssetBundlePath);
            UnityWebRequest svrverRequest = UnityWebRequestAssetBundle.GetAssetBundle(serverManifestPath);
            yield return svrverRequest.SendWebRequest();

            if (DownloadHandlerAssetBundle.GetContent(svrverRequest) != null)
            {
                var serverManifest = DownloadHandlerAssetBundle.GetContent(svrverRequest).LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                var serverAllAssetBundles = serverManifest.GetAllAssetBundles();//获取服务器的资源和依赖
                foreach (var abName in serverAllAssetBundles)
                {
                    Hash128 serverAssetBundleHash = serverManifest.GetAssetBundleHash(abName);
                    if (localAllAssetBundlesDict.ContainsKey(abName))
                    {
                        if (localAllAssetBundlesDict[abName] != serverAssetBundleHash)
                        {
                            mNeedDownLoadQueue.Enqueue(abName);
                        }
                    }
                    else
                    {
                        mNeedDownLoadQueue.Enqueue(abName);
                    }
                    SaveHs(serverAssetBundleHash, abName);//保存服务器资源的MD5值
                }
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in mNeedDownLoadQueue)
                {
                    stringBuilder.AppendLine(item);
                }
                D.Log("需要下载的资源：" + stringBuilder);
                if (mNeedDownLoadQueue.Count > 0)
                {
                    mNeedDownLoadQueue.Enqueue(AssetBundleConfig.AssetBundlesFolder);//一旦有任何新的资源下载，主assetbun都需要重新下载
                }
                DownloadHandlerAssetBundle.GetContent(svrverRequest).Unload(true);
            }

            svrverRequest.Dispose();



            //string manifestAssetBundlePath = StringUtil.PathConcat(AssetBundleConfig.AssetBundlesFolder, AssetBundleConfig.AssetBundlesFolder);

            //string localManifestPath = PathUtil.GetLocalFilePath(manifestAssetBundlePath, true);

            //UnityWebRequest localRequest = UnityWebRequestAssetBundle.GetAssetBundle(localManifestPath);

            //yield return localRequest.SendWebRequest();

            //var localManifest= DownloadHandlerAssetBundle.GetContent(localRequest).LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            //var localAllAssetBundles = new List<string>(localManifest.GetAllAssetBundles());
            //var localAllAssetBundlesDict = new Dictionary<string, Hash128>();
            //foreach (var abName in localAllAssetBundles)
            //{
            //    localAllAssetBundlesDict.Add(abName, localManifest.GetAssetBundleHash(abName));
            //}

            //DownloadHandlerAssetBundle.GetContent(localRequest).Unload(true);
            //localRequest.Dispose();


            //var serverManifestPath = PathUtil.GetServerFileURL(manifestAssetBundlePath);

            //UnityWebRequest svrverRequest = UnityWebRequestAssetBundle.GetAssetBundle(serverManifestPath);

            //yield return svrverRequest.SendWebRequest();

            //if (DownloadHandlerAssetBundle.GetContent(svrverRequest) != null)
            //{
            //    var serverManifest = DownloadHandlerAssetBundle.GetContent(svrverRequest).LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            //    var serverAllAssetBundles = serverManifest.GetAllAssetBundles();//获取服务器的资源和依赖
            //    foreach (var abName in serverAllAssetBundles)
            //    {
            //        if (localAllAssetBundlesDict.ContainsKey(abName))
            //        {
            //            var serverAssetBundleHash = serverManifest.GetAssetBundleHash(abName);
            //            if (localAllAssetBundlesDict[abName] != serverAssetBundleHash) mNeedDownLoadQueue.Enqueue(abName);
            //        }
            //        else
            //        {
            //            mNeedDownLoadQueue.Enqueue(abName);
            //        }
            //    }

            //    if (mNeedDownLoadQueue.Count > 0) mNeedDownLoadQueue.Enqueue(AssetBundleConfig.AssetBundlesFolder);//一旦有任何新的资源下载，主assetbun都需要重新下载

            //    DownloadHandlerAssetBundle.GetContent(svrverRequest).Unload(true);
            //}

            //svrverRequest.Dispose();
        }
        /// <summary>
        /// 保存服务器资源的MD5值
        /// </summary>
        /// <param name="serverAssetBundleHash">服务器资源的MD5值</param>
        /// <param name="abName">资源的名字</param>
        private void SaveHs(Hash128 serverAssetBundleHash, string abName)
        {
            if (mServerVersionConfig.assetMD5 == null)
            {
                mServerVersionConfig.assetMD5 = new Dictionary<string, Hash128>();
            }
            mServerVersionConfig.assetMD5.Add(abName, serverAssetBundleHash);
        }

        /// <summary>
        /// 检查本地保存的md5值，避免重复下载
        /// </summary>
        /// <param name="localAllAssetBundlesDict">本地AssetBundles的AssetBundleManifest保存的md5值，因为AssetBundles最后一个下载，所以其他文件下载下来后，新的MD5值自己保存</param>
        /// <param name="abName">资源的名字</param>
        private void CheckLocalMd5(Dictionary<string, Hash128> localAllAssetBundlesDict, string abName)
        {
            if (mLocalVersionConfig.assetMD5 != null)
            {
                mLocalVersionConfig.assetMD5.TryGetValue(abName, out Hash128 md5);
                if (md5 != null)
                {
                    localAllAssetBundlesDict[abName] = md5;
                }
            }
        }

        /// <summary>
        /// 加载结束
        /// </summary>
        private void Finished()
        {
            OnFinished?.Invoke();
        }

        /// <summary>
        /// 下载资源
        /// </summary>
        private void DownLoadResource()
        {
            if (mNeedDownLoadQueue.Count == 0)
            {
                UpdateVersionConfig();
                Finished();
                return;
            }
            var abName = mNeedDownLoadQueue.Dequeue();
            D.Log(StringUtil.Concat("DownLoad ", abName));
            var abPath = StringUtil.PathConcat("StreamingAssets/" + AssetBundleConfig.AssetBundlesFolder, abName);
            var url = PathUtil.GetServerFileURL(abPath);
            StartCoroutine(DownLoad(url, abName));
        }

        /// <summary>
        /// 替换本地的资源
        /// </summary>
        private void ReplaceLocalResource(string abName, byte[] data)
        {
            if (data == null) return;
            try
            {
                string abPath = StringUtil.PathConcat(AssetBundleConfig.AssetBundlesFolder, abName);
                string dirPath = Regex.Replace(abPath, @"/[^/]+$", "");
                if (!GameSDKManger.Instance.CheckDirectoryExists(dirPath))
                {
                    GameSDKManger.Instance.CreateDirectoryMkdir(dirPath);
                }
                GameSDKManger.Instance.WriteFileSync(abPath, data);
                if (mLocalVersionConfig.assetMD5 == null)//下载完一个文件后保存新的MD5值
                {
                    mLocalVersionConfig.assetMD5 = new Dictionary<string, Hash128>();
                }
                if (mLocalVersionConfig.assetMD5.ContainsKey(abName) && mServerVersionConfig.assetMD5.ContainsKey(abName))
                {
                    mLocalVersionConfig.assetMD5[abName] = mServerVersionConfig.assetMD5[abName];
                }
                else
                {
                    mLocalVersionConfig.assetMD5.Add(abName, mServerVersionConfig.assetMD5[abName]);
                }
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new Hash128Converter());
                string text = JsonConvert.SerializeObject(mLocalVersionConfig, settings);
                GameSDKManger.Instance.WriteFileSync(VERSIONCONFIGFILE, text);
            }
            catch (System.Exception e)
            {
                D.Error(e.Message);
            }
        }

        /// <summary>
        /// 下载资源
        /// </summary>
        IEnumerator DownLoad(string url, string abName)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                // 发送请求并等待响应
                yield return webRequest.SendWebRequest();

                // 检查错误
                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    D.Error("Error downloading: " + webRequest.error);
                }
                else
                {
                    D.Log(abName + "下载完成");
                    // 下载成功，替换本地资源
                    ReplaceLocalResource(abName, webRequest.downloadHandler.data);
                }
            }

            DownLoadResource();
        }
    }
}