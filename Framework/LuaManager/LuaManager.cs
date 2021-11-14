using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class LuaManager : BaseMgr<LuaManager>
{
    private LuaEnv luaEnv;

    /// <summary>
    /// 得到lua中的_G表
    /// </summary>
    public LuaTable Global
    {
        get { return luaEnv.Global; }
    }

    public void Init()
    {
        if (luaEnv == null)
        {
            luaEnv = new LuaEnv();
            // 加载lua脚本的重定向
            luaEnv.AddLoader(LuaScriptCustomLoader);
            luaEnv.AddLoader(LuaScriptCustomABLoader);
        }
    }

    /// <summary>
    /// Lua脚本执行
    /// </summary>
    /// <param name="luaScript"></param>
    public void DoString(string luaScript)
    {
        if (luaEnv != null)
            luaEnv.DoString(luaScript);
        else
            Debug.Log("解析器为空");
        
    }

    /// <summary>
    /// Lua脚本加载
    /// </summary>
    /// <param name="flieName"></param>
    public void DoLuaFile(string flieName)
    {
        DoString(string.Format("require('{0}')", flieName));
    }

    // Lua脚本最终会放在AB包
    // AB包无法识别.lua 需要将后缀改成.txt
    // 重定向ab包中的lua文件加载
    private byte[] LuaScriptCustomABLoader(ref string filePath)
    {
        //// 加载ab包
        //string abPath = Application.streamingAssetsPath + "/lua";
        //AssetBundle assetBundle = AssetBundle.LoadFromFile(abPath);

        //// 加载lua文件
        //TextAsset textAsset = assetBundle.LoadAsset<TextAsset>(filePath + ".lua");

        TextAsset textAsset = ABManager.Instance.LoadRes<TextAsset>("lua", filePath + ".lua");
        if (textAsset != null)
            // 加载byte[]
            return textAsset.bytes;
        else
            Debug.Log("AssetBundle重定向失败：" + filePath);
        return null;
    }

    // 垃圾回收
    public void Tick()
    {
        if (luaEnv != null)
            luaEnv.Tick();
        else
            Debug.Log("解析器为空");
    }

    // 销毁解释器
    public void Dispose()
    {
        if (luaEnv != null)
        {
            luaEnv.Dispose();
            luaEnv = null;
        }
        else
            Debug.Log("解析器为空");
    }

    // 自定义加载逻辑
    // 如果找不到 会去找默认路径
    private byte[] LuaScriptCustomLoader(ref string filePsth)
    {
        // 重定向自己定义的Lua文件路径
        string path = Application.dataPath + "/Scripts/Lua/" + filePsth + ".Lua";
        // 加载文件 利用c#自带的文件读写
        if (File.Exists(path))
        {
            return File.ReadAllBytes(path);
        }
        else
        {
            Debug.Log("/Scripts/Lua/路径重定向失败：" + filePsth);
            return null;
        }
    }
}
