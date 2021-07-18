using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : BaseMgr<MusicMgr>
{
    // 背景音乐是唯一的
    private AudioSource bkMusic = null;
    // 背景音乐大小
    private float bkVolume = 1;
    // 音效大小
    private float soundVolume = 1;

    // 音效不是唯一的 但可以控制挂载的游戏对象是唯一的
    private GameObject soundObj = null;
    private List<AudioSource> soundList = new List<AudioSource>();

    public MusicMgr()
    {
        // 检测音效是否播放完成
        MonoMgr.Instance.AddUpdateListener(Update);
    }

    private void Update()
    {
        for (int i = 0; i < soundList.Count; i++)
        {
            // 如果音效是否播放完成
            if (!soundList[i].isPlaying)
            {
                // 先移除音效脚本对象
                GameObject.Destroy(soundList[i]);
                // 再移除音效列表
                soundList.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name">音乐名</param>
    public void PlayBkMusic(string name)
    {
        // 第一次播放时创建背景音乐游戏对象
        if (bkMusic == null)
        {
            GameObject obj = new GameObject("BkMusic");
            bkMusic = obj.AddComponent<AudioSource>();
        }
        // 异步加载防止游戏卡顿
        ResMgr.Instance.LoadAsync<AudioClip>("Music/" + name, (clip) =>
        {
            // 加载得到的资源赋值给背景音乐脚本
            bkMusic.clip = clip;
            // 设置背景音乐循环
            bkMusic.loop = true;
            // 设置背景音乐大小
            bkMusic.volume = bkVolume;
            bkMusic.Play();
        });
    }

    /// <summary>
    /// 改变背景音乐大小
    /// </summary>
    /// <param name="volume">音量</param>
    public void ChangeBkVolume(float volume)
    {
        this.bkVolume = volume;
        if (bkMusic != null)
        {
            bkMusic.volume = bkVolume;
        }
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBkMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Pause();
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name">音效名</param>
    /// <param name="isLoop">是否循环</param>
    /// <param name="callBack">获得音效脚本对象的回调函数(希望音效对象去干什么，可以不传)</param>
    public void PlaySound(string name, bool isLoop, UnityAction<AudioSource> callBack = null)
    {
        if (soundObj == null)
        {
            soundObj = new GameObject("Sound");
        }
        // 异步加载防止游戏卡顿
        ResMgr.Instance.LoadAsync<AudioClip>("Sound/" + name, (clip) =>
        {
            // 添加音效
            AudioSource source = soundObj.AddComponent<AudioSource>();
            // 加载得到的资源赋值给音效脚本
            source.clip = clip;
            // 设置音效大小
            source.volume = bkVolume;
            // 设置音效是否循环
            source.loop = isLoop;
            source.Play();
            // 将音效加入音效列表
            soundList.Add(source);
            if (callBack != null)
            {
                callBack(source);
            }
        });
    }

    /// <summary>
    /// 改变音效大小
    /// </summary>
    /// <param name="volume">音量大小</param>
    public void ChangeSoundVolume(float volume)
    {
        soundVolume = volume;
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].volume = soundVolume;
        }
    }

    /// <summary>
    /// 停止指定音效
    /// </summary>
    /// <param name="name">音效名</param>
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            // 移除音效列表
            soundList.Remove(source);
            source.Stop();
            // 移除这个音效对象
            GameObject.Destroy(source);
        }
    }
}
