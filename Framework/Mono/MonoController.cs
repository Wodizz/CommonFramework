using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// MonoController类
/// </summary>
public class MonoController : MonoBehaviour
{
    private event UnityAction updateEvent;

    // Start is called before the first frame update
    void Start()
    {
        // 不移除对象
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (updateEvent != null)
        {
            updateEvent();
        }
    }

    /// <summary>
    /// 提供给外部 添加帧更新事件的函数
    /// </summary>
    /// <param name="fun">要添加的函数</param>
    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }

    /// <summary>
    /// 提供给外部 用于移除帧更新事件的函数
    /// </summary>
    /// <param name="fun">要移除的函数</param>
    public void RemoveUpdateLisenenr(UnityAction fun)
    {
        updateEvent -= fun;
    }
}
