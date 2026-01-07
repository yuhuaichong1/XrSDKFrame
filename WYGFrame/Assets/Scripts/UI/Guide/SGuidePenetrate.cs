using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SGuidePenetrate : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image mask;
    public List<RectTransform> penetrateObjs;
    public bool ifNext;

    void Awake()
    {
        penetrateObjs = new List<RectTransform>();
        mask = GetComponent<Image>();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (var penetrateObj in penetrateObjs)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(penetrateObj, eventData.position, eventData.pressEventCamera))
            {
                // 如果点击在目标区域内，则传递事件
                ExecuteEvents.Execute(penetrateObj.gameObject, eventData, ExecuteEvents.pointerClickHandler);

                FacadeGuide.NextStep();

                return;
            }
        }

        // 其他区域保持原有行为
        if (mask != null && mask.raycastTarget)
        {
            // 这里可以添加A被点击时的逻辑
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (var penetrateObj in penetrateObjs)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(penetrateObj, eventData.position, eventData.pressEventCamera))
            {
                ExecuteEvents.Execute(penetrateObj.gameObject, eventData, ExecuteEvents.pointerDownHandler);
            }
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (var penetrateObj in penetrateObjs)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(penetrateObj, eventData.position, eventData.pressEventCamera))
            {
                ExecuteEvents.Execute(penetrateObj.gameObject, eventData, ExecuteEvents.pointerUpHandler);
            }
        }
    }

}
