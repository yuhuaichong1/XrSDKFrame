using System.Collections.Generic;
using XrCode;

public class EventModule : BaseModule
{
    private Dictionary<string, EventListener> eventListenerDict;

    protected override void OnLoad()
    {
        eventListenerDict = new Dictionary<string, EventListener>();

        FacadeEvent.DispatchEvent += DispatchEvent;
        FacadeEvent.AddEventListener += AddEventListener;
        FacadeEvent.RemoveEventListener += RemoveEventListener;
        FacadeEvent.HasListener += HasListener;
    }

    //发送事件
    private void DispatchEvent(string evtType, object gameObject = null)
    {
        EventStruct evt = new EventStruct(evtType);
        if (eventListenerDict.ContainsKey(evt.eventType) == false)
        {
            D.Log($"dispatchEvent 事件: {evt.eventType}");
            return;
        }
        EventListener eventListener = eventListenerDict[evt.eventType];
        if (eventListener == null) return;
        evt.target = gameObject;
        eventListener.Excute(evt);
    }

    //监听（添加）事件
    private void AddEventListener(string eventType, EventListener.EventListenerDelegate callback)
    {
        if (!this.eventListenerDict.ContainsKey(eventType))
        {
            this.eventListenerDict.Add(eventType, new EventListener());
        }
        this.eventListenerDict[eventType].OnEvent += callback;
    }

    //取消监听（移除）事件
    private void RemoveEventListener(string eventType, EventListener.EventListenerDelegate callback)
    {
        if (this.eventListenerDict.ContainsKey(eventType))
        {
            this.eventListenerDict[eventType].OnEvent -= callback;
        }
    }

    //检测是否存在某监听
    public bool HasListener(string eventType)
    {
        return this.eventListenerDict.ContainsKey(eventType);
    }

    protected override void OnDispose()
    {
        eventListenerDict = null;
    }
}
