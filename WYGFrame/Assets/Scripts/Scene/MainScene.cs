namespace XrCode
{
    //主场景
    public class MainScene : BaseScene
    {
        /// 主场景进入
        protected override void OnLoad()
        {
            UIManager.Instance.OpenAsync<UIGamePlay>(EUIType.EUIGamePlay, (BaseUI) =>
            {
                UIManager.Instance.OpenAsync<UIEffect>(EUIType.EUIEffect);
                UIManager.Instance.OpenAsync<UIGuide>(EUIType.EUIGuide);
            });
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}