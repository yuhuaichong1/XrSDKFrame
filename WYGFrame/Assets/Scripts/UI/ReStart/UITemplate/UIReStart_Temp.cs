using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UIReStart : BaseUI
    {	protected RectTransform mPlane;	protected Button mExitBtn;	protected Button mReStartBtn;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mPlane = mTransform.Find("Plane").GetComponent<RectTransform>();		mExitBtn = mTransform.Find("Plane/ExitBtn").GetComponent<Button>();		mReStartBtn = mTransform.Find("Plane/ReStartBtn").GetComponent<Button>();
        }
    
        protected override void BindButtonEvent() 
        {
            		mExitBtn.onClick.AddListener( OnExitBtnClickHandle);		mReStartBtn.onClick.AddListener( OnReStartBtnClickHandle);
        }
    
        protected override void UnBindButtonEvent() 
        {
            		mExitBtn.onClick.RemoveAllListeners();		mReStartBtn.onClick.RemoveAllListeners();
        }
    
    }
}