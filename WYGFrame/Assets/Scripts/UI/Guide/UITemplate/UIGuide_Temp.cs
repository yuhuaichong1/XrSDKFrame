using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UIGuide : BaseUI
    {	protected RectTransform mGuidePlane;	protected CanvasGroup mHoleMask;	protected RectTransform mHander;	protected AutoHandSwing mHand;	protected RectTransform mGuideTextBg;	protected Text mGuideText;	protected RectTransform mHole;	protected Button mGuideBtn;	protected RectTransform mObjTrans;	protected SGuidePenetrate mMask;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mGuidePlane = mTransform.Find("GuidePlane").GetComponent<RectTransform>();		mHoleMask = mTransform.Find("GuidePlane/HoleMask/HoleMask").GetComponent<CanvasGroup>();		mHander = mTransform.Find("GuidePlane/Hander").GetComponent<RectTransform>();		mHand = mTransform.Find("GuidePlane/Hander/Hand").GetComponent<AutoHandSwing>();		mGuideTextBg = mTransform.Find("GuidePlane/GuideTextBg").GetComponent<RectTransform>();		mGuideText = mTransform.Find("GuidePlane/GuideTextBg/GuideText").GetComponent<Text>();		mHole = mTransform.Find("GuidePlane/HoleMask/HoleMask/Hole").GetComponent<RectTransform>();		mGuideBtn = mTransform.Find("GuidePlane/GuideBtn").GetComponent<Button>();		mObjTrans = mTransform.Find("GuidePlane/ObjTrans").GetComponent<RectTransform>();		mMask = mTransform.Find("GuidePlane/HoleMask/HoleMask/Mask").GetComponent<SGuidePenetrate>();
        }
    
        protected override void BindButtonEvent() 
        {
            		mGuideBtn.onClick.AddListener( OnGuideBtnClickHandle);
        }
    
        protected override void UnBindButtonEvent() 
        {
            		mGuideBtn.onClick.RemoveAllListeners();
        }
    
    }
}