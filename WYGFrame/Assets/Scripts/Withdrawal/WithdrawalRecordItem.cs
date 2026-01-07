using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithdrawalRecordItem
{
    public int LevelId;//订单生成时的关卡
    public string CreatedDate;//订单生成时的日期
    public EWithRecordState WRState;//订单状态
    public double WRMoney;//订单金额

    public void ChangeState(EWithRecordState state)
    {
        WRState = state;
        FacadeWithdrawal.SaveCurWithdrawalRecordItems();
    }
}
