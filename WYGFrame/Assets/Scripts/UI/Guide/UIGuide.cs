using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{

    public partial class UIGuide : BaseUI
    {
        private Action clickAction;
        private TDAnalyticsManager TDAnalyticsManager;

        private Dictionary<string, RectTransform> posTrans;

        private List<Transform> diglogs;
        private List<Transform> hands;
        private List<Transform> transparents;
        private List<Transform> clicks;

        protected override void OnAwake()
        {
            TDAnalyticsManager = ModuleMgr.Instance.TDAnalyticsManager;

            mGuidePlane.gameObject.SetActive(false);

            FacadeGuide.PlayGuide += PlayGuide;
            FacadeGuide.CloseGuide += CloseGuide;
        }
        protected override void OnEnable()
        {

        }

        //引导按钮回调
        private void OnGuideBtnClickHandle()
        {
            clickAction?.Invoke();
        }

        private void SetGuideShow()
        {
            GuideItem info = FacadeGuide.GetCurGuideItems();
            bool ifshow;

            ifshow = info.diglogPos != null;
            mGuideTextFather.gameObject.SetActive(ifshow);
            if (ifshow)
            {
                SetDialog(FindTrans(info.diglogPos), info.diglogContent);
            }

            ifshow = info.handPos != null;
            mHandFather.gameObject.SetActive(ifshow);
            if (ifshow)
            {
                SetHander(FindTrans(info.handPos));
            }

            mHoleMask.alpha = info.ifMask ? 1 : 0;

            ifshow = info.transparentPos != null;
            if (ifshow)
            {
                SetHole(FindTrans(info.transparentPos));
            }

            float autoHiddenTime = info.autohiddenTime;
            if (autoHiddenTime != 0)
            {
                STimerManager.Instance.CreateSDelay(autoHiddenTime, () =>
                {
                    FacadeGuide.NextStep();
                });
            }
            else
            {
                ifshow = info.clickPos != null;
                mMask.penetrateObjs.Clear();
                if (ifshow)
                {
                    List<Transform> transPath = FindTrans(info.clickPos);
                    foreach (Transform tran in transPath)
                    {
                        mMask.penetrateObjs.Add(tran.GetComponent<RectTransform>());
                    }

                    mMask.ifNext = info.ifNextPlay;
                }
            }

            if (info.extraStart != null && info.extraStart.Count != 0)
                SetExtraShow(info.extraStart);
        }

        /// <summary>
        /// 设置提示框
        /// </summary>
        /// <param name="newTarget">新的目标</param>
        /// <param name="newContent">新的内容</param>
        private void SetDialog(List<Transform> newTarget, List<string> newContent)
        {
            for (int i = 0; i < newTarget.Count; i++)
            {
                Transform tempTran;
                if (i < diglogs.Count)
                {
                    tempTran = diglogs[i];
                    tempTran.gameObject.SetActive(true);
                    Debug.LogError("111");
                }
                else
                {
                    GameObject obj = GameObject.Instantiate(mGuideText.gameObject, mGuideTextFather);
                    obj.gameObject.SetActive(true);
                    tempTran = obj.GetComponent<Transform>();
                    diglogs.Add(tempTran);
                    Debug.LogError("222");
                }

                tempTran.position = newTarget[i].position;
                tempTran.GetComponent<RectTransform>().sizeDelta = newTarget[i].GetComponent<RectTransform>().sizeDelta;
                tempTran.GetChild(1).GetComponent<Text>().text = newContent[i];
            }
        }

        /// <summary>
        /// 设置手位置
        /// </summary>
        /// <param name="newTarget"></param>
        private void SetHander(List<Transform> newTarget)
        {
            for (int i = 0; i < newTarget.Count; i++)
            {
                Transform tempTran;
                if (i < hands.Count)
                {
                    tempTran = hands[i];
                    tempTran.gameObject.SetActive(true);
                }
                else
                {
                    GameObject obj = GameObject.Instantiate(mHander.gameObject, mHandFather);
                    obj.gameObject.SetActive(true);
                    tempTran = obj.GetComponent<Transform>();
                    hands.Add(tempTran);
                }

                tempTran.position = newTarget[i].position;
                tempTran.GetComponent<RectTransform>().sizeDelta = newTarget[i].GetComponent<RectTransform>().sizeDelta;
                tempTran.GetChild(0).GetComponent<AutoHandSwing>().Reset();
            }
        }

        private void SetHole(List<Transform> newTarget)
        {
            for (int i = 0; i < newTarget.Count; i++)
            {
                Transform tempTran;
                if (i < transparents.Count)
                {
                    tempTran = transparents[i];
                    tempTran.gameObject.SetActive(true);
                }
                else
                {
                    GameObject obj = GameObject.Instantiate(mHole.gameObject, mHoleFather);
                    obj.gameObject.SetActive(true);
                    tempTran = obj.GetComponent<Transform>();
                    transparents.Add(tempTran);
                }

                tempTran.position = newTarget[i].position;
                tempTran.GetComponent<RectTransform>().sizeDelta = newTarget[i].GetComponent<RectTransform>().sizeDelta;
            }
        }

        private void SetExtraShow(Dictionary<string, string> extraData)
        {
            foreach (KeyValuePair<string, string> kvp in extraData)
            {
                switch (kvp.Key)
                {
                    case "falseT":

                        break;
                    case "OnceW":

                        break;
                }
            }
        }

        private void PlayGuide()
        {
            if (!FacadeGuide.GetIfTutorial())
                return;
            //TDAnalyticsManager.GuideStep(FacadeGuide.GetCurStep());

            mGuidePlane.gameObject.SetActive(true);

            foreach (Transform objTrans in diglogs)
            {
                objTrans.gameObject.SetActive(false);
            }
            foreach (Transform objTrans in hands)
            {
                objTrans.gameObject.SetActive(false);
            }
            foreach (Transform objTrans in transparents)
            {
                objTrans.gameObject.SetActive(false);
            }
            clicks.Clear();

            SetGuideShow();
        }

        private void CloseGuide()
        {
            mGuidePlane.gameObject.SetActive(false);
        }

        private List<Transform> FindTrans(List<string> paths)
        {
            List<Transform> trans = new List<Transform>();
            foreach (string path in paths)
            {
                trans.Add(GameObject.Find(path).transform);
            }

            return trans;
        }

        protected override void OnDisable() { }
        protected override void OnDispose() { }
    }
}