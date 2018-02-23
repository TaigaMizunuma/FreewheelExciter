using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Resources->se,bgmに入れとけば勝手に読み込んでくれるクラス
/// </summary>
public class AudioManager : MonoBehaviour
{
    private AudioClip[] seClips;
    
    private AudioClip[] bgmClips;

    private AudioSource bgmSource;
    private AudioSource seSource;

    private Dictionary<string, int> seIndexes = new Dictionary<string, int>();
    private Dictionary<string, int> bgmIndexes = new Dictionary<string, int>();

    public float bgmvolume =0;
    public float sevolume =1;

    void Start()
    {
        seClips = Resources.LoadAll<AudioClip>("SE");
        bgmClips = Resources.LoadAll<AudioClip>("BGM");

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        
        seSource = gameObject.AddComponent<AudioSource>();
        seSource.loop = false;
       

        for (int i = 0; i < seClips.Length; ++i)
        {
            seIndexes[seClips[i].name] = i;
        }

        for (int i = 0; i < bgmClips.Length; ++i)
        {
            bgmIndexes[bgmClips[i].name] = i;
        }

        //PlayBgm("battle1");
    }

    // Update is called once per frame
    void Update()
    {
        bgmSource.volume = bgmvolume;
        seSource.volume = sevolume;

        //デバッグ用
        //if (Input.GetKeyDown(KeyCode.RightAlt))
        //{
        //    var num = bgmvolume;
        //    num += 0.1f;
        //    SetBGMVolum(num);
        //}
        //if (Input.GetKeyDown(KeyCode.LeftAlt))
        //{
        //    var num = bgmvolume;
        //    num -= 0.1f;
        //    SetBGMVolum(num);
        //}
    }

    /// <summary>
    /// BGMの音量調整(0～1)
    /// </summary>
    /// <param name="value">0が音なし、1が最大</param>
    public void SetBGMVolum(float value)
    {
        bgmvolume = value;
    }

    /// <summary>
    /// SEの音量調整(0～1)
    /// </summary>
    /// <param name="value">0が音なし、1が最大</param>
    public void SetSeVolum(float value)
    {
        sevolume = value;
    }

    public int GetSeIndex(string name)
    {
        return seIndexes[name];
    }


    public int GetBgmIndex(string name)
    {
        return bgmIndexes[name];
    }

    /// <summary>
    /// BGM再生 name=ファイル名
    /// </summary>
    /// <param name="name"></param>
    public void PlayBgm(string name)
    {
        int index = bgmIndexes[name];
        bgmSource.Stop();
        bgmSource.clip = bgmClips[index];
        bgmSource.Play();
    }

    /// <summary>
    /// SE再生 name=ファイル名
    /// </summary>
    /// <param name="name"></param>
    public void PlaySe(string name)
    {
        int index = seIndexes[name];
        seSource.clip = seClips[index];
        seSource.Play();
    }
}
