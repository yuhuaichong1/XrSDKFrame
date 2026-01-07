using System;
using System.Collections.Generic;
using UnityEngine;
using XrCode;

public class RedDotModule : BaseModule
{
    public class RDNode//数据类：红点节点
    {
        public int num;//当前节点下的红点数量
        public RDNode Father;//父对象红点
        public Action<OverKind, int> changeAction;//改变数量时的回调方法

        public int maxValue;//可接受最大值
        public int minValue;//可接受最小值
        public bool autoCorrectMax;//当值过大时自动修正
        public bool autoCorrectMin;//当值过小时自动修正
    }

    private Dictionary<string, RDNode> RDNodeDic;//名称与点的词典，查找用

    /// <summary>
    /// Module初始化
    /// </summary>
    protected override void OnLoad()
    {
        RDNodeDic = new Dictionary<string, RDNode>();

        //CreateRDNode("OUT", "");
        //CreateRDNode("LT", "OUT");
        //CreateRDNode("ET", "OUT");
    }

    //====================================================================================================================================//

    /// <summary>
    /// 增加某红点的值
    /// </summary>
    /// <param name="rdNode">目标红点</param>
    /// <param name="value">增加的值</param>
    public void AddRDNode(RDNode rdNode, int value = 1)
    {
        if (!RDNodeDic.ContainsValue(rdNode))
        {
            Debug.LogError($"The rdNode is not exist.");
            return;
        }

        RDNode currentNode = rdNode;
        while (true)
        {
            OverKind overkind;
            if (currentNode.minValue == currentNode.maxValue)
            {
                bool b = currentNode.num != 1;
                currentNode.num = 1;
                currentNode.changeAction?.Invoke(OverKind.Overflow, 1);

                if (b)
                {
                    if (currentNode.Father == null)
                        return;
                    else
                        currentNode = currentNode.Father;
                }
                else
                {
                    return;
                }
            }
            else
            {
                currentNode.num += value;

                overkind = CheckRDNodeStatus(currentNode);
                if (overkind == OverKind.Overflow && currentNode.autoCorrectMax)
                    currentNode.num = currentNode.maxValue;

                currentNode.changeAction?.Invoke(overkind, currentNode.num);
                if (currentNode.Father == null)
                    return;
                else
                    currentNode = currentNode.Father;
            }
        }
    }

    /// <summary>
    /// 增加某红点的值
    /// </summary>
    /// <param name="name">目标红点名称</param>
    /// <param name="value">增加的值</param>
    public void AddRDNode(string name, int value = 1)
    {
        if (!RDNodeDic.ContainsKey(name))
        {
            Debug.LogError($"RedDot Node '{name}' is not exist.");
            return;
        }

        AddRDNode(RDNodeDic[name], value);
    }

    //====================================================================================================================================//

    /// <summary>
    /// 减少某红点的值
    /// </summary>
    /// <param name="rdNode">目标红点</param>
    /// <param name="value">减少的值</param>
    public void ReduceRDNode(RDNode rdNode, int value = 1)
    {
        if (!RDNodeDic.ContainsValue(rdNode))
        {
            Debug.LogError($"The rdNode is not exist.");
            return;
        }

        RDNode currentNode = rdNode;
        while (true)
        {
            OverKind overkind;
            if (currentNode.minValue == currentNode.maxValue)
            {
                bool b = currentNode.num != 0;
                currentNode.num = 0;
                currentNode.changeAction?.Invoke(OverKind.Underflow, 0);

                if (b)
                {
                    if (currentNode.Father == null)
                        return;
                    else
                        currentNode = currentNode.Father;
                }
                else
                {
                    return;
                }
            }
            else
            {
                currentNode.num -= value;

                overkind = CheckRDNodeStatus(currentNode);
                if (overkind == OverKind.Underflow && currentNode.autoCorrectMin)
                    currentNode.num = currentNode.minValue;

                currentNode.changeAction?.Invoke(overkind, currentNode.num);

                if (currentNode.Father == null)
                    break;
                else
                    currentNode = currentNode.Father;
            }
        }
    }

    /// <summary>
    /// 减少某红点的值
    /// </summary>
    /// <param name="name">目标红点名称</param>
    /// <param name="value">减少的值</param>
    public void ReduceRDNode(string name, int value = 1)
    {
        if (!RDNodeDic.ContainsKey(name))
        {
            Debug.LogError($"RedDot Node '{name}' is not exist.");
            return;
        }

        ReduceRDNode(RDNodeDic[name], value);
    }

    //====================================================================================================================================//

    /// <summary>
    /// 改变某红点的回调
    /// </summary>
    /// <param name="rdNode">目标红点</param>
    /// <param name="action">方法</param>
    /// <param name="kind">方法的操作</param>
    public void SetRDNodeAction(RDNode rdNode, Action<OverKind, int> action, SetRDNodeKind kind = SetRDNodeKind.Replace)
    {
        if (!RDNodeDic.ContainsValue(rdNode))
        {
            Debug.LogError($"The rdNode is not exist.");
            return;
        }
        switch (kind)
        {
            case SetRDNodeKind.Add:
                rdNode.changeAction += action;
                break;
            case SetRDNodeKind.Remove:
                rdNode.changeAction -= action;
                break;
            case SetRDNodeKind.Clear:
                rdNode.changeAction = null;
                break;
            case SetRDNodeKind.Replace:
                rdNode.changeAction = action;
                break;
        }
    }

    /// <summary>
    /// 改变某红点的回调
    /// </summary>
    /// <param name="name">目标红点名称</param>
    /// <param name="action">方法</param>
    /// <param name="kind">方法的操作</param>
    public void SetRDNodeAction(string name, Action<OverKind, int> action, SetRDNodeKind kind = SetRDNodeKind.Replace)
    {
        if (!RDNodeDic.ContainsKey(name))
        {
            Debug.LogError($"RedDot Node '{name}' is not exist.");
            return;
        }
        SetRDNodeAction(RDNodeDic[name], action, kind);
    }

    //====================================================================================================================================//
   
    /// <summary>
    /// 改变某红点的数量
    /// </summary>
    /// <param name="rdNode">目标红点</param>
    /// <param name="num">新数目</param>
    /// <param name="kind">新数目的操作</param>
    public void SetRDNodeNum(RDNode rdNode, int num, SetRDNodeKind kind = SetRDNodeKind.Replace)
    {
        if (!RDNodeDic.ContainsValue(rdNode))
        {
            Debug.LogError($"The rdNode is not exist.");
            return;
        }

        int oldValue = rdNode.num;
        int newValue = 0;

        switch (kind)
        {
            case SetRDNodeKind.Add:
                //rdNode.num += num;
                newValue = rdNode.num + num;
                break;
            case SetRDNodeKind.Remove:
                //rdNode.num -= num;
                newValue = rdNode.num - num;
                break;
            case SetRDNodeKind.Clear:
                //rdNode.num = 0;
                newValue = 0;
                break;
            case SetRDNodeKind.Replace:
                //rdNode.num = num;
                newValue = num;
                break;
        }

        int transformation = newValue - oldValue;
        if(transformation > 0)
            AddRDNode(rdNode, transformation);
        else if(transformation < 0)
            ReduceRDNode(rdNode, -transformation);
    }

    /// <summary>
    /// 改变某红点的数量
    /// </summary>
    /// <param name="name">目标红点名称</param>
    /// <param name="num">新数目</param>
    /// <param name="kind">新数目的操作</param>
    public void SetRDNodeNum(string name, int num, SetRDNodeKind kind = SetRDNodeKind.Replace)
    {
        if(!RDNodeDic.ContainsKey(name))
        {
            Debug.LogError($"RedDot Node '{name}' is not exist.");
            return;
        }
        SetRDNodeNum(RDNodeDic[name], num, kind);
    }

    //====================================================================================================================================//

    /// <summary>
    /// 获取某红点
    /// </summary>
    /// <param name="name">红点名称</param>
    /// <returns>目标红点</returns>
    public RDNode GetRDNode(string name)
    {
        if (RDNodeDic.ContainsKey(name))
            return RDNodeDic[name];
        else
        {
            Debug.LogError($"RedDot Node '{name}' is not exist.");
            return null;
        }  
    }

    //====================================================================================================================================//

    /// <summary>
    /// 判断目标红点状态
    /// </summary>
    /// <param name="rdNode">目标红点</param>
    /// <returns>目标红点状态</returns>
    public OverKind CheckRDNodeStatus(RDNode rdNode)
    {
        int num = rdNode.num;
        if(num < rdNode.minValue) 
        { 
            return OverKind.Underflow;
        }
        else if(num > rdNode.maxValue) 
        {
            return OverKind.Overflow;
        }
        else
        {
            return OverKind.InRange;
        }
    }

    /// <summary>
    /// 判断目标红点状态
    /// </summary>
    /// <param name="name">目标红点名称</param>
    /// <returns></returns>
    public OverKind CheckRDNodeStatus(string name)
    {
        return CheckRDNodeStatus(RDNodeDic[name]);
    }

    //====================================================================================================================================//

    /// <summary>
    /// 新建一个红点（当maxValue == minValue时，会自动转为bool类型的红点【调用AddRDNode时默认溢出，num为1，调用ReduceRDNode时默认下溢，num为0】）
    /// </summary>
    /// <param name="name">红点名称</param>
    /// <param name="fatherName">父红点名称</param>
    /// <param name="action">改变值时的回调</param>
    /// <param name="num">初始数量</param>
    /// <param name="maxValue">红点检测最大值</param>
    /// <param name="autoCorrectMax">超过最大值后是否自动调整</param>
    /// <param name="minValue">红点检测最小值</param>
    /// <param name="autoCorrectMin">超过最小值后是否自动调整</param>
    /// <returns>新建的红点</returns>
    public RDNode CreateRDNode(string name, string fatherName, Action<OverKind, int> action = null, int num = 0, int maxValue = 99, bool autoCorrectMax = false, int minValue = 0,  bool autoCorrectMin = true) 
    {
        if(!RDNodeDic.ContainsKey(name))
        {
            RDNode newRDNode = new RDNode();
            newRDNode.num = num;
            if (fatherName == "" || fatherName == null)
                newRDNode.Father = null;
            else
                newRDNode.Father = RDNodeDic[fatherName];
            newRDNode.changeAction = action;
            newRDNode.maxValue = maxValue;
            newRDNode.autoCorrectMax = autoCorrectMax;
            newRDNode.minValue = minValue;
            newRDNode.autoCorrectMin = autoCorrectMin;
            RDNodeDic.Add(name, newRDNode);

            return newRDNode;
        }
        else
        {
            Debug.LogError($"RedDot Node '{name}' has been exist.");
            return RDNodeDic[name];
        }
    }
    
    /// <summary>
    /// 新建（添加）一个红点
    /// </summary>
    /// <param name="name">红点名称</param>
    /// <param name="newRDNode">要添加的红点</param>
    public void CreateRDNode(string name, RDNode newRDNode)
    {
        if (!RDNodeDic.ContainsKey(name))
            Debug.LogError($"RedDot Node '{name}' has been exist.");
        else
            RDNodeDic.Add(name, newRDNode);
    }

    //====================================================================================================================================//

    /// <summary>
    /// 删除某个红点
    /// </summary>
    /// <param name="name">红点名称</param>
    public void DeleteRDNode(string name)
    {
        if (RDNodeDic.ContainsKey(name))
            RDNodeDic.Remove(name);
        else
            Debug.LogError($"RedDot Node '{name}' is not exist.");
    }

    /// <summary>
    /// 删除某个红点2
    /// </summary>
    /// <param name="newRDNode">目标红点</param>
    public void DeleteRDNode(RDNode newRDNode)
    {
        if (RDNodeDic.ContainsValue(newRDNode))
        {
            List<string> keysToRemove = new List<string>();
            foreach (string key in  RDNodeDic.Keys)
            {
                if (RDNodeDic[key] == newRDNode) 
                    keysToRemove.Add(key);
            }
            foreach (string key in keysToRemove) 
            {
                RDNodeDic.Remove(key);
            }
        }
        else
        {
            Debug.LogError($"RedDot Node is not exist.");
        }
    }

    //====================================================================================================================================//

    /// <summary>
    /// 刷新所有的红点
    /// </summary>
    public void RefushRDNode()
    {
        foreach(KeyValuePair<string, RDNode> item in RDNodeDic)
        {
            int num = item.Value.num;
            OverKind kind = CheckRDNodeStatus(item.Value);
            item.Value.changeAction?.Invoke(kind, num);
        }
    }

    /// <summary>
    /// 刷新指定红点
    /// </summary>
    /// <param name="rdNode">指定红点</param>
    /// <param name="ifRefushF">是否影响父对象</param>
    public void RefushRDNode(RDNode rdNode, bool ifRefushF = true)
    {
        RDNode curRDNode = rdNode;
        while (true)
        {
            int num = rdNode.num;
            OverKind kind = CheckRDNodeStatus(rdNode);
            rdNode.changeAction?.Invoke(kind, num);

            if (ifRefushF && rdNode.Father != null)
                curRDNode = rdNode.Father;
            else
                break;
        }
    }

    /// <summary>
    /// 刷新指定红点
    /// </summary>
    /// <param name="name">指定红点名称</param>
    /// <param name="ifRefushF">是否影响父对象</param>
    public void RefushRDNode(string name, bool ifRefushF = true)
    {
        RefushRDNode(RDNodeDic[name], ifRefushF);
    }
}

//操作枚举
public enum SetRDNodeKind
{
    Add,//添加 | 增加
    Remove,//移除 | 减少
    Clear,//清除 | 归零
    Replace//替换 | 替换
}
//红点数目判别种类
public enum OverKind
{
    Overflow,//过大
    InRange,//正常区间
    Underflow//过小
}
