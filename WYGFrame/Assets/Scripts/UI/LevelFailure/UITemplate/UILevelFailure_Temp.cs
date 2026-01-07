using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UILevelFailure : BaseUI
    {	protected RectTransform mPlane;	protected Image mPainting;	protected Slider mLevelProgress;	protected Text mProgressText;	protected Button mAdBtn;	protected Button mOnlyBtn;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mPlane = mTransform.Find("Plane").GetComponent<RectTransform>();		mPainting = mTransform.Find("Plane/LevelPainting/Painting").GetComponent<Image>();		mLevelProgress = mTransform.Find("Plane/LevelProgress").GetComponent<Slider>();		mProgressText = mTransform.Find("Plane/LevelProgress/ProgressText").GetComponent<Text>();		mAdBtn = mTransform.Find("Plane/AdBtn").GetComponent<Button>();		mOnlyBtn = mTransform.Find("Plane/OnlyBtn").GetComponent<Button>();
        }
    
        protected override void BindButtonEvent() 
        {
            		mAdBtn.onClick.AddListener( OnAdBtnClickHandle);		mOnlyBtn.onClick.AddListener( OnOnlyBtnClickHandle);
        }
    
        protected override void UnBindButtonEvent() 
        {
            		mAdBtn.onClick.RemoveAllListeners();		mOnlyBtn.onClick.RemoveAllListeners();
        }
    
    }
}