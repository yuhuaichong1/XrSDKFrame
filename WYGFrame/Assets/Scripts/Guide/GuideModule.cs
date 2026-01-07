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
    private int curStep;//当前引导步骤
    private GuideItem curGuideItems;//当前引导数据

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

    /// <summary>
    /// 获取引导数据
    /// </summary>
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

        SetCurGuideItems(curStep);
    }

    /// <summary>
    /// 设置当前引导数据
    /// </summary>
    /// <param name="step">引导步骤</param>
    private void SetCurGuideItems(int step)
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
        curGuideItems.diglogContent = GetLanguageContent(guideData.DiglogContentId);
        curGuideItems.diglogPos = GetUIRectTrans(guideData.DiglogPos);
        curGuideItems.handPos = GetUIRectTrans(guideData.HandPos);
        curGuideItems.ifMask = guideData.IfMask;
        curGuideItems.transparentPos = GetUIRectTrans(guideData.TransparentPos);
        curGuideItems.clickPos = GetClickRectTrans(guideData.ClickPos);
        curGuideItems.extraStart = guideData.ExtraStart;
        curGuideItems.extraEnd = guideData.ExtraEnd;
    }

    /// <summary>
    /// 获取当前引导数据
    /// </summary>
    /// <returns>当前引导数据</returns>
    private GuideItem GetCurGuideItems()
    {
        return curGuideItems;
    }

    /// <summary>
    /// 进入下一步引导（依据条件判断是否进入）
    /// </summary>
    private void NextStep()
    {
        if (curGuideItems.ifNextStep && !CheckGuideEnd())
        {
            SetExtraEnd(curGuideItems.extraEnd);

            bool orginBool = curGuideItems.ifNextPlay;
            curStep = curGuideItems.nextStep;
            SetCurGuideItems(curStep);
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
        SetCurGuideItems(curStep);
        SPlayerPrefs.SetInt(PlayerPrefDefines.curStep, curStep);
        SPlayerPrefs.Save();
    }

    /// <summary>
    /// 获取语言集合
    /// </summary>
    /// <param name="languageIds">语言id</param>
    /// <returns>目标语言文本集合</returns>
    private List<string> GetLanguageContent(List<int> languageIds)
    {
        if (languageIds == null || languageIds.Count == 0) return null;

        List<string> temp = new List<string>();
        foreach (int languageId in languageIds)
        {
            temp.Add(FacadeLanguage.GetText(languageId.ToString()));
        }

        return temp;
    }

    /// <summary>
    /// 获取UI路径
    /// </summary>
    /// <param name="pathData">路径的str数据</param>
    /// <returns>目标UI</returns>
    private List<string> GetUIRectTrans(List<string> pathData)
    {
        if (pathData == null || pathData.Count == 0) return null;

        List<string> temp = new List<string>();
        foreach (string item in pathData)
        {
            string[] diglongPosStr = item.Split(',');
            string path = UIManager.Instance.GetUIPath(diglongPosStr);
            temp.Add(path);
        }
        return temp;
    }

    /// <summary>
    /// 单独处理可点击部分的位置
    /// </summary>
    /// <param name="pathData">路径（可能是其他类型）</param>
    /// <param name="item">当前引导项目</param>
    /// <returns>目标路径</returns>
    private List<string> GetClickRectTrans(List<string> pathData)
    {
        List<string> temp = new List<string>();
        for (int i = 0; i < pathData.Count; i++)
        {
            switch (pathData[i])
            {
                case "handPos":
                    temp.Add(curGuideItems.handPos[i]);
                    break;
                case "transparentPos":
                    temp.Add(curGuideItems.transparentPos[i]);
                    break;
                default:
                    temp.Add(GetUIRectTrans(new List<string>() { pathData[i] })[0]);
                    break;
            }
        }

        return temp;
    }

    /// <summary>
    /// 得到当前引导步骤
    /// </summary>
    /// <returns>当前引导步骤</returns>
    private int GetCurStep()
    {
        return curStep;
    }

    /// <summary>
    /// 获取当前是否处于引导状态
    /// </summary>
    /// <returns>当前是否处于引导状态</returns>
    private bool GetIfTutorial()
    {
        return ifTutorial;
    }

    /// <summary>
    /// 设置当前是否处于引导状态
    /// </summary>
    /// <param name="b">当前是否处于引导状态</param>
    private void SetIfTutorial(bool b)
    {
        ifTutorial = b;
        SPlayerPrefs.SetBool(PlayerPrefDefines.ifTutorial, ifTutorial);
        SPlayerPrefs.Save();
    }

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

    /// <summary>
    /// 激活引导结束时的回调
    /// </summary>
    /// <param name="extraData">回调数据类型</param>
    private void SetExtraEnd(Dictionary<string, string> extraData)
    {
        foreach (KeyValuePair<string, string> kvp in extraData)
        {
            switch (kvp.Key)
            {
                case "cord":

                    break;
            }
        }
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
