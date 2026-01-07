using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XrCode
{

    public static class FacadeScene
    {
        public static Action OnSceneLoadFinish;
        public static Func<ESceneType> GetCurrentSceneType;
    }
}