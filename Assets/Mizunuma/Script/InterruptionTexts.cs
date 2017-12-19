using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InterruptionTexts : MonoBehaviour
{ 
    public GameObject MapFrame;
    /*はい*/
    public GameObject YesButton;
    /*はい*/
    public GameObject NoButton;
    /*メニュー*/
    public GameObject MenuButton;
    /*イベントシステム*/
    public EventSystem eventSystem;
    private int UIcount = 0;

    /*グローバル関数*/
    private Text Titletext;

    void Start()
    {
        Titletext = GameObject.Find("InterruptionText_01").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (UIcount)
        {
            /*ケース1を飛ばしてケース3が実行されるため、ケース2を挟んだ*/
            /*ケース1 メニューボタンロックし、UI表示カウントアップ*/
            case 1:
                FindObjectOfType<MenuManager>().SetMainControlFlag(true);
                InterruptionTrue();
                eventSystem.SetSelectedGameObject(YesButton);
                UIcount++;
                break;
            case 2:
                break;
            /*ケース2 UI非表示 メニューボタンロック解除 メニュー戻れないようにする解除*/
            case 3:
                InterruptionFalse();
                FindObjectOfType<MenuManager>().SetMainControlFlag(false);
                eventSystem.SetSelectedGameObject(MenuButton);
                UIcount = 0;
                break;
        }

    }
    private void InterruptionTrue()
    {
        /*メニュー表示*/
        MapFrame.SetActive(true);
        Titletext.enabled = true;
        YesButton.GetComponent<Text>().enabled = true;
        NoButton.GetComponent<Text>().enabled = true;
    }
    private void InterruptionFalse()
    {
        /*メニュー非表示*/
        MapFrame.SetActive(false);
        Titletext.enabled = false;
        YesButton.GetComponent<Text>().enabled = false;
        NoButton.GetComponent<Text>().enabled = false;
    }
    public void InterruptionCount()
    {
        UIcount++;
    }

    public void YesButtonPushed()
    {
        Debug.Log("セーブしました");

    }
    public void NoButtonPushed()
    {
        Debug.Log("キャンセルしました");
        UIcount++;
    }

}
