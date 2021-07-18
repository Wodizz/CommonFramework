using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 缓存池模块
/// </summary>
public class poolMgr
{
    #region 单例
    private static poolMgr instance = new poolMgr();
    public static poolMgr Instance => instance;
    private poolMgr() { }
    #endregion

    // 缓存池(衣柜) 整个池子可以理解成一个大字典
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    // 作为缓存池的根节点
    public GameObject poolObj;

    /// <summary>
    /// 往外拿东西
    /// </summary>
    /// <param name="name">抽屉名字(文件地址)</param>
    /// <returns></returns>
    public GameObject GetObj(string name)
    {
        GameObject obj = null;
        // 有对应抽屉 并且抽屉里有东西
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        {
            obj = poolDic[name].GetObj();
        }
        else
        {
            obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            // 对象名字与路径名字一致
            obj.name = name;
        }
        return obj;
    }

    /// <summary>
    /// 把不用的东西还给对应的抽屉
    /// </summary>
    public void PushObj(string name, GameObject obj)
    {
        // 如果池子是空 就新建一个缓存池
        if (poolObj == null)
            poolObj = new GameObject("Pool");
        // 里面有抽屉
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].PushObj(obj);
        }
        // 没抽屉就加一个抽屉 把东西放进去
        else
        {
            poolDic.Add(name, new PoolData(obj, poolObj));
        }
    }
    
    /// <summary>
    /// 清空方法 因为过场景时 游戏物体自动移除 但池子里依旧建立着与失活对象的联系
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
