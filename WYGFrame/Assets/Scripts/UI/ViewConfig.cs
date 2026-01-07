

using System;
using System.Collections.Generic;

namespace XrCode
{
    public class ViewConfig
    {
        /// <summary>
        /// 类型和Prefab路径的对应
        /// </summary>
        private static Dictionary<Type, string> mTypePathDict;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            mTypePathDict = new Dictionary<Type, string>();
            mTypePathDict.Add(typeof(LoadingView), "Prefabs/Views/Common/LoadingView.prefab");
        }

        /// <summary>
        /// 获得UI路径
        /// </summary>
        public static string GetViewPath(Type type)
        {
            string path = string.Empty;
            mTypePathDict.TryGetValue(type, out path);
            return path;
        }
    }
}