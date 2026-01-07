
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{

    public partial class UIGamePlay : BaseUI
    {
        protected override void OnAwake()
        {
            FacadeAdd();

            InitShow();
        }

        #region Facade

        /// <summary>
        /// 添加Facade接口
        /// </summary>
        private void FacadeAdd()
        {
            FacadeGamePlay.SetCurMoneyShow += SetCurMoneyShow;
            FacadeGamePlay.SetProp1CountShow += SetProp1CountShow;
            FacadeGamePlay.SetProp2CountShow += SetProp2CountShow;
            FacadeGamePlay.SetProp3CountShow += SetProp3CountShow;
            FacadeGamePlay.SetCurLevelText += SetCurLevelText;
            FacadeGamePlay.SetWithdrawalTip += SetWithdrawalTip;

            FacadeGamePlay.GetCMDialogTextPos += GetCMDialogTextPos;
            FacadeGamePlay.GetFlyObjGoalPos += GetFlyObjGoalPos;

            FacadeGamePlay.SetCurEnergyShow += SetCurEnergyShow;
        }

        /// <summary>
        /// 去除Facade接口
        /// </summary>
        private void FacadeRemove()
        {
            FacadeGamePlay.SetCurMoneyShow -= SetCurMoneyShow;
            FacadeGamePlay.SetProp1CountShow -= SetProp1CountShow;
            FacadeGamePlay.SetProp2CountShow -= SetProp2CountShow;
            FacadeGamePlay.SetProp3CountShow -= SetProp3CountShow;
            FacadeGamePlay.SetCurLevelText -= SetCurLevelText;
            FacadeGamePlay.SetWithdrawalTip -= SetWithdrawalTip;

            FacadeGamePlay.GetCMDialogTextPos -= GetCMDialogTextPos;
            FacadeGamePlay.GetFlyObjGoalPos -= GetFlyObjGoalPos;

            FacadeGamePlay.SetCurEnergyShow -= SetCurEnergyShow;
        }

        #endregion

        private void InitShow()
        {
            mMoneyIcon.gameObject.SetActive(!GameDefines.ifIAA);
            mIAAMoneyIcon.gameObject.gameObject.SetActive(GameDefines.ifIAA);
            mCMBtn.gameObject.SetActive(!GameDefines.ifIAA);
            mCMDialog.gameObject.SetActive(!GameDefines.ifIAA);

            mCurLevel.text = $"{FacadeLanguage.GetText?.Invoke("10004")} {FacadePlayer.GetLevel?.Invoke()}";

            SetCurMoneyShow();
            SetProp1CountShow();
            SetProp2CountShow();
            SetProp3CountShow();
            SetCurEnergyShow(EShowEnergyType.All);
        }

        protected override void OnEnable()
        {
        
        }

        #region 设置部分UI的显示

        /// <summary>
        /// 设置当前金钱的显示
        /// </summary>
        private void SetCurMoneyShow()
        {
            double money = FacadePlayer.GetMoney?.Invoke() ?? 0;
            mCurMoneyText.text = FacadePayType.RegionalChange?.Invoke(money);
        }

        /// <summary>
        /// 设置当前道具1的数量的显示
        /// </summary>
        private void SetProp1CountShow()
        {
            int count = FacadePlayer.GetProp1Num();
            bool b = count > 0;

            mProp1_count_icon.gameObject.SetActive(b);
            mProp1_ad.gameObject.SetActive(!b);

            if (count > 0)
                mProp1_count_Text.text = count.ToString();
        }

        /// <summary>
        /// 设置当前道具2的数量的显示
        /// </summary>
        private void SetProp2CountShow()
        {
            int count = FacadePlayer.GetProp2Num?.Invoke() ?? 0;
            bool b = count > 0;

            mProp2_count_icon.gameObject.SetActive(b);
            mProp2_ad.gameObject.SetActive(!b);

            if (count > 0)
                mProp2_count_Text.text = count.ToString();
        }

        /// <summary>
        /// 设置当前道具3的数量的显示
        /// </summary>
        private void SetProp3CountShow()
        {
            int count = FacadePlayer.GetProp3Num();
            bool b = count > 0;

            mProp3_count_icon.gameObject.SetActive(b);
            mProp3_ad.gameObject.SetActive(!b);

            if (count > 0)
                mProp3_count_Text.text = count.ToString();
        }

        /// <summary>
        /// 设置当前关卡的显示
        /// </summary>
        private void SetCurLevelText()
        {
            int curLevel = FacadePlayer.GetLevel();
            mCurLevel.text = string.Format(FacadeLanguage.GetText("10006"), curLevel);
        }

        /// <summary>
        /// 设置提现目标显示
        /// </summary>
        private void SetWithdrawalTip()
        {
            //mCMDialogText.text = string.Format(FacadeLanguage.GetText(), );
        }

        /// <summary>
        /// 设置当前体力显示
        /// </summary>
        public void SetCurEnergyShow(EShowEnergyType type)
        {
            switch(type)
            {
                case EShowEnergyType.Energy:
                    int energy = FacadePlayer.GetEnergy?.Invoke() ?? 0;
                    mCurEnergyText.text = energy.ToString();
                    break;
                case EShowEnergyType.Time:
                    int time = FacadePlayer.GetCurRemainingTime?.Invoke() ?? 0;
                    mCurEnergyTimeText.text = time.MSConvert();
                    break;
                case EShowEnergyType.All:
                    int energy2 = FacadePlayer.GetEnergy?.Invoke() ?? 0;
                    mCurEnergyText.text = energy2.ToString();
                    int time2 = FacadePlayer.GetCurRemainingTime?.Invoke() ?? 0;
                    mCurEnergyTimeText.text = time2.MSConvert();
                    break;
            }
        }

        #endregion

        #region 获取部分UI

        /// <summary>
        /// 获取兑现提示框的位置
        /// </summary>
        /// <returns>兑现提示框的位置</returns>
        private Vector3 GetCMDialogTextPos()
        {
            return mCMDialogText.transform.position;
        }

        /// <summary>
        /// 获取飞行小特效物体的目标点
        /// </summary>
        /// <returns>飞行小特效物体的目标点</returns>
        private Dictionary<ERewardType, Vector3> GetFlyObjGoalPos() 
        { 
            Dictionary<ERewardType, Vector3> goals = new Dictionary<ERewardType, Vector3>() 
            {
                {ERewardType.Money, mCurMoneyText.transform.position},
                {ERewardType.Prop1, mBtn_Prop1.transform.position},
                {ERewardType.Prop2, mBtn_Prop2.transform.position},
                {ERewardType.Prop3, mBtn_Prop3.transform.position},
            };

            return goals;
        }

        #endregion

        #region 按钮事件
        private void OnSettingBtnClickHandle()        {            UIManager.Instance.OpenAsync<UISetting>(EUIType.EUISetting);        }	    private void OnReStartBtnClickHandle()        {            UIManager.Instance.OpenAsync<UIReStart>(EUIType.EUIReStart);        }	    private void OnBtn_Prop1ClickHandle()        {            if(FacadePlayer.GetProp1Num() > 0)
            {
                FacadePlayer.AddProp1Num(-1);
                FacadeGamePlay.Func_Porp1();
                SetProp1CountShow();
            }            else                UIManager.Instance.OpenAsync<UIProp>(EUIType.EUIProp, null, EFuncType.Prop1);        }	    private void OnBtn_Prop2ClickHandle()        {
            if (FacadePlayer.GetProp2Num() > 0)
            {
                FacadePlayer.AddProp2Num(-1);
                FacadeGamePlay.Func_Porp2();
                SetProp2CountShow();
            }            else
                UIManager.Instance.OpenAsync<UIProp>(EUIType.EUIProp, null, EFuncType.Prop2);        }	    private void OnBtn_Prop3ClickHandle()        {
            if (FacadePlayer.GetProp3Num() > 0)
            {
                FacadePlayer.AddProp3Num(-1);
                FacadeGamePlay.Func_Porp3();
                SetProp3CountShow();
            }            else
                UIManager.Instance.OpenAsync<UIProp>(EUIType.EUIProp, null, EFuncType.Prop3);        }	    private void OnCMBtnClickHandle()
        {
            if (!string.IsNullOrEmpty(FacadeWithdrawal.GetWName?.Invoke()) && !string.IsNullOrEmpty(FacadeWithdrawal.GetWPhoneOrEmail?.Invoke()))
                UIManager.Instance.OpenAsync<UIConfirm>(EUIType.EUIConfirm);
            else
                UIManager.Instance.OpenAsync<UIEnterInfomation>(EUIType.EUIEnterInfomation);
        }

        #endregion

        protected override void OnDisable()
        {
        
        }
        protected override void OnDispose()
        {
            FacadeRemove();
        }
    }
}