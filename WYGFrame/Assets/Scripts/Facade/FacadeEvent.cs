using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FacadeEvent
{
    public static Action<string, object> DispatchEvent;
    public static Action<string, EventListener.EventListenerDelegate> AddEventListener;
    public static Action<string, EventListener.EventListenerDelegate> RemoveEventListener;
    public static Func<string, bool> HasListener;
}
