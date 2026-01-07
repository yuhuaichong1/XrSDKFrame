using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace XrCode
{
    /// <summary>
    /// 消息体
    /// </summary>
    public class Notification : IEnumerable<KeyValuePair<string, object>>
    {
        //消息名
        public string name { get; set; }
        //消息号
        public int Sid { get; set; }
        public Notification() { }
        public Notification(int id)
        {
            this.Sid = id;
            DicData.Add("msgId", this.Sid);
        }
        public Notification(string name)
        {
            this.name = name;
        }
        //发送者
        public object sender { get; set; }
        private int frameId;
        //数据字典
        private Dictionary<string, object> dicData;
        public Dictionary<string, object> DicData
        {
            get
            {
                if (dicData == null)
                {
                    dicData = new Dictionary<string, object>();
                }
                return dicData;
            }
        }
        public object Content
        {
            set { dicData = DeserializeDicToJson<string, object>(value.ToString()); }
            get
            {
                if (dicData == null) return "";
                return SerializeDicToJson<string, object>(dicData);
            }
        }

        public object this[string key]
        {
            get
            {
                object obj = null;
                dicData.TryGetValue(key, out obj);
                return obj;
            }
            set
            {
                if (dicData == null) dicData = new Dictionary<string, object>();
                if (!dicData.ContainsKey(key))
                {
                    dicData.Add(key, value);
                }
                else dicData[key] = value;
            }
        }

        #region 实现IEunmerable接口成员
        public IEnumerator GetEnumerator()
        {
            yield return dicData.GetEnumerator();
            //return   dicData.GetEnumerator();
        }
        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            if (dicData == null) yield break;
            foreach (KeyValuePair<string, object> item in dicData)
            {
                yield return item;
            }
        }
        private string SerializeDicToJson<TKey, TValue>(Dictionary<TKey, TValue> dic)
        {
            string json = "";
            if (dic != null && dic.Count != 0)
            {
                json = MiniJson.Json.Serialize(dic);
            }
            return json;
        }
        private Dictionary<TKey, TValue> DeserializeDicToJson<TKey, TValue>(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return new Dictionary<TKey, TValue>();
            }
            //Dictionary<TKey, TValue> dic1 = JsonUtility.FromJson<Dictionary<TKey, TValue>>(content);
            Dictionary<TKey, TValue> dic = MiniJson.Json.Deserialize(content) as Dictionary<TKey, TValue>;
            return dic;
        }
    }
    #endregion


}