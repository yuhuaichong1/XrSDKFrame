using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XrCode
{
    public abstract class SDKBase
    {
        public abstract void Init();//初始化
        public abstract bool CheckDirectoryExists(string path);//检查文件夹是否存在
        public abstract void CreateDirectoryMkdir(string path);//创建文件夹
        public abstract void DeleteDirectory(string path);//删除一个文件夹
        public abstract void CreatFile(string path);//创建文件
        public abstract void DeleteFile(string path);//删除一个文件
        public abstract bool CheckFileExists(string path);//检查一个文件是否存在
        public abstract void WriteFileSync(string path, string content);//同步写入文件
        public virtual void WriteFileSync(string path, byte[] content) { }//同步写入文件
        public abstract void ReadFileSync(string path, Action<string> action = null);//读取文件
        public virtual void ReadFileSync(string path, Action<byte[]> action = null) { }//读取文件
        public abstract void ShowVideoAd(string adId, Action<bool, int> closeCallBack, Action<int, string> errorCallBack);//展示广告

    }


}
