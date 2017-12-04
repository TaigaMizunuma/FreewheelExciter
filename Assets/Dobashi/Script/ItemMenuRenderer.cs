using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemMenuRenderer : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject[] ItemRenderer;
    public GameObject Frame;
    public GameObject DummyButton;
    private int UIcount;
    private int MenuStartFlagCount;
    private GameObject UIMenu01;


    // Use this for initialization
    void Start()
    {
        IniItemMenu();

    }
    public void SubMenuStart()
    {
        ///if (FindObjectOfType<BattleFlowTest>().state_ != State_.choose_mode) return;// 11/07 追加

        MenuStartFlagCount += 1;
        switch (MenuStartFlagCount)
        {
            case 1:
                for (UIcount = 0; UIcount <= 4; UIcount++)
                {
                    ItemRenderer[UIcount].GetComponent<Text>().enabled = true;
                }
                Frame.SetActive(true);
                eventSystem.SetSelectedGameObject(UIMenu01);
                break;

            case 2:
                for (UIcount = 0; UIcount <= 4; UIcount++)
                {
                    ItemRenderer[UIcount].GetComponent<Text>().enabled = false;
                }
                Frame.SetActive(false);
                eventSystem.SetSelectedGameObject(DummyButton);
                break;
        }
        if (MenuStartFlagCount == 2)
        {
            MenuStartFlagCount = 0;
        }
    }

    public void MenuEnd()
    {
        SubMenuStart();
        FindObjectOfType<MenuManager>().MainMenuEventSystemStart();
    }

    public void IniItemMenu()
    {
        for (UIcount = 0; UIcount <= 4; UIcount++)
        {
            ItemRenderer[UIcount].GetComponent<Text>().enabled = false;
        }
        Frame.SetActive(false);
        UIMenu01 = GameObject.FindGameObjectWithTag("UIMenuActive03");
        MenuStartFlagCount = 0;
    }

    public void OnRescue()
    {
        /*アイテム１*/
        Debug.Log("アイテム1");
    }

    public void OnAttack()
    {
        /*アイテム2*/
        Debug.Log("アイテム2");
    }
    public void OnMove()
    {
        /*アイテム3*/
        Debug.Log("アイテム3");
    }

    public void OnTool()
    {
        /*アイテムⅣ*/
        Debug.Log("アイテムⅣ");
    }
    public void OnEnd()
    {
        /*10アイテム5*/
        Debug.Log("アイテム5");
    }
}
