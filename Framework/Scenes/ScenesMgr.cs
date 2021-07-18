using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


/// <summary>
/// 场景切换模块
/// </summary>
public class ScenesMgr : BaseMgr<ScenesMgr>
{
    /// <summary>
    /// 同步切换场景方法
    /// </summary>
    /// <param name="name">场景名</param>
    /// <param name="action">要运行的函数</param>
    public void LoadScene(string name, UnityAction action)
    {
        // 场景同步加载
        SceneManager.LoadScene(name);
        // 加载完成后才会执行
        action();
    }

    /// <summary>
    /// 异步切换场景方法
    /// </summary>
    /// <param name="name">场景名</param>
    /// <param name="action">要运行的函数</param>
    public void LoadSceneAsyn(string name, UnityAction action)
    {
        // 因为要开启协程 这个类必须继承Mono 所有用公共Mono管理类中封装的协程方法
        MonoMgr.Instance.StartCoroutine(LoadSceneAsynByCoroutine(name, action));
    }

    // 只要是异步 肯定运用协程
    private IEnumerator LoadSceneAsynByCoroutine(string name, UnityAction action)
    {
        // Unity自带异步加载场景方法
        // AsyncOperation异步操作协同程序类 具体看笔记
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        while (!ao.isDone)
        {
            // 去触发画进度条事件
            EventCenter.Instance.EventTrigger("进度条更新事件", ao.progress);
            // 如果没完成 返回进度值
            yield return ao.progress;
        }
        yield return null;
        // 加载完成后 调用的方法
        action();
    }

    
}
