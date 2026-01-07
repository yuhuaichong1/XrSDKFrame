
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{

    public partial class UINotice : BaseUI
    {
        private GameObject NItem;

        private Stack<GameObject> NItems;
        protected override void OnAwake() 
        {
            NItem = ResourceMod.Instance.SyncLoad<GameObject>("Prefabs/UI/Notice/NoticeItem.prefab");
            NItems = new Stack<GameObject>();
        }

        protected override void OnEnable()
        {
            mGameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public GameObject ShowInfo(string info, float time = 2)
        {
            GameObject temp;
            if (NItems.Count != 0)
            {
                temp = NItems.Pop();
                temp.SetActive(true);
            }
            else
            {
                temp = GameObject.Instantiate(NItem, mTransform);
            }
            float elapsedTime = 0;
            temp.transform.position = mStartPos.position;
            temp.transform.GetChild(0).GetComponent<Text>().text = info;

            STimerManager.Instance.CreateSTimer(time, 0, false, true, () =>
            {
                temp.SetActive(false);
                NItems.Push(temp);
            }, (detalTime) =>
            {
                elapsedTime += detalTime;
                float t = Mathf.Clamp01(elapsedTime/ time);
                temp.transform.position = Vector3.Lerp(mStartPos.position, mEndPos.position, t);
            });

            return temp;
        }

        public void HideInfo(GameObject obj)
        {
            obj.SetActive(false);
            NItems.Push(obj);
        }

        protected override void OnDisable()
        {
        
        }
        protected override void OnDispose()
        {
        
        }
    }
}