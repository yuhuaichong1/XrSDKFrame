using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XrCode
{
    public class BaseComponent : IComponent<IEntity>, IDispose
    {
        public IEntity Owner { get; }

        public void Enable()
        {
        }

        public void Disable()
        {
            OnDisable();
        }
        protected virtual void OnDisable() { }

        public void Dispose()
        {
            OnDispose();
        }
        public virtual void OnDispose() { }

    }
}