using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideItem
{
    public int step;                            //当前步骤
    public int nextStep;                        //下一步骤
    public string note;                         //说明
    public int backStep;                        //引导中断后，再次进入时跳转步骤
    public bool ifBackPlay;                     //引导中断后，再次进入时是否执行跳转的步骤
    public bool ifNextStep;                     //点击/倒计时结束后是否前进一步
    public bool ifNextPlay;                     //前进一步后是否执行接下来的步骤
    public float autohiddenTime;                //引导自动消失时间
    public string diglogContent;                //提示框内容（对应Language表）
    public string diglogPos;                    //提示框位置
    public string handPos;                      //手位置
    public bool ifMask;                         //是否有黑色遮罩
    public string transparentPos;               //透明框位置
    public List<string> clickPos;               //教程可点击部分
    public Dictionary<string, string> extra;    //额外字段
}
