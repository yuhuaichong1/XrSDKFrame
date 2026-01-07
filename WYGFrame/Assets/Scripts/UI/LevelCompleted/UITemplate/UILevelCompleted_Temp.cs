using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UILevelCompleted : BaseUI
    {	protected RectTransform mPlane;	protected Image mPainting;	protected RectTransform mDialog;	protected Text mMoneyText;	protected Button mAdBtn;	protected Button mWithdrawBtn;	protected Button mOnlyBtn;	protected Text mOnlyText;	protected RectTransform mMoneyIcon;	protected RectTransform mIAAMoneyIcon;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mPlane = mTransform.Find("Plane").GetComponent<RectTransform>();		mPainting = mTransform.Find("Plane/LevelPainting/Painting").GetComponent<Image>();		mDialog = mTransform.Find("Plane/LevelPainting/Dialog").GetComponent<RectTransform>();		mMoneyText = mTransform.Find("Plane/MoneyText").GetComponent<Text>();		mAdBtn = mTransform.Find("Plane/AdBtn").GetComponent<Button>();		mWithdrawBtn = mTransform.Find("Plane/WithdrawBtn").GetComponent<Button>();		mOnlyBtn = mTransform.Find("Plane/OnlyBtn").GetComponent<Button>();		mOnlyText = mTransform.Find("Plane/OnlyBtn/OnlyText").GetComponent<Text>();		mMoneyIcon = mTransform.Find("Plane/MoneyText/MoneyIcon").GetComponent<RectTransform>();		mIAAMoneyIcon = mTransform.Find("Plane/MoneyText/IAAMoneyIcon").GetComponent<RectTransform>();
        }
    
        protected override void BindButtonEvent() 
        {
            		mAdBtn.onClick.AddListener( OnAdBtnClickHandle);		mWithdrawBtn.onClick.AddListener( OnWithdrawBtnClickHandle);		mOnlyBtn.onClick.AddListener( OnOnlyBtnClickHandle);
        }
    
        protected override void UnBindButtonEvent() 
        {
            		mAdBtn.onClick.RemoveAllListeners();		mWithdrawBtn.onClick.RemoveAllListeners();		mOnlyBtn.onClick.RemoveAllListeners();
        }
    
    }
}