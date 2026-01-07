using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UIConfirm : BaseUI
    {	protected RectTransform mPlane;	protected Button mExitBtn;	protected Image mCurPayIcon;	protected Text mInfoText;	protected Button mReEnterBtn;	protected Button mWNEnterBtn;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mPlane = mTransform.Find("Plane").GetComponent<RectTransform>();		mExitBtn = mTransform.Find("Plane/ExitBtn").GetComponent<Button>();		mCurPayIcon = mTransform.Find("Plane/Bg2/CurPayIcon").GetComponent<Image>();		mInfoText = mTransform.Find("Plane/InfoText").GetComponent<Text>();		mReEnterBtn = mTransform.Find("Plane/ReEnterBtn").GetComponent<Button>();		mWNEnterBtn = mTransform.Find("Plane/WNEnterBtn").GetComponent<Button>();
        }
    
        protected override void BindButtonEvent() 
        {
            		mExitBtn.onClick.AddListener( OnExitBtnClickHandle);		mReEnterBtn.onClick.AddListener( OnReEnterBtnClickHandle);		mWNEnterBtn.onClick.AddListener( OnWNEnterBtnClickHandle);
        }

        protected override void UnBindButtonEvent() 
        {
            		mExitBtn.onClick.RemoveAllListeners();		mReEnterBtn.onClick.RemoveAllListeners();		mWNEnterBtn.onClick.RemoveAllListeners();
        }
    
    }
}