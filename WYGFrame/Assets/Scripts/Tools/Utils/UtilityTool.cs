using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;
namespace XrCode
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class UtilityTool
    {
        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="code_type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string DecodeByBase64(string content)
        {
            string result = "";
            byte[] bytes = Convert.FromBase64String(content);
            try
            {
                result = Encoding.GetEncoding("utf-8").GetString(bytes);
            }
            catch
            {
                result = content;
            }
            return result;
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="codeType"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string EncodeBase64(string content)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(content);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = content;
            }
            return encode;
        }

        public static bool isOutTime(long timeStamp, int day)
        {

            return true;
        }

        public static DateTime GetCurrentTime()
        {
            DateTime time = DateTime.Today;
            return time;
        }

        /// <summary>
        /// 获取相对时间文本
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="tsType"></param>
        /// <returns></returns>
        public static string GetRelativeTime(long timeStamp)
        {
            DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(timeStamp).ToLocalTime();
            TimeSpan ts = DateTime.Now.Subtract(time).Duration();
            //dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";

            if (ts.Days < 1)
            {
                return "今天";
            }
            else if (ts.Days < 30)
            {
                return string.Format("{0}天前", ts.Days);
            }
            else if (ts.Days < 365)
            {
                int month = ts.Days / 30;
                return string.Format("{0}月前", month);
            }
            return "很久以前";
        }
        public static int GetDayBySecond(int second)
        {
            return second / 86400;
        }

        //获取时间戳的年月日
        public static string GetDateByTimeStamp(long timeStamp)
        {
            DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(timeStamp).ToLocalTime();
            string showTime = time.Year + "年" + time.Month + "月" + time.Day + "日";
            return showTime;
            return time.ToLongDateString();
        }
        /// <summary>
        /// 验证是否为yyyy-mm-dd的时间格式
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        //public static bool ValidateDataTime(string text)
        //{
        //    //if (text.Length > 0)
        //    //{
        //    //    if (Regex.IsMatch(text.Trim(), @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$"))
        //    //    {
        //    //        string[] arr = text.Split('-');
        //    //        if (int.Parse(arr[0]) < 1900)
        //    //        {
        //    //            GameDebug.LogError("年龄大了");
        //    //        }
        //    //        return true;
        //    //    }
        //    //    else
        //    //    {
        //    //        return false;
        //    //    }
        //    //}
        //    //return false;
        //}


        public static int GetScaneContentId(string argContent)
        {
            // "{ \"type\": 1,\"company\": 900000 }";
            Dictionary<string, object> dic = MiniJson.Json.Deserialize(argContent) as Dictionary<string, object>;
            int comType = int.Parse(dic["type"].ToString());
            int n_contentId = 0;
            switch (comType)
            {
                case 1:
                    n_contentId = int.Parse(dic["company"].ToString());
                    //var data = PrototypeModule.Instance.GetData<CompanyData>(EPropertyType.CompanyData, n_contentId);

                    break;
                case 2:

                    break;
            }
            return n_contentId;
        }

        //填写生日的
        public static string GetBirthdayTimeStr(string inputText)
        {
            var charList = inputText.ToCharArray();
            List<string> tempcharList = new List<string>();

            for (int i = 0; i < charList.Length; i++)
            {
                var tempchar = charList[i];
                if (tempchar != '-')
                {
                    tempcharList.Add(charList[i].ToString());
                    if (tempcharList.Count == 4 || tempcharList.Count == 7)
                    {
                        tempcharList.Add("-");
                    }
                }
            }
            string tempStr = string.Join("", tempcharList.ToArray());
            return tempStr;
        }

        // 获取时间格式文本 -- 包含验证时间是否合法,标准格式 1990-10-9
        // 验证不合法返回null
        public static string GetDateTimeContent(string dtText, char c)
        {
            string dtContent = dtText;
            //检查是否为标准格式
            if (dtContent.Contains(c.ToString()))
            {
                string[] arr = dtContent.Split(c);
                if (arr.Length == 3)
                {
                    //验证是否为整数（包含非法字符无法转成int）
                    int year, month, day;
                    if (int.TryParse(arr[0], out year))
                    {
                        string strMonth = AmendTwoNumbleWithZero(arr[1]);
                        if (int.TryParse(strMonth, out month))
                        {
                            string strDay = AmendTwoNumbleWithZero(arr[2]);
                            if (int.TryParse(strDay, out day))
                            {
                                //验证日期数字是否非法
                                if (ValidateDateTime(year, month, day))
                                {
                                    string monthText = EnsureStringIsTwoLen(month);
                                    string dayText = EnsureStringIsTwoLen(day);
                                    return string.Format("{0}-{1}-{2}", year, monthText, dayText);
                                }
                            }
                        }
                    }
                }
            }
            D.Log("请使用样例格式输入出生日期");
            return null;
        }
        /// <summary>
        /// 保证数字的格式必须为两位，1位补0
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private static string EnsureStringIsTwoLen(int num)
        {
            if (num < 10)
            {
                return string.Format("0{0}", num);
            }
            return num.ToString();
        }

        /// <summary>
        /// 为了后续正常转整形，修正两位数中存在0的情况
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private static string AmendTwoNumbleWithZero(string num)
        {
            if (num.Length == 2)
            {
                if (num.StartsWith("0"))
                {
                    num = num.Remove(0, 1);
                }
            }
            return num;
        }

        //设置日期
        public static bool ValidateDateTime(int year, int month, int day)
        {
            //基础验证
            int curYear = DateTime.Now.Year;
            if (year < 1900 || year > curYear - 3) return false;
            if (month < 1 || month > 12) return false;
            if (day < 1 || day > 31) return false;
            //具体日期验证
            int maxDay = 31;
            if (month == 2)
            {
                maxDay = 28;
                if (year % 4 == 0)
                    maxDay = 29;
            }
            else
            {
                //检测是不是31天的月份
                List<int> monthList = new List<int>() { 1, 3, 5, 7, 8, 10, 12 };
                if (!monthList.Contains(month))
                {
                    maxDay = 30;
                }
            }
            if (day > maxDay) return false;
            return true;
        }

        public static float GetRoleStep()
        {

            return 1;
        }
        /// <summary>
        /// 输入是否为手机号
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public static bool IsHandset(string str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"13\d{9}|14[57]\d{8}|15[012356789]\d{8}|18[01256789]\d{8}|17[0678]\d{8}");
        }
    }

}