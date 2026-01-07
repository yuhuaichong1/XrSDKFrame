using System;
using System.Collections.Generic;
using XrCode;

public class STimerManager : Singleton<STimerManager>, ILoad, IDispose
{
    public LinkedList<STimer> runingSTimer;//运行中的计时器
    public LinkedList<STimer> pauseSTimer;//暂停中的计时器
    public Stack<STimer> standbySTimer;//待机中（对象池中）的计时器

    public void Load()
    {
        runingSTimer = new LinkedList<STimer>();
        pauseSTimer = new LinkedList<STimer>();
        standbySTimer = new Stack<STimer>();
    }

    /// <summary>
    /// 创建计时器
    /// </summary>
    /// <param name="targetTime">目标结束时间</param>
    /// <param name="loopCount">循环次数</param>
    /// <param name="ifscale">是否被时间缩放影响</param>
    /// <param name="ifAutoPushPool">是否在计时结束后回收对象池</param>
    /// <param name="endAction">计时结束事件</param>
    /// <param name="updateAction">计时更新事件</param>
    /// <param name="timingActions">计时节点事件集合</param>
    /// <returns>创建完成的计时器</returns>
    public STimer CreateSTimer(float targetTime, int loopCount = 0, bool ifscale = false, bool ifAutoPushPool = true, Action endAction = null, Action<float> updateAction = null, params timingActions[] timingActions)
    {
        bool b = standbySTimer.Count != 0;
        STimer timer = b ? standbySTimer.Pop() : new STimer();
        timer.SetSTimeInfo(targetTime, loopCount, ifscale, ifAutoPushPool, endAction, updateAction, timingActions);
        timer.Start();
        return timer;
    }

    /// <summary>
    /// 创建简易的延迟执行计时器
    /// </summary>
    /// <param name="targetTime">目标结束时间</param>
    /// <param name="endAction">计时结束事件</param>
    /// <returns>创建完成的计时器</returns>
    public STimer CreateSDelay(float targetTime, Action endAction)
    {
        return(CreateSTimer(targetTime, 0, false, true, endAction));
    }

    /// <summary>
    /// 创建简易的反复执行计时器
    /// </summary>
    /// <param name="targetTime">目标结束时间</param>
    /// <param name="updateAction">计时更新事件</param>
    /// <returns>创建完成的计时器</returns>
    public STimer CreateSUpdate(float targetTime, Action<float> updateAction)
    {
        return (CreateSTimer(targetTime, 0, false, true, null, updateAction));
    }

    /// <summary>
    /// 清除计时器
    /// </summary>
    public void ClearSTimer()
    {
        runingSTimer.Clear();
        pauseSTimer.Clear();
        standbySTimer.Clear();
    }

    /// <summary>
    /// 更新计时器
    /// </summary>
    private void UpdateSTimer()
    {
        LinkedListNode<STimer> current = runingSTimer.Last;
        while (current != null)
        {
            STimer timer = current.Value;
            switch (timer.STimerState)
            {
                case STimerState.Running:
                    timer.Update();
                    break;
                case STimerState.Pause:
                    pauseSTimer.AddLast(timer);
                    runingSTimer.Remove(current);
                    break;
                case STimerState.Standby:
                    standbySTimer.Push(timer);
                    runingSTimer.Remove(current);
                    break;
            }
            current = current.Previous;
        }
    }

    public void UpdateInstance()
    {
        UpdateSTimer();
    }

    public void Dispose()
    {
        ClearSTimer();
    }
}

