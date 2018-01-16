using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SubMenuRenderer : MonoBehaviour
{
    /*イベントシステムの取得*/
    public EventSystem eventSystem;
    /*ボタンテキスト取得*/
    public GameObject[] SubRenderer;
    /*UIフレーム取得*/
    public GameObject Frame;
   
    /*グローバル変数*/
    private int UIcount;
    private int MenuStartFlagCount;
    private GameObject UIMenu02;
    private GameObject DummyButton;
    private bool SubMenuControlFlag = false;


    void Start ()
    {
        GameObjectAcquisition();
        MainMenuFalse();
        // ボタンが押された時の処理を登録
        GameObject.Find("Menu5_Attack").GetComponent<Button>().onClick.AddListener(() => FindObjectOfType<BattleFlowTest>().AttackBt());
        GameObject.Find("Menu6_Skill").GetComponent<Button>().onClick.AddListener(() => FindObjectOfType<BattleFlowTest>().SkillBt());
        GameObject.Find("Menu7_Item").GetComponent<Button>().onClick.AddListener(() => FindObjectOfType<BattleFlowTest>().ItemBt());
        GameObject.Find("Menu8_Return").GetComponent<Button>().onClick.AddListener(() => FindObjectOfType<BattleFlowTest>().TurnEnd());
    }
    public void SubMenuStart()
    {
        eventSystem.sendNavigationEvents = true;
        eventSystem.SetSelectedGameObject(UIMenu02);
        MenuStartFlagCount += 1;
        switch(MenuStartFlagCount)
        {
            case 1:
                MainMenuTrue();
                /*メインメニュー制御可能*/
                //FindObjectOfType<MenuManager>().GetMainControlFlag(true);
            break;
            case 2:
                MainMenuFalse();
                /*メインメニュー制御可能*/
                //FindObjectOfType<MenuManager>().GetMainControlFlag(false);
                break;
        }
    }
    private void MainMenuTrue()
    {
        /*メニュー表示*/
        for (UIcount = 0; UIcount <= 3; UIcount++)
        {
            SubRenderer[UIcount].GetComponent<Text>().enabled = true;
        }
        Frame.SetActive(true);
        /*メインボタンセット*/
        eventSystem.SetSelectedGameObject(UIMenu02);

    }
    private void MainMenuFalse()
    {
        /*メニュー非表示*/
        for (UIcount = 0; UIcount <= 3; UIcount++)
        {
            SubRenderer[UIcount].GetComponent<Text>().enabled = false;
        }
        Frame.SetActive(false);
        /*ダミーボタンセット*/
        eventSystem.SetSelectedGameObject(DummyButton);
        MenuStartFlagCount = 0;
    }
    private void GameObjectAcquisition()
    {
        UIMenu02 = GameObject.Find("Menu5_Attack");
        DummyButton = GameObject.Find("DummyButton");
    }
    public void GetSubControlFlag(bool Flag)
    {
        /*trueなら制御可能falseなら制御不可*/
        SubMenuControlFlag = Flag;
    }
}
