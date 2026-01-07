using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace XrCode
{
    //�ýű��ǲ����Ѿ������ˣ�
    //�ýű��ǲ����Ѿ������ˣ�
    //�ýű��ǲ����Ѿ������ˣ�
    public class NetModule : BaseModule
    {

        protected override void OnLoad()
        {
            base.OnLoad();
        }

        public void Login()
        {
            LoginData data = new LoginData() { id = "151515678", name = "����" };
            string json = JsonUtility.ToJson(data);
            Game.Instance.StartCoroutine(SendToServer(json, responseLogin));

        }

        public void responseLogin(string result)
        {
            LoginData data = JsonUtility.FromJson<LoginData>(result);
            D.Log(data.id);
        }

        private IEnumerator SendToServer(string data, Action<string> callBack)
        {
            string url = "http://172.16.20.5:6008/user/insertOrUpdateUser";
#if UNITY_2022_3_OR_NEWER
            UnityWebRequest request = UnityWebRequest.PostWwwForm(url, data);

#else
            UnityWebRequest request = UnityWebRequest.Post(url, data);
    
#endif
            request.timeout = 10;
            yield return request.SendWebRequest();
            D.Log(" _____________________ �������� _______________________ ");
            if (request.result == UnityWebRequest.Result.Success)
            {
                if (request.downloadHandler.data != null)
                {
                    string downloadData = Encoding.UTF8.GetString(request.downloadHandler.data);

                    callBack?.Invoke(downloadData);
                }
            }
            callBack?.Invoke(null);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }

    [Serializable]
    public class LoginData
    {
        public string id;
        public string name;
    }
}