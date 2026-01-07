/**
 * UI视图管理器
 */

using UnityEngine;
using System.Collections.Generic;
using System;
using cfg;
using UnityEngine.UI;
using System.IO;

namespace XrCode
{
    public class UIManager : Singleton<UIManager>, ILoad, IDispose
    {
        // UI根节点
        private Transform canvas;
        private BaseUI curUI;
        private GraphicRaycaster graphicRaycaster;
        // UI字典
        private Dictionary<byte, BaseUI> uiDic;

        private Dictionary<int, Transform> uiLevelNodeDic;
        public Camera UICamera { get; private set; }

        private UINotice uiNotice;

        /// <summary>
        /// 初始化Canvas
        /// </summary>
        public void Load()
        {
            uiDic = new Dictionary<byte, BaseUI>();
            canvas = GameObject.Find("Canvas").transform;
            UICamera = canvas.Find("UICamera").GetComponent<Camera>();
            graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
            uiLevelNodeDic = new Dictionary<int, Transform>()
        {
            {  1, canvas.Find("Level1")},
            {  2, canvas.Find("Level2")},
            {  3, canvas.Find("Level3")},
            {  4, canvas.Find("Level4")},
            {  5, canvas.Find("Level5")},
            {  6, canvas.Find("Level6")},
            {  7, canvas.Find("Level7")},
            {  8, canvas.Find("Level8")},
        };
            ViewConfig.Init();
        }
        public void Dispose() { }


        ///// <summary>
        ///// 创建View
        ///// </summary>
        //private BaseUI Create<T>(params object[] data) where T : BaseUI, new()
        //{
        //    var path = ViewConfig.GetViewPath(typeof(T));
        //    var go = GameObjectManager.Instance.SyncSpawn(path);
        //    go.transform.SetParent(canvas, false);
        //    var view = new T();
        //    //view.Create(go);
        //    if (data != null)
        //    {
        //        view.SetParam(data);
        //    }
        //    uiDic.Add(view.ID, view);
        //    return view;
        //}

        #region 异步打开UIPanel

        // 异步打开UI界面
        public void OpenAsync<T>(EUIType uType, System.Action<BaseUI> callBack = null, params object[] data) where T : BaseUI, new()
        {
            // 同一UI不重复打开
            if (curUI != null && curUI.UIType == uType) return;

            //从缓存中取ui
            if (!uiDic.TryGetValue((byte)uType, out BaseUI ui))
            {
                AsyncLoadUI<T>(uType, callBack, data);
                return;
            }
            OpenUI(ui);
            ui.SetParam(data);
            ui.Show();
            ui.Enable();
            callBack?.Invoke(ui);
        }

        //异步加载UI
        private void AsyncLoadUI<T>(EUIType uType, Action<BaseUI> cb, params object[] data) where T : BaseUI, new()
        {
            // 第一次加载新的ui
            ConfUIRes conf = ConfigModule.Instance.Tables.TbUIRes.GetOrDefault((int)uType);
            if (conf != null)
            {
                Debug.Log($"[ConfUI]: 加载ui {conf.Sn} ___ {conf.UiPath}");
                ResourceMod.Instance.AsyncLoad<UnityEngine.Object>(conf.UiPath,
                    (obj) =>
                    {
                        GameObject uiObj = InitUITransform(obj, conf.UiLevel);
                        T t = LoadUI<T>(uType, uiObj, conf, data);
                        cb?.Invoke(t);
                    });
            }

        }

        #endregion

        #region 同步打开UIPanel

        // 同步打开UI界面
        public void OpenSync<T>(EUIType uType, System.Action<BaseUI> callBack = null, params object[] data) where T : BaseUI, new()
        {
            // 同一UI不重复打开
            if (curUI != null && curUI.UIType == uType) return;

            //从缓存中取ui
            if (!uiDic.TryGetValue((byte)uType, out BaseUI ui))
            {
                SyncLoadUI<T>(uType, callBack, data);
                return;
            }

            OpenUI(ui);
            ui.SetParam(data);
            ui.Show();
            ui.Enable();
            callBack?.Invoke(ui);
        }

        //同步加载UI
        private void SyncLoadUI<T>(EUIType uType, Action<BaseUI> cb, params object[] data) where T : BaseUI, new()
        {
            // 第一次加载新的ui
            ConfUIRes conf = ConfigModule.Instance.Tables.TbUIRes.GetOrDefault((int)uType);
            if (conf != null)
            {
                Debug.Log($"[ConfUI]: 加载ui {conf.Sn}");
#if UNITY_EDITOR
                var obj = ResourceMod.Instance.SyncLoad<GameObject>(conf.UiPath);
                GameObject go = InitUITransform(obj, conf.UiLevel);
                T t = LoadUI<T>(uType, go, conf, data);
                cb?.Invoke(t);
#elif UNITY_WEBGL
            //webgl平台同步加载也为异步加载
            ResourceMod.Instance.AsyncLoad<UnityEngine.Object>(conf.UiPath, (obj)=>
            {
                GameObject uiObj = InitUITransform(obj, conf.UiLevel);
                T t = LoadUI<T>(uType, uiObj, conf, data);
                cb?.Invoke(t);
            });
#endif
            }
        }

        #endregion

        //打开ui
        private void OpenUI(BaseUI ui)
        {
            if (curUI != null)
            {
                if (ui.IsFullScreen) curUI.Hide();
                curUI.Disable();
            }
            curUI = ui;
        }

        // 初始化ui gameObject
        private GameObject InitUITransform(UnityEngine.Object obj, int UiLevel)
        {
            GameObject go = GameObject.Instantiate(obj) as GameObject;
            Transform parent = uiLevelNodeDic[UiLevel];
            go.transform.parent = parent;
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMax = Vector2.one;
            rt.anchorMin = Vector2.zero;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
            rt.localScale = Vector3.one;
            rt.localPosition = Vector3.zero;
            return go;
        }

        // 加载ui对象
        private T LoadUI<T>(EUIType uType, GameObject go, ConfUIRes conf, params object[] data) where T : BaseUI, new()
        {
            T ui = new T();
            ui.Create(go);
            ui.SetFullScreen(conf.IsFullScreen);

            OpenUI(ui);

            ui.SetUIType(uType);
            ui.SetParam(data);
            ui.Awake();
            ui.Show();
            ui.Enable();
            if (!uiDic.ContainsKey((byte)uType))
                uiDic.Add((byte)uType, ui);
            curUI = ui;
            return (T)ui;
        }
        public void CloseUI(EUIType uiType)
        {
            if (uiDic.TryGetValue((byte)uiType, out BaseUI ui))
            {
                ui.Hide();
                if (curUI != null && curUI.UIType == uiType)
                {
                    curUI.Disable();
                    curUI = null;
                }
            }
        }

        public void Show(EUIType uiType)
        {
            if (uiDic.TryGetValue((byte)uiType, out BaseUI ui))
            {
                ui.Show();
            }
        }

        #region 以弹窗的形式异步打开UI（不抢占主UI）
        //public void OpenWindowAsync<T>(EUIType uType, System.Action<BaseUI> callBack = null, params object[] data) where T : BaseUI, new()
        //{
        //    if(!uiDic.TryGetValue((byte)uType, out BaseUI ui))
        //    {
        //        AsyncLoadWindowUI<T>(uType, callBack, data);
        //        return;
        //    }
        //    ui.SetParam(data);
        //    ui.Show();
        //    ui.Enable();
        //    callBack?.Invoke(ui);
        //}

        //private void AsyncLoadWindowUI<T>(EUIType uType, Action<BaseUI> cb, params object[] data) where T : BaseUI, new()
        //{
        //    // 第一次加载新的ui
        //    ConfUIRes conf = ConfigModule.Instance.Tables.TbUIRes.GetOrDefault((int)uType);
        //    if (conf != null)
        //    {
        //        Debug.Log($"[ConfUI]: 加载ui {conf.Sn} ___ {conf.UiPath}");
        //        ResourceMod.Instance.AsyncLoad<UnityEngine.Object>(conf.UiPath,
        //            (obj) =>
        //            {
        //                GameObject uiObj = InitUITransform(obj, conf.UiLevel);
        //                T t = LoadWindowUI<T>(uType, uiObj, conf, data);
        //                cb?.Invoke(t);
        //            });
        //    }
        //}

        //private T LoadWindowUI<T>(EUIType uType, GameObject go, ConfUIRes conf, params object[] data) where T : BaseUI, new()
        //{
        //    T ui = new T();
        //    ui.Create(go);
        //    ui.SetFullScreen(conf.IsFullScreen);

        //    ui.SetUIType(uType);
        //    ui.SetParam(data);
        //    ui.Awake();
        //    ui.Show();
        //    ui.Enable();
        //    if (!uiDic.ContainsKey((byte)uType))
        //        uiDic.Add((byte)uType, ui);
        //    if (uType != EUIType.EUIPopup) curUI = ui;
        //    return (T)ui;
        //}
        #endregion

        public GameObject OpenNotice(string msg, float time = 2)
        {
            GameObject obj = null;
            if(uiNotice != null)
            {
                obj = uiNotice.ShowInfo(msg, time);
            }
            else
            {
                ConfUIRes conf = ConfigModule.Instance.Tables.TbUIRes.GetOrDefault((int)EUIType.EUINotice);
                if (conf != null)
                {
                    Debug.Log($"[ConfUI]: 加载ui {conf.Sn} ___ {conf.UiPath}");
                    ResourceMod.Instance.AsyncLoad<UnityEngine.Object>(conf.UiPath,
                        (obj) =>
                        {
                            GameObject uiObj = InitUITransform(obj, conf.UiLevel);
                            uiNotice = LoadUI<UINotice>(EUIType.EUINotice, uiObj, conf, null);
                            obj = uiNotice.ShowInfo(msg, time);
                        });
                }
                
            }

            return obj;
        }

        public void HideNotice(GameObject notice)
        {
            uiNotice.HideInfo(notice);
        }

        public string GetUIPath(string[] strArray)
        {
            ConfUIRes confData = ConfigModule.Instance.Tables.TbUIRes.Get(int.Parse(strArray[0]));
            int uiLevel = confData.UiLevel;
            string uiName = Path.GetFileNameWithoutExtension(confData.UiPath);
            string objPath = strArray[1];

            return $"{canvas.name}/{uiLevelNodeDic[uiLevel].name}/{uiName}(Clone)/{objPath}";
        }

        public void Update()
        {
            curUI?.Update();
        }

        public void SetGraphicRaycaster(bool b)
        {
            if (graphicRaycaster != null)
                graphicRaycaster.enabled = b;
        }
    }

}