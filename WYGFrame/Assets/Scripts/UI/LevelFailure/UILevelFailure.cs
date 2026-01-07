
using cfg;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{

    public partial class UILevelFailure : BaseUI
    {
        private int curCompletedLevel;
        private float curProgress;
        private bool ifUnlock;

        protected override void OnAwake()
        {
            
        }
        protected override void OnSetParam(params object[] args)
        {
            curCompletedLevel = (int)args[0];
            curProgress = (float)args[1];
            ifUnlock = (bool)args[2];
        }

        protected override void OnEnable()
        {
            InitShow();
            ShowAnim(mPlane);
            FacadeAudio.PlayEffect(EAudioType.ELevelFailed);
        }

        private void InitShow()
        {
            mLevelProgress.value = curProgress;
            mProgressText.text = $"{(int)(curProgress * 100)}%";

        }

        private void OnAdBtnClickHandle()        {            FacadeAd.PlayRewardAd(EAdSource.LevelFailureTryAgain, (amount) =>             {
                HideAnim(mPlane, () =>
                {
                    UIManager.Instance.CloseUI(EUIType.EUILevelFailure);
                });            }, null);        }	    private void OnOnlyBtnClickHandle()
        {
            HideAnim(mPlane, () => 
            {
                UIManager.Instance.CloseUI(EUIType.EUILevelFailure);
                FacadeGamePlay.RePlay();
            });
        }


        protected override void OnDisable()
        {
        
        }
        protected override void OnDispose()
        {
        
        }
    }
}