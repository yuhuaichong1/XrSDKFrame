using UnityEngine;

static public partial class GameObjectExtension
{
    /// <summary>
    /// 获得或添加组件
    /// </summary>
    /// <param name="self">GameObject</param>
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null) component = go.AddComponent<T>();
        return component;
    }

    /// <summary>
    /// 获得或添加组件
    /// </summary>
    /// <param name="self">Transform</param>
    public static T GetOrAddComponent<T>(this Transform go) where T : Component
    {
        return GetOrAddComponent<T>(go.gameObject);
    }
}
