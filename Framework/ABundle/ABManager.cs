﻿#define UNITY_PC
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ab包的特点 ：不能重复加载
/// </summary>
public class ABManager : SingleAutoMono<ABManager>
{
    // 主包
    private AssetBundle mainAB = null;

    // 包字典
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    // 主包配置
    private AssetBundleManifest manifest = null;

    #region 路径相关

    private string currentPath = null;

    /// <summary>
    /// 不一定会使用Application.streamingAssetsPath
    /// 比较常用的是Application.persistentDataPath
    /// 而且不同平台命名可能不一样 所以封装一个路径
    /// </summary>
    public string CurrentPath
    {
        get
        {
            if (currentPath == null)
            {
                currentPath = Application.streamingAssetsPath;
            }
            return currentPath + "/";
        }
        set
        {
            currentPath = value;
        }
    }

    private string mainABName = null;

    /// <summary>
    /// 主包名 默认根据当前平台来
    /// </summary>
    public string MainABName
    {
        get
        {
            if (mainABName == null)
            {
#if UNITY_IOS
                mainABName = "IOS";
#elif UNITY_ANDROID
                mainABName = "Android";
#elif UNITY_PC
                mainABName = "PC";
#endif
            }
            return mainABName;
        }
        set
        {
            mainABName = value;
        }
    }

    #endregion


    /// <summary>
    /// 同步加载
    /// </summary>
    public Object LoadRes(string abName, string resName)
    {
        // 加载之前 必须先获得依赖包的相关信息
        // 如果没加载过主包
        if (mainAB == null)
        {
            // 1.加载主包
            mainAB = AssetBundle.LoadFromFile(CurrentPath + MainABName);
            // 2.加载manifest
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        // 3.获取依赖包
        AssetBundle ab;
        string[] dependencies = manifest.GetAllDependencies(abName);
        for (int i = 0; i < dependencies.Length; i++)
        {
            // 判断包是否加载过
            if (!abDic.ContainsKey(dependencies[i]))
            {
                ab = AssetBundle.LoadFromFile(CurrentPath + dependencies[i]);
                // 加入字典
                abDic.Add(dependencies[i], ab);
            }
        }
        // 4.加载目标包 也是先判断一遍字典
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(CurrentPath + abName);
            // 加入字典
            abDic.Add(abName, ab);
        }
        // 5.加载目标资源
        Object obj = abDic[abName].LoadAsset(resName);
        // 如果是GameObject 直接实例化返回出去
        if (obj is GameObject)
        {
            return Instantiate(obj);
        }
        return abDic[abName].LoadAsset(resName);
    }

    /// <summary>
    /// 同步加载
    /// 因为Lua不支持泛型 所以得传指定类型
    /// </summary>
    public Object LoadRes(string abName, string resName, System.Type type)
    {
        // 加载之前 必须先获得依赖包的相关信息
        // 如果没加载过主包
        if (mainAB == null)
        {
            // 1.加载主包
            mainAB = AssetBundle.LoadFromFile(CurrentPath + MainABName);
            // 2.加载manifest
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        // 3.获取依赖包
        AssetBundle ab;
        string[] dependencies = manifest.GetAllDependencies(abName);
        for (int i = 0; i < dependencies.Length; i++)
        {
            // 判断包是否加载过
            if (!abDic.ContainsKey(dependencies[i]))
            {
                ab = AssetBundle.LoadFromFile(CurrentPath + dependencies[i]);
                // 加入字典
                abDic.Add(dependencies[i], ab);
            }
        }
        // 4.加载目标包 也是先判断一遍字典
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(CurrentPath + abName);
            // 加入字典
            abDic.Add(abName, ab);
        }
        // 5.加载目标资源
        return abDic[abName].LoadAsset(resName, type);
    }

    /// <summary>
    /// 同步加载
    /// 泛型重载 lua不能用
    /// </summary>
    public T LoadRes<T>(string abName, string resName) where T:Object
    {
        // 加载之前 必须先获得依赖包的相关信息
        // 如果没加载过主包
        if (mainAB == null)
        {
            // 1.加载主包
            mainAB = AssetBundle.LoadFromFile(CurrentPath + MainABName);
            // 2.加载manifest
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        // 3.获取依赖包
        AssetBundle ab;
        string[] dependencies = manifest.GetAllDependencies(abName);
        for (int i = 0; i < dependencies.Length; i++)
        {
            // 判断包是否加载过
            if (!abDic.ContainsKey(dependencies[i]))
            {
                ab = AssetBundle.LoadFromFile(CurrentPath + dependencies[i]);
                // 加入字典
                abDic.Add(dependencies[i], ab);
            }
        }
        // 4.加载目标包 也是先判断一遍字典
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(CurrentPath + abName);
            // 加入字典
            abDic.Add(abName, ab);
        }
        // 5.加载目标资源
        return abDic[abName].LoadAsset<T>(resName);
    }

    /// <summary>
    /// 单个包卸载
    /// </summary>
    public void UnLoad(string abName)
    {
        if (abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
    }

    /// <summary>
    /// 所有包卸载
    /// </summary>
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        // 主包和配置清空
        mainAB = null;
        manifest = null;
    }

    /// <summary>
    /// 异步加载
    /// </summary>
    public void AsyncLoadRes(string abName, string resName)
    {

    }
}