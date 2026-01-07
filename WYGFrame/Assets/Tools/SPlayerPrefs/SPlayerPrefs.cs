using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SPlayerPrefs
{
    public const string Separator1 = ",";//分隔符1
    public const string Separator2 = "/";//分隔符2

    #region int、float、string
    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
    public static int GetInt(string key, int defaultValue = 0) 
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetInt(key);
        else
            return defaultValue;
    }

    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    public static float GetFloat(string key, float defaultValue = 0) 
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetFloat(key);
        else
            return defaultValue;
    }

    public static void SetString(string key, string value) 
    { 
        PlayerPrefs.SetString(key, value);
    }
    public static string GetString(string key, string defaultValue = "") 
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetString(key);
        else
            return defaultValue;
    }
    #endregion

    /// <summary>
    /// 存储double
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">存储值</param>
    public static void SetDouble(string key, double value)
    {
        PlayerPrefs.SetString(key, value.ToString());
    }

    /// <summary>
    /// 读取double
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="defaultValue">当键不存在时的默认值</param>
    /// <returns>double值</returns>
    public static double GetDouble(string key, double defaultValue = 0)
    {
        if(PlayerPrefs.HasKey(key))
            return double.Parse(PlayerPrefs.GetString(key));
        else 
            return defaultValue;
    }

    /// <summary>
    /// 存储Bool
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="boolean">存储值</param>
    public static void SetBool(string key, bool boolean)
    {
        PlayerPrefs.SetInt(key, boolean ? 1 : 0);
    }

    /// <summary>
    /// 读取Bool
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="defaultValue">当键不存在时的默认值</param>
    /// <returns>Bool值</returns>
    public static bool GetBool(string key, bool defaultValue = false)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetInt(key) == 1;
        else
            return defaultValue;
    }

    /// <summary>
    /// 存储DateTime
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">存储值</param>
    public static void SetDateTime(string key, DateTime value)
    {
        PlayerPrefs.SetString(key, value.ToString());
    }

    /// <summary>
    /// 读取DateTime
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>DateTime值</returns>
    public static DateTime GetDateTime(string key, DateTime defaultValue = default(DateTime))
    {
        if(PlayerPrefs.HasKey(key))
            return DateTime.Parse(PlayerPrefs.GetString(key));
        else
            return defaultValue; 
    }

    /// <summary>
    /// 存储List
    /// </summary>
    /// <typeparam name="T">List类型</typeparam>
    /// <param name="key">Key</param>
    /// <param name="list">存储值</param>
    /// <param name="sepa">分隔符</param>
    public static void SetList<T>(string key, List<T> list, string sepa = Separator1)
    {
        List<string> strings = new List<string>();
        foreach (T item in list) 
        { 
            string str = item.ToString();
            strings.Add(str);
        }
        string value = string.Join(sepa, strings.ToArray());

        PlayerPrefs.SetString(key, value);
    }

    /// <summary>
    /// 读取List
    /// </summary>
    /// <typeparam name="T">List类型</typeparam>
    /// <param name="key">Key</param>
    /// <param name="defaultValue">当键不存在时的默认值</param>
    /// <param name="sepa">分隔符</param>
    /// <returns>List值</returns>
    public static List<T> GetList<T>(string key, List<T> defaultValue = null, string sepa = Separator1)
    {
        if(!PlayerPrefs.HasKey(key))
            return defaultValue;

        try
        {
            string value = PlayerPrefs.GetString(key);
            string[] strings = value.Split(sepa);
            List<T> list = new List<T>();
            foreach (string str in strings)
            {
                list.Add((T)Convert.ChangeType(str, typeof(T)));
            }

            return list;
        }
        catch
        {
            Debug.LogWarning($"List '{key}' parsing failed");
            return defaultValue;
        }
    }

    /// <summary>
    /// 存储Stack
    /// </summary>
    /// <typeparam name="T">Stack类型</typeparam>
    /// <param name="key">Key</param>
    /// <param name="stack">存储值</param>
    /// <param name="sepa">分隔符</param>
    public static void SetStack<T>(string key, Stack<T> stack, string sepa = Separator1)
    {
        SetList(key, new List<T>(stack), sepa);
    }

    /// <summary>
    /// 读取Stack
    /// </summary>
    /// <typeparam name="T">Stack类型</typeparam>
    /// <param name="key">Key</param>
    /// <param name="defaultValue">当键不存在时的默认值</param>
    /// <param name="sepa">分隔符</param>
    /// <returns>Stack值</returns>
    public static Stack<T> GetStack<T>(string key, Stack<T> defaultValue = null, string sepa = Separator1) 
    {
        if (!PlayerPrefs.HasKey(key))
            return defaultValue;

        try
        {
            string value = PlayerPrefs.GetString(key);
            string[] strings = value.Split(sepa);
            Array.Reverse(strings);
            Stack<T> stack = new Stack<T>();
            foreach (string str in strings)
            {
                stack.Push((T)Convert.ChangeType(str, typeof(T)));
            }

            return stack;
        }
        catch
        {
            Debug.LogWarning($"Stack '{key}' parsing failed");
            return defaultValue;
        }
    }

    /// <summary>
    /// 存储Queue
    /// </summary>
    /// <typeparam name="T">Queue类型</typeparam>
    /// <param name="key">Key</param>
    /// <param name="queue">存储值</param>
    /// <param name="sepa">分隔符</param>
    public static void SetQueue<T>(string key, Queue<T> queue, string sepa = Separator1)
    {
        SetList(key, new List<T>(queue), sepa);
    }

    /// <summary>
    /// 读取Queue
    /// </summary>
    /// <typeparam name="T">Queue类型</typeparam>
    /// <param name="key">Key</param>
    /// <param name="defaultValue">当键不存在时的默认值</param>
    /// <param name="sepa">分隔符</param>
    /// <returns>Queue值</returns>
    public static Queue<T> GetQueue<T>(string key, Queue<T> defaultValue = null, string sepa = Separator1)
    {
        if (!PlayerPrefs.HasKey(key))
            return defaultValue;

        try
        {
            string value = PlayerPrefs.GetString(key);
            string[] strings = value.Split(sepa);
            Queue<T> queue = new Queue<T>();
            foreach (string str in strings)
            {
                queue.Enqueue((T)Convert.ChangeType(str, typeof(T)));
            }

            return queue;
        }
        catch
        {
            Debug.LogWarning($"Queue '{key}' parsing failed");
            return defaultValue;
        }
    }

    /// <summary>
    /// 存储Dictionary
    /// </summary>
    /// <typeparam name="T">Dictionary的Keys的类型</typeparam>
    /// <typeparam name="U">Dictionary的Values的类型</typeparam>
    /// <param name="key">Key</param>
    /// <param name="dictionary">存储值</param>
    /// <param name="sepa">分隔符1</param>
    /// <param name="sepa2">分隔符2</param>
    public static void SetDictionary<T, U>(string key, Dictionary<T, U> dictionary, string sepa = Separator1, string sepa2 = Separator2)
    {
        List<string> dicKeys = new List<string>();
        List<string> dicValues = new List<string>();
        foreach (T keyItem in dictionary.Keys)
        {
            string str = keyItem.ToString();
            dicKeys.Add(str);
        }
        foreach (U valueItem in dictionary.Values)
        {
            string str = valueItem.ToString();
            dicValues.Add(str);
        }
        string DKStr = string.Join(sepa, dicKeys.ToArray());
        string DVStr = string.Join(sepa, dicValues.ToArray());
        string value = $"{DKStr}{sepa2}{DVStr}";

        PlayerPrefs.SetString(key, value);
    }

    /// <summary>
    /// 读取Dictionary
    /// </summary>
    /// <typeparam name="T">Dictionary的Keys的类型</typeparam>
    /// <typeparam name="U">Dictionary的Values的类型</typeparam>
    /// <param name="key">Key</param>
    /// <param name="defaultValue">当键不存在时的默认值</param>
    /// <param name="sepa">分隔符1</param>
    /// <param name="sepa2">分隔符1</param>
    /// <returns>Dictionary值</returns>
    public static Dictionary<T,U> GetDictionary<T, U>(string key, Dictionary<T, U> defaultValue = null, string sepa = Separator1, string sepa2 = Separator2)
    {
        if (!PlayerPrefs.HasKey(key))
            return defaultValue;

        try
        {
            Dictionary<T, U> dic = new Dictionary<T, U>();
            string[] KeyValue = PlayerPrefs.GetString(key).Split(sepa2);
            string[] dicKeys = KeyValue[0].Split(sepa);
            string[] dicValues = KeyValue[1].Split(sepa);
            for (int i = 0; i < dicKeys.Length; i++)
            {
                T keyItem = (T)Convert.ChangeType(dicKeys[i], typeof(T));
                U valueItem = (U)Convert.ChangeType(dicValues[i], typeof(U));
                dic.Add(keyItem, valueItem);
            }

            return dic;
        }
        catch 
        {
            Debug.LogWarning($"Dictionary '{key}' parsing failed");
            return defaultValue; 
        }
    }

    #region Save、DeleteAll、Delete、HasKey
    public static void Save()
    {
        PlayerPrefs.Save();
    }
    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
    public static void Delete(string key) 
    { 
        PlayerPrefs.DeleteKey(key);
    }
    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
    #endregion
}
