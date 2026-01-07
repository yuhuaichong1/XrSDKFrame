/**定时器管理器
 */

using System;
using System.Collections.Generic;
using UnityEngine;
namespace XrCode
{

    public class TimerManager : Singleton<TimerManager>, ILoad, IDisposable
    {
        /// <summary>
        /// 所有定时器集合
        /// </summary>
        private HashSet<Timer> mTimerHashSet;
        /// <summary>
        /// 要删除的定时器列表
        /// </summary>
        private List<Timer> mRemoveTimerList;
        /// <summary>
        /// 定时器对象池缓存
        /// </summary>
        private ClassObjectPool<Timer> mTimerPool;

        public void Load()
        {
            mTimerHashSet = new HashSet<Timer>();
            mRemoveTimerList = new List<Timer>();
            mTimerPool = ClassObjectManager.Instance.GetOrCreateClassPool<Timer>();
        }

        /// <summary>
        /// 创建定时器
        /// </summary>
        /// <param name="duration">时间间隔</param>
        /// <param name="finishAction">更新回调</param>
        /// <param name="loop">循环次数</param>
        /// <param name="unScale">时间是否缩放</param>
        public Timer CreateTimer(float duration, Action finishAction, int loop = 1, bool unScale = false)
        {

            var timer = mTimerPool.Spawn();
            timer.Init(duration, finishAction, loop, unScale);
            timer.Start();
            mTimerHashSet.Add(timer);
            return timer;
        }

        public void Dispose()
        {
            mTimerHashSet.Clear();
            mRemoveTimerList.Clear();
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            foreach (var timer in mTimerHashSet)
            {
                timer.Update();
                if (timer.IsOver) mRemoveTimerList.Add(timer);
            }
            foreach (var timer in mRemoveTimerList)
            {
                mTimerHashSet.Remove(timer);
                mTimerPool.Recycle(timer);
            }
            mRemoveTimerList.Clear();
        }
    }
}