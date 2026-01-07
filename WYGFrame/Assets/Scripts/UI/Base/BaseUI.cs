/**
 * 基础视图
 */

using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
namespace XrCode
{
    public abstract class BaseUI : Entity, IUnityObject
    {
        private CanvasGroup _canvasGroup;
        private GraphicRaycaster _graphicRaycaster;
        protected GameObject mGameObject;
        protected Transform mTransform;

        public EUIType UIType { get; private set; }
        public bool IsFullScreen { get; private set; }

        /// 创建
        public void Create(GameObject gameObject)
        {
            mGameObject = gameObject;
            mTransform = mGameObject.transform;
            LoadPanel();
            BindButtonEvent();
            BindUIEvent();
        }

        public void SetFullScreen(bool full)
        {
            IsFullScreen = full;
        }


        public void SetUIType(EUIType uiType)
        {
            UIType = uiType;
        }
        //加载ui面板
        protected virtual void LoadPanel()
        {
            this._canvasGroup = mTransform.GetComponent<CanvasGroup>();
            this._graphicRaycaster = this.mTransform.GetComponent<GraphicRaycaster>();
        }
        //设置ui启动参数
        public void SetParam(params object[] args) { OnSetParam(args); }
        protected virtual void OnSetParam(params object[] args) { }
        public void Awake() { OnAwake(); }
        protected virtual void OnAwake() { }
        public void SetAlpha(int alpha)
        {
            this._canvasGroup.alpha = alpha;
        }

        public bool GetActive()
        {
            return this._canvasGroup.alpha == 1;
        }

        public void Show()
        {
            if (this._graphicRaycaster != null) this._graphicRaycaster.enabled = true;
            this._canvasGroup.blocksRaycasts = true;
            SetAlpha(1);
        }

        public void Hide()
        {
            if (this._graphicRaycaster != null) this._graphicRaycaster.enabled = true;
            this._canvasGroup.blocksRaycasts = false;
            SetAlpha(0);
        }

        public void Enable() { OnEnable(); }
        public void Update() { OnUpdate(); }
        public void Disable() { OnDisable(); }

        protected virtual void OnEnable() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnDisable() { }

        protected override void OnDispose()
        {
            base.OnDispose();
            UnBindButtonEvent();
            UnBindUIEvent();
        }

        protected virtual void BindUIEvent() { }
        protected virtual void UnBindUIEvent() { }
        protected virtual void BindButtonEvent() { }
        protected virtual void UnBindButtonEvent() { }

        /// 设置层级
        public virtual void SetSiblingIndex(int siblingIndex)
        {
            if (mGameObject != null)
            {
                mGameObject.transform.SetSiblingIndex(siblingIndex);
            }
        }

        protected void ShowAnim(RectTransform target, Action successAction = null)
        {
            UIManager.Instance.SetGraphicRaycaster(false);
            target.localScale = Vector3.zero;
            target.DOScale(Vector3.one, GameDefines.ShowAnimTime).SetEase(Ease.OutBack).OnComplete(() => 
            { 
                successAction?.Invoke();
                UIManager.Instance.SetGraphicRaycaster(true);
            });
        }

        protected void HideAnim(RectTransform target, Action successAction = null)
        {
            UIManager.Instance.SetGraphicRaycaster(false);
            target.localScale = Vector3.one;
            target.DOScale(Vector3.zero, GameDefines.ShowAnimTime).SetEase(Ease.InBack).OnComplete(() =>
            {
                target.localScale = Vector3.one;
                successAction?.Invoke();
                UIManager.Instance.SetGraphicRaycaster(true);
            });
        }

    }
}