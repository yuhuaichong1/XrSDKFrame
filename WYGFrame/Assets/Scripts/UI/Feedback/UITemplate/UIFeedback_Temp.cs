using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UIFeedback : BaseUI
    {	protected RectTransform mPlane;	protected Button mExitBtn;	protected Button mConfirmBtn;	protected InputField mAddressOrPhoneInput;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mPlane = mTransform.Find("Plane").GetComponent<RectTransform>();		mExitBtn = mTransform.Find("Plane/ExitBtn").GetComponent<Button>();		mConfirmBtn = mTransform.Find("Plane/ConfirmBtn").GetComponent<Button>();		mAddressOrPhoneInput = mTransform.Find("Plane/AddressOrPhoneInput").GetComponent<InputField>();
        }
    
        protected override void BindButtonEvent() 
        {
            		mExitBtn.onClick.AddListener( OnExitBtnClickHandle);		mConfirmBtn.onClick.AddListener( OnConfirmBtnClickHandle);
        }
    
        protected override void UnBindButtonEvent() 
        {
            		mExitBtn.onClick.RemoveAllListeners();		mConfirmBtn.onClick.RemoveAllListeners();
        }
    
    }
}