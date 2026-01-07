using cfg;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{

    public partial class UILevelCompleted : BaseUI
    {
        private int curCompletedLevel;//当前完成的关卡
        private float curCompletedMoney;//当前完成的关卡的通关奖励
        private float curOnlyMoney;//通关奖励÷10
        private TBLevel TBLevel;//关卡表

        protected override void OnAwake()
        {
            TBLevel = ConfigModule.Instance.Tables.TBLevel;

            mMoneyIcon.gameObject.SetActive(!GameDefines.ifIAA);
            mIAAMoneyIcon.gameObject.SetActive(GameDefines.ifIAA);
        }

        protected override void OnSetParam(params object[] args)
        {
            curCompletedLevel = (int)args[0];
        }

        protected override void OnEnable()
        {
            InitShow();
            FacadeAudio.PlayEffect(EAudioType.ELevelComplete);

            if(GameDefines.ifIAA)
            {
                mAdBtn.gameObject.SetActive(true);
                mOnlyBtn.gameObject.SetActive(true);
                mWithdrawBtn.gameObject.SetActive(false);
            }
            else
            {
                bool ifWLv = ifWlevel();
                mAdBtn.gameObject.SetActive(!ifWLv);
                mOnlyBtn.gameObject.SetActive(!ifWLv);
                mWithdrawBtn.gameObject.SetActive(ifWLv);
                if (ifWLv)
                    mMoneyText.text = FacadePayType.RegionalChange(FacadePlayer.GetMoney());

                CheckGuide();
            }
        }

        private void InitShow()
        {
            curCompletedMoney = TBLevel.Get(curCompletedLevel).LevelReward;
            curOnlyMoney = curCompletedMoney / 10;
            mMoneyText.text = $"+{FacadePayType.RegionalChange(curCompletedMoney)}";
            mOnlyText.text = string.Format(FacadeLanguage.GetText("10012"), FacadePayType.RegionalChange(curOnlyMoney));
            //mDialog.gameObject.SetActive(true);
        }
        	    private void OnAdBtnClickHandle()        {            FacadeAd.PlayRewardAd(EAdSource.LevelCompleted, (amount) =>             {
                FacadePlayer.AddMoney(curCompletedMoney);                FacadeEffect.PlayGetRewardEffect(new ERewardItemStruct[]                {                     new ERewardItemStruct                    {                        Type = ERewardType.Money,                        Count = curCompletedMoney,                    }                }, null);                GoNextLevel();            }, null);        }        private void OnWithdrawBtnClickHandle()
        {
            GoNextLevel();
        }
        private void OnOnlyBtnClickHandle()
        {
            FacadePlayer.AddMoney(curOnlyMoney);
            GoNextLevel();
        }

        private void GoNextLevel()
        {
            UIManager.Instance.CloseUI(EUIType.EUILevelCompleted);
            FacadeGamePlay.CreateLevel();
        }

        private bool ifWlevel()
        {
            int[] Wlevels = GameDefines.WithdrawalLevels;
            for (int i = 0; i < Wlevels.Length; i++) 
            { 
                if (curCompletedLevel + 1 == Wlevels[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 临时的引导判断
        /// </summary>
        private void CheckGuide()
        {
            int wtype = ConfigModule.Instance.Tables.TBLevel.Get(curCompletedLevel).LevelType;
            
            if(wtype == 1)
            {
                FacadeGuide.SetIfTutorial(true);
            }
        }

        protected override void OnDisable() 
        {
        
        }
        protected override void OnDispose()
        {
            TBLevel = null;
        }
    }
}