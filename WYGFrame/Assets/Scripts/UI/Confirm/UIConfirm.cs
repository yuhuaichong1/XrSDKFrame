
using cfg;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{

    public partial class UIConfirm : BaseUI
    {
        private TBPayChannel PayChannelTable;
        protected override void OnAwake()
        {
            PayChannelTable = ConfigModule.Instance.Tables.TBPayChannel;
        }

        protected override void OnEnable()
        {
            string name = FacadeWithdrawal.GetWName();
            string msg = FacadeWithdrawal.GetWPhoneOrEmail();
            Sprite icon = ResourceMod.Instance.SyncLoad<Sprite>(PayChannelTable.Get((int)(FacadeWithdrawal.GetPayType?.Invoke())).PicPath);

            mCurPayIcon.sprite = icon;
            mInfoText.text = name != "" ? $"{name}\n{msg}" : $"{msg}";
        }

        private void OnExitBtnClickHandle()
        {
            HideAnim(mPlane, () => 
            { 
                UIManager.Instance.CloseUI(EUIType.EUIConfirm);
            });
        }

        private void OnReEnterBtnClickHandle()
        {
            HideAnim(mPlane, () =>
            {
                UIManager.Instance.CloseUI(EUIType.EUIConfirm);
                UIManager.Instance.OpenAsync<UIEnterInfomation>(EUIType.EUIEnterInfomation);
            });
        }

        private void OnWNEnterBtnClickHandle()
        {
            HideAnim(mPlane, () =>
            {
                UIManager.Instance.CloseUI(EUIType.EUIConfirm);

                //UIManager.Instance.OpenAsync<UIWithdrawalAmount>(EUIType.EUIWithdrawalAmount, null, target);
            });
        }

        protected override void OnDisable() { }
        protected override void OnDispose() 
        {
            PayChannelTable = null;
        }
    }
}