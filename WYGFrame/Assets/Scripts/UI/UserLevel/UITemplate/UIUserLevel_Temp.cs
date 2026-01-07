using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UIUserLevel : BaseUI
    {	protected RectTransform mPlane;	protected Button mExitBtn;	protected Text mCurLevelText;	protected Button mKeepPlayingBtn;	protected Slider mLevelProgress;	protected Text mLevelProgressText;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mPlane = mTransform.Find("Plane").GetComponent<RectTransform>();		mExitBtn = mTransform.Find("Plane/ExitBtn").GetComponent<Button>();		mCurLevelText = mTransform.Find("Plane/CurLevelText").GetComponent<Text>();		mKeepPlayingBtn = mTransform.Find("Plane/KeepPlayingBtn").GetComponent<Button>();		mLevelProgress = mTransform.Find("Plane/LevelProgress").GetComponent<Slider>();		mLevelProgressText = mTransform.Find("Plane/LevelProgress/LevelProgressText").GetComponent<Text>();
        }
    
        protected override void BindButtonEvent() 
        {
            		mExitBtn.onClick.AddListener( OnExitBtnClickHandle);		mKeepPlayingBtn.onClick.AddListener( OnKeepPlayingBtnClickHandle);
        }
    
        protected override void UnBindButtonEvent() 
        {
            		mExitBtn.onClick.RemoveAllListeners();		mKeepPlayingBtn.onClick.RemoveAllListeners();
        }
    
    }
}