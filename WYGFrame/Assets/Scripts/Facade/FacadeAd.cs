using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FacadeAd
{
    public static Action<EAdSource, Action<int>, Action> PlayRewardAd;                      //播放激励广告
    public static Action<EAdSource, Action<int>, Action> PlayInterAd;                       //播放插屏广告
    public static Action<EAdSource, Action<int>, Action> PlayBannerAd;                      //播放横幅广告
    public static Action<EAdSource> StopBannerAd;                                           //停止横幅广告

    public static Action<string, double, double, string> OnRewardAdLoaded;                  //激励广告加载成功回调
    public static Action<string, string> OnRewardAdLoadFailed;                              //激励广告加载失败回调
    public static Action<string, double, double, string> OnRewardAdDisplayed;               //激励广告播放成功回调
    public static Action<string, string> OnRewardAdDisplayFailed;                           //激励广告播放失败回调
    public static Action<string, double, double, string> OnRewardAdCompleted;               //激励广告结束回调
    public static Action<string, double, double, string> OnRewardAdClicked;                 //激励广告点击回调
    public static Action<string, double, double, string> OnRewardAdClosed;                  //激励广告关闭回调
    public static Action<string, double, double, string> OnRewardAdRevenuePaid;             //激励广告收入回调
    public static Action OnRewardAdNotReady;                                                //激励广告未准备回调
    public static Action<string, double, double, string, int> OnRewardAdReceivedReward;     //激励广告奖励回调

    public static Action<string, double, double, string> OnInterstitialAdLoaded;            //插屏广告加载成功回调
    public static Action<string, string> OnInterstitialAdLoadFailed;                        //插屏广告加载失败回调
    public static Action<string, double, double, string> OnInterstitialAdDisplayed;         //插屏广告播放成功回调
    public static Action<string, string> OnInterstitialAdDisplayFailed;                     //插屏广告播放失败回调
    public static Action<string, double, double, string> OnInterstitialAdCompleted;         //插屏广告结束回调
    public static Action<string, double, double, string> OnInterstitialAdClicked;           //插屏广告点击回调
    public static Action<string, double, double, string> OnInterstitialAdClosed;            //插屏广告关闭回调
    public static Action<string, double, double, string> OnInterstitialAdRevenuePaid;       //插屏广告收入回调
    public static Action OnInterstitialAdNotReady;                                          //激励广告未准备回调

    public static Action<string, double, double, string> OnBannerAdLoaded;                  //横幅广告加载成功回调
    public static Action<string, string> OnBannerAdLoadFailed;                              //横幅广告加载失败回调
    public static Action<string, double, double, string> OnBannerAdDisplayed;               //横幅广告播放成功回调
    public static Action<string, string> OnBannerAdDisplayFailed;                           //横幅广告播放失败回调
    public static Action<string, double, double, string> OnBannerAdCompleted;               //横幅广告结束回调
    public static Action<string, double, double, string> OnBannerAdClicked;                 //横幅广告点击回调
    public static Action<string, double, double, string> OnBannerAdClosed;                  //横幅广告关闭回调
    public static Action<string, double, double, string> OnBannerAdRevenuePaid;             //横幅广告收入回调
    public static Action OnBannerAdNotReady;                                                //横幅广告未准备回调
}
