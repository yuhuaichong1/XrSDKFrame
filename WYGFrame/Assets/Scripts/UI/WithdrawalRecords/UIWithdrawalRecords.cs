
using SuperScrollView;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{

    public partial class UIWithdrawalRecords : BaseUI
    {
        private bool ifDetial;
        private bool ifDetialSuccess;

        List<WithdrawalRecordItem> withdrawalRecordItems;

        protected override void OnAwake()
        {
            withdrawalRecordItems = FacadeWithdrawal.GetWithdrawalRecordItems();
            mOrderScrollView.InitGridView(withdrawalRecordItems.Count, OrdersCallBack);
        }
        protected override void OnEnable()
        {
            ShowAnim(mPlane);

            int wriLength = withdrawalRecordItems.Count;
            mOrderScrollView.SetListItemCount(wriLength);
            mOrderScrollView.RefreshAllShownItem();
        }

        private void OnExitBtnClickHandle()
        {
            HideAnim(mPlane, () => 
            {
                UIManager.Instance.CloseUI(EUIType.EUIWithdrawalRecords);
            });
        }

        private LoopGridViewItem OrdersCallBack(LoopGridView cell, int index, int row, int column)
        {
            LoopGridViewItem item = mOrderScrollView.NewListViewItem("OrderItem");
            OrderItem orderItem = item.GetComponent<OrderItem>();
            orderItem.SetInfo(withdrawalRecordItems[index]);
            return item;
        }

        protected override void OnDisable()
        {
        
        }
        protected override void OnDispose()
        {
        
        }
    }
}