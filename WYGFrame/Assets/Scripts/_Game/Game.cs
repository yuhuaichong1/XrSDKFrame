using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace XrCode
{
    public class Game : MonoBehaviour, IGame
    {
        public static Game Instance { get; private set; }
        private EGameState gameState;
        private int preloadCount = 0;
        private Queue<Action> startup;

        public EGameState GameState
        {
            get { return gameState; }
            set
            {
                gameState = value;
            }
        }

        public bool isContent;
        public bool ifCheckNetwork; //是否将网络请求加入加载队列
        public bool ifIAA;//是否切换IAA（ifCheckNetwork必须为false）

        void Awake()
        {
            Instance = this;
            gameState = EGameState.Load;
            if(!ifCheckNetwork)
                GameDefines.ifIAA = ifIAA;
            Load();
        }

        public void Load()
        {
            D.Error("[Game]: 游戏启动");
            preloadCount = 0;
            startup = new Queue<Action>();
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = 60;
            if (AppConfig.LoadAssetWithResources)
            {
                AppConfig.UseAssetBundle = false;
                AppConfig.LoadAssetWithServer = false;
            }

            if(ifCheckNetwork)
                RegistPreloadFunc(out NetworkModule.Instance.OnFinished);
            RegistPreloadFunc(out AssetBundleMod.Instance.OnFinished);
            RegistPreloadFunc(out ConfigModule.Instance.OnFinished);

            startup.Enqueue(AssetBundleMod.Instance.StartUp);//加入队列，保证热更新资源完毕后，再开始。获取资源路径
            startup.Enqueue(ConfigModule.Instance.StartUp);//获取配置表信息
            if (ifCheckNetwork)
                NetworkModule.Instance.GetNetworkInitInfo();
            GameSDKManger.Instance.StartUp();//各个平台的文件系统，初始化提前。因为热更新需要用
                                             //版本暂留
            if (AppConfig.CheckVersionUpdate)
            {
                RegistPreloadFunc(out ResourceUpdater.Instance.OnFinished);
                ResourceUpdater.Instance.Startup();//需要热更新，则热更新
            }
            else
            {
                if (startup.Count > 0)//否则直接开始AssetBundleMod.Instance.StartUp
                {
                    startup.Dequeue()?.Invoke();
                }
            }
            gameState = EGameState.Start;
        }

        private void RegistPreloadFunc(out System.Action ac)
        {
            preloadCount++;
            ac = OnFinished;
        }

        //预加载流程结束
        private void OnFinished()
        {
            //SQLiteHelper.Instance.InitConnection(PathUtil.GetLocalConfigPath());
            if (preloadCount > 0)
            {
                preloadCount--;
                if (startup.Count > 0)
                {
                    StartCoroutine(SetStartUp());
                }
            }
            if (preloadCount == 0)
            {
                D.Error("[Game]: 资源加载完成");
                ModuleMgr.Instance.Start();
                gameState = EGameState.Run;
            }
        }
        IEnumerator SetStartUp()
        {
            yield return new WaitForEndOfFrame();
            startup.Dequeue()?.Invoke();
        }
        public void Update()
        {
            TimerManager.Instance.Update();
            STimerManager.Instance.UpdateInstance();
            if (gameState != EGameState.Run) return;
            ModuleMgr.Instance.Update();
        }

        public void FixedUpdate()
        {
            if (gameState != EGameState.Run) return;
            ModuleMgr.Instance.FixedUpdate();
        }

        public void RegisterUpdateObject(IUpdate updateObj) { }

        public void UnRegisterUpdateObject(IUpdate updateObj) { }

        void OnDestroy()
        {
            gameState = EGameState.Exit;
            Dispose();
        }
        public void Dispose()
        {
            ModuleMgr.Instance.Dispose();
            TimerManager.Instance.Dispose();
            SQLiteHelper.Instance.Dispose();
        }

    }
}

