using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public static class FacadeLanguage
    {
        public static Action<int> OnLanguageChange;                 //多语言类型变更
        public static Func<string, string> GetText;                 //获取对应多语言
    }
}