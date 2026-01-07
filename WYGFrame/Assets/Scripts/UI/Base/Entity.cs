using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XrCode
{
    public class Entity : IEntity, IDispose
    {
        protected int id;
        protected Vector3 position;

        public int ID { get { return id; } }
        public Vector3 Position { get { return position; } }

        public void Dispose() { OnDispose(); }
        protected virtual void OnDispose() { }
    }
}