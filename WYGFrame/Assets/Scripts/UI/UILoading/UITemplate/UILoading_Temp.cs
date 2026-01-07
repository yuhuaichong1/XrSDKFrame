using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XrCode
{
    public partial class UILoading : BaseUI
    {	protected Image mGameTitle;	protected Text mLoadingSche;	protected Slider mLoadingSlider;	protected LanguageText mLoadingText;
        protected override void LoadPanel()
        {
            base.LoadPanel();
            		mGameTitle = mTransform.Find("Top/GameTitle").GetComponent<Image>();		mLoadingSche = mTransform.Find("Bottom/LoadingSche").GetComponent<Text>();		mLoadingSlider = mTransform.Find("Bottom/LoadingSlider").GetComponent<Slider>();		mLoadingText = mTransform.Find("Bottom/LoadingText").GetComponent<LanguageText>();
        }
    
        protected override void BindButtonEvent() 
        {
            
        }
    
        protected override void UnBindButtonEvent() 
        {
            
        }
    
    }
}