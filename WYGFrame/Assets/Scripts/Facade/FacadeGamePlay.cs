using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public static class FacadeGamePlay
    {
        #region GamePlayModule

        public static Action CreateLevel;                                               //创建关卡
        public static Action Func_Porp1;                                                //功能1
        public static Action Func_Porp2;                                                //功能2
        public static Action Func_Porp3;                                                //功能3
        public static Action RePlay;                                                    //重新开始游戏
        public static Func<float> GetCurLevelProgress;                                  //获得当前关卡进度

        #endregion

        #region UIGamePlay

        public static Action SetCurMoneyShow;                                           //设置当前金钱数的显示
        public static Func<Vector3> GetCMDialogTextPos;                                 //获取关卡目标提示框位置
        public static Action SetProp1CountShow;                                         //设置道具1“添加额外格子”的显示
        public static Action SetProp2CountShow;                                         //设置道具2“清理”的显示
        public static Action SetProp3CountShow;                                         //设置道具3“锤子”的显示
        public static Func<Dictionary<ERewardType, Vector3>> GetFlyObjGoalPos;          //获取飞行物体特效的终点
        public static Action SetCurLevelText;                                           //设置当前关卡的显示
        public static Action SetWithdrawalTip;                                          //设置当前兑现通知

        public static Action<EShowEnergyType> SetCurEnergyShow;                         //设置当前体力显示

        #endregion
    }
}
