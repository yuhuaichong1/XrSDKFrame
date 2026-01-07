using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UILuckyReward : BaseUI
    {	protected RectTransform mPlane;	protected Text mMoneyText;	protected Button mAdBtn;	protected Button mOnlyBtn;	protected Text mOnlyText;	protected RectTransform mMoneyIcon;	protected RectTransform mIAAMoneyIcon;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mPlane = mTransform.Find("Plane").GetComponent<RectTransform>();		mMoneyText = mTransform.Find("Plane/MoneyText").GetComponent<Text>();		mAdBtn = mTransform.Find("Plane/AdBtn").GetComponent<Button>();		mOnlyBtn = mTransform.Find("Plane/OnlyBtn").GetComponent<Button>();		mOnlyText = mTransform.Find("Plane/OnlyBtn/OnlyText").GetComponent<Text>();		mMoneyIcon = mTransform.Find("Plane/LightBg/MoneyIcon").GetComponent<RectTransform>();		mIAAMoneyIcon = mTransform.Find("Plane/LightBg/IAAMoneyIcon").GetComponent<RectTransform>();
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