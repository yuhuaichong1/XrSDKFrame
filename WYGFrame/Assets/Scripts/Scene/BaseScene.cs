/**
 * 场景基类
 */
namespace XrCode
{
    public abstract class BaseScene
    {

        /// <summary>
        /// 是否预加载完成
        /// </summary>
        public bool IsPreloadDone { get; protected set; }

        /// <summary>
        /// 场景进入
        /// </summary>
        public void Load() { OnLoad(); }
        protected virtual void OnLoad() { }

        /// <summary>
        /// 场景退出
        /// </summary>
        public void Dispose() { OnDispose(); }
        protected virtual void OnDispose() { }

        /// <summary>
        /// 场景更新
        /// </summary>
        public void Update() { OnUpdate(); }
        protected virtual void OnUpdate() { }

        public void FixedUpdate() { FixedOnUpdate(); }
        protected virtual void FixedOnUpdate() { }

        /// <summary>
        /// 预加载资源
        /// </summary>
        public void Preload() { OnPreload(); }
        protected virtual void OnPreload()
        {
            IsPreloadDone = true;
        }

        /// <summary>
        /// 初始化场景表格数据
        /// </summary>
        protected virtual void InitSceneConf()
        {
            //if (SceneId == 0) return;
            //mSceneSheet = SheetManager.Instance.GetScene(SceneId);
            //if (string.IsNullOrEmpty(SceneName)) SceneName = mSceneSheet.Name;
        }
    }
}