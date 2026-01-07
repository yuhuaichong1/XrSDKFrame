
using System;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{

    public partial class UISetting : BaseUI
    {
        protected override void OnAwake()
        {
            
        }
        protected override void OnEnable()
        {
            bool sT = FacadeAudio.GetEffectsVolume() == 1;
            bool vT = FacadeAudio.GetVibrate();
            mS_Toggle.isOn = sT;
            mV_Toggle.isOn = vT;
            mS_Icon.localPosition = new Vector3(88 * (sT ? 1 : -1), 36, 0);
            mV_Icon.localPosition = new Vector3(88 * (vT ? 1 : -1), 36, 0);

            mUserNameText.text = $"{FacadePlayer.GetPlayerName()}";
            mUserIDText.text = $"{FacadeLanguage.GetText("10009")}:{FacadePlayer.GetPlayerID().Substring(0, 13)}..."; 
            mUserLv.text = $"{FacadeLanguage.GetText("10010")}.{FacadePlayer.GetPlayerLevel() + 1}";

            ShowAnim(mPlane);
            //MT_Show(FacadeAudio.GetMusicVolume() == 1);
        }

        private void OnExitBtnClickHandle()
        {
            HideAnim(mPlane, () => {
                UIManager.Instance.CloseUI(EUIType.EUISetting);
            });
        }

        private void OnUserLevelBtnClickHandle()
        {
            HideAnim(mPlane, () => {
                UIManager.Instance.CloseUI(EUIType.EUISetting);
                UIManager.Instance.OpenAsync<UIUserLevel>(EUIType.EUIUserLevel);
            });
        }

        private void OnWRBtnClickHandle()
        {
            HideAnim(mPlane, () => {
                UIManager.Instance.CloseUI(EUIType.EUISetting);
                UIManager.Instance.OpenAsync<UIWithdrawalRecords>(EUIType.EUIWithdrawalRecords);
            });
        }

        #region 控制（背景）音乐，目前不用（并入Sound）
        //private void OnM_ToggleValueChange(bool b)
        //{
        //    FacadeAudio.SetMusicVolume(b ? 1 : 0);
        //    MT_Show(b);
        //}

        //private void MT_Show(bool b)
        //{
        //    Debug.LogError(b);
        //    mMON.color = b ? toggleSelectedColor : toggleUnSelectedColor;
        //    mMOFF.color = b ? toggleUnSelectedColor : toggleSelectedColor;
        //}
        #endregion

        private void OnS_ToggleValueChange(bool b)
        {
            FacadeAudio.SetEffectsVolume(b ? 1 : 0);
            FacadeAudio.SetMusicVolume(b ? 1 : 0);
            mS_Icon.localPosition = new Vector3(b? 88 : -88, 36, 0);
        }

        private void OnV_ToggleValueChange(bool b)
        {
            FacadeAudio.SetVibrate(b);
            mV_Icon.localPosition = new Vector3(88 * (b ? 1 : -1), 36, 0);
        }

        protected override void OnDisable()
        {
            
        }

        protected override void OnDispose()
        {
            
        }
    }
}