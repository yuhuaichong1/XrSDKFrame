using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRoundMove : MonoBehaviour
{
    public float roundTime = 1;//往返一次的时间
    public float roundDis = 100;//往返一次的距离
    public MoveDir roundDir = MoveDir.Horizontal;//往返移动方向
    public bool ifAwake = true;//是否在一开始就启用

    private Vector3 orginPos;
    private RectTransform rect;
    private float targetPos;
    void Awake()
    {
        orginPos = transform.position;
        rect = this.GetComponent<RectTransform>();

        if (ifAwake)
        {
            StartMove();
        }
    }

    public void StartMove()
    {
        this.transform.position = orginPos;
        Sequence sequence = DOTween.Sequence();

        if (roundDir == MoveDir.Horizontal)
        {
            float startX = rect.localPosition.x;
            float goalX = startX + roundDis;
            sequence.Append(this.transform.DOLocalMoveX(goalX, roundTime / 2).SetEase(Ease.Linear));
            sequence.Append(this.transform.DOLocalMoveX(startX, roundTime / 2).SetEase(Ease.Linear));
            sequence.SetLoops(-1);
        }
        else// == MoveDir.Vertical
        {
            float startY = rect.localPosition.y;
            float goalY = startY + roundDis;
            sequence.Append(this.transform.DOLocalMoveY(startY, roundTime / 2).SetEase(Ease.Linear));
            sequence.Append(this.transform.DOLocalMoveY(goalY, roundTime / 2).SetEase(Ease.Linear));
            sequence.SetLoops(-1);
        }
    }

    public void StopMove()
    {
        this.transform.position = orginPos;
        this.transform.DOKill();
    }

    //移动方向
    public enum MoveDir
    {
        Horizontal,
        Vertical,
    }
}
