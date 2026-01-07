using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWAItem : MonoBehaviour
{
    public GameObject ProcessingIcon;
    public GameObject CheckedIcon;
    public GameObject Line;
    public Text DateText;
    public GameObject ErrorMsg;
    public GameObject ErrorText;

    /// <summary>
    /// 设置状态
    /// </summary>
    /// <param name="status">状态</param>
    public void SetStatus(EWOrderState status)
    {
        bool b = status == EWOrderState.Processing;
        bool b2 = status == EWOrderState.Error;

        if (ProcessingIcon != null)
            ProcessingIcon.SetActive(b);
        if(CheckedIcon != null)
            CheckedIcon.SetActive(!b);
        if(Line != null)
            Line.SetActive(!b);
        if (DateText != null)
            DateText.gameObject.SetActive(!b && !b2);
        if (ErrorMsg != null)
            ErrorMsg.gameObject.SetActive(b2);
    }

    /// <summary>
    /// 设置时间
    /// </summary>
    public void SetDateText()
    {
        if (DateText != null)
            DateText.text = DateTime.Now.ToString("yyyy-MM-dd");
    }    
}
