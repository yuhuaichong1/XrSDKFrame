using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XrCode
{
    // 登录模块外部接口定义
    public static class FacadePlayer
    {
        public static Func<double> GetMoney;            //获取当前金钱数
        public static Action<double> SetMoney;          //设置当前金钱数
        public static Action<double> AddMoney;          //增加当前金钱数

        public static Func<int> GetDiamond;             //获取当前钻石数
        public static Action<int> SetDiamond;           //设置当前钻石数
        public static Action<int> AddDiamond;           //增加当前钻石数

        public static Func<int> GetEnergy;              //获取当前体力
        public static Func<int, bool> AddEnergy;        //增加当前体力
        public static Func<int> GetCurRemainingTime;    //获取剩余时间

        public static Func<int> GetLevel;               //获取当前关卡
        public static Action<int> SetLevel;             //设置当前关卡
        public static Action<int> AddLevel;             //增加当前关卡

        public static Func<int> GetProp1Num;            //获得当前“道具1”数
        public static Action<int> SetProp1Num;          //设置当前“道具1”数
        public static Action<int> AddProp1Num;          //增加当前“道具1”数

        public static Func<int> GetProp2Num;            //获得当前“道具2”数
        public static Action<int> SetProp2Num;          //设置当前“道具2”数
        public static Action<int> AddProp2Num;          //增加当前“道具2”数

        public static Func<int> GetProp3Num;            //获得当前“道具3”数
        public static Action<int> SetProp3Num;          //设置当前“道具3”数
        public static Action<int> AddProp3Num;          //增加当前“道具3”数

        public static Func<string> GetPlayerName;       //获取当前玩家姓名

        public static Func<string> GetPlayerID;         //获取当前玩家ID

        public static Func<int> GetPlayerLevel;         //获取当前玩家等级

        public static Action<int> AddPlayerExp;         //增加经验
        public static Func<int> GetPlayerExp;           //获取当前经验
        
    }

}