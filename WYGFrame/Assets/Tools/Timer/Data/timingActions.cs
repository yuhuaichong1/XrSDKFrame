using System;

//计时节点事件结构图
public struct timingActions
{
    public float timing;//目标节点
    public Action<float> clockAction;//事件
    public ClockActionType clockActionType;//计时节点事件类型
}
