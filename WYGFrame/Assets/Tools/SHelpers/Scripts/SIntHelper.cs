using System;
using System.Text;
using UnityEngine;

public static class SIntHelper
{
    [ThreadStatic]
    private static StringBuilder sb;

    #region 转换为时间形式

    /// <summary>
    /// 将总秒数转换为分:秒的形式
    /// </summary>
    /// <param name="timeValue">总秒数</param>
    /// <param name="Msuffix">“分”后的补充</param>
    /// <param name="MifAddZero">是否为“分”补充0</param>
    /// <param name="Ssuffix">“秒”后的补充</param>
    /// <param name="SifAddZero">是否为“秒”补充0</param>
    /// <returns>修改结果</returns>
    public static string MSConvert(this int timeValue, string Msuffix = ":", bool MifAddZero = true, string Ssuffix = "", bool SifAddZero = true)
    {
        if (sb == null)
            sb = new StringBuilder(16);
        else
            sb.Clear();

        if (timeValue < 0)
        {
            Debug.LogWarning("The time value cannot be less than 0");
            sb.Append(MifAddZero ? "00" : "0");
            sb.Append(Msuffix);
            sb.Append(SifAddZero ? "00" : "0");
            sb.Append(Ssuffix);
            return sb.ToString();
        }

        int value = timeValue;

        int minute = value / 60;
        string mZero = MifAddZero && minute < 10 ? "0" : "";
        sb.Append(mZero);
        sb.Append(minute);
        sb.Append(Msuffix);

        int second = value % 60;
        string sZero = SifAddZero && second < 10 ? "0" : "";
        sb.Append(sZero);
        sb.Append(second);
        sb.Append(Ssuffix);

        return sb.ToString();
    }

    /// <summary>
    /// 将总秒数转换为时:分:秒的形式
    /// </summary>
    /// <param name="timeValue">总秒数</param>
    /// <param name="Hsuffix">“时”后的补充</param>
    /// <param name="HifAddZero">是否为“时”补充0</param>
    /// <param name="Msuffix">“分”后的补充</param>
    /// <param name="MifAddZero">是否为“分”补充0</param>
    /// <param name="Ssuffix">“秒”后的补充</param>
    /// <param name="SifAddZero">是否为“秒”补充0</param>
    /// <returns>修改结果</returns>
    public static string HMSConvert(this int timeValue, string Hsuffix = ":", bool HifAddZero = true, string Msuffix = ":", bool MifAddZero = true, string Ssuffix = "", bool SifAddZero = true)
    {
        if (sb == null)
            sb = new StringBuilder(16);
        else
            sb.Clear();

        if (timeValue < 0)
        {
            Debug.LogWarning("The time value cannot be less than 0");
            sb.Append(HifAddZero ? "00" : "0");
            sb.Append(Hsuffix);
            sb.Append(MifAddZero ? "00" : "0");
            sb.Append(Msuffix);
            sb.Append(SifAddZero ? "00" : "0");
            sb.Append(Ssuffix);
            return sb.ToString();
        }

        int value = timeValue;

        int hour = value / 3600;
        string hZero = HifAddZero && hour < 10 ? "0" : "";
        sb.Append(hZero);
        sb.Append(hour);
        sb.Append(Hsuffix);

        int minute = (value % 3600) / 60;
        string mZero = MifAddZero && minute < 10 ? "0" : "";
        sb.Append(mZero);
        sb.Append(minute);
        sb.Append(Msuffix);

        int second = value % 60;
        string sZero = SifAddZero && second < 10 ? "0" : "";
        sb.Append(sZero);
        sb.Append(second);
        sb.Append(Ssuffix);

        return sb.ToString();
    }

    /// <summary>
    /// 将总秒数转换为时:分的形式
    /// </summary>
    /// <param name="timeValue">总秒数</param>
    /// <param name="Hsuffix">“时”后的补充</param>
    /// <param name="HifAddZero">是否为“时”补充0</param>
    /// <param name="Msuffix">“分”后的补充</param>
    /// <param name="MifAddZero">是否为“分”补充0</param>
    /// <returns>修改结果</returns>
    public static string HMConvert(this int timeValue, string Hsuffix = ":", bool HifAddZero = true, string Msuffix = "", bool MifAddZero = true)
    {
        if (sb == null)
            sb = new StringBuilder(16);
        else
            sb.Clear();

        if (timeValue < 0)
        {
            Debug.LogWarning("The time value cannot be less than 0");
            sb.Append(HifAddZero ? "00" : "0");
            sb.Append(Hsuffix);
            sb.Append(MifAddZero ? "00" : "0");
            sb.Append(Msuffix);
            return sb.ToString();
        }

        int value = timeValue + 60;

        int hour = value / 3600;
        string hZero = HifAddZero && hour < 10 ? "0" : "";
        sb.Append(hZero);
        sb.Append(hour);
        sb.Append(Hsuffix);

        int minute = (value % 3600) / 60;
        string mZero = MifAddZero && minute < 10 ? "0" : "";
        sb.Append(mZero);
        sb.Append(minute);
        sb.Append(Msuffix);

        return sb.ToString();
    }

    /// <summary>
    /// 将总秒数转换为天:时:分的形式
    /// </summary>
    /// <param name="timeValue">总秒数</param>
    /// <param name="Dsuffix">“天”后的补充</param>
    /// <param name="DifAddZero">是否为“天”补充0</param>
    /// <param name="Hsuffix">“时”后的补充</param>
    /// <param name="HifAddZero">是否为“时”补充0</param>
    /// <param name="Msuffix">“分”后的补充</param>
    /// <param name="MifAddZero">是否为“分”补充0</param>
    /// <returns>修改结果</returns>
    public static string DHMConvert(this int timeValue, string Dsuffix = " Days ", bool DifAddZero = true, string Hsuffix = " Hours ", bool HifAddZero = true, string Msuffix = " Mins", bool MifAddZero = true)
    {
        if (sb == null)
            sb = new StringBuilder(16);
        else
            sb.Clear();

        if (timeValue < 0)
        {
            Debug.LogWarning("The time value cannot be less than 0");
            sb.Append(DifAddZero ? "00" : "0");
            sb.Append(Dsuffix);
            sb.Append(HifAddZero ? "00" : "0");
            sb.Append(Hsuffix);
            sb.Append(MifAddZero ? "00" : "0");
            sb.Append(Msuffix);
            return sb.ToString();
        }

        int value = (timeValue + 60);

        int day = value / 86400;
        string dZero = DifAddZero && day < 10 ? "0" : "";
        sb.Append(dZero);
        sb.Append(day);
        sb.Append(Dsuffix);

        int hour = (value % 86400) / 3600;
        string hZero = HifAddZero && hour < 10 ? "0" : "";
        sb.Append(hZero);
        sb.Append(hour);
        sb.Append(Hsuffix);

        int minute = (value % 3600) / 60;
        string mZero = MifAddZero && minute < 10 ? "0" : "";
        sb.Append(mZero);
        sb.Append(minute);
        sb.Append(Msuffix);

        return sb.ToString();
    }

    /// <summary>
    /// 将总秒数转换为天:时的形式
    /// </summary>
    /// <param name="timeValue">总秒数</param>
    /// <param name="Dsuffix">“天”后的补充</param>
    /// <param name="DifAddZero">是否为“天”补充0</param>
    /// <param name="Hsuffix">“时”后的补充</param>
    /// <param name="HifAddZero">是否为“时”补充0</param>
    /// <returns>修改结果</returns>
    public static string DHConvert(this int timeValue, string Dsuffix = " Days ", bool DifAddZero = true, string Hsuffix = " Hours", bool HifAddZero = true)
    {
        if (sb == null)
            sb = new StringBuilder(16);
        else
            sb.Clear();

        if (timeValue < 0)
        {
            Debug.LogWarning("The time value cannot be less than 0");
            sb.Append(DifAddZero ? "00" : "0");
            sb.Append(Dsuffix);
            sb.Append(HifAddZero ? "00" : "0");
            sb.Append(Hsuffix);
            return sb.ToString();
        }

        int value = timeValue + 60;

        int day = value / 86400;
        string dZero = DifAddZero && day < 10 ? "0" : "";
        sb.Append(dZero);
        sb.Append(day);
        sb.Append(Dsuffix);

        int hour = (value % 86400) / 3600;
        string hZero = HifAddZero && hour < 10 ? "0" : "";
        sb.Append(hZero);
        sb.Append(hour);
        sb.Append(Hsuffix);

        return sb.ToString();
    }

    #endregion

    #region 添加分隔符

    /// <summary>
    /// 将int分隔
    /// </summary>
    /// <param name="intValue">指定的int值</param>
    /// <param name="integerLength">整数部分的分隔长度，“<0”不显示，“=0”原样，“>0”按位数添加</param>
    /// <param name="separator">分隔符</param>
    /// <returns>分隔后的字符串</returns>
    public static string SeparateConvert(this int intValue, int integerLength = 3, string separator = ",")
    {
        if (sb == null)
            sb = new StringBuilder(16);

        bool ifNegative = intValue < 0;
        if (ifNegative)
            intValue *= -1;

        string[] str = intValue.ToString().Split(".");
        string integerStr = str[0];


        sb.Clear();
        int sepNum = 0;
        sepNum = integerStr.Length % integerLength;
        for (int i = 0; i < integerStr.Length; i++)
        {
            if (i == sepNum)
            {
                sepNum += integerLength;
                if (i != 0)
                    sb.Append(separator);
            }

            sb.Append(integerStr[i]);
        }

        return $"{(ifNegative ? "-" : "")}{sb}";
    }

    #endregion

}
