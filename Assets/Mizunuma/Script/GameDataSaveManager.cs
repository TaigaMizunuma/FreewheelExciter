using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GameDataSaveManager : MonoBehaviour
{
    //グローバル変数[テキスト]
    /// <summary>
    /// セーブデータ:主人公の名前
    /// </summary>
    private Text SaveDataNameText1;
    private Text SaveDataNameText2;
    /// <summary>
    /// セーブデータ:累計タイム
    /// </summary>
    private Text SaveDataTimeText1;
    private Text SaveDataTimeText2;
    /// <summary>
    /// セーブデータ:ターン数
    /// </summary>
    private Text SaveDataTurnText1;
    private Text SaveDataTurnText2;
    /// <summary>
    /// セーブデータ:勝利条件
    /// </summary>
    private Text SaveDataConditionsText1;
    private Text SaveDataConditionsText2;
    /// <summary>
    /// セーブデータ:写真
    /// </summary>
    private Image SaveDataPicture1;
    private Image SaveDataPicture2;
    /// <summary>
    /// セーブデータ:NoData
    /// </summary>
    private Text SaveDataNoText1;
    private Text SaveDataNoText2;
    /// <summary>
    /// 章番号
    /// </summary>
    private int[] SaveNumber = { 0,0,0};
    /// <summary>
    /// 章名前
    /// </summary>
    private string[] SaveSlotPurpose = { "","",""};
    /// <summary>
    /// 章時間
    /// </summary>
    private float[] SaveSlotTimes = { 0, 0, 0};
    private float SaveRenderTimes;
    /// <summary>
    /// 章累計ターン数
    /// </summary>
    private int[] SaveSlotTurnString = { 0,0,0};

    /// <summary>
    /// ボタン
    /// </summary>
    private GameObject DummyButton;
    public EventSystem eventSystem;

    /// <summary>
    /// セーブ識別番号
    /// </summary>
    private int SaveSlotNumber = 0;
    // Use this for initialization
    void Start()
    {
        FindObjectOfType<Fade>().SetInFade(true);
        SavaDataGetTexts();
        SetSaveSlot1();
        SetSaveSlot2();
    }
    /// <summary>
    /// TextGetComponent
    /// </summary>
    private void SavaDataGetTexts()
    {
        //スロット1
        SaveDataConditionsText1 = GameObject.Find("SaveDataConditionsText").GetComponent<Text>();
        SaveDataTimeText1 = GameObject.Find("SaveDataTimeText").GetComponent<Text>();
        SaveDataTurnText1 = GameObject.Find("SaveDataTurnText").GetComponent<Text>();
        SaveDataNameText1 = GameObject.Find("SaveDataNameText").GetComponent<Text>();
        SaveDataNoText1 = GameObject.Find("NoDataText").GetComponent<Text>();
        //スロット2
        SaveDataConditionsText2 = GameObject.Find("SaveDataConditionsText2").GetComponent<Text>();
        SaveDataTimeText2 = GameObject.Find("SaveDataTimeText2").GetComponent<Text>();
        SaveDataTurnText2 = GameObject.Find("SaveDataTurnText2").GetComponent<Text>();
        SaveDataNameText2 = GameObject.Find("SaveDataNameText2").GetComponent<Text>();
        SaveDataNoText2 = GameObject.Find("NoDataText2").GetComponent<Text>();
        //ダミーボタン
        DummyButton = GameObject.Find("DummyButton");

    }
    /// <summary>
    /// スロット1
    /// </summary>
    public void GetSaveButton1()
    {
        /*セーブ識別*/
        SaveSlotNumber = 1;
        /*セーブテキスト非表示*/
        SaveDataNoText1.enabled = false;
        StartSave();

        Debug.Log("タイトル読み込み");
        /*01タイトル*/
        SaveDataConditionsText1.text = "第" + SaveNumber[1] + 
                                       "章 " + SaveSlotPurpose[1];
        Debug.Log("プレイ時間読み込み");
        /*02プレイ時間*/
        SaveRenderTimes = SaveSlotTimes[1];
        SaveDataTimeText1.text = ("プレイ時間 " + string.Format("{1:00}:{2:00}",
                                             Mathf.Floor(SaveRenderTimes / 3600f),
                                             Mathf.Floor(SaveRenderTimes / 60f),
                                             Mathf.Floor(SaveRenderTimes % 60f),
                                             SaveRenderTimes % 1 * 99));
        Debug.Log("ターン数読み込み");
        /*03累計ターン数*/
        SaveDataTurnText1.text = "累計ターン数 " + SaveSlotTurnString[1];
        Debug.Log("Characterの名前読み込み");
        /*04キャラクターの名前*/
        SaveDataNameText1.text = "ヒュー";
        SaveDistribute();
        Debug.Log("スロット1 セーブしました");

    }
    public void GetSaveButton2()
    {
        /*セーブ識別*/
        SaveSlotNumber = 2;
        SaveDataNoText2.enabled = false;
        StartSave();
        /*01タイトル*/
        SaveDataConditionsText2.text = "第" + SaveNumber[2] +
                                      "章 " + SaveSlotPurpose[2];

        /*02プレイ時間*/
        SaveRenderTimes = SaveSlotTimes[2];
        SaveDataTimeText2.text = ("プレイ時間 " + string.Format("{1:00}:{2:00}",
                                             Mathf.Floor(SaveRenderTimes / 3600f),
                                             Mathf.Floor(SaveRenderTimes / 60f),
                                             Mathf.Floor(SaveRenderTimes % 60f),
                                             SaveRenderTimes % 1 * 99));
        /*03累計ターン数*/
        SaveDataTurnText2.text = "累計ターン数 " + SaveSlotTurnString[2];
        /*04キャラクターの名前*/
        SaveDataNameText2.text = "ヒュー";
        SaveDistribute();
        Debug.Log("スロット2 セーブしました");

    }

    public void SetSaveSlot1()
    {

        /*データ存在時*/
        if (SaveData.HasKey("SaveNumber1") == true && SaveData.HasKey("SaveSlotPurpose1") == true &&
            SaveData.HasKey("SaveSlotTimes1") == true && SaveData.HasKey("SaveSlotTurnString1") == true)
        {
            /*01タイトル*/
            SaveDataConditionsText1.text = "第" + SaveData.GetInt("SaveNumber1") +
                                           "章 " + SaveData.GetString("SaveSlotPurpose1");
            /*02プレイ時間*/
            SaveRenderTimes = SaveData.GetFloat("SaveSlotTimes1");
            SaveDataTimeText1.text = ("プレイ時間 " + string.Format("{1:00}:{2:00}",
                                                 Mathf.Floor(SaveRenderTimes / 3600f),
                                                 Mathf.Floor(SaveRenderTimes / 60f),
                                                 Mathf.Floor(SaveRenderTimes % 60f),
                                                 SaveRenderTimes % 1 * 99));
            /*03累計ターン数*/
            SaveDataTurnText1.text = "累計ターン数" + SaveData.GetInt("SaveSlotTurnString1");
            /*04キャラクターの名前*/
            SaveDataNameText1.text = "ヒュー";
            if (SaveDataNoText1.enabled == true)
            {
                SaveDataNoText1.enabled = false;
            }
        }
    }
    public void SetSaveSlot2()
    {

        if (SaveData.HasKey("SaveNumber2") == true && SaveData.HasKey("SaveSlotPurpose2") == true &&
            SaveData.HasKey("SaveSlotTimes2") == true && SaveData.HasKey("SaveSlotTurnString2") == true)
        {
            /*01タイトル*/
            SaveDataConditionsText2.text = "第" + SaveData.GetInt("SaveNumber2") +
                                           "章 " + SaveData.GetString("SaveSlotPurpose2");
            /*02プレイ時間*/
            SaveRenderTimes = SaveData.GetFloat("SaveSlotTimes2");
            SaveDataTimeText2.text = ("プレイ時間 " + string.Format("{1:00}:{2:00}",
                                                 Mathf.Floor(SaveRenderTimes / 3600f),
                                                 Mathf.Floor(SaveRenderTimes / 60f),
                                                 Mathf.Floor(SaveRenderTimes % 60f),
                                                 SaveRenderTimes % 1 * 99));
            /*03累計ターン数*/
            SaveDataTurnText2.text = "累計ターン数" + SaveData.GetInt("SaveSlotTurnString2");
            /*04キャラクターの名前*/
            SaveDataNameText2.text = "ヒュー";
            if (SaveDataNoText2.enabled == true)
            {
                SaveDataNoText2.enabled = false;
            }
        }
    }

    public void SetFadeOut()
    {
        FindObjectOfType<Fade>().SetOutFade(true);
        FindObjectOfType<Fade>().SetSceneChangeSwitch(true);
        FindObjectOfType<Fade>().SetScene("Title");

    }
    /*ボタンが押された後に保存*/
    private void SaveDistribute()
    {
        /*ダミーボタンセット*/
        eventSystem.SetSelectedGameObject(DummyButton);
        /*数値の保存*/
        /*章番号*/
        SaveData.SetInt("SaveNumber" + SaveSlotNumber, SaveNumber[SaveSlotNumber]);
        /*章の名前*/
        SaveData.SetString("SaveSlotPurpose" + SaveSlotNumber, SaveSlotPurpose[SaveSlotNumber]);
        /*章の時間*/
        SaveData.SetFloat("SaveSlotTimes" + SaveSlotNumber, SaveSlotTimes[SaveSlotNumber]);
        /*章のターン*/
        SaveData.SetInt("SaveSlotTurnString" + SaveSlotNumber, SaveSlotTurnString[SaveSlotNumber]);
        SaveData.Save();
        /*遅延実行*/
        Invoke("SetFadeOut", 1.5f);
        Debug.Log("保存");
    }
    /// <summary>
    /// ゲームから要素をロードします
    /// </summary>
    private void StartSave()
    {
        /*セーブの割り当て*/
        /*ターン*/
        SaveSlotTurnString[SaveSlotNumber] = SaveData.GetInt("GameDataTurn");
        /*タイム*/
        SaveSlotTimes[SaveSlotNumber] = SaveData.GetFloat("GameDataTime");
        /*章タイトル*/
        SaveSlotPurpose[SaveSlotNumber] = SaveData.GetString("GameDataTitle");
        /*章番号*/
        SaveNumber[SaveSlotNumber] = SaveData.GetInt("GameDataNumber");
        Debug.Log("要素取得");

    }
}

