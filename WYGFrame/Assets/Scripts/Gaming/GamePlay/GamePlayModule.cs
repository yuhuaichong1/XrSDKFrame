using DG.Tweening;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XrCode;

public partial class GamePlayModule : BaseModule
{
    protected override void OnLoad()
    {
        base.OnLoad();
        FacadeAdd();

        LoadData();
        InitGame();
    }

    #region Facade

    /// <summary>
    /// 添加Facade接口
    /// </summary>
    private void FacadeAdd()
    {
        FacadeGamePlay.CreateLevel += CreateLevel;
        FacadeGamePlay.Func_Porp1 += Func_Porp1;
        FacadeGamePlay.Func_Porp2 += Func_Porp2;
        FacadeGamePlay.Func_Porp3 += Func_Porp3;
        FacadeGamePlay.RePlay += RePlay;
        FacadeGamePlay.GetCurLevelProgress += GetCurLevelProgress;
    }

    /// <summary>
    /// 去除Facade接口
    /// </summary>
    private void FacadeRemove()
    {
        FacadeGamePlay.CreateLevel += CreateLevel;
        FacadeGamePlay.Func_Porp1 += Func_Porp1;
        FacadeGamePlay.Func_Porp2 += Func_Porp2;
        FacadeGamePlay.Func_Porp3 += Func_Porp3;
        FacadeGamePlay.RePlay += RePlay;
        FacadeGamePlay.GetCurLevelProgress -= GetCurLevelProgress;
    }

    #endregion

    /// <summary>
    /// 加载持久化数据
    /// </summary>
    private void LoadData()
    {
        
    }

    /// <summary>
    /// 初始化游戏进程
    /// </summary>
    private void InitGame()
    {
        CreateLevel();
    }

    /// <summary>
    /// 创建关卡
    /// </summary>
    private void CreateLevel()
    {

    }

    /// <summary>
    /// 重新开始
    /// </summary>
    private void RePlay()
    {

    }

    /// <summary>
    /// 获取当前关卡进度
    /// </summary>
    private float GetCurLevelProgress()
    {
        return 0;
    }

    #region 下三功能

    private void Func_Porp1()
    {

    }

    private void Func_Porp2()
    {

    }

    private void Func_Porp3()
    {

    }

    #endregion

    protected override void OnDispose()
    {
        FacadeRemove();
    }
}
