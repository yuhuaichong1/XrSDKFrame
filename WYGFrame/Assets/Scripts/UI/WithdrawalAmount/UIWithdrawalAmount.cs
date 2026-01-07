
using System;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{

    public partial class UIWithdrawalAmount : BaseUI
    {
        private bool ifSuccess;
        private Action finishAction;
        private WithdrawalRecordItem item;
        protected override void OnAwake()
        {
            string dataStr = DateTime.Now.ToString("yyyy-MM-dd");
            mP1Date.text = dataStr;
            mP2Date.text = dataStr;
            mP3Date.text = dataStr;
        }

        protected override void OnSetParam(params object[] args)
        {
            ifSuccess = false;
            item = (WithdrawalRecordItem)args[0];
        }

        protected override void OnEnable() 
        {
            bool isEarning = FacadeWithdrawal.GetIsEarning();
            mExitBtn.gameObject.SetActive(isEarning);
            if (isEarning)
            {
                ShowAnim(mPlane);
                EarningShow();
            }
            else
            {
                ClearShow();
                ShowAnim(mPlane, () => 
                {
                    PlayShow();
                });

            }
        }

        /// <summary>
        /// 重置显示
        /// </summary>
        private void ClearShow()
        {
            mProgress1.gameObject.SetActive(false);
            mProgress2.gameObject.SetActive(false);
            mProgress3.gameObject.SetActive(false);

            mConfirmBtn.gameObject.SetActive(false);
            mKeepEarnBtn.gameObject.SetActive(false);
        }

        /// <summary>
        /// “攒钱中”的情况显示
        /// </summary>
        private void EarningShow()
        {
            mProgress1.gameObject.SetActive(true);
            ShowProgressState1(true);

            mProgress2.gameObject.SetActive(true);
            ShowProgressState2(true);

            mProgress3.gameObject.SetActive(true);
            ShowProgressState3(true);
            ShowProgressState3_2();
        }

        /// <summary>
        /// 开始逐个显示
        /// </summary>
        private void PlayShow()
        {
            ShowProgressState1(false);
            ShowProgressState2(false);
            ShowProgressState3(false);

            float delayTime1 = GetRandomDelay();
            float delayTime2 = GetRandomDelay();
            float delayTime3 = GetRandomDelay();

            mProgress1.gameObject.SetActive(true);
            STimerManager.Instance.CreateSTimer(delayTime1 + delayTime2 + delayTime3, 0, true, true, () => 
            {
                mExitBtn.gameObject.SetActive(true);
                ShowProgressState3(true);
                ShowProgressState3_2();

                if (ifSuccess)
                    item.ChangeState(EWithRecordState.UnderReview);

            }, null, new timingActions 
            {
                timing = delayTime1,
                clockActionType = ClockActionType.Once,
                clockAction = (time) => 
                {
                    ShowProgressState1(true);
                    mProgress2.gameObject.SetActive(true);
                }

            }, new timingActions
            {
                timing = delayTime1 + delayTime2,
                clockActionType = ClockActionType.Once,
                clockAction = (time) =>
                {
                    ShowProgressState2(true);
                    mProgress3.gameObject.SetActive(true);
                }
            });
        }


	    private void OnExitBtnClickHandle()
        {
            HideAnim(mPlane, () => 
            { 
                UIManager.Instance.CloseUI(EUIType.EUIWithdrawalAmount);
            });
        }

        private void OnConfirmBtnClickHandle()
        {
            HideAnim(mPlane, () =>
            {
                UIManager.Instance.CloseUI(EUIType.EUIWithdrawalAmount);
            });
        }

        private void OnKeepEarnBtnClickHandle()
        {
            HideAnim(mPlane, () =>
            {
                UIManager.Instance.CloseUI(EUIType.EUIWithdrawalAmount);
            });
        }

        private void ShowProgressState1(bool b)
        {
            mP1Icon1.gameObject.SetActive(b);
            mP1Icon2.gameObject.SetActive(!b);
            mP1Line.gameObject.SetActive(b);
            mP1Date.gameObject.SetActive(b);
        }
        private void ShowProgressState2(bool b)
        {
            mP2Icon1.gameObject.SetActive(b);
            mP2Icon2.gameObject.SetActive(!b);
            mP2Line.gameObject.SetActive(b);
            mP2Date.gameObject.SetActive(b);
        }
        private void ShowProgressState3(bool b)
        {
            mP3Icon1.gameObject.SetActive(b);
            mP3Icon2.gameObject.SetActive(!b);
            mP3Date.gameObject.SetActive(b);
            mErrContent.gameObject.SetActive(false);
        }
        private void ShowProgressState3_2()
        {
            mP3Icon1.gameObject.SetActive(ifSuccess);
            mP3Icon2.gameObject.SetActive(!ifSuccess);
            mErrContent.gameObject.SetActive(!ifSuccess);
            mP3Date.gameObject.SetActive(ifSuccess);
            if(!ifSuccess)
            {
                string mwa = FacadePayType.RegionalChange(GameDefines.MinWithdrawalAmount);
                string diff = FacadePayType.RegionalChange(GameDefines.MinWithdrawalAmount - FacadePlayer.GetMoney());
                mErrorMsg.text = string.Format(FacadeLanguage.GetText("10058"), mwa, diff);
            }

            mConfirmBtn.gameObject.SetActive(ifSuccess);
            mKeepEarnBtn.gameObject.SetActive(!ifSuccess);
        }

        private float GetRandomDelay()
        {
            return UnityEngine.Random.Range(GameDefines.WithdrawalAmountRandomTime.x, GameDefines.WithdrawalAmountRandomTime.y);
        }

        protected override void OnDisable()
        {
        
        }
        protected override void OnDispose()
        {
        
        }
    }
}