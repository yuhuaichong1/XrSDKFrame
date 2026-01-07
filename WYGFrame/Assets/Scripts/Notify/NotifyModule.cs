using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace XrCode
{
    /// <summary>
    /// _yhc 通知管理器
    /// </summary>
    public class NotifyModule : BaseModule
    {
        private Dictionary<string, List<NotifyHandle>> dicNotifyData;
        protected override void OnLoad()
        {
            dicNotifyData = new Dictionary<string, List<NotifyHandle>>();
        }
        /// <summary>
        /// 注册消息监听
        /// </summary>
        /// <param name="codeId">消息号</param>
        /// <param name="handle"></param>
        public void RegisterNotification(int codeId, NotifyHandle handle)
        {
            string name = NotifyDefine.GetNetMessageName(codeId);
            RegisterNotification(name, handle);
        }

        /// <summary>
        /// 注册通知消息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="handle"></param>
        public void RegisterNotification(string name, NotifyHandle handle)
        {
            List<NotifyHandle> handleList = null;
            if (dicNotifyData.TryGetValue(name, out handleList))
            {
                handleList.Add(handle);
            }
            else
            {
                handleList = new List<NotifyHandle>();
                handleList.Add(handle);
                dicNotifyData.Add(name, handleList);
            }
        }
        /// <summary>
        /// 移除消息监听
        /// </summary>
        /// <param name="codeId">消息号</param>
        /// <param name="handle"></param>
        public void RemoveNotification(int codeId, NotifyHandle handle)
        {
            string name = NotifyDefine.GetNetMessageName(codeId);
            RemoveNotification(name, handle);
        }
        /// <summary>
        /// 移除消息监听
        /// </summary>
        /// <param name="name">消息名</param>
        /// <param name="handle"></param>
        public void RemoveNotification(string name, NotifyHandle handle)
        {
            if (!dicNotifyData.ContainsKey(name))
                return;
            List<NotifyHandle> handleList = dicNotifyData[name];
            if (handleList.Contains(handle))
            {
                handleList.Remove(handle);
            }
            if (handleList.Count <= 0)
            {
                dicNotifyData.Remove(name);
            }
        }
        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="isNet">是否网络消息</param>
        public void SendNotify(Notification notification, bool isNet = false)
        {
            if (isNet)
                SendNetNotify(notification);
            else
                SendLocalNotify(notification);
        }
        //发送本地通知
        private void SendLocalNotify(Notification notification)
        {
            if (!dicNotifyData.ContainsKey(notification.name))
            {
                Debug.LogError("没有任何位置在监听此消息：" + notification.Sid);
            }
            else
            {
                List<NotifyHandle> handleList = dicNotifyData[notification.name];
                for (int i = 0; i < handleList.Count; i++)
                {
                    NotifyHandle handle = handleList[i];
                    if (handle != null)
                    {
                        handle(notification);
                    }
                    else
                        handleList.Remove(handle);
                }
            }
        }
        protected override void OnDispose()
        {
            foreach (var item in dicNotifyData)
            {
                item.Value.Clear();
            }
            dicNotifyData.Clear();
        }
        //发送网络消息
        private void SendNetNotify(Notification notification)
        {
            NetworkModule.Instance.SendMessage(notification);
        }
    }
}