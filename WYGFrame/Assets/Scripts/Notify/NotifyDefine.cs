using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XrCode
{

    #region 事件 -- 这里集成的功能可能不需要了，使用Action Func 更为简洁易读

    /////////////////////////////////////////////// - 事件类型 - begin ////////////////////////////////////////////////

    public delegate void NotifyHandle(Notification notify);

    /////////////////////////////////////////////// - 事件类型 -  end  ////////////////////////////////////////////////
    #endregion

    /// <summary>
    /// 实际此处应该配表
    /// </summary>
    public static class NotifyDefine
    {

        #region network notify msg

        public static Dictionary<int, NetMessageUnit> nMsgDic = new Dictionary<int, NetMessageUnit>()
    {
        { (int)EMsgCode.ES2C_Login, new NetMessageUnit () { codeId = (int)EMsgCode.ES2C_Login,handleName = "ResponseLoginHandle",url = "http://172.16.20.5:6008/user/insertOrUpdateUser",sendType = "Post"} },
    };

        public static NetMessageUnit GetNetMessageUnit(int code)
        {
            for (int i = 0; i < nMsgDic.Count; i++)
            {
                if (nMsgDic[i].codeId == code)
                {
                    return nMsgDic[i];
                }
            }
            UnityEngine.Debug.LogError("未找到相关网络消息协议配置");
            return null;
        }

        public static string GetNetMessageName(int codeId)
        {
            NetMessageUnit unit;
            nMsgDic.TryGetValue(codeId, out unit);
            if (unit != null)
            {
                return unit.handleName;
            }
            return "GLOBELERROR";
        }

        public static string GetNetMessageUrl(int codeId)
        {
            NetMessageUnit unit;
            if (nMsgDic.TryGetValue(codeId + 1, out unit))
            {
                return unit.url;
            }
            return "";
        }

        #endregion
    }

    /// <summary>
    /// 网络消息单位
    /// </summary>
    public class NetMessageUnit
    {
        public int codeId;                      //消息号
        public string handleName;               //消息名
        public string url;                      //请求地址
        public string sendType = "Post";        //请求类型
    }

}