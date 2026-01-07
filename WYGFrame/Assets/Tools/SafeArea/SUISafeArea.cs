using UnityEngine;

//当使用Device Simulator Devices时，该组件会出现问题，
//但是实际打包是不受影响的，
//或者，将OnEnable改为Start也能解决问题，但是仅能在游戏运行时查看。
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
[ExecuteAlways]
public class SUISafeArea : MonoBehaviour
{
    public float leftCorrection;//左边距补正
    public float rightCorrection;//右边距补正
    public float topCorrection;//上边距补正
    public float bottomCorrection;//下边距补正

    private DrivenRectTransformTracker m_Tracker;//限定RectTransform部分无法修改

    void Start()
    {
        RectTransform rectTrans = GetComponent<RectTransform>();
        Rect safeArea = Screen.safeArea;

        // 计算 SafeArea 的边界（相对于屏幕左下角）
        float safeXMin = safeArea.x;
        float safeYMin = safeArea.y;
        float safeXMax = safeArea.x + safeArea.width;
        float safeYMax = safeArea.y + safeArea.height;

        // 设置锚点范围（覆盖 SafeArea）
        rectTrans.anchorMin = new Vector2(0, 0);
        rectTrans.anchorMax = new Vector2(1, 1);

        // 使用 offsetMin（左下）和 offsetMax（右上）调整 SafeArea 边界
        rectTrans.offsetMin = new Vector2(
            safeXMin + leftCorrection,      // 左边界
            safeYMin + bottomCorrection     // 下边界
        );
        rectTrans.offsetMax = new Vector2(
            -(Screen.width - safeXMax) + rightCorrection,  // 右边界
            -(Screen.height - safeYMax) + topCorrection    // 上边界
        );

        m_Tracker = new DrivenRectTransformTracker();
        m_Tracker.Clear();
        m_Tracker.Add(this, rectTrans,
            DrivenTransformProperties.AnchorMinX |
            DrivenTransformProperties.AnchorMinY |
            DrivenTransformProperties.AnchorMaxX |
            DrivenTransformProperties.AnchorMaxY);


        #region 旧的，以左下角为锚点的判定
        //RectTransform rectTrans = GetComponent<RectTransform>();

        //Vector2 orginAnchorMin = rectTrans.anchorMin;
        //Vector2 orginAnchorMax = rectTrans.anchorMax;
        //Vector2 orginPivot = rectTrans.pivot;

        //rectTrans.anchorMin = Vector2.zero;
        //rectTrans.anchorMax = Vector2.zero;
        //rectTrans.pivot = Vector2.zero;

        //Rect safeArea = Screen.safeArea;
        //rectTrans.anchoredPosition = new Vector2(safeArea.x + leftCorrection, safeArea.y + bottomCorrection);
        //rectTrans.sizeDelta = new Vector2(safeArea.width - rightCorrection - leftCorrection, safeArea.height - topCorrection - bottomCorrection);

        //rectTrans.anchorMin = orginAnchorMin;
        //rectTrans.anchorMax = orginAnchorMax;
        //rectTrans.pivot = orginPivot;
        #endregion
    }

    private void OnDisable()
    {
        m_Tracker.Clear();
    }
}

/* 旧的判定

*/
