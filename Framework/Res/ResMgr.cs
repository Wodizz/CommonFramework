using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载模块
/// </summary>
public class ResMgr : BaseMgr<ResMgr>
{
    // 同步加载资源
    public T Load<T>(string name) where T:Object
    {
        T res = null;
        res = Resources.Load<T>(name);
        // 如果要加载的对象是一个GameObject 就实例化返回出去
        if (res is GameObject)
        {
            return GameObject.Instantiate(res);
        }
        // TextAsset AudioClip
        else
        {
            // 不需要实例化的就return出去
            return res;
        }
    }

    /// <summary>
    /// 异步加载资源方法
    /// </summary>
    /// <typeparam name="T">该资源的类型</typeparam>
    /// <param name="name">路径(名字)</param>
    /// <param name="action">需要这个资源干的事</param>
    /// <returns></returns>
    public T LoadAsync<T>(string name, UnityAction<T> action) where T : Object
    {
        T res = null;
        // 调用公共Mono模块中的开启协程
        MonoMgr.Instance.StartCoroutine(LoadAsyncByCoroutine<T>(name, action));
        return res;
    }

    private IEnumerator LoadAsyncByCoroutine<T>(string name, UnityAction<T> action) where T : Object
    {
        ResourceRequest rr = Resources.LoadAsync<T>(name);
        yield return rr;
        // rr.asset 获得正在加载的资源
        if (rr.asset is GameObject)
        {
            // 实例化之后调用需要该资源干的方法
            action(GameObject.Instantiate(rr.asset) as T);
        }
        else
        {
            // 调用需要该资源干的方法
            action(rr.asset as T);
        }
    }

}
