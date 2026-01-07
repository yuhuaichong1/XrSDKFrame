using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XrCode
{
    public interface IEntity
    {
        int ID { get; }
        Vector3 Position { get; }
    }
}