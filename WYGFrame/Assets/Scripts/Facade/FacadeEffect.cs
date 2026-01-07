using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FacadeEffect
{
    public static Action<Action> PlayLevelTargetEffect;                     //播放提现目标特效
    public static Action<ERewardItemStruct[], Action> PlayGetRewardEffect;  //播放获取奖励特效
    public static Action PlayCongratulationEffect;                          //播放祝贺特效
    public static Action<Transform, int, float, Action> PlayFlyMoney;       //播放飞钱特效
    public static Action<float> PlayFlyMoneyTip;                            //播放飞钱提示特效
    public static Action<Transform, ERewardType, Action> PlayFlyProp;       //播放飞道具特效
    public static Action<Transform> PlayClickEffect;                        //播放点击特效
    public static Action<Action> PlayDifficultyUpEffect;                    //播放难度提升特效
}
