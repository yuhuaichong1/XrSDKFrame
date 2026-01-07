using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XrCode
{
    public class DontDestroy : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}