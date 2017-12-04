using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScript : MonoBehaviour {

    //なんか押してねってテキスト
    [SerializeField]
    Text anyKeyText;

    //タイトルのボタン
    [SerializeField]
    GameObject[] Titlebutton;

    //フェードスクリプト
    SceneChange sceneChange;
    Fade fade;

	void Start ()
    {
        sceneChange = GetComponent<SceneChange>();
        fade = GetComponent<Fade>();
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
            for (int i = 0; i < Titlebutton.Length; i++)
            {
                Titlebutton[i].SetActive(true);
            }
        }
    }

    //Continue選択時
    //セーブデータからシーン情報を読み込んで飛ぶ
    public void Continue()
    {
        //ここでセーブデータから呼ぶ
        //今はないのでテストシーンに飛ばしている
        sceneChange.sceneName = "Story";
        fade.isFadeOut = true;
        fade.sceneChangeSwitch = true;
    }

    //NewGame選択時
    //強制で特定シーンに飛ぶ(会話シーン)
    public void NewGame()
    {
        sceneChange.sceneName = "Story";
        fade.isFadeOut = true;
        fade.sceneChangeSwitch = true;
    }

    //クレジット画面に飛ぶ
    public void GameCredit()
    {
        //本来はクレジットに飛ぶ
        sceneChange.sceneName = "Story";
        fade.isFadeOut = true;
        fade.sceneChangeSwitch = true;
    }

    //ゲーム終了
    public void GameExit()
    {
        Application.Quit();
    }
}
