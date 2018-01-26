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

	void Start ()
    {
        sceneChange = GetComponent<SceneChange>();
        fade = GetComponent<Fade>();
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
        //ここでセーブデータから呼ぶ
        //今はないのでテストシーンに飛ばしている
        FindObjectOfType<Fade>().SetScene("SaveData");
        FindObjectOfType<Fade>().SetOutFade(true);
        FindObjectOfType<Fade>().SetSceneChangeSwitch(true);
    }

    //NewGame選択時
    //強制で特定シーンに飛ぶ(会話シーン)
    public void NewGame()
    {
        FindObjectOfType<Fade>().SetScene("Story");
        FindObjectOfType<Fade>().SetOutFade(true);
        FindObjectOfType<Fade>().SetSceneChangeSwitch(true);
    }

    //クレジット画面に飛ぶ
    public void GameCredit()
    {
        //本来はクレジットに飛ぶ
        FindObjectOfType<Fade>().SetScene("Credit");
        FindObjectOfType<Fade>().SetOutFade(true);
        FindObjectOfType<Fade>().SetSceneChangeSwitch(true);
    }

    //ゲーム終了
    public void GameExit()
    {
        Application.Quit();
    }
}
