using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 输入管理类
/// </summary>
public class InputMgr : BaseMgr<InputMgr>
{
    // 是否开启输入管理模块
    private bool isStart = false;
    // 建立字典取对应方向和键
    private Dictionary<string, KeyCode> keyDic = new Dictionary<string, KeyCode>()
    {
        {"forward",KeyCode.W },
        {"back",KeyCode.S },
        {"left",KeyCode.A },
        {"right",KeyCode.D },
    };
    

    /// <summary>
    /// 构造函数中去把检测输入的函数添加给公共mono的Update
    /// </summary>
    public InputMgr()
    {
        MonoMgr.Instance.AddUpdateListener(ForUpdate);
    }

    /// <summary>
    /// 开启或关闭输入检测 默认关闭
    /// </summary>
    public void StartOrEndCheck(bool isStart)
    {
        this.isStart = isStart;
    }

    private void CheckKeyCode(string pos ,KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            // 事件中心分发事件 触发对应事件
            EventCenter.Instance.EventTrigger("方向键按下", pos);
        }
    }

    /// <summary>
    /// 添加到公共mono的Update事件中的函数
    /// </summary>
    private void ForUpdate()
    {
        // 如果未开启 则直接return
        if (!this.isStart)
        {
            return;
        }
        CheckKeyDic("forward");
        CheckKeyDic("back");
        CheckKeyDic("left");
        CheckKeyDic("right");
    }

    private void CheckKeyDic(string pos)
    {
        CheckKeyCode(pos, keyDic[pos]);
    }

    public void ChangeKey(string pos, KeyCode key)
    {
        if (keyDic.ContainsKey(pos))
        {
            keyDic[pos] = key;
        }
    }
}
