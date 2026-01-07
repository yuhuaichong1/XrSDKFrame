// 开始场景
using UnityEngine;
namespace XrCode
{
    public class StartScene : BaseScene
    {
        /// <summary>
        /// 开始场景进入
        /// 先检测热更新
        /// </summary>
        protected override void OnLoad()
        {
            ModuleMgr.Instance.SceneMod.LoadScene(ESceneType.MainScene);
        }

        /// <summary>
        /// 热更新资源检测下载结束 
        /// </summary>
        private void OnFinished()
        {
            ResourceUpdater.Instance.Destroy();
            IsPreloadDone = true;
        }
    }
}