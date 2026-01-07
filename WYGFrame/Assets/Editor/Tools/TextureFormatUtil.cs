using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
public class TextureFormatUtil
{
#if UNITY_ANDROID
    private static string targetPlatform = "Android";
#elif UNITY_IOS
    private static string targetPlatform = "iPhone";
#elif UNITY_WEBGL
    private static string targetPlatform = "WebGL";
#else
    private static string targetPlatform = "Standalone";
#endif 


    [MenuItem("Assets/CustomArea/TextureFormat_Normal")]
    private static void TextureFormat_Normal()
    {
        TextureFormat(true);
    }

    [MenuItem("Assets/CustomArea/TextureFormat_Compress")]
    private static void TextureFormat_Compress()
    {
        TextureFormat(false);
    }

    [MenuItem("Assets/CustomArea/TextureFormat_Cancel")]
    private static void TextureFormat_Cancel()
    {
        CancelTextureFormat(true);
    }

    [MenuItem("Assets/CustomArea/GetTextureFormat")]
    private static void GetTextureFormat()
    {
        Object curSelect = Selection.activeObject;
        if (curSelect == null)
        {
            Debug.LogError("[TextureFormat]: 请在工程视图选择需要检查的文件夹或文件");
            return;
        }

        Debug.Log(curSelect.ToString());
        StringBuilder sb = new StringBuilder();
        sb.Append("[TextureFormat]:");

        StringBuilder sb1 = new StringBuilder();
        sb1.Append("[TextureFormat]:");

        Object[] objs = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].GetType() == typeof(Texture2D))
            {
                TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(objs[i]));
                int height = ((Texture2D)objs[i]).height;
                int width = ((Texture2D)objs[i]).width;

                int oH = height % 4;
                int oW = width % 4;

                if (oH == 0 && oW == 0) continue;

                height = (oH > 2) ? height + 4 - oH : height - oH;
                width = (oW > 2) ? width + 4 - oW : width - oW;

                sb.Append("\n");
                sb.Append($" {objs[i].name} ： 宽：{width}     高：{height}");
                sb1.Append("\n");
                sb1.Append($" {AssetDatabase.GetAssetPath(objs[i])}");
            }
        }
        Debug.Log(sb1.ToString());
        Debug.Log(sb.ToString());
    }

    private static void TextureFormat(bool isNormal)
    {
        Object curSelect = Selection.activeObject;
        if (curSelect == null)
        {
            Debug.LogError("[TextureFormat]: 请在工程视图选择需要检查的文件夹或文件");
            return;
        }

        Debug.Log(curSelect.ToString());

        Object[] objs = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].GetType() == typeof(Texture2D))
            {
                TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(objs[i]));
                int horw = Mathf.Max(((Texture2D)objs[i]).height, ((Texture2D)objs[i]).width);
                //获取最大尺寸
                int maxSize = 0;
                if (isNormal) maxSize = SetMaxSize(horw);
                else maxSize = SetMaxSizeOffset(horw);

                //importer.maxTextureSize = 128;
                TextureImporterPlatformSettings settings = importer.GetPlatformTextureSettings(targetPlatform);
                settings.overridden = true;
                settings.maxTextureSize = maxSize;
                //settings.format = TextureImporterFormat.DXT5;
                //settings.format = TextureImporterFormat.ETC2_RGBA8;
                //settings.format = TextureImporterFormat.ETC2_RGB4_PUNCHTHROUGH_ALPHA;
                settings.format = TextureImporterFormat.ASTC_8x8;
                importer.mipmapEnabled = false;
                importer.SetPlatformTextureSettings(settings);
                //EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.LogError("图片尺寸转换完了");
    }

    private static void CancelTextureFormat(bool isNormal)
    {
        Object curSelect = Selection.activeObject;
        if (curSelect == null)
        {
            Debug.LogError("[TextureFormat]: 请在工程视图选择需要检查的文件夹或文件");
            return;
        }

        Debug.Log(curSelect.ToString());

        Object[] objs = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].GetType() == typeof(Texture2D))
            {
                TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(objs[i]));
                int horw = Mathf.Max(((Texture2D)objs[i]).height, ((Texture2D)objs[i]).width);
                //importer.maxTextureSize = 2048;
                importer.mipmapEnabled = false;

                TextureImporterPlatformSettings settings = importer.GetPlatformTextureSettings(targetPlatform);
                settings.maxTextureSize = 2048;
                settings.overridden = true;
                settings.overridden = false;
                importer.mipmapEnabled = false;
                importer.SetPlatformTextureSettings(settings);
                //EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.LogError("图片尺寸转换完了");
    }

    private static int SetMaxSize(int value)
    {
        for(int i = 5; i < 12;i++)
        {
            if(value <= Mathf.Pow(2,i))
            {
                 return (int)Mathf.Pow(2, i);
            }
        }
        return 2048;
    }

    private static int SetMaxSizeOffset(int value)
    {
        for (int i = 5; i < 12; i++)
        {
            if (value <= Mathf.Pow(2, i))
            {
                if (i >= 10)
                    return (int)Mathf.Pow(2, i - 1);
                else
                    return (int)Mathf.Pow(2, i);
            }
        }
        return 1024;
    }
}
