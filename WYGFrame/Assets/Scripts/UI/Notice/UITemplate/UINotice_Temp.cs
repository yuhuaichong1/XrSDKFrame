using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UINotice : BaseUI
    {	protected RectTransform mStartPos;	protected RectTransform mEndPos;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mStartPos = mTransform.Find("startPos").GetComponent<RectTransform>();		mEndPos = mTransform.Find("endPos").GetComponent<RectTransform>();
        }
    
        protected override void BindButtonEvent() 
        {
            
        }
    
        protected override void UnBindButtonEvent() 
        {
            
        }
    
    }
}