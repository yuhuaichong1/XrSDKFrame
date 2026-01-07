
using System;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{

    public partial class UIReStart : BaseUI
    {
        protected override void OnAwake() { }
        protected override void OnEnable()
        {
            ShowAnim(mPlane);
        }
        	    private void OnExitBtnClickHandle()        {            HideAnim(mPlane, () =>             {
                UIManager.Instance.CloseUI(EUIType.EUIReStart);            });        }	    private void OnReStartBtnClickHandle()
        {
            HideAnim(mPlane, () =>
            {
                UIManager.Instance.CloseUI(EUIType.EUIReStart);                FacadeGamePlay.RePlay?.Invoke();            });
        }
        protected override void OnDisable() { }
        protected override void OnDispose() { }
    }
}