using System;
using System.IO;
using UnityEngine;
namespace XrCode
{
    public class EditorSDK : SDKBase
    {
        private string filepath;
        public override void Init()
        {
#if UNITY_EDITOR
            filepath = Application.streamingAssetsPath;
#elif UNITY_ANDROID && !UNITY_EDITOR
        filepath = Application.persistentDataPath;
#endif
        }
        public override bool CheckDirectoryExists(string path)
        {
            D.Log("检查" + filepath + "/" + path + "是否存在");
            return Directory.Exists(filepath + "/" + path);
        }

        public override bool CheckFileExists(string path)
        {
            D.Log("检查" + filepath + "/" + path + "文件是否存在");
            return File.Exists(filepath + "/" + path);
        }
        public override void CreatFile(string path)
        {
            if (!File.Exists(path))
            {
                D.Log("创建" + filepath + "/" + path + "文件");
                using (FileStream fs = File.Create(filepath + "/" + path))
                {

                }
            }
        }
        public override void WriteFileSync(string path, byte[] content)
        {
            File.WriteAllBytes(filepath + "/" + path, content);
        }
        public override void DeleteDirectory(string path)
        {
            Directory.Delete(filepath + "/" + path, true); // true ��ʾ�ݹ�ɾ��
        }
        public override void CreateDirectoryMkdir(string path)
        {
            D.Log("创建" + filepath + "/" + path + "文件夹");
            Directory.CreateDirectory(filepath + "/" + path);
        }

        public override void DeleteFile(string path)
        {
            D.Log("删除" + filepath + "/" + path + "文件");
            File.Delete(filepath + "/" + path);
        }
        public override void ReadFileSync(string path, Action<string> action = null)
        {
            string s = File.ReadAllText(filepath + "/" + path);
            action?.Invoke(s);
        }
        public override void ReadFileSync(string path, Action<byte[]> action = null)
        {
            byte[] s = File.ReadAllBytes(filepath + "/" + path);
            D.Log("读取" + filepath + "/" + path + "::" + s);
            action?.Invoke(s);
        }
        public override void ShowVideoAd(string adId, Action<bool, int> closeCallBack, Action<int, string> errorCallBack)
        {
            closeCallBack?.Invoke(true, 0);
        }

        public override void WriteFileSync(string path, string content)
        {
            //D.Log("写入" + filepath + "/" + path + "::" + content);
            File.WriteAllText(filepath + "/" + path, content);
        }
    }
}