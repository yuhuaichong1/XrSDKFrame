using System;

public static class FacadeGuide
{
    public static Action PlayGuide;
    public static Action NextStep;
    public static Action NextStepHead;
    public static Func<GuideItem> GetCurGuideItems;
    public static Func<int> GetCurStep;
    public static Action CloseGuide;

    public static Func<bool> GetIfTutorial;                             //获得是否处于新手引导
    public static Action<bool> SetIfTutorial;                           //设置是否处于新手引导
}
