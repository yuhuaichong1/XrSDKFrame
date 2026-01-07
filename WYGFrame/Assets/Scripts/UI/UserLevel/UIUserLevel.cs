
using cfg;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{

    public partial class UIUserLevel : BaseUI
    {
        private TBUserLevel TBUserLevel;
        protected override void OnAwake()
        {
            TBUserLevel = ConfigModule.Instance.Tables.TBUserLevel;
        }

        protected override void OnEnable()
        {
            ShowAnim(mPlane);
            int curUserLevel = FacadePlayer.GetPlayerLevel();
            mCurLevelText.text = string.Format(FacadeLanguage.GetText("10012"), curUserLevel += 1);

            int nextLevelExp = TBUserLevel.Get(curUserLevel).NextLvNeedExp;
            float progress = FacadePlayer.GetPlayerExp() * 1f / nextLevelExp;
            mLevelProgressText.text = $"{(int)(progress * 100)}%";
            mLevelProgress.value = progress;
        }
        	    private void OnExitBtnClickHandle()        {            HideAnim(mPlane, () =>             {                 UIManager.Instance.CloseUI(EUIType.EUIUserLevel);            });        }	    private void OnKeepPlayingBtnClickHandle()
        {
            HideAnim(mPlane, () =>
            {
                UIManager.Instance.CloseUI(EUIType.EUIUserLevel);
            });
        }

        protected override void OnDisable()
        {
        
        }

        protected override void OnDispose()
        {
            TBUserLevel = null;
        }
    }
}