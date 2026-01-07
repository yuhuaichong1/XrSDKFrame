// 场景模块

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
namespace XrCode
{
    public class SceneMod : BaseModule
    {
        private ESceneType sceneType;
        private BaseScene curScene;                             /// 当前场景
        private float mLoadProgress;                            /// 进度条
        private const string LOADING_SCENE = "Loading";         /// 加载场景

        private Dictionary<byte, BaseScene> sceneDic;
        private Dictionary<byte, string> sceneNameDic;

        public Action<string> OnLoadChanged;

        public Action<float> LoadingValue;

        protected override void OnLoad()
        {
            sceneDic = new Dictionary<byte, BaseScene>()
        {
            { (byte)ESceneType.MainScene, new MainScene() },
        };
            sceneNameDic = new Dictionary<byte, string>()
        {
            { (byte)ESceneType.MainScene, "MainCity" },
        };
            AddEvent();
            RegisetUpdateObj();
        }

        //注冊事件
        private void AddEvent()
        {
            FacadeScene.GetCurrentSceneType += GetCurrentSceneType;
        }
        //
        private void RemoveEvent()
        {
            FacadeScene.GetCurrentSceneType -= GetCurrentSceneType;
        }

        protected override void OnDispose()
        {
            RemoveEvent();
            if (curScene != null)
                if (curScene.IsPreloadDone)
                    curScene.Update();
        }

        /// 加载场景
        public void LoadScene(ESceneType sType)
        {
            //取出场景名称
            if (sceneNameDic.TryGetValue((byte)sType, out string sName))
            {
                //找到场景对象
                if (sceneDic.TryGetValue((byte)sType, out BaseScene scene))
                {
                    if (curScene != null) curScene.Dispose();
                    sceneType = sType;
                    curScene = scene;
                    Game.Instance.StartCoroutine(StartLoad(sName));
                }
            }
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        IEnumerator StartLoad(string name)
        {
            D.Log($"[SceneMod]: 加载场景 {name}");

            // 创建进度控制器
            var progressController = new LoadingProgressController((progress) =>
            {
                LoadingValue?.Invoke(progress);
            });

            // 打开Loading界面
            UIManager.Instance.OpenAsync<UILoading>(EUIType.EUILoading, (UILoading) =>
            {
                OnLoadChanged?.Invoke(name);
            });

            yield return null;

            // 加载Loading场景
            SceneManager.LoadScene(LOADING_SCENE);
            progressController.SetTargetProgress(10f); // 设置初始进度

            while (progressController.UpdateProgress())
                yield return null;

            // 资源清理
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            progressController.SetTargetProgress(20f);

            while (progressController.UpdateProgress())
                yield return null;

            // 开始异步加载目标场景
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
            //ModuleMgr.Instance.TDAnalyticsManager.LoadFinish();

            asyncLoad.allowSceneActivation = false; // 先不激活场景

            while (asyncLoad.progress < 0.9f) // Unity异步加载到0.9就停止
            {
                // 将0-0.9的进度映射到20-90的显示进度
                float targetProgress = 20f + (asyncLoad.progress / 0.9f) * 70f;
                progressController.SetTargetProgress(targetProgress);

                while (progressController.UpdateProgress())
                    yield return null;

                yield return null;
            }

            // 准备最终加载
            progressController.SetTargetProgress(90f);
            while (progressController.UpdateProgress())
                yield return null;

            // 激活场景
            asyncLoad.allowSceneActivation = true;
            progressController.SetTargetProgress(100f);

            while (progressController.UpdateProgress())
                yield return null;

            // 场景预加载
            curScene.Preload();
            while (!curScene.IsPreloadDone)
            {
                yield return null;
            }

            // 加载结束
            curScene.Load();
            FacadeScene.OnSceneLoadFinish?.Invoke();
        }

        //獲取當前場景類型
        private ESceneType GetCurrentSceneType()
        {
            return sceneType;
        }

        /// <summary>
        /// 实时更新
        /// </summary>
        protected override void OnUpdate()
        {
            if (curScene != null)
                if (curScene.IsPreloadDone)
                    curScene.Update();
        }

        protected override void OnFixedUpdate()
        {
            if (curScene != null)
                if (curScene.IsPreloadDone)
                    curScene.FixedUpdate();
        }
    }
}