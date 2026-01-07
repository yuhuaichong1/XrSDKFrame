

using System.Collections.Generic;
using UnityEngine;

namespace XrCode
{
    /**
     * 版本配置文件
     */
    [System.Serializable]
    public class VersionConfig
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version;
        /// <summary>
        /// 对应版本的更新包大小
        /// </summary>
        public float Size;
        public Dictionary<string, Hash128> assetMD5;
    }
}