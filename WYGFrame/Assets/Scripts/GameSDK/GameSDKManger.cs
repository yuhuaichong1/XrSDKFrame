using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XrCode
{
    public class GameSDKManger : Singleton<GameSDKManger>, ILoad
    {
        SDKBase sdkBase;

        public SDKBase SdkBase { get => sdkBase; set => sdkBase = value; }

        public void Load()
        {

        }
        public void StartUp()
        {
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
            sdkBase = new EditorSDK();
#elif WX
        sdkBase = new WeChatSDK();
#endif
            sdkBase.Init();
        }
        public bool CheckDirectoryExists(string path)
        {
            return sdkBase.CheckDirectoryExists(path);//检查文件夹是否存在
        }
        public void CreateDirectoryMkdir(string path)
        {
            sdkBase.CreateDirectoryMkdir(path);//创建文件夹
        }
        public void DeleteFile(string path)
        {
            sdkBase.DeleteFile(path);//删除一个文件
        }
        public void CreatFile(string path)
        {
            sdkBase.CreatFile(path);//创建一个文件
        }
        public bool CheckFileExists(string path)
        {
            return sdkBase.CheckFileExists(path); //检查一个文件是否存在
        }
        public void WriteFileSync(string path, string content)
        {
            sdkBase.WriteFileSync(path, content);//同步写入文件
        }
        public void WriteFileSync(string path, byte[] content)
        {
            sdkBase.WriteFileSync(path, content);//同步写入文件
        }
        public void ReadFileSync(string path, Action<string> action = null)
        {
            sdkBase.ReadFileSync(path, action);//读取文件
        }
        public void ReadFileSync(string path, Action<byte[]> action = null)
        {
            sdkBase.ReadFileSync(path, action);//读取文件
        }
        public void ShowVideoAd(string adId, Action<bool, int> closeCallBack, Action<int, string> errorCallBack)
        {
            sdkBase.ShowVideoAd(adId, closeCallBack, errorCallBack);//展示广告
        }
        public void DeleteDirectory(string path)
        {
            sdkBase.DeleteDirectory(path);
        }

    }
}