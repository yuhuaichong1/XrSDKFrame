
using DG.Tweening;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    //请在UIGamePlayUI初始化后再进行该UI的初始化
    public partial class UIEffect : BaseUI
    {
        private STimer GetRewardEffectTimer;//获取奖励特效计时器
        private Stack<EffectRewardItem> ERItemPool;//获取奖励特效特效项池
        private List<EffectRewardItem> curERItem;//获取奖励特效项当前正在显示的奖励
        private Vector3 LTEGoldTrans;//关卡目标特效消失终点

        private char[] nameChars;//随机名称字符组

        private Stack<GameObject> flyMoneyPool;//飞行钱的对象池
        private Stack<GameObject> flyMoneyTipPool;//飞行钱提示的对象池
        private Stack<GameObject> flyPropPool;//飞行道具的对象池
        private Stack<SkeletonGraphic> clickPool;//点击特效对象池
        private Dictionary<ERewardType, Vector3> flyObjGoldDic;//飞行物体终点集合

        protected override void OnAwake()
        {
            FacadeEffect.PlayLevelTargetEffect += PlayLevelTargetEffect;
            FacadeEffect.PlayGetRewardEffect += PlayGetRewardEffect;
            FacadeEffect.PlayCongratulationEffect += PlayCongratulationEffect;
            FacadeEffect.PlayFlyMoney += PlayFlyMoney;
            FacadeEffect.PlayFlyMoneyTip += PlayFlyMoneyTip;
            FacadeEffect.PlayFlyProp += PlayFlyProp;
            FacadeEffect.PlayClickEffect += PlayClickEffect;
            FacadeEffect.PlayDifficultyUpEffect += PlayDifficultyUpEffect;

            ERItemPool = new Stack<EffectRewardItem>();
            curERItem = new List<EffectRewardItem>();
            flyMoneyPool = new Stack<GameObject>();
            flyMoneyTipPool = new Stack<GameObject>();
            flyPropPool = new Stack<GameObject>();
            clickPool = new Stack<SkeletonGraphic>();

            flyObjGoldDic = FacadeGamePlay.GetFlyObjGoalPos();

            LTEGoldTrans = FacadeGamePlay.GetCMDialogTextPos();

            mEffectRewardItem.gameObject.SetActive(false);
            mLevelTargetEffect.gameObject.SetActive(false);
            mGetRewardEffect.gameObject.SetActive(false);
            mFlyProp.gameObject.SetActive(false);
            mFlyMoney.gameObject.SetActive(false);
            mFlyIAAMoney.gameObject.SetActive(false);
            mFlyMoneyTip.gameObject.SetActive(false);
            mFlyIAAMoneyTip.gameObject.SetActive(false);
            mShaobaEffect.gameObject.SetActive(false);
            mHammerEffect.gameObject.SetActive(false);
            mClickEffect.gameObject.SetActive(false);
            mDifficultyUpEffect.gameObject.SetActive(false);
        }

        protected override void OnEnable()
        {
        
        }

        #region 播放提现目标特效
        /// <summary>
        /// 播放提现目标特效
        /// </summary>
        /// <param name="finishAction">特效完成回调</param>
        private void PlayLevelTargetEffect(Action finishAction)
        {
            mLevelTargetEffect.gameObject.SetActive(true);
            mLTEPlane.transform.localScale = Vector3.one;
            SetLevelTargetText();

            mLTEPlane.transform.position = mLTEStartPoint.transform.position;
            DG.Tweening.Sequence sequence = DOTween.Sequence();
            sequence.Append(mLTEPlane.transform.DOLocalMoveY(0, GameDefines.LTE_MoveTime));
            sequence.AppendInterval(GameDefines.LTE_StayTime);
            sequence.Append(mLTEPlane.transform.DOMove(LTEGoldTrans, GameDefines.LTE_GoAwayTime));
            sequence.Join(mLTEPlane.transform.DOScale(0, GameDefines.LTE_GoAwayTime));
            sequence.OnComplete(() => 
            {
                mLevelTargetEffect.gameObject.SetActive(false);
                finishAction?.Invoke();
            });
        }

        /// <summary>
        /// 设置关卡目标特效文本
        /// </summary>
        private void SetLevelTargetText()
        {

            int actualLevel = FacadePlayer.GetLevel() + 1;
            mMiniTargetText.text = GetLevelDisplayText(actualLevel);
            mMiniDialog.gameObject.SetActive(true);

            mNextTargetText.text = FacadeWithdrawal.GetTipMsg();
            mDialogText.text = string.Format(FacadeLanguage.GetText("10020"), (int)UnityEngine.Random.Range(GameDefines.TargetArriveTime.x, GameDefines.TargetArriveTime.y + 1));
        }
        #endregion

        private string GetLevelDisplayText(int actualLevel)
        {
            return "";
        }

        #region 播放获取奖励特效
        /// <summary>
        /// 播放获取奖励特效
        /// </summary>
        /// <param name="items">奖励类型</param>
        /// <param name="finishAction">特效完成回调</param>
        private void PlayGetRewardEffect(ERewardItemStruct[] items, Action finishAction)
        {
            mGetRewardEffect.gameObject.SetActive(true);

            if (GetRewardEffectTimer == null)
                GetRewardEffectTimer = STimerManager.Instance.CreateSTimer(GameDefines.GRE_StayTime, 0, true, false, () => 
                {
                    mGetRewardEffect.gameObject.SetActive(false);
                    mGREMask.gameObject.SetActive(false);

                    foreach (EffectRewardItem item in curERItem) 
                    {
                        ERItemPool.Push(item);
                        GetRewardEffectAfterFly(item);
                    }
                    curERItem.Clear();
                    finishAction?.Invoke();
                });

            GetRewardEffectTimer.ReStart();
            foreach(ERewardItemStruct item in items) 
            {
                EffectRewardItem newItem = ERItemPool.Count != 0 ? ERItemPool.Pop() : GameObject.Instantiate(mEffectRewardItem, mRewardGroup).GetComponent<EffectRewardItem>();
                newItem.gameObject.SetActive(true);
                newItem.Show(item.Type, item.Count);
                curERItem.Add(newItem);
            }

            mGREMask.gameObject.SetActive(true);
        }

        /// <summary>
        /// 获取奖励后的飞物体特效
        /// </summary>
        /// <param name="ERItem">奖励项</param>
        private void GetRewardEffectAfterFly(EffectRewardItem ERItem)
        {
            switch(ERItem.ErType)
            {
                case ERewardType.Money:
                    PlayFlyMoney(ERItem.transform, GameDefines.GRE_FlyMoneyCount, ERItem.Count, () => 
                    {
                        FacadeGamePlay.SetCurMoneyShow();
                    });
                    break;
                case ERewardType.Prop1:
                    PlayFlyProp(ERItem.transform, ERewardType.Prop1, () => 
                    {
                        FacadeGamePlay.SetProp1CountShow();
                    });
                    break;
                case ERewardType.Prop2:
                    PlayFlyProp(ERItem.transform, ERewardType.Prop2, () =>
                    {
                        FacadeGamePlay.SetProp2CountShow();
                    });
                    break;
                case ERewardType.Prop3:
                    PlayFlyProp(ERItem.transform, ERewardType.Prop3, () =>
                    {
                        FacadeGamePlay.SetProp3CountShow();
                    });
                    break;
            }
        }

        #endregion

        #region 播放祝贺特效
        /// <summary>
        /// 播放祝贺特效
        /// </summary>
        private void PlayCongratulationEffect()
        {
            mCEContent.text = $"<color=#EC3123>{GetRandomPlayerName()}</color> passed this level ({GetRandomAttempt()} attempt),\r\nwithdrew <color=#EC3123>{GetRandomWMoney()}</color>";

            DG.Tweening.Sequence sequence = DOTween.Sequence();
            sequence.Append(mCongratulationEffect.transform.DOLocalMoveY(-480, GameDefines.CE_MoveTime));
            sequence.AppendInterval(GameDefines.CE_StayTime);
            sequence.OnComplete(() => 
            {
                mCongratulationEffect.transform.localPosition = Vector3.zero;
            });
        }

        /// <summary>
        /// 获取随机玩家姓名
        /// </summary>
        /// <returns>随机玩家姓名</returns>
        private string GetRandomPlayerName()
        {
            nameChars = GameDefines.NameString.ToCharArray();
            int length = nameChars.Length;
            char c1 = nameChars[UnityEngine.Random.Range(0, length)];
            char c2 = nameChars[UnityEngine.Random.Range(0, length)];

            return $"{FacadeLanguage.GetText("10025")}_{c1}{c2}";
        }

        /// <summary>
        /// 获得随机尝试次数
        /// </summary>
        /// <returns>随机尝试次数</returns>
        private int GetRandomAttempt()
        {
            int attempt = (int)UnityEngine.Random.Range(GameDefines.CE_Content_attemptTimes.x, GameDefines.CE_Content_attemptTimes.y + 1);
            return attempt;
        }

        /// <summary>
        /// 获取随机兑现金额
        /// </summary>
        /// <returns>随机兑现金额</returns>
        private string GetRandomWMoney()
        {
            float wMoney = UnityEngine.Random.Range(GameDefines.Withdrawal_RQuota.x, GameDefines.Withdrawal_RQuota.y);
            return FacadePayType.RegionalChange(wMoney);
        }
        #endregion

        #region 飞行物体特效
        /// <summary>
        /// 播放飞行钱特效
        /// </summary>
        /// <param name="startPoint">起始点</param>
        /// <param name="count">数量</param>
        private void PlayFlyMoney(Transform startPoint, int count, float money, Action successAction)
        {
            for (int i = 0; i < count; i++) 
            {
                GameObject flyObj = flyMoneyPool.Count > 0 ? flyMoneyPool.Pop() : GameObject.Instantiate(GameDefines.ifIAA ? mFlyIAAMoney.gameObject : mFlyMoney.gameObject, mFlyEffectParent);
                flyObj.gameObject.SetActive(true);

                flyObj.transform.position = startPoint.position;
                float r1 = UnityEngine.Random.Range(0, GameDefines.FlyMoney_RandomSpawnDist);
                float r2 = UnityEngine.Random.Range(0, GameDefines.FlyMoney_RandomSpawnDist);
                flyObj.transform.GetComponent<RectTransform>().localPosition += new Vector3(r1, r2, 0);

                flyObj.transform.DOMove(flyObjGoldDic[ERewardType.Money], GameDefines.FlyMoney_MoveTime)
                    .SetDelay(GameDefines.FlyMoney_DelayTime + i * GameDefines.FlyMoney_IntervalTime)
                    .OnComplete(() => 
                    {
                        successAction?.Invoke();
                        flyMoneyPool.Push(flyObj);
                        flyObj.gameObject.SetActive(false);
                    });
            }
            STimerManager.Instance.CreateSDelay(GameDefines.FlyMoneyTip_DelayTime, () => 
            {
                PlayFlyMoneyTip(money);
            });
            
        }

        /// <summary>
        /// 播放飞行钱提示特效
        /// </summary>
        /// <param name="money">获得金额</param>
        private void PlayFlyMoneyTip(float money)
        {
            GameObject flyObj = flyMoneyTipPool.Count > 0 ? flyMoneyTipPool.Pop() : GameObject.Instantiate(GameDefines.ifIAA ? mFlyIAAMoneyTip.gameObject : mFlyMoneyTip.gameObject, mFlyEffectParent);
            flyObj.gameObject.SetActive(true);
            flyObj.transform.GetChild(0).GetComponent<Text>().text = FacadePayType.RegionalChange(money);
            flyObj.transform.position = flyObjGoldDic[ERewardType.Money];
            float goldY = flyObj.transform.localPosition.y;
            flyObj.transform.DOLocalMoveY(goldY += GameDefines.FlyMoneyTip_MoveDist, GameDefines.FlyMoneyTip_MoveTime).OnComplete(() => 
            {
                flyMoneyTipPool.Push(flyObj);
                flyObj.gameObject.SetActive(false);
            });
        }

        /// <summary>
        /// 播放飞行道具特效
        /// </summary>
        /// <param name="startPoint">起始点</param>
        /// <param name="rewardType">奖励类型</param>
        private void PlayFlyProp(Transform startPoint, ERewardType rewardType, Action successAction)
        {
            string flyIconPath = GameDefines.ERHammerIconPath;
            switch (rewardType) 
            {
                case ERewardType.Prop1:
                    flyIconPath = GameDefines.ERAddSpaceIconPath;
                    break;
                case ERewardType.Prop2:
                    flyIconPath = GameDefines.ERClearIconPath;
                    break;
                case ERewardType.Prop3:
                    flyIconPath = GameDefines.ERHammerIconPath;
                    break;
            }

            GameObject flyObj = flyPropPool.Count > 0? flyPropPool.Pop() : GameObject.Instantiate(mFlyProp.gameObject, mFlyEffectParent);
            flyObj.gameObject.SetActive(true);
            flyObj.transform.position = startPoint.position;
            flyObj.GetComponent<Image>().sprite = ResourceMod.Instance.SyncLoad<Sprite>(flyIconPath);

            flyObj.transform.DOMove(flyObjGoldDic[rewardType], GameDefines.FlyProp_MoveTime).OnComplete(() => 
            {
                successAction?.Invoke();
                flyPropPool.Push(flyObj);
                flyObj.gameObject.SetActive(false);
            });
        }
        #endregion

        #region Spine动画
        /// <summary>
        /// 播放点击特效
        /// </summary>
        /// <param name="startPos">起始点</param>
        private void PlayClickEffect(Transform startPos)
        {
            SkeletonGraphic clickObj = clickPool.Count > 0 ? clickPool.Pop() : GameObject.Instantiate(mClickEffect, mClickEffectParent);
            clickObj.gameObject.SetActive(true);
            clickObj.transform.position = startPos.position;
            TrackEntry trackEntry = clickObj.AnimationState.SetAnimation(0, "touch", false);
            trackEntry.Complete += (trackEntry) =>
            {
                clickPool.Push(clickObj);
                clickObj.gameObject.SetActive(false);
            };
        }

        /// <summary>
        /// 播放难度提升特效
        /// </summary>
        /// <param name="endAction">结束事件</param>
        private void PlayDifficultyUpEffect(Action endAction)
        {
            mDifficultyUpEffect.gameObject.SetActive(true);
            TrackEntry trackEntry = mDifficultyUpEffect.AnimationState.SetAnimation(0, "animation", false);
            STimerManager.Instance.CreateSDelay(GameDefines.DifficultyUp_StayTime, () => 
            {
                mDifficultyUpEffect.AnimationState.ClearTrack(0);
                mDifficultyUpEffect.gameObject.SetActive(false);
                endAction?.Invoke();
            });
            //trackEntry.Complete += (trackEntry) =>
            //{
            //    mDifficultyUpEffect.gameObject.SetActive(false);
            //    endAction?.Invoke();
            //};
        }
        #endregion

        protected override void OnDisable()
        {
        
        }

        protected override void OnDispose()
        {
            FacadeEffect.PlayLevelTargetEffect -= PlayLevelTargetEffect;
            FacadeEffect.PlayGetRewardEffect -= PlayGetRewardEffect;
            FacadeEffect.PlayCongratulationEffect -= PlayCongratulationEffect;
            FacadeEffect.PlayFlyMoney -= PlayFlyMoney;
            FacadeEffect.PlayFlyMoneyTip -= PlayFlyMoneyTip;
            FacadeEffect.PlayFlyProp -= PlayFlyProp;
            FacadeEffect.PlayClickEffect -= PlayClickEffect;
            FacadeEffect.PlayDifficultyUpEffect -= PlayDifficultyUpEffect;

            ERItemPool.Clear();
            ERItemPool = null;
            curERItem.Clear();
            curERItem = null;
            flyMoneyPool.Clear();
            flyMoneyPool = null;
            flyMoneyTipPool.Clear();
            flyMoneyTipPool = null;
            flyPropPool.Clear();
            flyPropPool = null;
            clickPool.Clear();
            clickPool = null;
            flyObjGoldDic.Clear();
            flyObjGoldDic = null;

            if (GetRewardEffectTimer != null)
            {
                GetRewardEffectTimer.ifAutoPushPool = true;
                if (GetRewardEffectTimer.STimerState == STimerState.Running)
                    GetRewardEffectTimer.Stop();
                GetRewardEffectTimer.Close();
            }
        }
    }
}