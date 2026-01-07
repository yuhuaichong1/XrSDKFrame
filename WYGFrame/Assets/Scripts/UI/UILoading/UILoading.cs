
using System;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;
namespace XrCode
{
    public partial class UILoading : BaseUI
    {
        protected override void OnAwake()
        {
            ModuleMgr.Instance.SceneMod.LoadingValue += ChangeValue;
            ModuleMgr.Instance.SceneMod.OnLoadChanged += ChangeValue2;

        }
        protected override void OnEnable() 
        {
            mGameTitle.sprite = ResourceMod.Instance.SyncLoad<Sprite>($"UI/Logo/{FacadePayType.GetLanguage()}_Logo.png");
            mGameTitle.SetNativeSize();
        }

        void ChangeValue2(string str)
        {
            Debug.Log("当前加载的场景名称：：" + str);
        }


        private float needtime;
        private bool canload;
        public void SilderMove()
        {
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
            needtime = UnityEngine.Random.Range(1, 6);
            mLoadingSlider.value = 0;
            canload = true;
        }

        public void ChangeValue(float asynvalue)
        {
            mLoadingSlider.value = asynvalue;
            mLoadingSche.text = Mathf.RoundToInt(asynvalue).ToString() + "%";
        }


        protected override void OnUpdate()
        {
            /*
            if(canload)
            {
                mLoadingSche.text = Mathf.RoundToInt(mLoadingSlider.value).ToString() + "%";
                mLoadingSlider.value += Time.deltaTime * needtime;
            }
            */
        }

        protected override void OnDisable() { }
        protected override void OnDispose()
        {
            ModuleMgr.Instance.SceneMod.LoadingValue -= ChangeValue;
            ModuleMgr.Instance.SceneMod.OnLoadChanged -= ChangeValue2;
        }
    }
}