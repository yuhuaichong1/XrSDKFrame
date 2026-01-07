
using cfg;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{

    public partial class UIProp : BaseUI
    {
        private EFuncType eFuncType;
        private string titleName;
        private Sprite icon;
        private string desc;

        protected override void OnAwake()
        {

        }

        protected override void OnSetParam(params object[] args)
        {
            eFuncType = (EFuncType)args[0];
            ConfProp prop = ConfigModule.Instance.Tables.TBProp.GetOrDefault((int)eFuncType);
            titleName = FacadeLanguage.GetText(prop.Name);
            icon = ResourceMod.Instance.SyncLoad<Sprite>(prop.IconPath);
            desc = FacadeLanguage.GetText(prop.Desc);
        }



        protected override void OnEnable() 
        {
            InitShow();
        }

        private void InitShow()
        {
            mTitle.text = titleName;
            mPropIcon.sprite = icon;
            mPropDesc.text = desc;
            mLevelProgress.text = string.Format(FacadeLanguage.GetText?.Invoke("10065"), $"{(int)(FacadeGamePlay.GetCurLevelProgress?.Invoke() * 100)}%");

            ShowAnim(mPlane);
        }

        private void OnExitBtnClickHandle()        {            HideAnim(mPlane, () =>             {
                UIManager.Instance.CloseUI(EUIType.EUIProp);            });                    }	    private void OnAdBtnClickHandle()
        {
            FacadeAd.PlayRewardAd(EAdSource.Prop, GetProp, null);
        }

        /// <summary>
        /// 获取道具
        /// </summary>
        private void GetProp(int amount)
        {
            HideAnim(mPlane, () =>
            {
                UIManager.Instance.CloseUI(EUIType.EUIProp);                ERewardType type = ERewardType.Prop1;
                switch (eFuncType)
                {
                    case EFuncType.Prop1:
                        type = ERewardType.Prop1;
                        FacadePlayer.AddProp1Num(1);
                        break;
                    case EFuncType.Prop2:
                        type = ERewardType.Prop2;
                        FacadePlayer.AddProp2Num(1);
                        break;
                    case EFuncType.Prop3:
                        type = ERewardType.Prop3;
                        FacadePlayer.AddProp3Num(1);
                        break;
                }

                FacadeEffect.PlayGetRewardEffect(new ERewardItemStruct[]
                {
                    new ERewardItemStruct
                    {
                        Type = type,
                        Count = 1
                    }
                }, () =>                 {
                    //在UIEffect已有显示逻辑
                    //FacadeGamePlay.SetBATCountShow(FacadeUser.GetAddSpacePropNum());
                    //FacadeGamePlay.SetBCTCountShow(FacadeUser.GetClearPropNum());
                    //FacadeGamePlay.SetBRBCountShow(FacadeUser.GetHammerPropNum());
                });            });
            
        }

        protected override void OnDisable()
        { 
            
        }
        protected override void OnDispose()
        {
            
        }
    }
}