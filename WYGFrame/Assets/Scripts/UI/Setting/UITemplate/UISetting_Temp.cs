using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UISetting : BaseUI
    {	protected RectTransform mPlane;	protected Button mExitBtn;	protected Image mPlayerIcon;	protected Text mUserNameText;	protected Text mUserIDText;	protected Text mUserLv;	protected Button mUserLevelBtn;	protected Toggle mM_Toggle;	protected LanguageText mMON;	protected LanguageText mMOFF;	protected Toggle mS_Toggle;	protected Toggle mV_Toggle;	protected Button mWRBtn;	protected RectTransform mS_Icon;	protected RectTransform mV_Icon;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mPlane = mTransform.Find("Plane").GetComponent<RectTransform>();		mExitBtn = mTransform.Find("Plane/ExitBtn").GetComponent<Button>();		mPlayerIcon = mTransform.Find("Plane/Bg2/PlayerIcon").GetComponent<Image>();		mUserNameText = mTransform.Find("Plane/Bg2/UserNameText").GetComponent<Text>();		mUserIDText = mTransform.Find("Plane/Bg2/UserIDText").GetComponent<Text>();		mUserLv = mTransform.Find("Plane/Bg2/UserLv").GetComponent<Text>();		mUserLevelBtn = mTransform.Find("Plane/Bg2/UserLevelBtn").GetComponent<Button>();		mM_Toggle = mTransform.Find("Plane/Muslc/m_Toggle").GetComponent<Toggle>();		mMON = mTransform.Find("Plane/Muslc/m_Toggle/mON").GetComponent<LanguageText>();		mMOFF = mTransform.Find("Plane/Muslc/m_Toggle/mOFF").GetComponent<LanguageText>();		mS_Toggle = mTransform.Find("Plane/Sound/s_Toggle").GetComponent<Toggle>();		mV_Toggle = mTransform.Find("Plane/Vibration/v_Toggle").GetComponent<Toggle>();		mWRBtn = mTransform.Find("Plane/WRButton/WRBtn").GetComponent<Button>();		mS_Icon = mTransform.Find("Plane/Sound/s_Toggle/s_Icon").GetComponent<RectTransform>();		mV_Icon = mTransform.Find("Plane/Vibration/v_Toggle/v_Icon").GetComponent<RectTransform>();
        }
    
        protected override void BindButtonEvent() 
        {
            		mExitBtn.onClick.AddListener( OnExitBtnClickHandle);		mUserLevelBtn.onClick.AddListener( OnUserLevelBtnClickHandle);		mWRBtn.onClick.AddListener( OnWRBtnClickHandle);
			mS_Toggle.onValueChanged.AddListener(OnS_ToggleValueChange);
			mV_Toggle.onValueChanged.AddListener(OnV_ToggleValueChange);
        }
    
        protected override void UnBindButtonEvent() 
        {
            		mExitBtn.onClick.RemoveAllListeners();		mUserLevelBtn.onClick.RemoveAllListeners();		mWRBtn.onClick.RemoveAllListeners();
        }
    
    }
}