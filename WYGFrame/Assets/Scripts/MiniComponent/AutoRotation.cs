using DG.Tweening;
using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    public bool playOnAwake = true;
    public bool clockwise = true;
    public float oneRoundTime = 1;

    void Awake()
    {
        if (playOnAwake)
            StartRotate();
    }

    public void StartRotate()
    {
        this.transform.DOLocalRotate(Vector3.forward * 360 * (clockwise ? -1 : 1), oneRoundTime, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }

    public void StopRotate()
    {
        this.transform.DOKill();
    }
}
