using SuperScrollView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UIWithdrawalRecords : BaseUI
    {	protected RectTransform mPlane;	protected Button mExitBtn;	protected Text mCurMoneyText;	protected LoopGridView mOrderScrollView;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mPlane = mTransform.Find("Plane").GetComponent<RectTransform>();		mExitBtn = mTransform.Find("Plane/ExitBtn").GetComponent<Button>();		mCurMoneyText = mTransform.Find("Plane/CurMoneyBg/CurMoneyText").GetComponent<Text>();		mOrderScrollView = mTransform.Find("Plane/OrderScrollView").GetComponent<LoopGridView>();
        }
    
        protected override void BindButtonEvent() 
        {
            		    mExitBtn.onClick.AddListener( OnExitBtnClickHandle);
        }
    
        protected override void UnBindButtonEvent() 
        {
            		mExitBtn.onClick.RemoveAllListeners();
        }
    
    }
}