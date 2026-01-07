using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class STimer
{
    public float nowTime;//当前时间
    public float targetTime;//目标结束时间

    public Action endAction;//计时结束事件
    public Action<float> updateAction;//计时更新事件
    public List<timingActions> timingActions;//计时节点事件集合
    public List<timingActions> onceActions;//执行过的一次性计时节点事件集合

    public bool iftiming;//是否在计时
    public int loopCount;//循环次数，0代表不循环，-1（小于0）代表无限循环，其他大于0的的数代表有限次循环（比如1代表循环1次）
    public int currentlc;//当前的循环次数
    public bool ifscale;//是否收时间缩放影响
    public bool ifAutoPushPool;//是否在停止后自动归于对象池中（自动删除）

    private STimerState sTimerState;//当前计时器状态
    public STimerState STimerState {get { return sTimerState;}}

    private bool ifClose;//是否关闭

    public STimer()
    {
        timingActions = new List<timingActions>();
        onceActions = new List<timingActions>();
    }

    /// <summary>
    /// 设置计时器信息
    /// </summary>
    /// <param name="targetTime">目标结束时间</param>
    /// <param name="loopCount">循环次数</param>
    /// <param name="endAction">计时结束事件</param>
    /// <param name="updateAction">计时更新事件</param>
    /// <param name="timingActions">计时节点事件集合</param>
    public void SetSTimeInfo(float targetTime, int loopCount = 0, bool ifscale = false, bool ifAutoPushPool = true, Action endAction = null, Action<float> updateAction = null, params timingActions[] timingActions)
    {
        ifClose = false;
        nowTime = 0;
        this.targetTime = targetTime;
        this.loopCount = loopCount;
        this.ifscale = ifscale;
        this.ifAutoPushPool = ifAutoPushPool;
        this.endAction = endAction;
        this.updateAction = updateAction;
        this.timingActions.Clear();
        foreach (timingActions tAction in timingActions) 
        {
            this.timingActions.Add(tAction);
        }
        onceActions.Clear();
        currentlc = loopCount;

        if (this.timingActions != null)
        {
            this.updateAction += (nowTime) =>
            {
                for (int i = this.timingActions.Count - 1; i >= 0; i--)
                {
                    timingActions ca = this.timingActions[i];
                    switch (ca.clockActionType)
                    {
                        case ClockActionType.Once: 
                            if(nowTime >= ca.timing)
                            {
                                ca.clockAction?.Invoke(ca.timing);
                                onceActions.Add(ca);
                                this.timingActions.Remove(ca);
                            }
                            break;
                        case ClockActionType.Before: 
                            if(nowTime <= ca.timing)
                            {
                                ca.clockAction?.Invoke(nowTime);
                            }
                            break;
                        case ClockActionType.After: 
                            if(nowTime >= ca.timing)
                            {
                                ca.clockAction?.Invoke(nowTime);
                            }
                            break;
                    }
                }
            };
        }
    }

    /// <summary>
    /// 开始计时
    /// </summary>
    public void Start()
    {
        //if (CheckClose()) return;

        if (sTimerState != STimerState.Running)
        {
            STimerManager.Instance.runingSTimer.AddLast(this);
            STimerManager.Instance.pauseSTimer.Remove(this);
        }

        iftiming = true;
        sTimerState = STimerState.Running;
    }

    /// <summary>
    /// 暂停计时
    /// </summary>
    public void Pause()
    {
        //if (CheckClose()) return;

        iftiming = false;
        sTimerState = STimerState.Pause;     
    }

    /// <summary>
    /// 停止计时
    /// </summary>
    public void Stop()
    {
        //if (CheckClose()) return;

        ReStart();
        iftiming = false;
    }

    /// <summary>
    /// 重新开始
    /// </summary>
    public void ReStart()
    {
        //if (CheckClose()) return;

        nowTime = 0;
        currentlc = loopCount;
        timingActions?.AddRange(onceActions);
        onceActions.Clear();
        Start();
    }

    /// <summary>
    /// 关闭计时器
    /// </summary>
    public void Close()
    {
        if(ifAutoPushPool)
        {
            ifClose = true;
            iftiming = false;
            //timingActions.Clear();
            onceActions.Clear();
            sTimerState = STimerState.Standby;
        }
        else
        {
            Stop();
        }
    }

    /// <summary>
    /// 推进计时
    /// </summary>
    public void Update()
    {
        if (iftiming)
        {
            if (nowTime > targetTime) 
            {
                nowTime = targetTime;
                updateAction?.Invoke(nowTime);
                endAction?.Invoke();

                if (currentlc == 0)
                    Close();
                else if(currentlc > 0)
                {
                    nowTime = 0;
                    currentlc -= 1;
                }
                else if (currentlc < 0)
                    ReStart();
                
            }
            else
            {
                updateAction?.Invoke(nowTime);
                nowTime += ifscale ? Time.deltaTime : Time.unscaledDeltaTime;
            }
        }
    }

    /// <summary>
    /// 检测该计时器是否已经被关闭
    /// </summary>
    /// <returns>该计时器是否已经被关闭</returns>
    private bool CheckClose()
    {
        if (ifClose)
            Debug.LogError("This STimer has been closed, please create a new one");
        return ifClose;
    }
}
