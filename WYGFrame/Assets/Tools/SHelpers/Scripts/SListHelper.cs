using System.Collections.Generic;
using System;

public static class SListHelper
{
    /// <summary>
    /// 洗牌算法
    /// </summary>
    /// <typeparam name="T">List的类型</typeparam>
    /// <param name="list">混淆完毕的List</param>
    public static void Shuffle<T>(this List<T> list)
    {
        int n = list.Count;
        // 在方法内部创建 Random 实例
        System.Random rng = new System.Random();
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
