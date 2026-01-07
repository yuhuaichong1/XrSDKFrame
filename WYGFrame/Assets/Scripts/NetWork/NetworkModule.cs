using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace XrCode
{
    //网络模块类
    public class NetworkModule : Singleton<NetworkModule>, ILoad, IDispose
    {
        static readonly object m_lockObject = new object();
        private Dictionary<string, List<NotifyHandle>> dicNotifyData;

        //消息发送间隔
        private float msgDeltaTime = .5f;
        //当前累计时间
        private float AccumulatedTime = 0;
        //待发送消息队列锁
        private object cacheMsgObj = new object();
        //待发送队列
        private Queue<Notification> sendMsgQue;
        private List<int> recordCodeId;

        private string sessionId = "";
        private string accountId = "0";
        public System.Action OnFinished;        //初始加载完成回调

        public void Load()
        {
            sendMsgQue = new Queue<Notification>();
            recordCodeId = new List<int>();
            dicNotifyData = new Dictionary<string, List<NotifyHandle>>();
        }

        //释放
        public void Dispose()
        {
            Debug.Log("~NetworkManager was destroy");
        }

        public void Update()
        {
            CheckCacheMessage();
        }

        /// <summary>
        /// 发送网络消息
        /// </summary>
        /// <param name="notify"></param>
        public void SendMessage(Notification notify)
        {
            sendMsgQue.Enqueue(notify);
        }
        public void SetAccountId(string accountId)
        {
            this.accountId = accountId;
        }
        public void SetSessionId(string sessionId)
        {
            this.sessionId = sessionId;
        }

        /// <summary>
        /// 注册账户信息
        /// </summary>
        /// <param name="sessionId">sessionId</param>
        /// <param name="accountId">accountId</param>
        public void RegisterAccountInfo(string accountId, string sessionId)
        {
            this.accountId = accountId;
            this.sessionId = sessionId;
        }


        #region ToServer

        public void CheckCacheMessage()
        {
            int limitCount = 10;
            while (limitCount > 0 && sendMsgQue.Count > 0)
            {
                lock (cacheMsgObj)
                {
                    Notification notify = sendMsgQue.Dequeue();
                    Game.Instance.StartCoroutine(SendMessageToServer(notify));
                }
            }
        }
        //发送消息到服务器
        private IEnumerator SendMessageToServer(Notification notify)
        {
            string url = NotifyDefine.GetNetMessageUrl(notify.Sid);
            using (UnityWebRequest webReq = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
            {
                D.Log($"::::::: to server:  {notify.Sid}");
                byte[] postBytes = Encoding.UTF8.GetBytes(notify.Content.ToString());
                webReq.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
                webReq.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                webReq.SetRequestHeader("Content-Type", "application/json");
                webReq.timeout = 30;
                yield return webReq.SendWebRequest();

                if (webReq.isDone)
                {
                    if (recordCodeId.Contains(notify.Sid)) recordCodeId.Remove(notify.Sid);
                    if (webReq.result == UnityWebRequest.Result.Success)
                    {
                        RecieveMessage(webReq.downloadHandler.text);
                    }
                    else
                    {

                        Debug.LogError(":::::: server page can't be found. :::::: " + webReq.responseCode);
                        Debug.LogError(":::::: UnityWebRequest  error :::::: " + webReq.error);
                        //UIManager.Instance.OpenTextTips("请求服务器错误,请重试。");
                    }
                }
            }
        }

        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="msg">数据</param>
        public void RecieveMessage(string msg)
        {
            //string content = UtilityTool.DecodeByBase64(msg.Trim('"'));
            //D.Log("::::::: server return: " + msg);
            string content = msg.Replace("\n", "");
            //仅删除最外层"[]"，不影响结构内，以此修改json数据格式
            if (content.StartsWith("["))
            {
                content = content.Remove(0, 1);
            }
            if (content.EndsWith("]"))
            {
                content = content.Remove(content.Length - 1, 1);
            }
            Notification notify = new Notification();
            notify.Content = content;
            notify.Sid = int.Parse(notify["msgId"].ToString());
            notify.name = NotifyDefine.GetNetMessageName(notify.Sid);
            ModuleMgr.Instance.NotifyMod.SendNotify(notify);
        }

        #endregion

        #region 网络请求
        public void GetNetworkInitInfo()
        {
            Notification notify = new Notification();
            Game.Instance.StartCoroutine(GetInfoformServer(notify));
        }

        private IEnumerator GetInfoformServer(Notification notify)
        {
            string url = GameDefines.URL;
            //using (UnityWebRequest webReq = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
            using (UnityWebRequest webReq = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET))
            {
                byte[] postBytes = Encoding.UTF8.GetBytes(notify.Content.ToString());
                webReq.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
                webReq.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                webReq.SetRequestHeader("Content-Type", "application/json");
                webReq.timeout = 20;
                Debug.LogError(":::::: 请求服务器数据 :::::: ");
                yield return webReq.SendWebRequest();

                if (webReq.isDone)
                {
                    if (recordCodeId.Contains(notify.Sid)) recordCodeId.Remove(notify.Sid);
                    if (webReq.result == UnityWebRequest.Result.Success)
                    {
                        Debug.LogError(":::::: 获取到服务器数据 :::::: ");
                        ReciveveMessage(webReq.downloadHandler.text);
                        OnFinished?.Invoke();
                    }
                    else
                    {
                        Debug.LogError(":::::: server page can't be found. :::::: " + webReq.responseCode);
                        Debug.LogError(":::::: UnityWebRequest  error :::::: " + webReq.error);

                        TimerManager.Instance.CreateTimer(7, () =>
                        {
                            Debug.LogError(":::::: 重新连接 :::::: ");
                            GetNetworkInitInfo();
                        }, 0);
                    }
                }
            }
        }

        /// <summary>
        /// 解析后台结果
        /// </summary>
        /// <param name="msg">后台返回值</param>
        public void ReciveveMessage(string msg) 
        {
            //msg的消息为：{"code":200,"msg":"Success","data":true,"ext":"{\"adtime\":\"6000\",\"firstCount\":\"3\",\"sedCount\":\"2\",\"LevelWinPlayAd\":false}"}

            string content = msg.Replace("\n", "");
            if (content.StartsWith("["))
            {
                content = content.Remove(0, 1);
            }
            if (content.EndsWith("]"))
            {
                content = content.Remove(content.Length - 1, 1);
            }
            Notification notify = new Notification();
            notify.Content = content;

            
            //游戏IAA判断
            if (notify["data"] != null)
            {
                bool iaa = bool.Parse(notify["data"].ToString());
                GameDefines.ifIAA = iaa;
            }

            Dictionary<string, object> iaaDic = notify["data"] as Dictionary<string, object>;
            if (notify["ext"] != null)
            {
                Notification notify1 = new Notification();
                notify1.Content = notify["ext"];
                Dictionary<string, object> adDic = notify1.DicData;

                // 插屏广告间隔时间
                //if (adDic.TryGetValue("adtime", out object at))
                //{
                //    GameDefines.adtime = int.Parse(at.ToString());
                //}
            }
        }

        #endregion
    }
}