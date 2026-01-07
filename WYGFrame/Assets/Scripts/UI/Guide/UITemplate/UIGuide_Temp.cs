using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UIGuide : BaseUI
    {	protected RectTransform mGuidePlane;	protected CanvasGroup mHoleMask;	protected RectTransform mHoleFather;	protected RectTransform mHole;	protected SGuidePenetrate mMask;	protected RectTransform mHandFather;	protected RectTransform mHander;	protected RectTransform mGuideTextFather;	protected RectTransform mGuideText;	protected Text mGTContent;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mGuidePlane = mTransform.Find("GuidePlane").GetComponent<RectTransform>();		mHoleMask = mTransform.Find("GuidePlane/HoleMask").GetComponent<CanvasGroup>();		mHoleFather = mTransform.Find("GuidePlane/HoleMask/HoleFather").GetComponent<RectTransform>();		mHole = mTransform.Find("GuidePlane/HoleMask/HoleFather/Hole").GetComponent<RectTransform>();		mMask = mTransform.Find("GuidePlane/HoleMask/Mask").GetComponent<SGuidePenetrate>();		mHandFather = mTransform.Find("GuidePlane/HandFather").GetComponent<RectTransform>();		mHander = mTransform.Find("GuidePlane/HandFather/Hander").GetComponent<RectTransform>();		mGuideTextFather = mTransform.Find("GuidePlane/GuideTextFather").GetComponent<RectTransform>();		mGuideText = mTransform.Find("GuidePlane/GuideTextFather/GuideText").GetComponent<RectTransform>();		mGTContent = mTransform.Find("GuidePlane/GuideTextFather/GuideText/GTContent").GetComponent<Text>();
        }
    
        protected override void BindButtonEvent() 
        {
            
        }
    
        protected override void UnBindButtonEvent() 
        {
            
        }
    
    }
}