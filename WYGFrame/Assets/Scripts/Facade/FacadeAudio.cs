using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//“Ù∆µ∂®“Â¿‡
public static class FacadeAudio
{
    public static Action PlayBgm;
    public static Action StopBgm;
    public static Action<EAudioType> PlayEffect;
    public static Action PlayVibrate;
    public static Action<float> SetMusicVolume;
    public static Action<float> SetEffectsVolume;
    public static Action<bool> SetVibrate;
    public static Func<float> GetMusicVolume;
    public static Func<float> GetEffectsVolume;
    public static Func<bool> GetVibrate;
    public static Func<string, EAudioType> GetEATypeByString;
}

