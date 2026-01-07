using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XrCode
{
    public class BaseModule : ILoad, IDispose
    {
        public void Load() { OnLoad(); }

        protected virtual void OnLoad() { }

        public void Update() { OnUpdate(); }

        public void FixedUpdate() { OnFixedUpdate(); }

        protected virtual void OnUpdate() { }

        protected virtual void OnFixedUpdate() { }

        public void Dispose() { OnDispose(); }

        protected virtual void OnDispose() { }
        // ×¢²áUpdateÖ´ÐÐ
        protected void RegisetUpdateObj()
        {
            ModuleMgr.Instance.RegistUpdateObj(this);
        }
    }
}