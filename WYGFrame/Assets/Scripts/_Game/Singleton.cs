
namespace XrCode
{
    //单例
    public abstract class Singleton<T> where T : ILoad, new()
    {
        private static object obj = new object();
        private static T mInstance;

        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (obj)
                    {
                        mInstance = new T();
                        mInstance.Load();
                    }
                }
                return mInstance;
            }
        }

        protected virtual void DestroyInstance()
        {
            mInstance = default;
        }
    }
}