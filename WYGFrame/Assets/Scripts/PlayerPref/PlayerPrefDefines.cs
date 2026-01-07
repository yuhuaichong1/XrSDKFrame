using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerPrefDefines
{
    private static string GetKey(string baseKey)
    {
        if (GameDefines.ifIAA)
        {
            return $"IAA_{baseKey}";
        }
        return baseKey;
    }

    #region AudioModule

    public static string musicToggle => GetKey("■_musicToggle");                //AudioModule_音乐开关
    public static string soundToggle => GetKey("■_soundToggle");                //AudioModule_音效开关
    public static string vibrateToggle => GetKey("■_vibrateToggle");            //AudioModule_震动开关

    #endregion

    #region PlayerModule

    public static string money => GetKey("■_money");                            //PlayerModule_玩家金钱数
    public static string diamond => GetKey("■_diamond");                        //PlayerModule_玩家钻石数
    public static string energy => GetKey("■_energy");                          //PlayerModule_玩家体力
    public static string level => GetKey("■_level");                            //PlayerModule_当前关卡
    public static string prop1Num => GetKey("■_prop1Num");                      //PlayerModule_“A”道具数量
    public static string prop2Num => GetKey("■_prop2Num");                      //PlayerModule_“B”道具数量
    public static string prop3Num => GetKey("■_prop3Num");                      //PlayerModule_“C”道具数量
    public static string userLevel => GetKey("■_userLevel");                    //PlayerModule_玩家等级
    public static string userExp => GetKey("■_userExp");                        //PlayerModule_玩家当前经验
    public static string userName => GetKey("■_userName");                      //PlayerModule_玩家姓名
    public static string userID => GetKey("■_userID");                          //PlayerModule_玩家ID
    public static string lastRecoverTime => GetKey("■_lastRecoverTime");         //PlayerModule_上次体力恢复时间

    #endregion

    #region WithdrawalModule

    public static string withdrawalCoin => GetKey("■_withdrawalCoin");          //WithdrawalModule_玩家累计兑现金额
    public static string wName => GetKey("■_wName");                        //WithdrawalModule_兑现姓名
    public static string wPhoneOrEmail => GetKey("■_wPhoneOrEmail");        //WithdrawalModule_兑现信息
    public static string poeType => GetKey("■_poeType");                    //WithdrawalModule_兑现信息类型
    public static string isEarning => GetKey("■_isEarning");                //WithdrawalModule_是否处于争取指定金额兑现过程中
    public static string wrisTemp => GetKey("■_wrisTemp");                  //WithdrawalModule_兑现记录

    #endregion

    #region GamePlayModule

    public static string ifContinue => GetKey("■_ifContinue");                             //GamePlayModule_是否继续上一局游戏（数据持久化判断）

    #endregion

    #region GuideModule

    public static string curStep => GetKey("■_curStep");                                   //GuideModule_当前引导步骤
    public static string ifTutorial => GetKey("■_ifTutorial");                             //GuideModule_是否处于引导状态

    #endregion
}
