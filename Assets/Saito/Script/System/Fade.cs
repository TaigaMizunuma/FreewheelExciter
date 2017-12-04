//フェードアウト/インの制御
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //ストーリーからシーンチェンジにシーン名を引き渡す
    //ストーリーリーダーがないシーンもあるのでここに書きました
    public string changeName;

    //シーンチェンジのスクリプト
    SceneChange sceneChange;

	void Start ()
    {
        sceneChange = GetComponent<SceneChange>();

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
        sceneChange.sceneName = changeName;
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
                sceneChange.Change();
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
}
