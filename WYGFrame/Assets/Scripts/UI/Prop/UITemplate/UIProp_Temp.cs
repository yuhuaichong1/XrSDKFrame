using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UIProp : BaseUI
    {	protected RectTransform mPlane;	protected Button mExitBtn;	protected Text mTitle;	protected Image mPropIcon;	protected Text mLevelProgress;	protected Text mPropDesc;	protected Button mAdBtn;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mPlane = mTransform.Find("Plane").GetComponent<RectTransform>();		mExitBtn = mTransform.Find("Plane/ExitBtn").GetComponent<Button>();		mTitle = mTransform.Find("Plane/Title").GetComponent<Text>();		mPropIcon = mTransform.Find("Plane/PropIcon").GetComponent<Image>();		mLevelProgress = mTransform.Find("Plane/LevelProgress").GetComponent<Text>();		mPropDesc = mTransform.Find("Plane/PropDesc").GetComponent<Text>();		mAdBtn = mTransform.Find("Plane/AdBtn").GetComponent<Button>();
        }
    
        protected override void BindButtonEvent() 
        {
            		mExitBtn.onClick.AddListener( OnExitBtnClickHandle);		mAdBtn.onClick.AddListener( OnAdBtnClickHandle);
        }
    
        protected override void UnBindButtonEvent() 
        {
            		mExitBtn.onClick.RemoveAllListeners();		mAdBtn.onClick.RemoveAllListeners();
        }
    
    }
}