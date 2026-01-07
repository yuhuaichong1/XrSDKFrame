using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UIEnterInfomation : BaseUI
    {	protected Button mExitBtn;	protected Button mHelpBtn;	protected Toggle mPayType1Toggle;	protected Image mPY1Icon;	protected Toggle mPayType2Toggle;	protected Image mPY2Icon;	protected Toggle mPayType3Toggle;	protected Image mPY3Icon;	protected InputField mNameInput;	protected InputField mAddressInput;	protected InputField mAddressOrPhoneInput;	protected RectTransform mPhone;	protected Text mAreaCodeText;	protected InputField mPhoneInput;	protected Button mConfirmBtn;	protected RectTransform mPlane;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mExitBtn = mTransform.Find("Plane/ExitBtn").GetComponent<Button>();		mHelpBtn = mTransform.Find("Plane/Payment/HelpBtn").GetComponent<Button>();		mPayType1Toggle = mTransform.Find("Plane/Payment/ToggleGroup/PayType1Toggle").GetComponent<Toggle>();		mPY1Icon = mTransform.Find("Plane/Payment/ToggleGroup/PayType1Toggle/PY1Icon").GetComponent<Image>();		mPayType2Toggle = mTransform.Find("Plane/Payment/ToggleGroup/PayType2Toggle").GetComponent<Toggle>();		mPY2Icon = mTransform.Find("Plane/Payment/ToggleGroup/PayType2Toggle/PY2Icon").GetComponent<Image>();		mPayType3Toggle = mTransform.Find("Plane/Payment/ToggleGroup/PayType3Toggle").GetComponent<Toggle>();		mPY3Icon = mTransform.Find("Plane/Payment/ToggleGroup/PayType3Toggle/PY3Icon").GetComponent<Image>();		mNameInput = mTransform.Find("Plane/NameInput").GetComponent<InputField>();		mAddressInput = mTransform.Find("Plane/AddressInput").GetComponent<InputField>();		mAddressOrPhoneInput = mTransform.Find("Plane/AddressOrPhoneInput").GetComponent<InputField>();		mPhone = mTransform.Find("Plane/Phone").GetComponent<RectTransform>();		mAreaCodeText = mTransform.Find("Plane/Phone/PhoneAreaCode/AreaCodeText").GetComponent<Text>();		mPhoneInput = mTransform.Find("Plane/Phone/PhoneInput").GetComponent<InputField>();		mConfirmBtn = mTransform.Find("Plane/ConfirmBtn").GetComponent<Button>();		mPlane = mTransform.Find("Plane").GetComponent<RectTransform>();
        }
    
        protected override void BindButtonEvent() 
        {
            		mExitBtn.onClick.AddListener( OnExitBtnClickHandle);		mHelpBtn.onClick.AddListener( OnHelpBtnClickHandle);		mConfirmBtn.onClick.AddListener( OnConfirmBtnClickHandle);
			mPayType1Toggle.onValueChanged.AddListener(OnPayType1ToggleValueChanged);
            mPayType2Toggle.onValueChanged.AddListener(OnPayType2ToggleValueChanged);
            mPayType3Toggle.onValueChanged.AddListener(OnPayType3ToggleValueChanged);
        }

        protected override void UnBindButtonEvent() 
        {
            		mExitBtn.onClick.RemoveAllListeners();		mHelpBtn.onClick.RemoveAllListeners();		mConfirmBtn.onClick.RemoveAllListeners();
        }
    
    }
}