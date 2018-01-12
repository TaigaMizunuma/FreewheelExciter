//フェードアウト/インの制御
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour {

    //フェードの速度
    [SerializeField]
    float fadeSpeed;

    //フェードするまでの時間
    [SerializeField]
    float fadeTime;

    //フェード用の画像
    //黒い画像でも用意しておいてください
    //色の変更も可能ですが、それが一番早いです
    [SerializeField]
    Image fadeImage;

    //アルファの数値
    [SerializeField]
    float alfa;

    //フラグ管理
    public bool isFadeOut;
    public bool isFadeIn;

    [HideInInspector]
    public bool sceneChangeSwitch;

    private string changeName;

    void Start ()
    {
        isFadeIn = true;
        isFadeOut = false;

        sceneChangeSwitch = false;

        FadeTime();

    }

    void Update ()
    {
        FadeFlag();

        if (alfa < 0)
        {
            alfa = 0;
        }
        else if (alfa > 1)
        {
            alfa = 1;
        }

	}

    void FadeFlag()
    {
        if (isFadeIn == true)
        {
            SceneFadeIn();
        }

        if (isFadeOut == true)
        {
            SceneFadeOut();
        }
    }

    void SceneFadeOut()
    {
        fadeImage.GetComponent<Image>().color = new Color(1, 1, 1, alfa);
        if(fadeImage.enabled == false)
        {
            fadeImage.enabled = true;
        }

        alfa += fadeSpeed;
        if (alfa >= 1)
        {
            isFadeOut = false;

            if (sceneChangeSwitch == true)
            {
                SceneManager.LoadScene(changeName);
            }
        }
    }

    void SceneFadeIn()
    {
        fadeImage.GetComponent<Image>().color = new Color(1, 1, 1, alfa);
        alfa -= fadeSpeed;
        if (alfa <= 0)
        {
            fadeImage.enabled = false;
            isFadeIn = false;
        }
    }

    void FadeTime()
    {
        fadeSpeed = Time.deltaTime / (fadeTime);
    }
    /// <summary>
    /// フェードインを実行するフラグ　黒→白
    /// </summary>
    /// <param name="flag"></param>
    public void SetInFade(bool flag)
    {
        isFadeIn = flag;
    }
    /// <summary>
    /// フェードアウトを実行するフラグ　白→黒
    /// </summary>
    public void SetOutFade(bool flag)
    {
        if (isFadeIn != true)
        {
            isFadeOut = flag;
        }
    }

    /// <summary>
    /// シナリオを読む全部読むためのフラグ ONにしない限りScene移動しない
    /// </summary>
    public void SetSceneChangeSwitch(bool flag)
    {
        if (isFadeIn != true)
        {
            sceneChangeSwitch = flag;
        }
    }

    /// <summary>
    /// シーン名セットします
    /// </summary>
    /// <param name="name"></param>
    public void SetScene(string name)
    {
        changeName = name;
    }
    /// <summary>
    /// 現在のシーンを取得します
    /// </summary>
    /// <returns></returns>
    public string GetScene()
    {
        return changeName;
    }
}
