/**
 * 资源异步加载器
 */
namespace XrCode
{
    using System;
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class AssetLoader : IDisposable
    {
        /// <summary>
        /// 加载成功后的回调列表
        /// </summary>
        private List<Action<UnityEngine.Object>> mCallbackList = new List<Action<UnityEngine.Object>>();
        //------------------------------------------------
        /// <summary>
        /// 加载的资源路径
        /// </summary>
        public string Path { set; get; }
        /// <summary>
        /// 是否正在加载
        /// </summary>
        public bool IsLoading { get; private set; }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <returns></returns>
        public IEnumerator AsyncLoadAsset()
        {
            IsLoading = true;
            var unit = AssetBundleMod.Instance.GetAssetBundleUnit(Path);
            AssetBundleMod.Instance.RefAllAssetBundles(Path);//计算资源引用次数
            var assetBundle = unit.AssetBundle;
            if (assetBundle != null)//assetBundle!=null 正确从服务器加载下载ab包
            {
                string asspath = "";
                AssetBundleRequest abRequest;
                if (!AppConfig.LoadAssetWithServer)
                {
                    asspath = Path;
                    abRequest = IsSprite(asspath) ? assetBundle.LoadAssetAsync<Sprite>(asspath) : assetBundle.LoadAssetAsync(asspath);
                }
                else
                {
                    //asspath = AssetBundleMod.Instance.PathToBundle(Path).Replace(".assetbundle", "");//webgl平台，ab包内资源的名字和资源的下载路径不一致，需要获取ab内资源的名字，
                    asspath = Path.Replace($"{AppConfig.ServerResourceURL}StreamingAssets/{AssetBundleConfig.AssetBundlesFolder}/", "").Replace(".assetbundle", "");//webgl平台，ab包内资源的名字和资源的下载路径不一致，需要获取ab内资源的名字，
                    abRequest = IsSprite(asspath) ? assetBundle.LoadAssetAsync<Sprite>(asspath) : assetBundle.LoadAssetAsync(asspath);
                }
                D.Log("[AssetBundle] 开始加载 {0}", Path);
                yield return abRequest;
                D.Log("[AssetBundle] 加载完成 {0}", Path);
                for (var i = 0; i < mCallbackList.Count; i++)
                {
                    mCallbackList[i](abRequest.asset);
                }
            }
            else
            {
                string resAssetpath = Path.Replace("Assets/", "");
                if (AppConfig.LoadAssetWithServer)
                {
                    resAssetpath = resAssetpath.Replace($"{AppConfig.ServerResourceURL}StreamingAssetBundles/", "");
                    resAssetpath = resAssetpath.Replace($".assetbundle", "");
                }
                resAssetpath = Regex.Replace(resAssetpath, @"\.[^.]+$", "");
                UnityEngine.Object obg = Resources.Load<UnityEngine.Object>(resAssetpath);
                yield return new WaitForEndOfFrame();
                if (obg == null)
                {
                    D.Error($"从CDN下载资源失败，并且Resources.Load也加载失败");
                }
                for (var i = 0; i < mCallbackList.Count; i++)
                {
                    if (obg != null)
                    {
                        mCallbackList[i](obg);
                    }
                }
            }
            AssetBundleMod.Instance.LoadAssetFinished(this);
        }

        /// <summary>
        /// 判断是否Sprite
        /// </summary>
        private bool IsSprite(string asspath)
        {
            return asspath.EndsWith(".png", StringComparison.OrdinalIgnoreCase) || asspath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 判断是否能开始异步加载了，取决于AssetBundle是否完成
        /// 这里先同步加载下AssetBundle
        /// </summary>
        public bool CanLoadAssetAsync()
        {
            if (IsLoading) return false;
            return AssetBundleMod.Instance.CheckAllAssetBundleLoaded(Path);
        }

        /// <summary>
        /// 当请求相同资源时，添加对应回调到回调列表
        /// </summary>
        public void AddCallback(Action<UnityEngine.Object> callback)
        {
            if (mCallbackList.Contains(callback)) return;
            mCallbackList.Add(callback);
        }

        /// <summary>
        /// 销毁重置
        /// </summary>
        public void Dispose()
        {
            IsLoading = false;
            Path = string.Empty;
            mCallbackList.Clear();
        }
    }
}