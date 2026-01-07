public class EventStruct
{
    //事件类别
    public string eventType;
    //参数
    public object eventParams;
    //事件抛出者
    public object target;

    public EventStruct(string eventType, object eventParams = null)
    {
        this.eventType = eventType;
        this.eventParams = eventParams;
    }
}
