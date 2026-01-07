using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UIEffect : BaseUI
    {	protected RectTransform mLTEMask;	protected RectTransform mLTEPlane;	protected RectTransform mLTEStartPoint;	protected RectTransform mLevelTargetEffect;	protected Text mNextTargetText;	protected RectTransform mMiniDialog;	protected Text mMiniTargetText;	protected Text mDialogText;	protected RectTransform mGREMask;	protected RectTransform mGetRewardEffect;	protected RectTransform mRewardGroup;	protected EffectRewardItem mEffectRewardItem;	protected RectTransform mCongratulationEffect;	protected Text mCEContent;	protected RectTransform mFlyEffectParent;	protected RectTransform mFlyProp;	protected RectTransform mFlyMoney;	protected RectTransform mFlyIAAMoney;	protected RectTransform mFlyMoneyTip;	protected RectTransform mFlyIAAMoneyTip;	protected SkeletonGraphic mShaobaEffect;	protected SkeletonGraphic mHammerEffect;	protected RectTransform mClickEffectParent;	protected SkeletonGraphic mClickEffect;	protected SkeletonGraphic mDifficultyUpEffect;	protected LanguageText mDUEText;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mLTEMask = mTransform.Find("LevelTargetEffect/LTEMask").GetComponent<RectTransform>();		mLTEPlane = mTransform.Find("LevelTargetEffect/LTEPlane").GetComponent<RectTransform>();		mLTEStartPoint = mTransform.Find("LevelTargetEffect/LTEStartPoint").GetComponent<RectTransform>();		mLevelTargetEffect = mTransform.Find("LevelTargetEffect").GetComponent<RectTransform>();		mNextTargetText = mTransform.Find("LevelTargetEffect/LTEPlane/NextTargetText").GetComponent<Text>();		mMiniDialog = mTransform.Find("LevelTargetEffect/LTEPlane/MiniDialog").GetComponent<RectTransform>();		mMiniTargetText = mTransform.Find("LevelTargetEffect/LTEPlane/MiniDialog/MiniTargetText").GetComponent<Text>();		mDialogText = mTransform.Find("LevelTargetEffect/LTEPlane/Dialog/DialogText").GetComponent<Text>();		mGREMask = mTransform.Find("GetRewardEffect/GREMask").GetComponent<RectTransform>();		mGetRewardEffect = mTransform.Find("GetRewardEffect").GetComponent<RectTransform>();		mRewardGroup = mTransform.Find("GetRewardEffect/Plane/RewardGroup").GetComponent<RectTransform>();		mEffectRewardItem = mTransform.Find("GetRewardEffect/Plane/RewardGroup/EffectRewardItem").GetComponent<EffectRewardItem>();		mCongratulationEffect = mTransform.Find("CongratulationEffect").GetComponent<RectTransform>();		mCEContent = mTransform.Find("CongratulationEffect/CEContent").GetComponent<Text>();		mFlyEffectParent = mTransform.Find("FlyEffectParent").GetComponent<RectTransform>();		mFlyProp = mTransform.Find("FlyEffectParent/FlyProp").GetComponent<RectTransform>();		mFlyMoney = mTransform.Find("FlyEffectParent/FlyMoney").GetComponent<RectTransform>();		mFlyIAAMoney = mTransform.Find("FlyEffectParent/FlyIAAMoney").GetComponent<RectTransform>();		mFlyMoneyTip = mTransform.Find("FlyEffectParent/FlyMoneyTip").GetComponent<RectTransform>();		mFlyIAAMoneyTip = mTransform.Find("FlyEffectParent/FlyIAAMoneyTip").GetComponent<RectTransform>();		mShaobaEffect = mTransform.Find("ShaobaEffect").GetComponent<SkeletonGraphic>();		mHammerEffect = mTransform.Find("HammerEffect").GetComponent<SkeletonGraphic>();		mClickEffectParent = mTransform.Find("ClickEffectParent").GetComponent<RectTransform>();		mClickEffect = mTransform.Find("ClickEffectParent/ClickEffect").GetComponent<SkeletonGraphic>();		mDifficultyUpEffect = mTransform.Find("DifficultyUpEffect").GetComponent<SkeletonGraphic>();		mDUEText = mTransform.Find("DifficultyUpEffect/DUEText").GetComponent<LanguageText>();
        }
    
        protected override void BindButtonEvent() 
        {
            
        }
    
        protected override void UnBindButtonEvent() 
        {
            
        }
    
    }
}