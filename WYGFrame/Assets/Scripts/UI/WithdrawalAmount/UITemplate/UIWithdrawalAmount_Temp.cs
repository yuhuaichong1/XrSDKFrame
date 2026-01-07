using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UIWithdrawalAmount : BaseUI
    {	protected RectTransform mPlane;	protected Button mExitBtn;	protected Text mCurMoneyText;	protected Button mConfirmBtn;	protected Button mKeepEarnBtn;	protected RectTransform mProgress1;	protected RectTransform mP1Icon1;	protected RectTransform mP1Icon2;	protected RectTransform mP1Line;	protected Text mP1Date;	protected RectTransform mProgress2;	protected RectTransform mP2Icon1;	protected RectTransform mP2Icon2;	protected RectTransform mP2Line;	protected Text mP2Date;	protected RectTransform mProgress3;	protected RectTransform mP3Icon1;	protected RectTransform mP3Icon2;	protected Text mP3Date;	protected Text mP3Content;	protected RectTransform mErrContent;	protected Text mErrorMsg;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mPlane = mTransform.Find("Plane").GetComponent<RectTransform>();		mExitBtn = mTransform.Find("Plane/ExitBtn").GetComponent<Button>();		mCurMoneyText = mTransform.Find("Plane/CurMoneyBg/CurMoneyText").GetComponent<Text>();		mConfirmBtn = mTransform.Find("Plane/ConfirmBtn").GetComponent<Button>();		mKeepEarnBtn = mTransform.Find("Plane/KeepEarnBtn").GetComponent<Button>();		mProgress1 = mTransform.Find("Plane/OrderDetials/Progress/Progress1").GetComponent<RectTransform>();		mP1Icon1 = mTransform.Find("Plane/OrderDetials/Progress/Progress1/P1Icon/P1Icon1").GetComponent<RectTransform>();		mP1Icon2 = mTransform.Find("Plane/OrderDetials/Progress/Progress1/P1Icon/P1Icon2").GetComponent<RectTransform>();		mP1Line = mTransform.Find("Plane/OrderDetials/Progress/Progress1/P1Line").GetComponent<RectTransform>();		mP1Date = mTransform.Find("Plane/OrderDetials/Progress/Progress1/P1Content/P1Date").GetComponent<Text>();		mProgress2 = mTransform.Find("Plane/OrderDetials/Progress/Progress2").GetComponent<RectTransform>();		mP2Icon1 = mTransform.Find("Plane/OrderDetials/Progress/Progress2/P2Icon/P2Icon1").GetComponent<RectTransform>();		mP2Icon2 = mTransform.Find("Plane/OrderDetials/Progress/Progress2/P2Icon/P2Icon2").GetComponent<RectTransform>();		mP2Line = mTransform.Find("Plane/OrderDetials/Progress/Progress2/P2Line").GetComponent<RectTransform>();		mP2Date = mTransform.Find("Plane/OrderDetials/Progress/Progress2/P2Content/P2Date").GetComponent<Text>();		mProgress3 = mTransform.Find("Plane/OrderDetials/Progress/Progress3").GetComponent<RectTransform>();		mP3Icon1 = mTransform.Find("Plane/OrderDetials/Progress/Progress3/P3Icon/P3Icon1").GetComponent<RectTransform>();		mP3Icon2 = mTransform.Find("Plane/OrderDetials/Progress/Progress3/P3Icon/P3Icon2").GetComponent<RectTransform>();		mP3Date = mTransform.Find("Plane/OrderDetials/Progress/Progress3/P3Content/P3Date").GetComponent<Text>();		mP3Content = mTransform.Find("Plane/OrderDetials/Progress/Progress3/P3Content").GetComponent<Text>();		mErrContent = mTransform.Find("Plane/OrderDetials/Progress/Progress3/P3Content/ErrContent").GetComponent<RectTransform>();		mErrorMsg = mTransform.Find("Plane/OrderDetials/Progress/Progress3/P3Content/ErrContent/ErrorMsg").GetComponent<Text>();
        }
    
        protected override void BindButtonEvent() 
        {
            		mExitBtn.onClick.AddListener( OnExitBtnClickHandle);		mConfirmBtn.onClick.AddListener( OnConfirmBtnClickHandle);		mKeepEarnBtn.onClick.AddListener( OnKeepEarnBtnClickHandle);
        }
    
        protected override void UnBindButtonEvent() 
        {
            		mExitBtn.onClick.RemoveAllListeners();		mConfirmBtn.onClick.RemoveAllListeners();		mKeepEarnBtn.onClick.RemoveAllListeners();
        }
    
    }
}