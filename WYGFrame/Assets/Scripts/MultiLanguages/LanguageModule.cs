using cfg;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using XrCode;

public class LanguageModule : BaseModule
{
    //private SystemLanguage currLanguageType;
    private ELanguageType curELanguageType;

    protected override void OnLoad()
    {
        //curELanguageType = ELanguageType.English;
        LoadCache();
        FacadeLanguage.GetText += GetText;
    }

    // 获取指定文本
    public string GetText(string id)
    {
        ConfLanguage conf = ConfigModule.Instance.Tables.TbLanguage.GetOrDefault(id);
        if (conf != null)
        {
            //switch (curELanguageType)
            //{
            //    case SystemLanguage.ChineseSimplified:
            //    case SystemLanguage.Chinese:
            //        return conf.ChineseS;
            //    case SystemLanguage.German:
            //        return conf.German;
            //    case SystemLanguage.English:
            //        return conf.English;
            //    case SystemLanguage.Japanese:
            //        return conf.Japanese;
            //    case SystemLanguage.Bulgarian:
            //        return conf.BrazilianPortuguese;
            //    default:
            //        return conf.English;
            //}

            switch (curELanguageType)
            {
                case ELanguageType.Chinese_s:
                case ELanguageType.Chinese_t:
                    return conf.ChineseS;
                case ELanguageType.English:
                    return conf.English;
                case ELanguageType.German:
                    return conf.German;
                case ELanguageType.Japanese:
                    return conf.Japanese;
                case ELanguageType.Brazilian_Portuguese:
                    return conf.BrazilianPortuguese;
                case ELanguageType.French:
                    return conf.French;
                case ELanguageType.Spanish:
                    return conf.Spanish;
                case ELanguageType.Korean:
                    return conf.Korean;
                case ELanguageType.Indonesian:
                    return conf.Indonesian;
                case ELanguageType.Russian:
                    return conf.Russian;
                case ELanguageType.Hindi:
                    return conf.Hindi;
                case ELanguageType.Thai:
                    return conf.Thai;
                case ELanguageType.Turkish:
                    return conf.Turkish;
                case ELanguageType.LengthTest:
                    return conf.LengthTest;
                default:
                    return conf.English;
            }
        }
        return "";
    }

    public ELanguageType GetLanguage()
    {
        //return currLanguageType;
        return curELanguageType;
    }

    public void SetLanguage(ELanguageType value)
    {
        curELanguageType = value;
        SaveCache();
        FacadeLanguage.OnLanguageChange?.Invoke((int)curELanguageType);
    }
    public void SaveCache()
    {
        //PlayerPrefs.SetInt("CurrentLanguage", (int)currLanguageType);
        PlayerPrefs.SetInt("CurELanguageType", (int)curELanguageType);
    }
    public void LoadCache()
    {
        //currLanguageType = (SystemLanguage)PlayerPrefs.GetInt("CurrentLanguage", (int)SystemLanguage.English);
        curELanguageType = (ELanguageType)PlayerPrefs.GetInt("CurELanguageType", (int)ELanguageType.English);
    }
    public bool IsArabic()
    {
        //return currLanguageType == SystemLanguage.Arabic;
        return curELanguageType == ELanguageType.Arabic;
    }

    protected override void OnDispose()
    {
        curELanguageType = ELanguageType.None;
        FacadeLanguage.GetText -= GetText;
    }

}
