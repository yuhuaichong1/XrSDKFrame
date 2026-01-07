/**
 * Mono单例
 */

using UnityEngine;

namespace XrCode
{

    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>, ILoad
    {
        private static T mInstance;

        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    var go = new GameObject(typeof(T).Name);
                    mInstance = go.AddComponent<T>();
                    GameObject.DontDestroyOnLoad(go);
                    mInstance.Load();
                }
                return mInstance;
            }
        }

        public void Destroy()
        {
            mInstance = null;
        }

    }
}