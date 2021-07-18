using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// UI层级
/// </summary>
public enum E_UILayer
{
    Bot,
    Mid,
    Top,
    System,
}

/// <summary>
/// UI管理器
/// 1.管理所有显示的面板
/// 2.提供给外部显示和隐藏的接口
/// </summary>
public class UIManager : BaseMgr<UIManager>
{
    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    // 方便外部使用
    public GameObject canvas;

    // 各层对象
    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;

    public UIManager()
    {
        // 初始化动态创建canvas
        canvas =  ResMgr.Instance.Load<GameObject>("UI/Canvas");
        // 过场景不移除canvas
        GameObject.DontDestroyOnLoad(canvas);

        // 找到各层级
        bot = canvas.transform.Find("Bot");
        mid = canvas.transform.Find("Mid");
        top = canvas.transform.Find("Top");
        system = canvas.transform.Find("System");

        // 创建EventSystem
        GameObject eventSystem = ResMgr.Instance.Load<GameObject>("UI/EventSystem");
        // 过场景不移除EventSystem
        GameObject.DontDestroyOnLoad(eventSystem);
    }

    /// <summary>
    /// 加载显示预设体面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="layer">显示在哪一层</param>
    /// <param name="callBack">控制面板脚本对象的回调函数 可不传</param>
    public void ShowPanel<T>(string panelName, E_UILayer layer, UnityAction<T> callBack = null) where T : BasePanel
    {
        // 如果已经存在面板 直接去调用回调函数
        if (panelDic.ContainsKey(panelName))
        {
            // 如果传了回调函数
            if (callBack != null)
            {
                // 去操作脚本对象干一些事
                callBack(panelDic[panelName] as T);
            }
            return;
        }
        // 调用资源模块异步加载
        ResMgr.Instance.LoadAsync<GameObject>("UI/" + panelName, (obj) => 
        {
            // 默认父对象为最下层
            Transform father = bot;
            // 找到父对象在哪一层
            switch (layer)
            {
                case E_UILayer.Mid:
                    father = mid;
                    break;
                case E_UILayer.Top:
                    father = top;
                    break;
                case E_UILayer.System:
                    father = system;
                    break;
                default:
                    break;
            }
            // 设置父对象
            (obj.transform as RectTransform).SetParent(father, false);

            // 防止显示问题
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            //// 防止偏移问题
            //(obj.transform as RectTransform).offsetMax = Vector2.zero;
            //(obj.transform as RectTransform).offsetMin = Vector2.zero;

            // 找到obj上的面板脚本对象
            T panel = obj.GetComponent<T>();

            // 如果传了回调函数
            if (callBack != null)
            {
                // 去操作脚本对象干一些事
                callBack(panel);
            }
            // 放入字典
            panelDic.Add(panelName, panel);
        });
    }

    /// <summary>
    /// 加载显示预设体面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="parent">面板父类</param>
    /// <param name="callBack">控制面板脚本对象的回调函数 可不传</param>
    public void ShowPanel<T>(string panelName, Transform parent, UnityAction<T> callBack = null) where T : BasePanel
    {
        // 如果已经存在面板 直接去调用回调函数
        if (panelDic.ContainsKey(panelName))
        {
            // 如果传了回调函数
            if (callBack != null)
            {
                // 去操作脚本对象干一些事
                callBack(panelDic[panelName] as T);
            }
            return;
        }
        // 调用资源模块异步加载
        ResMgr.Instance.LoadAsync<GameObject>("UI/" + panelName, (obj) =>
        {
            // 设置父对象
            obj.transform.SetParent(parent, false);

            // 防止显示问题
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            //// 防止偏移问题
            //(obj.transform as RectTransform).offsetMax = Vector2.zero;
            //(obj.transform as RectTransform).offsetMin = Vector2.zero;

            // 找到obj上的面板脚本对象
            T panel = obj.GetComponent<T>();

            // 如果传了回调函数
            if (callBack != null)
            {
                // 去操作脚本对象干一些事
                callBack(panel);
            }
            // 放入字典
            panelDic.Add(panelName, panel);
        });
    }

    /// <summary>
    /// 移除面板
    /// </summary>
    /// <param name="panelName">面板路径</param>
    public void HidePanel(string panelName)
    {
        // 先判断有没有这个面板
        if (panelDic.ContainsKey(panelName))
        {
            // 删除面板
            GameObject.Destroy(panelDic[panelName].gameObject);
            // 字典中移除面板
            panelDic.Remove(panelName);
        }
    }

    /// <summary>
    /// 得到指定面板脚本对象
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="name">面板名</param>
    /// <returns>面板对象</returns>
    public T GetPanel<T>(string name) where T : BasePanel
    {
        if (panelDic.ContainsKey(name))
        {
            return panelDic[name] as T;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 得到层级对象
    /// </summary>
    /// <param name="e_UILayer">层级枚举</param>
    /// <returns></returns>
    public Transform GetLayer(E_UILayer e_UILayer)
    {
        switch (e_UILayer)
        {
            case E_UILayer.Bot:
                return bot;
            case E_UILayer.Mid:
                return mid;
            case E_UILayer.Top:
                return top;
            case E_UILayer.System:
                return system;
        }
        return null;
    }

    /// <summary>
    /// 给控件添加自定义事件的监听
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="type">事件类型</param>
    /// <param name="callBack">事件响应函数</param>
    public void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack)
    {
        // 先去得EventTrigger组件
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        // 如果没有就加一个
        if (trigger == null)
        {
            trigger = control.gameObject.AddComponent<EventTrigger>();
        }
        // new一个EventTrigger的内部类Entry
        EventTrigger.Entry entry = new EventTrigger.Entry();
        // 选择对应的事件
        entry.eventID = type;
        // 添加事件对应的函数
        entry.callback.AddListener(callBack);
        // 把entry对应添加给触发器对象 
        trigger.triggers.Add(entry);
    }
}
