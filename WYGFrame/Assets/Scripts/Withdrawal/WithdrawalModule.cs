using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using XrCode;

public class WithdrawalModule : BaseModule
{
    private string wName;//兑现姓名
    private string wPhoneOrEmail;//兑现信息
    private EPayType poeType;//兑现信息类型
    private bool isEarning;//正在争取中

    private List<WithdrawalRecordItem> withdrawalRecordItems;//兑现记录数据

    protected override void OnLoad()
    {
        FacadeWithdrawal.GetWName += GetWName;
        FacadeWithdrawal.SetWName += SetWName;
        FacadeWithdrawal.GetWPhoneOrEmail += GetWPhoneOrEmail;
        FacadeWithdrawal.SetWPhoneOrEmail += SetWPhoneOrEmail;
        FacadeWithdrawal.GetPayType += GetPOEType;
        FacadeWithdrawal.SetPayType += SetPOEType;
        FacadeWithdrawal.GetTipMsg += GetTipMsg;
        FacadeWithdrawal.GetIsEarning += GetIsEarning;
        FacadeWithdrawal.SetIsEarning += SetIsEarning;
        FacadeWithdrawal.CreateOrder += CreateOrder;
        FacadeWithdrawal.SaveCurWithdrawalRecordItems += SaveCurWithdrawalRecordItems;
        FacadeWithdrawal.GetWithdrawalRecordItems += GetWithdrawalRecordItems;
        FacadeWithdrawal.CheckOpenUI += CheckOpenUI;

        withdrawalRecordItems = new List<WithdrawalRecordItem>();

        LoadData();
    }

    private void LoadData()
    {
        wName = SPlayerPrefs.GetString(PlayerPrefDefines.wName, "");
        wPhoneOrEmail = SPlayerPrefs.GetString(PlayerPrefDefines.wPhoneOrEmail, "");
        poeType = (EPayType)SPlayerPrefs.GetInt(PlayerPrefDefines.poeType, (int)EPayType.Other);
        isEarning = SPlayerPrefs.GetBool(PlayerPrefDefines.isEarning, false);
        List<string> wrisTemp = SPlayerPrefs.GetList<string>(PlayerPrefDefines.wrisTemp, new List<string>());
        foreach (string wri in wrisTemp)
        {
            string[] values = wri.Split("_");
            WithdrawalRecordItem item = new WithdrawalRecordItem()
            {
                LevelId = int.Parse(values[0]),
                CreatedDate = values[1],
                WRState = (EWithRecordState)int.Parse(values[2]),
                WRMoney = float.Parse(values[3]),
            };
            withdrawalRecordItems.Add(item);
        }
        
    }

    #region Get/Set

    #region wName
    private string GetWName()
    {
        return wName;
    }
    private void SetWName(string value)
    {
        wName = value;
        SPlayerPrefs.SetString(PlayerPrefDefines.wName, wName);
        SPlayerPrefs.Save();
    }
    #endregion

    #region wPhoneOrEmail
    private string GetWPhoneOrEmail()
    {
        return wPhoneOrEmail;
    }

    private void SetWPhoneOrEmail(string value)
    {
        wPhoneOrEmail = value;
        SPlayerPrefs.SetString(PlayerPrefDefines.wPhoneOrEmail, wPhoneOrEmail);
        SPlayerPrefs.Save();
    }
    #endregion

    #region poeType
    private EPayType GetPOEType()
    {
        return poeType;
    }
    private void SetPOEType(EPayType value)
    {
        poeType = value;
        SPlayerPrefs.SetInt(PlayerPrefDefines.poeType, (int)poeType);
        SPlayerPrefs.Save();
    }
    #endregion

    #region isEarning
    private bool GetIsEarning()
    {
        return isEarning;
    }

    private void SetIsEarning(bool value)
    {
        isEarning = value;

        SPlayerPrefs.SetBool(PlayerPrefDefines.isEarning, isEarning);
        SPlayerPrefs.Save();
    }
    #endregion

    #region withdrawalRecordItems
    private List<WithdrawalRecordItem> GetWithdrawalRecordItems()
    {
        return withdrawalRecordItems;
    }
    #endregion

    #endregion

    /// <summary>
    /// 获取提现提示信息
    /// </summary>
    /// <returns>提现提示信息</returns>
    private string GetTipMsg()
    {
        string msg = "";

        return msg;
    }

    /// <summary>
    /// 创建订单
    /// </summary>
    private void CreateOrder(int level)
    {
        withdrawalRecordItems.Add(new WithdrawalRecordItem 
        {
            LevelId = level,
            CreatedDate = DateTime.Now.ToString("yyyy-MM-dd"),
            WRState = EWithRecordState.GoWithdrawal,
            WRMoney = FacadePlayer.GetMoney(),
        });

        SaveCurWithdrawalRecordItems();
    }

    /// <summary>
    /// 保存单当前订单数据
    /// </summary>
    private void SaveCurWithdrawalRecordItems()
    {
        List<string> wrisTemp = new List<string>();

        foreach(WithdrawalRecordItem item in withdrawalRecordItems)
        {
            string str = $"{item.LevelId}_{item.CreatedDate}_{(int)item.WRState}_{item.WRMoney}";
            wrisTemp.Add(str);
        }

        SPlayerPrefs.SetList<string>(PlayerPrefDefines.wrisTemp, wrisTemp);
        SPlayerPrefs.Save();
    }

    /// <summary>
    /// 检测应该打开UIEnterInfo还是UIConfirm
    /// </summary>
    private void CheckOpenUI(bool b, WithdrawalRecordItem item)
    {
        if(string.IsNullOrEmpty(wName))
        {
            UIManager.Instance.OpenAsync<UIEnterInfomation>(EUIType.EUIEnterInfomation);
        }
        else
        {
            UIManager.Instance.OpenAsync<UIConfirm>(EUIType.EUIConfirm, null, b, item);
        }
    }

    protected override void OnDispose()
    {
        FacadeWithdrawal.GetWName -= GetWName;
        FacadeWithdrawal.SetWName -= SetWName;
        FacadeWithdrawal.GetWPhoneOrEmail -= GetWPhoneOrEmail;
        FacadeWithdrawal.SetWPhoneOrEmail -= SetWPhoneOrEmail;
        FacadeWithdrawal.GetPayType -= GetPOEType;
        FacadeWithdrawal.SetPayType -= SetPOEType;
        FacadeWithdrawal.GetTipMsg -= GetTipMsg;
        FacadeWithdrawal.GetIsEarning -= GetIsEarning;
        FacadeWithdrawal.SetIsEarning -= SetIsEarning;
        FacadeWithdrawal.CreateOrder -= CreateOrder;
        FacadeWithdrawal.SaveCurWithdrawalRecordItems -= SaveCurWithdrawalRecordItems;
        FacadeWithdrawal.GetWithdrawalRecordItems += GetWithdrawalRecordItems;
        FacadeWithdrawal.CheckOpenUI -= CheckOpenUI;

        withdrawalRecordItems.Clear();
        withdrawalRecordItems = null;
    }
}
