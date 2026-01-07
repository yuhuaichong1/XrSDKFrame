using System;
using System.Globalization;
using System.Text;
using UnityEngine;

public static class SDoubleHelper
{
    #region 添加分隔符

    /// <summary>
    /// 将double分隔
    /// </summary>
    /// <param name="doubleValue">指定的double值</param>
    /// <param name="integerLength">整数部分的分隔长度，“<0”不显示，“=0”原样，“>0”按位数添加</param>
    /// <param name="decimalLength">小数部分的分隔长度</param>
    /// <returns>分隔后的字符串</returns>
    [Obsolete("'SeparateConvert' consumes too much performance, try to use 'CurrencyConvertV2'(Cannot separate decimals)")]
    public static string SeparateConvert(this double doubleValue, int integerLength = 3, int decimalLength = 3, string separator = ",")
    {
        Debug.LogWarning("");

        bool ifNegative = doubleValue < 0;
        if (ifNegative)
            doubleValue *= -1;

        string[] str = doubleValue.ToString().Split(".");
        string integerStr = str[0];


        StringBuilder stringBuilder = new StringBuilder();
        int sepNum = 0;
        sepNum = integerStr.Length % integerLength;
        for (int i = 0; i < integerStr.Length; i++)
        {
            if (i == sepNum)
            {
                sepNum += integerLength;
                if (i != 0)
                    stringBuilder.Append(separator);
            }

            stringBuilder.Append(integerStr[i]);
        }

        if (str.Length == 1)
        {
            return $"{(ifNegative ? "-" : "")}{stringBuilder}";
        }

        stringBuilder.Append(".");

        string decimalStr = str[1];
        sepNum = decimalLength;

        for (int i = 0; i < decimalStr.Length; i++)
        {
            if (i == sepNum)
            {
                sepNum += decimalLength;
                stringBuilder.Append(separator);
            }

            stringBuilder.Append(decimalStr[i]);
        }

        return $"{(ifNegative ? "-" : "")}{stringBuilder.ToString()}";
    }

    /// <summary>
    /// 将double变为货币的显示方式
    /// </summary>
    /// <param name="doubleValue">指定的double值</param>
    /// <param name="currencySymbol">货币符号</param>
    /// <param name="integerLength">整数部分的分隔长度，“<0”不显示，“=0”原样，“>0”按位数添加</param>
    /// <param name="retainDecimals">小数部分的显示位数</param>
    /// <returns>分隔后的字符串</returns>
    [Obsolete("'CurrencyConvert' is obsolete, please use 'CurrencyConvertV2'")]
    public static string CurrencyConvert(this double doubleValue, string currencySymbol = "$", int integerLength = 3, int retainDecimals = 2)
    {
        bool ifNegative = doubleValue < 0;
        if (ifNegative)
            doubleValue *= -1;

        string[] str = doubleValue.ToString().Split(".");
        string integerStr = str[0];

        StringBuilder stringBuilder = new StringBuilder();
        int sepNum = 0;
        sepNum = integerStr.Length % integerLength;
        for (int i = 0; i < integerStr.Length; i++)
        {
            if (i == sepNum)
            {
                sepNum += integerLength;
                if (i != 0)
                    stringBuilder.Append(",");
            }

            stringBuilder.Append(integerStr[i]);
        }

        string decimalsShow = $"D{retainDecimals}";
        if (str.Length == 1)
        {
            return $"{currencySymbol}{(ifNegative ? "-" : "")}{stringBuilder.ToString()}.{0.ToString(decimalsShow)}";
        }
        else
        {
            int temp = int.Parse(str[1]);
            return $"{currencySymbol}{(ifNegative ? "-" : "")}{stringBuilder.ToString()}.{temp.ToString(decimalsShow)}";
        }
    }

    /// <summary>
    /// 将double变为货币的显示方式
    /// </summary>
    /// <param name="doubleValue">指定的double值</param>
    /// <param name="currencySymbol">货币符号</param>
    /// <param name="separator">整数部分的分隔符号</param>
    /// <param name="integerLength">整数部分的分隔长度</param>
    /// <param name="retainDecimals">小数部分的显示位数</param>
    /// <returns></returns>
    public static string CurrencyConvertV2(this double doubleValue, string currencySymbol = "$", string separator = ",", int integerLength = 3, int retainDecimals = 2)
    {
        NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
        nfi.NumberGroupSeparator = separator;
        nfi.NumberGroupSizes = new int[] { integerLength };
        nfi.CurrencySymbol = currencySymbol;
        nfi.CurrencyDecimalDigits = retainDecimals;

        return doubleValue.ToString("C", nfi);
    }


    #endregion
}
