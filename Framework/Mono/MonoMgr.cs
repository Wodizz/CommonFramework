using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// MonoController封装后的管理类
/// </summary>
public class MonoMgr : BaseMgr<MonoMgr>
{
    // 声明一个MonoController对象
    private MonoController controller;

    public MonoMgr()
    {
        // 在单例初始化时 就会new一个物体
        GameObject obj = new GameObject("MonoController");
        // 给这个物体挂载MonoController脚本
        controller = obj.AddComponent<MonoController>();
    }

    /// <summary>
    /// 提供给外部 添加帧更新事件的函数
    /// </summary>
    /// <param name="fun">要添加的函数</param>
    public void AddUpdateListener(UnityAction fun)
    {
        controller.AddUpdateListener(fun);

        
    }

    /// <summary>
    /// 提供给外部 用于移除帧更新事件的函数
    /// </summary>
    /// <param name="fun">要移除的函数</param>
    public void RemoveUpdateLisenenr(UnityAction fun)
    {
        controller.RemoveUpdateLisenenr(fun);
    }

    /// <summary>
    /// 外部开启协程方法(重写Mono中的协程)
    /// </summary>
    /// <param name="routine">IEnumerator函数</param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }

    /// <summary>
    /// 外部关闭协程方法
    /// </summary>
    /// <param name="routine">IEnumerator函数</param>
    public void StopCoroutine(IEnumerator routine)
    {
        controller.StopCoroutine(routine);
    }
}
