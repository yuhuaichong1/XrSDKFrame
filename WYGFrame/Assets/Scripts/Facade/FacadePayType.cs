using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FacadePayType
{
    public static Func<List<PayNode>> GetPayItems;                  //获取支付类型
    public static Func<double, string> RegionalChange;              //将值以汇率的方式显示
    public static Func<int> GetNANP;                                //获取国际长途电话区号
    public static Func<string> GetCountryCode;                      //获取国家码
    public static Func<string> GetLanguage;                         //获取语言
}