using System.Collections.Generic;

namespace XrCode
{
    public class ModuleMgr : Singleton<ModuleMgr>, ILoad, IDispose
    {
        private SceneMod sceneMod;
        private NotifyModule notifyMod;
        private PlayerModule userMod;
        private RedDotModule redDotModule;
        //private GameSDKManger gameSDKManger;
        private AudioModule audioMod;
        private TDAnalyticsManager tDAnalyticsManager;
        private LanguageModule languageMod;
        private GuideModule guideModule;
        private EventModule eventModdule;
        private GamePlayModule gamePlayModule;
        private AdModule adModule;
        private PayTypeModule payTypeModule;
        private WithdrawalModule withdrawalModule;
        public List<BaseModule> updateModList;

        public SceneMod SceneMod { get { return sceneMod; } }
        //public BulletMod BulletMod { get { return bulletMod; } }
        public NotifyModule NotifyMod { get { return notifyMod; } }
        public PlayerModule UserMod { get { return userMod; } }
        public GamePlayModule GamePlayModule { get { return gamePlayModule; } }
        public RedDotModule RedDotModule { get { return redDotModule; } }
        //public GameSDKManger GameSDKManger { get { return gameSDKManger; } }
        public AudioModule AudioMod { get { return audioMod; } }
        public TDAnalyticsManager TDAnalyticsManager { get { return tDAnalyticsManager; } }
        public LanguageModule LanguageMod { get { return languageMod; } }
        public GuideModule GuideModule { get { return guideModule; } }
        public AdModule AdModule { get { return adModule; } }
        public EventModule EventModdule { get { return eventModdule; } }
        public PayTypeModule PayTypeModule { get { return payTypeModule; } }
        public WithdrawalModule WithdrawalModule { get { return withdrawalModule; } }


        private bool isLoaded = false;
        public void Load()
        {
            languageMod = new LanguageModule();
            updateModList = new List<BaseModule>();
            notifyMod = new NotifyModule();
            userMod = new PlayerModule();
            sceneMod = new SceneMod();
            eventModdule = new EventModule();
            gamePlayModule = new GamePlayModule();
            redDotModule = new RedDotModule();
            //gameSDKManger = new GameSDKManger();
            audioMod = new AudioModule();
            tDAnalyticsManager = new TDAnalyticsManager();
            guideModule = new GuideModule();
            adModule = new AdModule();
            payTypeModule = new PayTypeModule();
            withdrawalModule = new WithdrawalModule();
        }
        public void Dispose()
        {
            userMod.Dispose();
            isLoaded = false;
            SceneMod.Dispose();
            notifyMod.Dispose();
            gamePlayModule.Dispose();
            redDotModule.Dispose();
            audioMod.Dispose();
            tDAnalyticsManager.Dispose();
            guideModule.Dispose();
            adModule.Dispose();
            eventModdule.Dispose();
            languageMod.Dispose();
            payTypeModule.Dispose();
            withdrawalModule.Dispose();
        }

        public void Start()
        {
            languageMod.Load();
            isLoaded = true;
            notifyMod.Load();
            payTypeModule.Load();
            userMod.Load();
            eventModdule.Load();
            gamePlayModule.Load();
            redDotModule.Load();
            audioMod.Load();
            guideModule.Load();
            tDAnalyticsManager.Load();
            sceneMod.Load();
            adModule.Load();
            withdrawalModule.Load();
            sceneMod.LoadScene(ESceneType.MainScene);
        }

        public void Update()
        {
            if (isLoaded)
            {
                AssetBundleMod.Instance.Update();
                for (int i = 0; i < updateModList.Count; i++)
                {
                    updateModList[i].Update();
                }

                UIManager.Instance.Update();
            }
        }

        public void FixedUpdate()
        {
            if (isLoaded)
            {
                //AssetBundleMod.Instance.Update();
                for (int i = 0; i < updateModList.Count; i++)
                {
                    updateModList[i].FixedUpdate();
                }
            }
        }

        //注册更新模块
        public void RegistUpdateObj(BaseModule module)
        {
            updateModList.Add(module);
        }
    }
}