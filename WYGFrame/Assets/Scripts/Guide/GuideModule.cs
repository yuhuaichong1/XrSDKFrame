using cfg;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using XrCode;

public class GuideModule : BaseModule
{
    private int curStep;
    private GuideItem curGuideItems;

    private bool ifTutorial;//是否处于引导状态

    protected override void OnLoad()
    {
        base.OnLoad();

        FacadeGuide.NextStep += NextStep;
        FacadeGuide.GetCurGuideItems += GetCurGuideItems;
        FacadeGuide.GetCurStep += GetCurStep;
        FacadeGuide.NextStepHead += NextStepHead;
        FacadeGuide.GetIfTutorial += GetIfTutorial;
        FacadeGuide.SetIfTutorial += SetIfTutorial;

        curGuideItems = new GuideItem();

        GetData();
    }

    private void GetData()
    {
        ifTutorial = SPlayerPrefs.GetBool(PlayerPrefDefines.ifTutorial, true);    
        curStep = SPlayerPrefs.GetInt(PlayerPrefDefines.curStep);
        if (curStep == 0) curStep = GameDefines.firstGuideId;
        else
        {
            var backStep = ConfigModule.Instance.Tables.TBGuides.Get(curStep).BackStep;
            curStep = backStep != 0 ? backStep : curStep;
        }

        SetGuide(curStep);
    }

    private void SetGuide(int step)
    {
        curStep = step;
        ConfGuides guideData = ConfigModule.Instance.Tables.TBGuides.Get(step);
        curGuideItems.step = step;
        curGuideItems.nextStep = guideData.NextStep;
        curGuideItems.note = guideData.Notes;
        curGuideItems.backStep = guideData.BackStep;
        curGuideItems.ifBackPlay = guideData.IfBackPlay;
        curGuideItems.ifNextStep = guideData.IfNextStep;
        curGuideItems.ifNextPlay = guideData.IfNextPlay;
        curGuideItems.autohiddenTime = guideData.AutohiddenTime;
        curGuideItems.diglogContent = FacadeLanguage.GetText(guideData.DiglogContentId.ToString());
        curGuideItems.diglogPos = GetUIRectTrans(guideData.DiglogPos);
        curGuideItems.handPos = GetUIRectTrans(guideData.HandPos);
        curGuideItems.ifMask = guideData.IfMask;
        curGuideItems.transparentPos = GetUIRectTrans(guideData.TransparentPos);
        curGuideItems.clickPos = GetClickRectTrans(guideData.ClickPos);
        curGuideItems.extra = guideData.Extra;
    }

    /// <summary>
    /// 进入下一步引导（依据条件判断是否进入）
    /// </summary>
    private void NextStep()
    {
        if (curGuideItems.ifNextStep && !CheckGuideEnd())
        {
            bool orginBool = curGuideItems.ifNextPlay;
            curStep = curGuideItems.nextStep;
            SetGuide(curStep);
            if (orginBool)
            {
                FacadeGuide.PlayGuide();
            }
            else
            {
                FacadeGuide.CloseGuide();
            }
        }
        else
        {
            FacadeGuide.CloseGuide();
        }

        SPlayerPrefs.SetInt(PlayerPrefDefines.curStep, curStep);
        SPlayerPrefs.Save();
    }

    /// <summary>
    /// 手动（强制）进入下一步骤
    /// </summary>
    private void NextStepHead()
    {
        curStep = curGuideItems.nextStep;
        CheckGuideEnd();
        SetGuide(curStep);
        SPlayerPrefs.SetInt(PlayerPrefDefines.curStep, curStep);
        SPlayerPrefs.Save();
    }

    /// <summary>
    /// 获取UI路径
    /// </summary>
    /// <param name="pathData">路径的str数据</param>
    /// <returns>目标UI</returns>
    private string GetUIRectTrans(string pathData)
    {
        if (pathData == "") return null;
        string[] diglongPosStr = pathData.Split(',');
        string path = UIManager.Instance.GetUIPath(diglongPosStr);
        return path;
    }

    /// <summary>
    /// 单独处理可点击部分的位置
    /// </summary>
    /// <param name="pathData">路径（可能是其他类型）</param>
    /// <param name="item">当前引导项目</param>
    /// <returns>目标路径</returns>
    private List<string> GetClickRectTrans(string pathData)
    {
        List<string> paths = new List<string>();
        string[] temp = pathData.Split(';');
        foreach(string value in temp) 
        {
            switch (value)
            {
                case "handPos":
                    paths.Add(curGuideItems.handPos);
                    break;
                case "transparentPos":
                    paths.Add(curGuideItems.transparentPos);
                    break;
                default:
                    paths.Add(GetUIRectTrans(value));
                    break;
            }
        }
        return paths;
    }

    private GuideItem GetCurGuideItems()
    {
        return curGuideItems;
    }

    private int GetCurStep()
    {
        return curStep;
    }

    #region Get/Set
    private bool GetIfTutorial()
    {
        return ifTutorial;
    }
    private void SetIfTutorial(bool b)
    {
        ifTutorial = b;
        SPlayerPrefs.SetBool(PlayerPrefDefines.ifTutorial, ifTutorial);
        SPlayerPrefs.Save();
    }
    #endregion


    /// <summary>
    /// 检测引导是否结束
    /// </summary>
    /// <returns>引导是否结束</returns>
    private bool CheckGuideEnd()
    {
        var nextStep = ConfigModule.Instance.Tables.TBGuides.Get(curStep)?.NextStep ?? 0;
        var isEnd = nextStep == 0;
        if (isEnd)
        {
            D.Error("Guide End");
            ifTutorial = false;
            SetIfTutorial(ifTutorial);
        }
        return isEnd;
    }

    protected override void OnDispose()
    {
        FacadeGuide.NextStep -= NextStep;
        FacadeGuide.GetCurGuideItems -= GetCurGuideItems;
        FacadeGuide.GetCurStep -= GetCurStep;
        FacadeGuide.NextStepHead -= NextStepHead;
        FacadeGuide.GetIfTutorial -= GetIfTutorial;
        FacadeGuide.SetIfTutorial -= SetIfTutorial;

        curGuideItems = null;
    }
}
