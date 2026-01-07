using cfg;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace XrCode
{
    public class AudioModule : BaseModule
    {
        private AudioSource musicSource;                                  // 音效和背景音乐的音源
        private List<AudioSource> effectSources;
        private Dictionary<int, AudioClip> clipMap;                       // 音频缓存
        private bool isPlayBgming;                                        // 是否正在播放背景音乐
        private GameObject gameObject;
        private float musicVolume = 0.45f;                                 // 音量控制
        private float effectsVolume = 0.6f;
        private float mCoolDownDur = 0.05f;                                // 控制按钮点击频率
        private bool enableBtn = true;                                    // 控制按钮是否激活

        private bool ifVibrate = true;                                    //是否震动

        protected override void OnLoad()
        {
            FacadeAudio.PlayBgm += PlayBgm;
            FacadeAudio.StopBgm += StopBgm;
            FacadeAudio.PlayEffect += PlayEffect;
            FacadeAudio.PlayVibrate += PlayVibrate;
            FacadeAudio.SetMusicVolume += SetMusicVolume;
            FacadeAudio.SetEffectsVolume += SetEffectsVolume;
            FacadeAudio.SetVibrate += SetVibrate;
            FacadeAudio.GetMusicVolume += GetMusicVolume;
            FacadeAudio.GetEffectsVolume += GetEffectsVolume;
            FacadeAudio.GetVibrate += GetVibrate;
            FacadeAudio.GetEATypeByString += GetEATypeByString;

            gameObject = new GameObject("AudioModule");
            GameObject.DontDestroyOnLoad(gameObject);
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;  // 背景音乐默认循环
            effectSources = new List<AudioSource>();
            clipMap = new Dictionary<int, AudioClip>();
            RedirectButton();

            LoadData();

            PlayBgm();
        }

        private void LoadData()
        {
            musicVolume = SPlayerPrefs.GetBool(PlayerPrefDefines.musicToggle, true) ? 1 : 0;
            effectsVolume = SPlayerPrefs.GetBool(PlayerPrefDefines.soundToggle, true) ? 1 : 0;

            ifVibrate = SPlayerPrefs.GetBool(PlayerPrefDefines.vibrateToggle, true);
        }

        // 统一处理按钮事件
        private void RedirectButton()
        {
            typeof(ExecuteEvents).GetField("s_PointerClickHandler", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, new ExecuteEvents.EventFunction<IPointerClickHandler>(OnPointerClick));

            typeof(ExecuteEvents).GetField("s_PointerDownHandler", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, new ExecuteEvents.EventFunction<IPointerDownHandler>(OnPointerDown));
            typeof(ExecuteEvents).GetField("s_PointerUpHandler", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, new ExecuteEvents.EventFunction<IPointerUpHandler>(OnPointerUp));
        }
        void OnPointerClick(IPointerClickHandler handler, BaseEventData eventData)
        {
            PointerEventData pointerEventData = ExecuteEvents.ValidateEventData<PointerEventData>(eventData);
            if (pointerEventData != null)
            {
                if (!enableBtn) return;

                //区分物品和按钮

                if (eventData.selectedObject == null)
                {
                    PlayButtonSound();
                }
                else
                {
                    
                }
                handler.OnPointerClick(pointerEventData);
                enableBtn = false;
                // 自己实现的一个计时器
                TimerManager.Instance.CreateTimer(mCoolDownDur, () => { enableBtn = true; }, 0);

                //ModuleMgr.Instance.TDAnalyticsManager.ButtonClick(eventData.selectedObject);
            }
        }

        public void OnPointerDown(IPointerDownHandler handler, BaseEventData eventData)
        {

            PointerEventData pointerEventData = ExecuteEvents.ValidateEventData<PointerEventData>(eventData);
            if (pointerEventData != null)
            {
                if (pointerEventData.pointerPressRaycast.gameObject != null)
                {
                    GameObject obj = pointerEventData.pointerPressRaycast.gameObject;
                    if (obj.transform.tag != "NoUpDownAnim" && obj.transform.GetComponent<Button>() != null)
                    {
                        obj.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
                    }
                }
                handler.OnPointerDown(pointerEventData);
            }
        }

        public void OnPointerUp(IPointerUpHandler handler, BaseEventData eventData)
        {
            
            PointerEventData pointerEventData = ExecuteEvents.ValidateEventData<PointerEventData>(eventData);
            if (pointerEventData != null)
            {
                if (eventData.selectedObject != null)
                {
                    GameObject obj = eventData.selectedObject;

                    if (obj.transform.tag != "NoUpDownAnim" && obj.transform.GetComponent<Button>() != null)
                    {
                        obj.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
                    }
                }
                handler.OnPointerUp(pointerEventData);
            }
        }


        protected override void OnDispose()
        {
            base.OnDispose();

            FacadeAudio.PlayBgm -= PlayBgm;
            FacadeAudio.StopBgm -= StopBgm;
            FacadeAudio.PlayEffect -= PlayEffect;
            FacadeAudio.PlayVibrate -= PlayVibrate;
            FacadeAudio.SetMusicVolume -= SetMusicVolume;
            FacadeAudio.SetEffectsVolume -= SetEffectsVolume;
            FacadeAudio.SetVibrate -= SetVibrate;
            FacadeAudio.GetMusicVolume -= GetMusicVolume;
            FacadeAudio.GetEffectsVolume -= GetEffectsVolume;
            FacadeAudio.GetVibrate -= GetVibrate;
            FacadeAudio.GetEATypeByString -= GetEATypeByString;

            gameObject = null;
            musicSource = null;

            StopBgm();

            if(effectSources != null)
            {
                for (int i = 0; i < effectSources.Count; i++)
                {
                    effectSources[i].clip = null;
                    effectSources = null;
                }
                effectSources.Clear();
            }

            clipMap.Clear();
            clipMap = null;
        }

        // 获取音频文件，优先缓存里面取，没有再加载。
        private AudioClip GetAudioClip(EAudioType aType)
        {
            int audioId = (int)aType;
            if (!clipMap.TryGetValue(audioId, out AudioClip clip))
            {
                ConfAudio conf = ConfigModule.Instance.Tables.TBAudio.Get(audioId);
                if (conf == null) return null;
                clip = ResourceMod.Instance.SyncLoad<AudioClip>(conf.AudioPath);
                clipMap.Add(audioId, clip);
            }
            return clip;
        }

        private void PlayButtonSound()
        {
            D.Log("统一播放按钮音效");
            AudioClip clip = GetAudioClip(EAudioType.EButton);
            if (clip == null) return;
            AudioSource source = GetAvailableAudioSource();
            source.clip = clip;
            source.volume = effectsVolume;
            source.Play();
        }

        // 播放背景音乐
        private void PlayBgm()
        {
            if (isPlayBgming) return;
            isPlayBgming = true;
            AudioClip bgm = GetAudioClip(EAudioType.EBgm);
            musicSource.clip = bgm;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
        // 停止背景音乐
        private void StopBgm()
        {
            if (!isPlayBgming) return;
            isPlayBgming = false;
            if(musicSource != null)
                musicSource.Stop();
        }

        // 播放音效
        private void PlayEffect(EAudioType aType)
        {
            AudioClip clip = GetAudioClip(aType);
            if (clip == null) return;
            AudioSource source = GetAvailableAudioSource();
            source.clip = clip;
            source.volume = effectsVolume;
            source.Play();
        }
        // 获取一个空闲的音源
        private AudioSource GetAvailableAudioSource()
        {
            AudioSource source = effectSources.Find(s => !s.isPlaying);
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
                effectSources.Add(source);
            }
            return source;
        }

        // 设置背景音乐音量
        private void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            musicSource.volume = musicVolume;

            SPlayerPrefs.SetBool(PlayerPrefDefines.musicToggle, volume == 1);
            SPlayerPrefs.Save();
        }

        // 设置音效音量
        private void SetEffectsVolume(float volume)
        {
            effectsVolume = Mathf.Clamp01(volume);
            foreach (var source in effectSources)
            {
                source.volume = effectsVolume;
            }

            SPlayerPrefs.SetBool(PlayerPrefDefines.soundToggle, volume == 1);
            SPlayerPrefs.Save();
        }

        private void PlayVibrate()
        {
            if (ifVibrate)
            {
#if UNITY_ANDROID
                if (SystemInfo.supportsVibration)
                    Handheld.Vibrate();
#elif UNITY_IOS
                if(SystemInfo.supportsVibration)
                    Handheld.Vibrate();
#endif
            }
        }

        private void SetVibrate(bool b)
        {
            ifVibrate = b;
            SPlayerPrefs.SetBool(PlayerPrefDefines.vibrateToggle, b);
            SPlayerPrefs.Save();
        }

        private float GetMusicVolume()
        {
            return musicVolume;
        }

        private float GetEffectsVolume()
        {
            return effectsVolume;
        }

        private bool GetVibrate()
        {
            return ifVibrate;
        }

        private EAudioType GetEATypeByString(string str)
        {
            switch (str) 
            {
                case "merge_1":
                    return EAudioType.EMerge_1;
                case "merge_2":
                    return EAudioType.EMerge_2;
                case "merge_3":
                    return EAudioType.EMerge_3;
                case "merge_4":
                    return EAudioType.EMerge_4;
                default:
                    return EAudioType.EMerge_1;
            }
        }

    }
}