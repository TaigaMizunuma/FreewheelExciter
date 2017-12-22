using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SituationTexts : MonoBehaviour
{
    /*各テキストの取得*/
    private Text StoryStringText;
    private Text VictoryCondition_02_1;
    private Text VictoryCondition_02;
    private Text EnemyCounterText;
    private Text PlayerCounterText;
    private Text TurnText_05;
    /*Player数Enemy数取得*/
    private GameObject[] EnemyObjects;
    private GameObject[] PlayerObjects;
    public GameObject MapFrame;
    public GameObject ReturnButton;
    public GameObject MenuButton;
    public EventSystem eventSystem;
    private int UIcount = 0;
    private int TurnCount = 1;
    /// <summary>
    /// セーブデータ:時間
    /// </summary>
    private float SaveGameTime;
    private Text PlayTimeText;

    void Start()
    {
        /*テキストの取得*/
        /*第何章などストーリー系*/
        StoryStringText = GameObject.Find("StoryString_01").GetComponent<Text>();
        VictoryCondition_02_1 = GameObject.Find("VictoryCondition_02_1").GetComponent<Text>();
        /*勝利条件系*/
        VictoryCondition_02 = GameObject.Find("VictoryCondition_02").GetComponent<Text>();
        /*エネミー数*/
        EnemyCounterText = GameObject.Find("EnemyNumber_03").GetComponent<Text>();
        /*プレイヤー数*/
        PlayerCounterText = GameObject.Find("PlayerNumber_04").GetComponent<Text>();
        TurnText_05 = GameObject.Find("TurnText_05").GetComponent<Text>();
        PlayTimeText = GameObject.Find("PlayTimeText").GetComponent<Text>();

        TurnCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        SaveGameTime += Time.deltaTime;
        PlayTimeText.text = ("TIME"+string.Format("{1:00}:{2:00}",
            Mathf.Floor(SaveGameTime / 3600f),
            Mathf.Floor(SaveGameTime / 60f),
            Mathf.Floor(SaveGameTime % 60f),
            SaveGameTime % 1 * 99));

        SituationTextUpdate();
        switch (UIcount)
        {
            /*ケース1を飛ばしてケース3が実行されるため、ケース2を挟んだ*/
            /*ケース1 メニューボタンロックし、UI表示カウントアップ*/
            case 1:

                FindObjectOfType<MenuManager>().SetMainControlFlag(true);
                SituationTrue();
                eventSystem.SetSelectedGameObject(ReturnButton);
                break;
            /*ケース2 UI非表示 メニューボタンロック解除 メニュー戻れないようにする解除*/
            case 2:
                SituationFalse();
                FindObjectOfType<MenuManager>().SetMainControlFlag(false);
                eventSystem.SetSelectedGameObject(MenuButton);
                UIcount = 0;
                break;
        }

    }
    private void SituationTrue()
    {
        /*メニュー表示*/
        MapFrame.SetActive(true);
        EnemyCounterText.enabled = true;
        PlayerCounterText.enabled = true;
        StoryStringText.enabled = true;
        VictoryCondition_02_1.enabled = true;
        VictoryCondition_02.enabled = true;
        TurnText_05.enabled = true;
        ReturnButton.GetComponent<Text>().enabled = true;
    }
    private void SituationFalse()
    {
        /*メニュー非表示*/
        MapFrame.SetActive(false);
        EnemyCounterText.enabled = false;
        PlayerCounterText.enabled = false;
        StoryStringText.enabled = false;
        VictoryCondition_02_1.enabled = false;
        VictoryCondition_02.enabled = false;
        TurnText_05.enabled = false;
        ReturnButton.GetComponent<Text>().enabled = false;
    }
    private void SituationTextUpdate()
    {
        /*章番号*/
        /*ストーリー番号 章タイトルを記入*/
        StoryStringText.text = "第" + FindObjectOfType<StoryCSVReader>().GetStoryNumber() + 
                               "章" + FindObjectOfType<StoryCSVReader>().GetStoryTitle();

        /*テキスト書き換え 敵の数関係取得*/
        EnemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        EnemyCounterText.text = "敵軍" + EnemyObjects.Length + "体";
        PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
        PlayerCounterText.text = "自軍" + PlayerObjects.Length + "体";

        TurnText_05.text = "ターン数: " + TurnCount;

        

    }
    public void SituationCount()
    {
        UIcount++;
    }

    public void TurnCountUp()
    {
        TurnCount++;
    }

    public int GetTurn()
    {
        return TurnCount;
    }

    public float GetTime()
    {
        return SaveGameTime;
    }

}

