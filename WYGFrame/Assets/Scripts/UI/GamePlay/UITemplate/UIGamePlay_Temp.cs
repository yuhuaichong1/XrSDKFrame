using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UIGamePlay : BaseUI
    {	protected Button mSettingBtn;	protected Button mReStartBtn;	protected Button mBtn_Prop1;	protected RectTransform mProp1_count_icon;	protected Text mProp1_count_Text;	protected RectTransform mProp1_ad;	protected Button mBtn_Prop2;	protected RectTransform mProp2_count_icon;	protected Text mProp2_count_Text;	protected RectTransform mProp2_ad;	protected Button mBtn_Prop3;	protected RectTransform mProp3_count_icon;	protected Text mProp3_count_Text;	protected RectTransform mProp3_ad;	protected Text mCurMoneyText;	protected Button mCMBtn;	protected Text mCMDialogText;	protected RectTransform mMoneyIcon;	protected RectTransform mIAAMoneyIcon;	protected RectTransform mCMDialog;	protected Text mCurLevel;	protected Text mCurEnergyText;	protected Text mCurEnergyTimeText;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mSettingBtn = mTransform.Find("Plane/Top/SettingBtn").GetComponent<Button>();		mReStartBtn = mTransform.Find("Plane/Top/ReStartBtn").GetComponent<Button>();		mBtn_Prop1 = mTransform.Find("Plane/Bottom/Btns/Btn_Prop1").GetComponent<Button>();		mProp1_count_icon = mTransform.Find("Plane/Bottom/Btns/Btn_Prop1/prop1_count_icon").GetComponent<RectTransform>();		mProp1_count_Text = mTransform.Find("Plane/Bottom/Btns/Btn_Prop1/prop1_count_icon/prop1_count_Text").GetComponent<Text>();		mProp1_ad = mTransform.Find("Plane/Bottom/Btns/Btn_Prop1/prop1_ad").GetComponent<RectTransform>();		mBtn_Prop2 = mTransform.Find("Plane/Bottom/Btns/Btn_Prop2").GetComponent<Button>();		mProp2_count_icon = mTransform.Find("Plane/Bottom/Btns/Btn_Prop2/prop2_count_icon").GetComponent<RectTransform>();		mProp2_count_Text = mTransform.Find("Plane/Bottom/Btns/Btn_Prop2/prop2_count_icon/prop2_count_Text").GetComponent<Text>();		mProp2_ad = mTransform.Find("Plane/Bottom/Btns/Btn_Prop2/prop2_ad").GetComponent<RectTransform>();		mBtn_Prop3 = mTransform.Find("Plane/Bottom/Btns/Btn_Prop3").GetComponent<Button>();		mProp3_count_icon = mTransform.Find("Plane/Bottom/Btns/Btn_Prop3/prop3_count_icon").GetComponent<RectTransform>();		mProp3_count_Text = mTransform.Find("Plane/Bottom/Btns/Btn_Prop3/prop3_count_icon/prop3_count_Text").GetComponent<Text>();		mProp3_ad = mTransform.Find("Plane/Bottom/Btns/Btn_Prop3/prop3_ad").GetComponent<RectTransform>();		mCurMoneyText = mTransform.Find("Plane/Top/CurMoney/Bg1/CurMoneyText").GetComponent<Text>();		mCMBtn = mTransform.Find("Plane/Top/CurMoney/CMBtn").GetComponent<Button>();		mCMDialogText = mTransform.Find("Plane/Top/CurMoney/CMDialog/CMDialogText").GetComponent<Text>();		mMoneyIcon = mTransform.Find("Plane/Top/CurMoney/Bg1/MoneyIcon/MoneyIcon").GetComponent<RectTransform>();		mIAAMoneyIcon = mTransform.Find("Plane/Top/CurMoney/Bg1/MoneyIcon/IAAMoneyIcon").GetComponent<RectTransform>();		mCMDialog = mTransform.Find("Plane/Top/CurMoney/CMDialog").GetComponent<RectTransform>();		mCurLevel = mTransform.Find("Plane/Top/CurLevel").GetComponent<Text>();		mCurEnergyText = mTransform.Find("Plane/Top/CurEnergy/Icon/CurEnergyText").GetComponent<Text>();		mCurEnergyTimeText = mTransform.Find("Plane/Top/CurEnergy/CurEnergyTimeText").GetComponent<Text>();
        }
    
        protected override void BindButtonEvent() 
        {
            		mSettingBtn.onClick.AddListener( OnSettingBtnClickHandle);		mReStartBtn.onClick.AddListener( OnReStartBtnClickHandle);		mBtn_Prop1.onClick.AddListener( OnBtn_Prop1ClickHandle);		mBtn_Prop2.onClick.AddListener( OnBtn_Prop2ClickHandle);		mBtn_Prop3.onClick.AddListener( OnBtn_Prop3ClickHandle);		mCMBtn.onClick.AddListener( OnCMBtnClickHandle);
        }
    
        protected override void UnBindButtonEvent() 
        {
            		mSettingBtn.onClick.RemoveAllListeners();		mReStartBtn.onClick.RemoveAllListeners();		mBtn_Prop1.onClick.RemoveAllListeners();		mBtn_Prop2.onClick.RemoveAllListeners();		mBtn_Prop3.onClick.RemoveAllListeners();		mCMBtn.onClick.RemoveAllListeners();
        }
    
    }
}