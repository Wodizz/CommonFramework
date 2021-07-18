using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 抽屉+节点打包类
/// </summary>
public class PoolData
{
    // 一种抽屉对应的父节点
    public GameObject fatherObj;
    // 抽屉
    public List<GameObject> poolList;

    public PoolData(GameObject obj, GameObject poolObj)
    {
        // 创建父节点与要push的对象名字一致
        fatherObj = new GameObject(obj.name);
        // 设置父节点的爹为缓存池根节点
        fatherObj.transform.parent = poolObj.transform;
        // new一个List来存对象
        poolList = new List<GameObject>();
        // 初始化完了调用塞东西方法
        PushObj(obj);
    }

    /// <summary>
    /// 往抽屉里塞东西方法
    /// </summary>
    /// <param name="obj">游戏对象</param>
    public void PushObj(GameObject obj)
    {
        poolList.Add(obj);
        // 设置父对象
        obj.transform.parent = fatherObj.transform;
        // 放进来就把这个东西失活
        obj.SetActive(false);
    }

    /// <summary>
    /// 抽屉里拿东西方法
    /// </summary>
    /// <returns>GameObject对象</returns>
    public GameObject GetObj()
    {
        GameObject obj = null;
        obj = poolList[0];
        poolList.RemoveAt(0);
        // 断开一切父子关系
        obj.transform.parent = null;
        // 取出池子就把这个东西激活
        obj.SetActive(true);
        return obj;
    }
}
