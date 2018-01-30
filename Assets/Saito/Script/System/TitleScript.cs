using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleScript : MonoBehaviour {

    public EventSystem eventSystem;

    //なんか押してねってテキスト
    [SerializeField]
    Text anyKeyText;

    //タイトルのボタン
    [SerializeField]
    Image[] titleButtonImage;

    [SerializeField]
    Text[] titleButtonText;

    //フェードスクリプト
    SceneChange sceneChange;
    Fade fade;

    [SerializeField]
    bool buttonPressed;
    [SerializeField]
    bool fadeIn;
    [SerializeField]
    bool fadeOut;

	void Start ()
    {
        buttonPressed = false;
        sceneChange = GetComponent<SceneChange>();
        fade = GetComponent<Fade>();
        fadeIn = fade.isFadeIn;
        fadeOut = fade.isFadeOut;
        for (int i = 0; i < titleButtonImage.Length; i++)
        {
            titleButtonImage[i].color = new Color(0, 0, 0, 0);
        }
        for (int j = 0; j < titleButtonText.Length; j++)
        {
            titleButtonText[j].enabled = false;
        }
        eventSystem.sendNavigationEvents = false;
    }

    void Update () {
        fadeIn = fade.isFadeIn;
        fadeOut = fade.isFadeOut;

        if (fadeIn == true || fadeOut == true)
        {
            eventSystem.enabled = false;
        }
        else if (fadeIn == false && fadeOut == false)
        {
            eventSystem.enabled = true;
        }

        TitleButtonPush();
	}

    //タイトルのメニューに遷移するためのメソッド
    void TitleButtonPush()
    {
        if (Input.anyKeyDown)
        {
            anyKeyText.enabled = false;
            eventSystem.sendNavigationEvents = true;
            for (int i = 0; i < titleButtonImage.Length; i++)
            {
                titleButtonImage[i].color = new Color(1,1,1,1);
            }
            for (int j = 0; j < titleButtonText.Length; j++)
            {
                titleButtonText[j].enabled = true;
            }
        }
    }

    //Continue選択時
    //セーブデータからシーン情報を読み込んで飛ぶ
    public void Continue()
    {
        if (buttonPressed == false && fadeIn == false && fadeOut == false)
        {
            //ここでセーブデータから呼ぶ
            FindObjectOfType<Fade>().SetScene("SaveData");
            FindObjectOfType<Fade>().SetOutFade(true);
            FindObjectOfType<Fade>().SetSceneChangeSwitch(true);
        }
    }

    //NewGame選択時
    //強制で特定シーンに飛ぶ(会話シーン)
    public void NewGame()
    {
        if (buttonPressed == false && fadeIn == false && fadeOut == false)
        {
            FindObjectOfType<Fade>().SetScene("Story");
            FindObjectOfType<Fade>().SetOutFade(true);
            FindObjectOfType<Fade>().SetSceneChangeSwitch(true);
            buttonPressed = true;
        }
    }

    //クレジット画面に飛ぶ
    public void GameCredit()
    {
        if (buttonPressed == false && fadeIn == false && fadeOut == false)
        {
            //本来はクレジットに飛ぶ
            FindObjectOfType<Fade>().SetScene("Credit");
            FindObjectOfType<Fade>().SetOutFade(true);
            FindObjectOfType<Fade>().SetSceneChangeSwitch(true);
            buttonPressed = true;
        }
    }

    //ゲーム終了
    public void GameExit()
    {
        Application.Quit();
    }
}
