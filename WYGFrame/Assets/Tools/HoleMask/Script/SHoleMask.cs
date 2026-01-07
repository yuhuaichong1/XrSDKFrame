using System;
using UnityEngine;
using UnityEngine.UI;

public class SHoleMask : MonoBehaviour, ICanvasRaycastFilter
{
    public bool ifMask;
    public Transform hole;
    public CanvasGroup holeMask;
    public Sprite SquareHole;
    public Sprite CircularHole;
    public Camera ObjCanvas;
    [Space]
    [Header("Scene Example")]
    public SHoleMaskEnum sGuideEnum;
    public Transform hitPoint;
    public Vector2 correction;
    public float CHitDistance;
    public Vector2 SHitDistance;
    [Space]
    public Color lineColor = Color.white;

    private Vector2 targetPos;

    void Start()
    {
        SetTarget();
        SetMask();
    }

    public void SetSTarget(Transform target, Vector2 SHitDistance, Vector2 correction)
    {
        sGuideEnum = SHoleMaskEnum.Square;
        this.SHitDistance = SHitDistance;

        SetTarget(target, correction);
    }

    public void SetCTarget(Transform target, float CHitDistance, Vector2 correction)
    {
        sGuideEnum = SHoleMaskEnum.Circular;
        this.CHitDistance = CHitDistance;

        SetTarget(target, correction);
    }

    private void SetTarget(Transform target, Vector2 correction)
    {
        hitPoint = target;
        this.correction = correction;
        SetTarget();
        SetMask();
    }

    public void SetTarget()
    {
        if (hitPoint == null)
        {
            Debug.LogError("targetPos not found!");
            return;
        }
            
        bool b = hitPoint.GetComponent<RectTransform>() == null;
        targetPos = b ? (Vector2)ObjCanvas.WorldToScreenPoint(hitPoint.position) : hitPoint.position;
        targetPos += correction;
    }

    public void SetMask()
    {
        hole.gameObject.SetActive(ifMask);
        holeMask.alpha = ifMask ? 1 : 0;
        if (ifMask)
        {
            hole.transform.position = targetPos;
            RectTransform rectTrans = hole.GetComponent<RectTransform>();
            Image image = hole.GetComponent<Image>();
            switch (sGuideEnum)
            {
                case SHoleMaskEnum.Square:
                    image.sprite = SquareHole;
                    rectTrans.sizeDelta = SHitDistance;
                    break;
                case SHoleMaskEnum.Circular:
                    image.sprite = CircularHole;
                    rectTrans.sizeDelta = new Vector2(CHitDistance, CHitDistance) * 2;
                    break;
                default:
                    break;
            }
        }
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return true;
        if(hitPoint == null)
            return true;

        bool b = hitPoint.GetComponent<RectTransform>() == null;
        if (b && ObjCanvas == null)
            return true;

        switch (sGuideEnum)
        {
            case SHoleMaskEnum.Circular:
                float dis = Vector2.Distance(targetPos, sp);
                return dis > CHitDistance;
            case SHoleMaskEnum.Square:
                float x = Mathf.Abs(sp.x - targetPos.x);
                float y = Mathf.Abs(sp.y - targetPos.y);
                return (x > SHitDistance.x / 2 || y > SHitDistance.y / 2);
            default:
                return true;
        }
    }

    void OnDrawGizmos()
    {
        if (hitPoint == null)
            return;

        bool b = hitPoint.GetComponent<RectTransform>() == null;
        if(b && ObjCanvas == null)
            return;

        Vector2 targetPos = b ? (Vector2)ObjCanvas.WorldToScreenPoint(hitPoint.position) : hitPoint.position;
        targetPos += correction;

        Gizmos.color = lineColor;
        switch(sGuideEnum)
        {
            case SHoleMaskEnum.Circular:
                Gizmos.DrawWireSphere(targetPos, CHitDistance);
                break;
            case SHoleMaskEnum.Square:
                Gizmos.DrawWireCube(targetPos, new Vector3(SHitDistance.x, SHitDistance.y, 0));
                break;
            default:
                break;
        }
    }
}
