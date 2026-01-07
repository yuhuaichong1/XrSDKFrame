using System;
using UnityEngine;
using XrCode;

public class AutoHandSwing : MonoBehaviour
{

    public bool ifAwake = true;//是否在一开始就启用
    private RectTransform rect;



    void Awake()
    {
        rect = this.GetComponent<RectTransform>();

        if (ifAwake)
        {
            StartSwing();
        }
    }

    private void StartSwing()
    {

    }

    public void Reset()
    {

    }
}
