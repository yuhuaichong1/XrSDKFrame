using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
//using ThinkingData.Analytics;
using UnityEngine;
namespace XrCode
{
    public class TDAnalyticsManager : BaseModule
    {
        //开关
#if UNITY_EDITOR
        bool isOpenTD = false;
#else
    bool isOpenTD = true;
#endif

        //玩家id
        private string accoundId = "";
        //游戏版本
        private string GameVersion = "1.0.0.0";
        //游戏编号，区分上线平台
        private string GameAppId = "405001";

        protected override void OnLoad()
        {
            //GameLoad(PlayerPrefs.GetString("accoundId"));
        }

        //游戏加载
        public void GameLoad(string accoundId)
        {
            //if (!isOpenTD) return;
            ////设置公共事件属性以后，每个事件都会带有公共事件属性
            //Dictionary<string, object> superProperties = new Dictionary<string, object>();
            //superProperties["GameVersion"] = GameVersion;
            //superProperties["GameAppId"] = GameAppId;
            //if (this.accoundId != "")
            //{
            //    this.accoundId = accoundId;
            //    //superProperties["accountId"] = accoundId;//字符串
            //    TDAnalytics.Login(accoundId);
            //}
            //else
            //{
            //    this.accoundId = UnityEngine.Random.Range(1000000, 9999999).ToString();
            //    PlayerPrefs.SetString("GoodMatch_accoundId", this.accoundId);
            //    //superProperties["accountId"] = accoundId;//字符串
            //    RegisterFinish(accoundId);
            //}
            //TDAnalytics.SetSuperProperties(superProperties);//设置公共事件属性
            //TDAnalytics.UserSet(new Dictionary<string, object>() { { "accountId", this.accoundId }, { "GameVersion", GameVersion }, { "GameAppId", GameAppId } }); //设置用户属性

            //if (isOpenTD)
            //{
            //    TDAnalytics.EnableAutoTrack(TDAutoTrackEventType.AppStart, new Dictionary<string, object>()
            //{
            //    { "accountId", this.accoundId }
            //});
            //    TDAnalytics.EnableAutoTrack(TDAutoTrackEventType.AppEnd, new Dictionary<string, object>()
            //{
            //    { "accountId", this.accoundId }
            //});
            //}
        }

        //例子，请勿调用
        //public void Example(int ex)
        //{
        //    if (!isOpenTD) return;
        //    Dictionary<string, object> properties = new Dictionary<string, object>();
        //    properties.Add("ex", ex);
        //    TDAnalytics.Track("ExampleKey", properties);
        //}
    }
}