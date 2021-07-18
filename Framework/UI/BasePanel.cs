using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 面板基类
/// 找到所有自己面板下的指定控件对象
/// 提供显示和隐藏行为
/// </summary>
public class BasePanel : MonoBehaviour
{
    // UGUI的所有控件有一个共同基类 UIBehivour
    // List<UIBehaviour>是因为一个物体可以同时挂载多个控件(Button,Image..)
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();
    
    // 保护类型虚函数 方便子类重写
    protected virtual void Awake()
    {
        FindChildrenControl<Button>();
        FindChildrenControl<Text>();
        FindChildrenControl<Image>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<Slider>();
        FindChildrenControl<InputField>();
    }

    /// <summary>
    /// 按钮点击逻辑虚函数
    /// </summary>
    /// <param name="btnName">按钮名</param>
    protected virtual void OnClick(string btnName)
    {

    }

    /// <summary>
    /// 单选框选择逻辑虚函数
    /// </summary>
    /// <param name="toggleName">单选框名字</param>
    /// <param name="value">是否选择</param>
    protected virtual void OnValueChange(string toggleName, bool value)
    {

    }



    /// <summary>
    /// 在子对象中找到所有对应控件添加到字典
    /// 并添加对应的事件
    /// </summary>
    /// <typeparam name="T">控件类型</typeparam>
    private void FindChildrenControl<T>() where T: UIBehaviour
    {
        // 可以找到自己挂载的子物体上所有T类型脚本对象(包括子物体的子物体)
        T[] controls = this.GetComponentsInChildren<T>();
        for (int i = 0; i < controls.Length; i++)
        {
            string objName = controls[i].gameObject.name;
            // 判断是否已经存在控件名(将同一物体的不同控件都放进去)
            if (controlDic.ContainsKey(objName))
            {
                controlDic[objName].Add(controls[i]);
            }
            else
            {
                // 没有就new一个list
                controlDic.Add(objName, new List<UIBehaviour>() { controls[i] });
            }
            // 如果是按钮控件
            if (controls[i] is Button)
            {
                // 拿无参lambda表达式包裹OnClick函数
                (controls[i] as Button).onClick.AddListener(() =>
                {
                    // 调用点击虚函数 如果子类重写了就是调用子类的
                    OnClick(objName);
                });
            }
            // 如果是一个单选框
            else if (controls[i] is Toggle)
            {
                // 拿无参lambda表达式包裹OnClick函数
                (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                {
                    // 调用选择虚函数 如果子类重写了就是调用子类的
                    OnValueChange(objName,value);
                });
            }

        }
    }

    /// <summary>
    /// 得到对应类型的控件脚本对象
    /// </summary>
    /// <typeparam name="T">控件类型</typeparam>
    /// <param name="controlName">控件名字</param>
    /// <returns></returns>
    protected T GetControl<T>(string controlName) where T:UIBehaviour
    {
        // 先判断有没有这个控件
        if (controlDic.ContainsKey(controlName))
        {
            // 遍历控件List
            for (int i = 0; i < controlDic[controlName].Count; i++)
            {
                if (controlDic[controlName][i] is T)
                {
                    return controlDic[controlName][i] as T;
                }
            }
        }
        return null;
    }

}
