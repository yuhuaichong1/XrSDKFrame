
using System;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{

    public partial class UILuckyReward : BaseUI
    {
        private float rewardMoeny;
        protected override void OnAwake() 
        { 
            mMoneyIcon.gameObject.SetActive(!GameDefines.ifIAA);
            mIAAMoneyIcon.gameObject.SetActive(GameDefines.ifIAA);
        }
        protected override void OnEnable()
        {
            SetRewardMoeny();
            mMoneyText.text = $"+{FacadePayType.RegionalChange(rewardMoeny)}";
            mOnlyText.text = string.Format(FacadeLanguage.GetText("10012"), FacadePayType.RegionalChange(rewardMoeny / 10));

            ShowAnim(mPlane);  
        }
                /// <summary>
        /// 设置奖励金额
        /// </summary>        private void SetRewardMoeny()
        {
            rewardMoeny = UnityEngine.Random.Range(GameDefines.LuckyReward_RandomRange.x, GameDefines.LuckyReward_RandomRange.y);
        }	    private void OnAdBtnClickHandle()        {            FacadeAd.PlayRewardAd(EAdSource.LuckyReward, (amount) =>             {
                HideAnim(mPlane, () =>                 {
                    AddReward(rewardMoeny);

                    UIManager.Instance.CloseUI(EUIType.EUILuckyReward);                });            }, null);        }	    private void OnOnlyBtnClickHandle()
        {
            HideAnim(mPlane, () =>
            {
                AddReward(rewardMoeny / 10);
                UIManager.Instance.CloseUI(EUIType.EUILuckyReward);
            });
        }

        /// <summary>
        /// 添加钱特效相关
        /// </summary>
        /// <param name="amount">数量</param>
        private void AddReward(float amount)
        {
            FacadePlayer.AddMoney(amount);
            FacadeEffect.PlayGetRewardEffect(new ERewardItemStruct[]
            {
                        new ERewardItemStruct()
                        {
                            Type = ERewardType.Money,
                            Count = amount,
                        }
            }, null);
        }

        protected override void OnDisable() 
        {
        
        }
        protected override void OnDispose()
        {
        
        }
    }
}