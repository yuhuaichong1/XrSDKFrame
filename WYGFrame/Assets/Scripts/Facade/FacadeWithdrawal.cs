using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FacadeWithdrawal
{
    public static Func<string> GetWName;                                                //获取兑现名称
    public static Action<string> SetWName;                                              //设置兑现名称
    public static Func<string> GetWPhoneOrEmail;                                        //获取兑现信息
    public static Action<string> SetWPhoneOrEmail;                                      //设置兑现信息
    public static Func<EPayType> GetPayType;                                            //获取兑现信息类型
    public static Action<EPayType> SetPayType;                                          //设置兑现信息类型
    public static Func<string> GetTipMsg;                                               //获取关卡兑现提示
    public static Func<bool> GetIsEarning;                                              //获取是否在争取中的状态
    public static Action<bool> SetIsEarning;                                            //设置是否在争取中的状态
    public static Action<int> CreateOrder;                                              //创建订单
    public static Action SaveCurWithdrawalRecordItems;                                  //保存单当前订单数据
    public static Func<List<WithdrawalRecordItem>> GetWithdrawalRecordItems;            //获取兑现记录数据
    public static Action<bool, WithdrawalRecordItem> CheckOpenUI;                       //检测应该打开UIEnterInfo还是UIConfirm
}
