using Bright.Serialization;
using cfg;
using cfg.item;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using XrCode;

//多语言工具类
public class LanguageUtils
{
    private static TbLanguage languageTab;
    public static TbLanguage LanguageTab
    {
        get
        {
            if (languageTab == null) languageTab = LoadConf();
            return languageTab;
        }
    }

    private static TbLanguage LoadConf()
    {
        languageTab = new TbLanguage(LoadByteBuf("tblanguage"));
        return languageTab;
    }
    private static ByteBuf LoadByteBuf(string file)
    {
        byte[] bytes;
        bytes = File.ReadAllBytes($"{Application.streamingAssetsPath}/Data/tblanguage.bytes");
        return new ByteBuf(bytes);
    }

    //private static SystemLanguage languageType = SystemLanguage.Unknown;
    private static ELanguageType eLanguageType = ELanguageType.None;
    public static ELanguageType GetLanguage()
    {
        //if (languageType == SystemLanguage.Unknown)
        //{
        //    LoadCache();
        //}
        //return languageType;

        if (eLanguageType == ELanguageType.None)
        {
            LoadCache();
        }
        return eLanguageType;
    }

    public static void SetLanguage(ELanguageType value)
    {
        //languageType = value;
        eLanguageType = value;
    }
    public static void SaveCache()
    {
        //PlayerPrefs.SetInt("CurrentLanguage", (int)languageType);
        PlayerPrefs.SetInt("CurELanguageType", (int)eLanguageType);
    }
    public static void LoadCache()
    {
        //var cur_language = (SystemLanguage)PlayerPrefs.GetInt("CurrentLanguage", (int)SystemLanguage.Unknown);
        //SetLanguage(cur_language);
        var cur_language = (ELanguageType)PlayerPrefs.GetInt("CurELanguageType", (int)ELanguageType.None);
        SetLanguage(cur_language);
    }
    public static void ClearCache()
    {
        //PlayerPrefs.DeleteKey("CurrentLanguage");
        PlayerPrefs.DeleteKey("CurELanguageType");
    }
    public static bool IsArabic()
    {
        //return languageType == SystemLanguage.Arabic;
        return eLanguageType == ELanguageType.Arabic;
    }


    public static void ReloadConfig()
    {
        languageTab = null;
        LoadConf();
    }

    public static string GetLanguage(string id)
    {
        ConfLanguage conf = LanguageTab.GetOrDefault(id);
        if (conf != null)
        {
            //switch (GetLanguage())
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

            switch (GetLanguage())
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
        return $"配置错误：多语言表中id: {id} 不存在";
    }

    //自动清理
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoClearCache()
    {
        languageTab = null;
    }
}