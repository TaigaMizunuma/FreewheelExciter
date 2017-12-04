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
    public void OnAttack()
    {
        /*05アタック*/
        Debug.Log("アタック");
    }
    public void OnSkill()
    {
        /*06スキル*/
        Debug.Log("スキル");
    }
    public void OnItem()
    {
        /*07道具*/
        Debug.Log("道具");
    }
    public void OnReturn()
    {
        /*08戻る*/
        Debug.Log("戻る");
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
