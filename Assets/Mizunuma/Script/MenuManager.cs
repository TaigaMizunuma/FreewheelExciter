using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{

    /*イベントシステムの取得*/
    public EventSystem eventSystem;
    /*ゲームフレーム取得*/
    public GameObject MapFrame;
    /*ボタン数取得*/
    public GameObject[] UIString;
    /*コントロールフラグ true=有効 false=無効*/
    private bool MenuControlFlag = false;

    /*グローバル変数*/
    private Image MapCursor;
    /*ダミーボタンの取得(ボタン、非アクティブ対策)*/
    private GameObject DummyButton;
    /*UIのメニュー(部隊)*/
    private GameObject UIMenu01;
    /*ボタン表示用*/
    private int UIcount;
    private int MenuCount = 0;
    private GameObject UIMainMenu1_Situation;

    void Start()
    {
        /*ゲームオブジェクト取得*/
        GameObjectAcquisition();
        /*非表示*/
        MainMenuFalse();
        /*ダミーボタンセット*/
        eventSystem.SetSelectedGameObject(DummyButton);
        eventSystem.sendNavigationEvents = false;
    }
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.M) && MenuControlFlag == false)
        {
            eventSystem.sendNavigationEvents = true;
            IniMainMenu();
        }
    }

    public void OnSituation()
    {
        /*カウントアップ 0=初期値　1=表示 2=非表示*/
        FindObjectOfType<SituationTexts>().SituationCount();
        
    }
    public void OnConfig()
    {
        /*02設定*/
        FindObjectOfType<SettingsTexts>().SettingsCount();
    }
    public void OnInterruption()
    {
        /*03中断*/
        FindObjectOfType<InterruptionTexts>().InterruptionCount();
    }
    public void OnEnd()
    {
        /*04終了*/
        eventSystem.sendNavigationEvents = false;
        IniMainMenu();
        FindObjectOfType<BattleFlowTest>().TurnEnd();
    }

    public void MainMenuEventSystemStart()
    {
        /*イベントシステムをセット*/
        eventSystem.SetSelectedGameObject(UIMenu01);
    }

    public void IniMainMenu()// 11/03 publicに変更
    {
        
        MenuCount += 1;
        switch (MenuCount)
        {
            case 1:
                /*メイン*/
                MainMenuTrue();
                /*サブメニュー制御不可*/
                FindObjectOfType<SubMenuRenderer>().GetSubControlFlag(true);
                /*カーソル制限*/
                FindObjectOfType<RayBox>().move_ = false;
                /*カーソル非表示仮*/
                MapCursor.enabled = false;
                FindObjectOfType<BattleFlowTest>().state_ = State_.menu_mode;
                break;
            case 2:
                MainMenuFalse();
                /*サブメニュー制御可能*/
                FindObjectOfType<SubMenuRenderer>().GetSubControlFlag(false);
                /*カーソル制限*/
                FindObjectOfType<RayBox>().move_ = true;
                /*カーソル表示仮*/
                MapCursor.enabled = true;
                FindObjectOfType<BattleFlowTest>().state_ = State_.simulation_mode;
                break;
        }
    }
    private void GameObjectAcquisition()
    {
        DummyButton = GameObject.Find("DummyButton");
        UIMenu01 = GameObject.Find("Menu1_Situation");
        UIMainMenu1_Situation = GameObject.Find("UIMainMenu1_Situation");
        MapCursor = GameObject.Find("MapCursor").GetComponent<Image>();
    }
    private void MainMenuTrue()
    {
        /*メニュー表示*/
        MapFrame.SetActive(true);
        for (UIcount = 0; UIcount <= 3; UIcount++)
        {
            UIString[UIcount].GetComponent<Text>().enabled = true;
        }
        MainMenuEventSystemStart();
    }
    private void MainMenuFalse()
    {
        /*メニュー非表示*/
        MapFrame.SetActive(false);
        for (UIcount = 0; UIcount <= 3; UIcount++)
        {
            UIString[UIcount].GetComponent<Text>().enabled = false;
        }
        eventSystem.SetSelectedGameObject(DummyButton);
        MenuCount = 0;
    }

    public void SetMainControlFlag(bool flag)
    {
        /*trueなら制御 falseなら制御解除*/
        MenuControlFlag = flag;
    }

    public void SetEventSystem(bool flag)
    {
        /*イベントシステムロック*/
        eventSystem.sendNavigationEvents = flag;
    }
}
