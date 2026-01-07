
using System;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{

    public partial class UIFeedback : BaseUI
    {
        protected override void OnAwake() 
        {
            mAddressOrPhoneInput.placeholder.GetComponent<Text>().text = FacadeLanguage.GetText("10030");
        }
        protected override void OnEnable()
        {
            ShowAnim(mPlane);
        }
        	    private void OnExitBtnClickHandle()        {            HideAnim(mPlane, () =>             {                 UIManager.Instance.CloseUI(EUIType.EUIFeedback);            });        }	    private void OnConfirmBtnClickHandle()
        {
            if(string.IsNullOrEmpty(mAddressOrPhoneInput.text))
            {
                UIManager.Instance.OpenNotice(FacadeLanguage.GetText("10030"));
            }
            else
            {
                HideAnim(mPlane, () =>
                {
                    UIManager.Instance.CloseUI(EUIType.EUIFeedback);
                    UIManager.Instance.CloseUI(EUIType.EUIEnterInfomation);

                    FacadeWithdrawal.SetWName("");
                    FacadeWithdrawal.SetWPhoneOrEmail(mAddressOrPhoneInput.text);
                    FacadeWithdrawal.SetPayType(EPayType.Other);

                    UIManager.Instance.OpenAsync<UIConfirm>(EUIType.EUIConfirm);
                });
            }
        }
        protected override void OnDisable() { }
        protected override void OnDispose() { }
    }
}