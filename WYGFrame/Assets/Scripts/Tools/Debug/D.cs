namespace XrCode
{
    public class D
    {
        /// <summary>
        /// 日志输出
        /// </summary>
        public static void Log(object message)
        {
            if (!GameDefines.ifDebug) return;
            UnityEngine.Debug.Log(message);
        }

        /// <summary>
        /// 带参输出
        /// </summary>
        public static void Log(string message, params object[] args)
        {
            if (!GameDefines.ifDebug) return;
            UnityEngine.Debug.Log(string.Format(message, args));
        }

        /// <summary>
        /// 根据结果打印消息
        /// </summary>
        public static void Log(bool err, string message)
        {
            if (!GameDefines.ifDebug) return;
            if (err) Error(message);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        public static void Error(string message, params object[] args)
        {
            if (!GameDefines.ifDebug) return;
            UnityEngine.Debug.LogError(string.Format(message, args));
        }

        public static void Error(string message)
        {
            if (!GameDefines.ifDebug) return;
            UnityEngine.Debug.LogError(message);
        }
    }
}