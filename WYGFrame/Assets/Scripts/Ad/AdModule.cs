using System;
using UnityEngine.UI;

namespace XrCode
{
    public class AdModule : BaseModule
    {
        private EAdSource rewardAdScore;//激励广告来源
        private EAdSource interstitialAdScore;//插屏广告来源
        private EAdSource bannerAdScore;//横幅广告来源
        private Action<int> rewardSuccessAction;//激励广告奖励回调
        private Action<int> interstitialSuccessAction;//插屏广告奖励回调
        private Action<int> bannerSuccessAction;//横幅广告奖励回调
        private Action rewardFailAction;//激励广告失败回调
        private Action interstitialFailAction;//插屏广告失败回调
        private Action bannerFailAction;//横幅广告失败回调

        private string AdFailMsg;

        protected override void OnLoad() 
        {
            AddFacade();
            AdFailMsg = FacadeLanguage.GetText("10088");
        }

        #region Facade

        private void AddFacade()
        {
            FacadeAd.PlayRewardAd += PlayRewardAd;
            FacadeAd.PlayInterAd += PlayInterAd;
            FacadeAd.PlayBannerAd += PlayBannerAd;
            FacadeAd.StopBannerAd += StopBannerAd;

            FacadeAd.OnRewardAdLoaded += OnRewardAdLoaded;
            FacadeAd.OnRewardAdLoadFailed += OnRewardAdLoadFailed;
            FacadeAd.OnRewardAdDisplayed += OnRewardAdDisplayed;
            FacadeAd.OnRewardAdDisplayFailed += OnRewardAdDisplayFailed;
            FacadeAd.OnRewardAdCompleted += OnRewardAdCompleted;
            FacadeAd.OnRewardAdClicked += OnRewardAdClicked;
            FacadeAd.OnRewardAdClosed += OnRewardAdClosed;
            FacadeAd.OnRewardAdRevenuePaid += OnRewardAdRevenuePaid;
            FacadeAd.OnRewardAdNotReady += OnRewardAdNotReady;
            FacadeAd.OnRewardAdReceivedReward += OnRewardAdReceivedReward;

            FacadeAd.OnInterstitialAdLoaded += OnInterstitialAdLoaded;
            FacadeAd.OnInterstitialAdLoadFailed += OnInterstitialAdLoadFailed;
            FacadeAd.OnInterstitialAdDisplayed += OnInterstitialAdDisplayed;
            FacadeAd.OnInterstitialAdDisplayFailed += OnInterstitialAdDisplayFailed;
            FacadeAd.OnInterstitialAdCompleted += OnInterstitialAdCompleted;
            FacadeAd.OnInterstitialAdClicked += OnInterstitialAdClicked;
            FacadeAd.OnInterstitialAdClosed += OnInterstitialAdClosed;
            FacadeAd.OnInterstitialAdRevenuePaid += OnInterstitialAdRevenuePaid;
            FacadeAd.OnInterstitialAdNotReady += OnInterstitialAdNotReady;

            FacadeAd.OnBannerAdLoaded += OnBannerAdLoaded;
            FacadeAd.OnBannerAdLoadFailed += OnBannerAdLoadFailed;
            FacadeAd.OnBannerAdDisplayed += OnBannerAdDisplayed;
            FacadeAd.OnBannerAdDisplayFailed += OnBannerAdDisplayFailed;
            FacadeAd.OnBannerAdCompleted += OnBannerAdCompleted;
            FacadeAd.OnBannerAdClicked += OnBannerAdClicked;
            FacadeAd.OnBannerAdClosed += OnBannerAdClosed;
            FacadeAd.OnBannerAdRevenuePaid += OnBannerAdRevenuePaid;
            FacadeAd.OnBannerAdNotReady += OnBannerAdNotReady;

        }

        private void RemoveFacade()
        {
            FacadeAd.PlayRewardAd -= PlayRewardAd;
            FacadeAd.PlayInterAd -= PlayInterAd;
            FacadeAd.PlayBannerAd -= PlayBannerAd;
            FacadeAd.StopBannerAd -= StopBannerAd;

            FacadeAd.OnRewardAdLoaded -= OnRewardAdLoaded;
            FacadeAd.OnRewardAdLoadFailed -= OnRewardAdLoadFailed;
            FacadeAd.OnRewardAdDisplayed -= OnRewardAdDisplayed;
            FacadeAd.OnRewardAdDisplayFailed -= OnRewardAdDisplayFailed;
            FacadeAd.OnRewardAdCompleted -= OnRewardAdCompleted;
            FacadeAd.OnRewardAdClicked -= OnRewardAdClicked;
            FacadeAd.OnRewardAdClosed -= OnRewardAdClosed;
            FacadeAd.OnRewardAdRevenuePaid -= OnRewardAdRevenuePaid;
            FacadeAd.OnRewardAdNotReady -= OnRewardAdNotReady;
            FacadeAd.OnRewardAdReceivedReward -= OnRewardAdReceivedReward;

            FacadeAd.OnInterstitialAdLoaded -= OnInterstitialAdLoaded;
            FacadeAd.OnInterstitialAdLoadFailed -= OnInterstitialAdLoadFailed;
            FacadeAd.OnInterstitialAdDisplayed -= OnInterstitialAdDisplayed;
            FacadeAd.OnInterstitialAdDisplayFailed -= OnInterstitialAdDisplayFailed;
            FacadeAd.OnInterstitialAdCompleted -= OnInterstitialAdCompleted;
            FacadeAd.OnInterstitialAdClicked -= OnInterstitialAdClicked;
            FacadeAd.OnInterstitialAdClosed -= OnInterstitialAdClosed;
            FacadeAd.OnInterstitialAdRevenuePaid -= OnInterstitialAdRevenuePaid;
            FacadeAd.OnInterstitialAdNotReady += OnInterstitialAdNotReady;

            FacadeAd.OnBannerAdLoaded -= OnBannerAdLoaded;
            FacadeAd.OnBannerAdLoadFailed -= OnBannerAdLoadFailed;
            FacadeAd.OnBannerAdDisplayed -= OnBannerAdDisplayed;
            FacadeAd.OnBannerAdDisplayFailed -= OnBannerAdDisplayFailed;
            FacadeAd.OnBannerAdCompleted -= OnBannerAdCompleted;
            FacadeAd.OnBannerAdClicked -= OnBannerAdClicked;
            FacadeAd.OnBannerAdClosed -= OnBannerAdClosed;
            FacadeAd.OnBannerAdRevenuePaid -= OnBannerAdRevenuePaid;
            FacadeAd.OnBannerAdNotReady += OnBannerAdNotReady;
        }

        #endregion

        #region 播放广告

        private void PlayRewardAd(EAdSource eAdSource, Action<int> successAction, Action failAction)
        {
            rewardAdScore = eAdSource;

            if (GameDefines.ifSkipAD)
                successAction?.Invoke(1);
            else
            {
                rewardSuccessAction = successAction;
                rewardFailAction = failAction;

                //播放激励广告
            }
        }

        private void PlayInterAd(EAdSource eAdSource, Action<int> successAction, Action failAction)
        {
            interstitialAdScore = eAdSource;

            if (GameDefines.ifSkipAD)
                successAction?.Invoke(1);
            else
            {
                interstitialSuccessAction = successAction;
                interstitialFailAction = failAction;

                //播放插屏广告
            }
        }

        private void PlayBannerAd(EAdSource eAdSource, Action<int> successAction, Action failAction)
        {
            bannerAdScore = eAdSource;

            if (GameDefines.ifSkipAD)
                successAction?.Invoke(1);
            else
            {
                bannerSuccessAction = successAction;
                bannerFailAction = failAction;

                //播放横幅广告
            }
        }

        private void StopBannerAd(EAdSource eAdSource)
        {
            //关闭横幅广告
        }

        #endregion

        #region 广告回调

        #region 公共广告回调（不加入FacadeAd，由“激励广告回调”、“插屏广告回调”、“横幅广告回调”中的方法进行调用）

        /// <summary>
        /// 广告加载成功
        /// </summary>
        /// <param name="eAdType">广告类型</param>
        /// <param name="eAdSource">广告来源</param>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnAdLoaded(EAdType eAdType, EAdSource eAdSource, string platform, double revenue, double ecpm, string precision)
        {
            D.Log($"'{eAdType}' Ad '{eAdSource}' loaded successfully");

        }

        /// <summary>
        /// 广告加载失败
        /// </summary>
        /// <param name="eAdType">广告类型</param>
        /// <param name="eAdSource">广告来源</param>
        /// <param name="platform">广告平台</param>
        /// <param name="errMsg">错误信息</param>
        private void OnAdLoadFailed(EAdType eAdType, EAdSource eAdSource, string platform, string errMsg)
        {
            D.Log($"'{eAdType}' Ad '{eAdSource}' loaded failed: '{errMsg}'");
            OnAdFailed(eAdType);

        }

        /// <summary>
        /// 广告显示成功
        /// </summary>
        /// <param name="eAdType">广告类型</param>
        /// <param name="eAdSource">广告来源</param>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnAdDisplayed(EAdType eAdType, EAdSource eAdSource, string platform, double revenue, double ecpm, string precision)
        {
            D.Log($"'{eAdType}' Ad '{eAdSource}' displayed successfully");
        }

        /// <summary>
        /// 广告显示失败
        /// </summary>
        /// <param name="eAdType">广告类型</param>
        /// <param name="eAdSource">广告来源</param>
        /// <param name="platform">广告平台</param>
        /// <param name="errMsg">错误信息</param>
        private void OnAdDisplayFailed(EAdType eAdType, EAdSource eAdSource, string platform, string errMsg)
        {
            D.Log($"'{eAdType}' Ad '{eAdSource}' displayed failed: '{errMsg}'");
            OnAdFailed(eAdType);

        }

        /// <summary>
        /// 广告播放完成
        /// </summary>
        /// <param name="eAdType">广告类型</param>
        /// <param name="eAdSource">广告来源</param>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnAdCompleted(EAdType eAdType, EAdSource eAdSource, string platform, double revenue, double ecpm, string precision)
        {
            D.Log($"'{eAdType}' Ad '{eAdSource}' completed");

        }

        /// <summary>
        /// 广告点击
        /// </summary>
        /// <param name="eAdType">广告类型</param>
        /// <param name="eAdSource">广告来源</param>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnAdClicked(EAdType eAdType, EAdSource eAdSource, string platform, double revenue, double ecpm, string precision)
        {
            D.Log($"'{eAdType}' Ad '{eAdSource}' clicked");

        }

        /// <summary>
        /// 广告关闭
        /// </summary>
        /// <param name="eAdType">广告类型</param>
        /// <param name="eAdSource">广告来源</param>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnAdClosed(EAdType eAdType, EAdSource eAdSource, string platform, double revenue, double ecpm, string precision)
        {
            D.Log($"'{eAdType}' Ad '{eAdSource}' closed");

        }

        /// <summary>
        /// 广告获取收入
        /// </summary>
        /// <param name="eAdType">广告类型</param>
        /// <param name="eAdSource">广告来源</param>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnAdRevenuePaid(EAdType eAdType, EAdSource eAdSource, string platform, double revenue, double ecpm, string precision)
        {
            D.Log($"'{eAdType}' Ad '{eAdSource}' revenue paid: '{revenue}'");

        }

        /// <summary>
        /// 广告未准备完毕
        /// </summary>
        /// <param name="eAdType">广告类型</param>
        private void OnAdNotReady(EAdType eAdType)
        {
            D.Log($"'{eAdType}' Ad not ready");
            OnAdFailed(eAdType);
        }

        /// <summary>
        /// 广告奖励发放（位置：激励——获取激励奖励、插屏——关闭插屏、横幅——点击横幅）
        /// </summary>
        /// <param name="eAdType">广告类型</param>
        /// <param name="eAdSource">广告来源</param>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        /// <param name="rewardAmount">奖励值（仅激励广告会有值，其余类型广告为0）</param>
        private void OnAdReceivedReward(EAdType eAdType, EAdSource eAdSource, string platform, double revenue, double ecpm, string precision, int rewardAmount)
        {
            D.Log($"'{eAdType}' Ad '{eAdSource}' received reward: '{rewardAmount}'");

            switch (eAdType) 
            { 
                case EAdType.Reward:
                    rewardSuccessAction?.Invoke(rewardAmount);
                    break;
                case EAdType.Interstitial:
                    interstitialSuccessAction?.Invoke(rewardAmount);
                    break;
                case EAdType.Banner:
                    bannerSuccessAction?.Invoke(rewardAmount);
                    break;
            }
        }

        /// <summary>
        /// 广告失败回调
        /// </summary>
        /// <param name="eAdType"></param>
        private void OnAdFailed(EAdType eAdType)
        {
            UIManager.Instance.OpenNotice(AdFailMsg);

            switch (eAdType)
            {
                case EAdType.Reward:
                    rewardFailAction?.Invoke();
                    break;
                case EAdType.Interstitial:
                    interstitialFailAction?.Invoke();
                    break;
                case EAdType.Banner:
                    bannerFailAction?.Invoke();
                    break;
            }
        }

        #endregion

        #region 激励广告回调

        /// <summary>
        /// 激励广告加载成功
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnRewardAdLoaded(string platform, double revenue, double ecpm, string precision)
        {
            OnAdLoaded(EAdType.Reward, rewardAdScore, platform, revenue, ecpm, precision);
        }

        /// <summary>
        /// 激励广告加载失败
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="errMsg">错误信息</param>
        private void OnRewardAdLoadFailed(string platform, string errMsg)
        {
            OnAdLoadFailed(EAdType.Reward, rewardAdScore, platform, errMsg);
        }

        /// <summary>
        /// 激励广告显示成功
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnRewardAdDisplayed(string platform, double revenue, double ecpm, string precision)
        {
            OnAdDisplayed(EAdType.Reward, rewardAdScore, platform, revenue, ecpm, precision);
        }

        /// <summary>
        /// 激励广告显示失败
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="errMsg">错误信息</param>
        private void OnRewardAdDisplayFailed( string platform, string errMsg)
        {
            OnAdDisplayFailed(EAdType.Reward, rewardAdScore, platform, errMsg);
        }

        /// <summary>
        /// 激励广告播放完成
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnRewardAdCompleted(string platform, double revenue, double ecpm, string precision)
        {
            OnAdCompleted(EAdType.Reward, rewardAdScore, platform, revenue, ecpm, precision);
        }

        /// <summary>
        /// 激励广告点击
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnRewardAdClicked(string platform, double revenue, double ecpm, string precision)
        {
            OnAdClicked(EAdType.Reward, rewardAdScore, platform, revenue, ecpm, precision);
        }

        /// <summary>
        /// 激励广告关闭
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnRewardAdClosed(string platform, double revenue, double ecpm, string precision)
        {
            OnAdClosed(EAdType.Reward, rewardAdScore, platform, revenue, ecpm, precision);
        }

        /// <summary>
        /// 激励广告获取收入
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnRewardAdRevenuePaid(string platform, double revenue, double ecpm, string precision)
        {
            OnAdRevenuePaid(EAdType.Reward, rewardAdScore, platform, revenue, ecpm, precision);
        }

        /// <summary>
        /// 激励广告未准备（加载）完毕
        /// </summary>
        private void OnRewardAdNotReady()
        {
            OnAdNotReady(EAdType.Reward);
        }

        /// <summary>
        /// 激励广告奖励发放
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        /// <param name="rewardAmount">奖励值</param>
        private void OnRewardAdReceivedReward(string platform, double revenue, double ecpm, string precision, int rewardAmount)
        {
            OnAdReceivedReward(EAdType.Reward, rewardAdScore, platform, revenue, ecpm, precision, rewardAmount);
        }

        #endregion

        #region 插屏广告回调

        /// <summary>
        /// 插屏广告加载成功
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnInterstitialAdLoaded(string platform, double revenue, double ecpm, string precision)
        {
            OnAdLoaded(EAdType.Interstitial, interstitialAdScore, platform, revenue, ecpm, precision);
        }

        /// <summary>
        /// 插屏广告加载失败
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="errMsg">错误信息</param>
        private void OnInterstitialAdLoadFailed(string platform, string errMsg)
        {
            OnAdLoadFailed(EAdType.Interstitial, interstitialAdScore, platform, errMsg);
        }

        /// <summary>
        /// 插屏广告显示成功
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnInterstitialAdDisplayed(string platform, double revenue, double ecpm, string precision)
        {
            OnAdDisplayed(EAdType.Interstitial, interstitialAdScore, platform, revenue, ecpm, precision);
        }

        /// <summary>
        /// 插屏广告显示失败
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="errMsg">错误信息</param>
        private void OnInterstitialAdDisplayFailed(string platform, string errMsg)
        {
            OnAdDisplayFailed(EAdType.Interstitial, interstitialAdScore, platform, errMsg);
        }

        /// <summary>
        /// 插屏广告播放完成
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnInterstitialAdCompleted(string platform, double revenue, double ecpm, string precision)
        {
            OnAdCompleted(EAdType.Interstitial, interstitialAdScore, platform, revenue, ecpm, precision);
        }

        /// <summary>
        /// 插屏广告点击
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnInterstitialAdClicked(string platform, double revenue, double ecpm, string precision)
        {
            OnAdClicked(EAdType.Interstitial, interstitialAdScore, platform, revenue, ecpm, precision);
        }

        /// <summary>
        /// 插屏广告关闭
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnInterstitialAdClosed(string platform, double revenue, double ecpm, string precision)
        {
            OnAdClosed(EAdType.Interstitial, interstitialAdScore, platform, revenue, ecpm, precision);
            OnAdReceivedReward(EAdType.Interstitial, interstitialAdScore, platform, revenue, ecpm, precision, 0);
        }

        /// <summary>
        /// 插屏广告获取收入
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnInterstitialAdRevenuePaid(string platform, double revenue, double ecpm, string precision)
        {
            OnAdRevenuePaid(EAdType.Interstitial, interstitialAdScore, platform, revenue, ecpm, precision);
        }

        /// <summary>
        /// 插屏广告未准备（加载）完毕
        /// </summary>
        private void OnInterstitialAdNotReady()
        {
            OnAdNotReady(EAdType.Interstitial);
        }

        #endregion

        #region 横幅广告回调

        /// <summary>
        /// 横幅广告加载成功
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnBannerAdLoaded(string platform, double revenue, double ecpm, string precision)
        {
            OnAdLoaded(EAdType.Banner, bannerAdScore, platform, revenue, ecpm, precision);
        }

        /// <summary>
        /// 横幅广告加载失败
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="errMsg">错误信息</param>
        private void OnBannerAdLoadFailed(string platform, string errMsg)
        {
            OnAdLoadFailed(EAdType.Banner, bannerAdScore, platform, errMsg);
        }

        /// <summary>
        /// 横幅广告显示成功
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnBannerAdDisplayed(string platform, double revenue, double ecpm, string precision)
        {
            OnAdDisplayed(EAdType.Banner, bannerAdScore, platform, revenue, ecpm, precision);
        }

        /// <summary>
        /// 横幅广告显示失败
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="errMsg">错误信息</param>
        private void OnBannerAdDisplayFailed(string platform, string errMsg)
        {
            OnAdDisplayFailed(EAdType.Banner, bannerAdScore, platform, errMsg);
        }

        /// <summary>
        /// 横幅广告播放完成
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnBannerAdCompleted(string platform, double revenue, double ecpm, string precision)
        {
            OnAdCompleted(EAdType.Banner, bannerAdScore, platform, revenue, ecpm, precision);
        }

        /// <summary>
        /// 横幅广告点击
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnBannerAdClicked(string platform, double revenue, double ecpm, string precision)
        {
            OnAdClicked(EAdType.Banner, bannerAdScore, platform, revenue, ecpm, precision);
            OnAdReceivedReward(EAdType.Banner, bannerAdScore, platform, revenue, ecpm, precision, 0);
        }

        /// <summary>
        /// 横幅广告关闭
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnBannerAdClosed(string platform, double revenue, double ecpm, string precision)
        {
            OnAdClosed(EAdType.Banner, bannerAdScore, platform, revenue, ecpm, precision);
        }

        /// <summary>
        /// 横幅广告获取收入
        /// </summary>
        /// <param name="platform">广告平台</param>
        /// <param name="revenue">单次收入</param>
        /// <param name="ecpm">ECPM</param>
        /// <param name="precision">精度</param>
        private void OnBannerAdRevenuePaid(string platform, double revenue, double ecpm, string precision)
        {
            OnAdRevenuePaid(EAdType.Banner, bannerAdScore, platform, revenue, ecpm, precision);
            
        }

        /// <summary>
        /// 横幅广告未准备（加载）完毕
        /// </summary>
        private void OnBannerAdNotReady()
        {
            OnAdNotReady(EAdType.Banner);
        }

        #endregion

        #endregion

        protected override void OnDispose()
        {
            RemoveFacade();
        }
    }
}
