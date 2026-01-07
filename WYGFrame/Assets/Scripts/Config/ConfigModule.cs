using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Bright.Serialization;
using UnityEngine;
using UnityEngine.Networking;

namespace XrCode
{
    public class ConfigModule : Singleton<ConfigModule>, ILoad
    {
        public cfg.Tables Tables;
        public System.Action OnFinished;        //初始加载完成回调

        public void Load() { }

        public void StartUp()
        {
#if UNITY_EDITOR || UNITY_EDITOR_WIN
            if (!AppConfig.LoadAssetWithServer)
            {
                Tables = new cfg.Tables(LoadByteBuf);
                OnFinish();
            }
            else
            {
                Tables = new cfg.Tables(OnFinish);
            }
#elif UNITY_ANDROID || UNITY_IOS
        Tables = new cfg.Tables(OnFinish);
#endif
        }

        private static ByteBuf LoadByteBuf(string file)
        {
            string path = file.ToLower().Replace('.', '_');
            byte[] bytes;
            if (!AppConfig.LoadAssetWithResources)
            {
                bytes = File.ReadAllBytes($"{Application.streamingAssetsPath}/Data/{path}.bytes");
            }
            else
            {
                bytes = Resources.Load<TextAsset>($"Data/{path}").bytes;
            }
            return new ByteBuf(bytes);
        }

        private void OnFinish()
        {
            OnFinished?.Invoke();
        }


    }
}