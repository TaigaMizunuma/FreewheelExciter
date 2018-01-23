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
    public EventSystem eventSystem;

    /// <summary>
    /// セーブ識別番号
    /// </summary>
    private int SaveSlotNumber = 0;

    /// <summary>
    /// 初回セーブ時に使用
    /// </summary>
    private int SaveSloatInitNamber = 0;
    private int SaveSloatInitNamber2 = 0;
    /// <summary>
    /// Animation
    /// </summary>
    private Animation m_Animation;
    private Animation m_Animation2;
    private Animation m_Animation3;

    /// <summary>
    /// 
    /// </summary>
    private Image SavePlayerPicture1;
    private Image SavePlayerPicture2;

    /// <summary>
    /// false = 削除モードオフ true = 削除モードオン
    /// </summary>
    private bool SaveDataDeleteFlag = false;
    private int D_Count;

    // Use this for initialization
    void Start()
    {
        FindObjectOfType<Fade>().SetInFade(true);
        SavaDataGetTexts();
        SetSaveSlot1();
        SetSaveSlot2();
        m_Animation = GameObject.Find("SaveDatas[1]").GetComponent<Animation>();
        m_Animation2 = GameObject.Find("SaveDatas[2]").GetComponent<Animation>();
        m_Animation3 = GameObject.Find("DeleteText").GetComponent<Animation>();
        SavePlayerPicture1 = GameObject.Find("SavaDataPlayerPicture").GetComponent<Image>();
        SavePlayerPicture2 = GameObject.Find("SavaDataPlayerPicture2").GetComponent<Image>();

        if (SaveSloatInitNamber == 1 || SaveData.HasKey("SaveNumber1") == true)
        {
            SavePlayerPicture1.enabled = true;
        }
        if (SaveSloatInitNamber2 == 1|| SaveData.HasKey("SaveNumber2") == true)
        {
            SavePlayerPicture2.enabled = true;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            DataClear();
        }   
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

    }
    /// <summary>
    /// スロット1
    /// </summary>
    public void GetSaveButton1()
    {
        SaveSlotNumber = 1;
        m_Animation.Play();
        StartCoroutine(DelayMethod.DelayMethodCall(0.25f, () =>
        {   
            /*セーブテキスト非表示*/
            SaveDataNoText1.enabled = false;
            /*セーブモード切替 false=新規作成モード true=セーブモード*/
            if (SaveData.HasKey("GameSaveCheck") == false)
            {
                if (SaveSloatInitNamber == 0 && SaveDataDeleteFlag == false)
                {
                    Debug.Log("01_新規作成モード");
                    /*新規作成モード*/
                    /*01タイトル*/
                    SaveDataConditionsText1.text = "第" + "1" +
                                                   "章 " + "赤光 白熱";
                    Debug.Log("プレイ時間読み込み");
                    /*02プレイ時間*/
                    SaveDataTimeText1.text = ("プレイ時間 " + "00:00");
                    Debug.Log("ターン数読み込み");
                    /*03累計ターン数*/
                    SaveDataTurnText1.text = "累計ターン数 " + "0";
                    Debug.Log("Characterの名前読み込み");
                    /*04キャラクターの名前*/
                    SaveDataNameText1.text = "ヒュー";
                    //初回用セーブ
                    SaveNumber[1] = 1;
                    SaveSlotPurpose[1] = "赤光 白熱";
                    SavePlayerPicture1.enabled = true;
                    SaveDistribute();
                }
                else if(SaveSloatInitNamber == 1 && SaveDataDeleteFlag == false)
                {
                    Debug.Log("02_ロードモード");
                    /*遅延実行*/
                    Invoke("SetFadeOut", 1.5f);
                    eventSystem.sendNavigationEvents = false;
                }
                else if(SaveDataDeleteFlag == true)
                {
                    Debug.Log("04_データの削除をしました");
                    /*データ削除*/
                    SaveData.Remove("SaveNumber1");
                    SaveData.Remove("SaveSlotPurpose1");
                    SaveData.Remove("SaveSlotTimes1");
                    SaveData.Remove("SaveSlotTurnString1");
                    /*セーブテキスト非表示*/
                    SaveDataNoText1.enabled = true;
                    SaveDataNameText1.enabled = false;
                    SaveDataTimeText1.enabled = false;
                    SaveDataTurnText1.enabled = false;
                    SaveDataConditionsText1.enabled = false;
                    SavePlayerPicture1.enabled = false;
                    Invoke("SetSaveOut", 1.5f);
                    eventSystem.sendNavigationEvents = false;


                }
            }
            /*セーブモード*/
            if (SaveData.HasKey("GameSaveCheck") == true && SaveDataDeleteFlag == false)
            {
                Debug.Log("03_セーブモード");
                /*上書きモード*/
                if (SaveData.HasKey("SaveNumber1") == true && SaveData.HasKey("SaveSlotPurpose1") == true &&
                SaveData.HasKey("SaveSlotTimes1") == true && SaveData.HasKey("SaveSlotTurnString1") == true ||
                SaveData.HasKey("SaveNumber2") == true && SaveData.HasKey("SaveSlotPurpose2") == true &&
                SaveData.HasKey("SaveSlotTimes2") == true && SaveData.HasKey("SaveSlotTurnString2") == true)
                {
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
                else
                {
                    Debug.Log("エラー番号01:セーブデータがありません");
                }
            }
        }));
    }
    public void GetSaveButton2()
    {
        /*セーブ識別*/
        SaveSlotNumber = 2;
        //初回時
        m_Animation2.Play();
        StartCoroutine(DelayMethod.DelayMethodCall(0.25f, () =>
        {
            /*セーブモード切替 false=新規作成モード true=セーブモード*/
            if (SaveData.HasKey("GameSaveCheck") == false)
            {

                if (SaveSloatInitNamber2 == 0 && SaveDataDeleteFlag == false)
                {
                    Debug.Log("01_新規作成モード");
                    SaveDataNoText2.enabled = false;
                    /*01タイトル*/
                    SaveDataConditionsText2.text = "第" + "1" +
                                                   "章 " + "赤光 白熱";
                    Debug.Log("プレイ時間読み込み");
                    /*02プレイ時間*/
                    SaveDataTimeText2.text = ("プレイ時間 " + "00:00");
                    Debug.Log("ターン数読み込み");
                    /*03累計ターン数*/
                    SaveDataTurnText2.text = "累計ターン数 " + "0";
                    Debug.Log("Characterの名前読み込み");
                    /*04キャラクターの名前*/
                    SaveDataNameText2.text = "ヒュー";
                    //初回用セーブ
                    SaveNumber[2] = 1;
                    SaveSlotPurpose[2] = "赤光 白熱";
                    SavePlayerPicture2.enabled = true;
                    SaveDistribute();
                }
                else if(SaveSloatInitNamber2 == 1 && SaveDataDeleteFlag == false)
                {
                    Debug.Log("02_ロードモード");
                    /*ダミーボタンセット*/
                    eventSystem.sendNavigationEvents = false;
                    /*遅延実行*/
                    Invoke("SetFadeOut", 1.5f);
                }
                else if (SaveDataDeleteFlag == true)
                {
                    Debug.Log("04_データの削除をしました");
                    Invoke("SetSaveOut", 1.5f);
                    eventSystem.sendNavigationEvents = false;
                    SaveData.Remove("SaveNumber2");
                    SaveData.Remove("SaveSlotPurpose2");
                    SaveData.Remove("SaveSlotTimes2");
                    SaveData.Remove("SaveSlotTurnString2");
                    Debug.Log("04_データの削除をしました");
                    /*セーブテキスト非表示*/
                    SaveDataNoText2.enabled = true;
                    SaveDataNameText2.enabled = false;
                    SaveDataTimeText2.enabled = false;
                    SaveDataTurnText2.enabled = false;
                    SaveDataConditionsText2.enabled = false;
                    SavePlayerPicture2.enabled = false;
                    Invoke("SetSaveOut", 1.5f);
                    eventSystem.sendNavigationEvents = false;

                }
            }
            if (SaveData.HasKey("GameSaveCheck") == true && SaveDataDeleteFlag == false)
            {
                if (SaveData.HasKey("SaveNumber2") == true && SaveData.HasKey("SaveSlotPurpose2") == true &&
                SaveData.HasKey("SaveSlotTimes2") == true && SaveData.HasKey("SaveSlotTurnString2") == true ||
                SaveData.HasKey("SaveNumber1") == true && SaveData.HasKey("SaveSlotPurpose1") == true &&
                SaveData.HasKey("SaveSlotTimes1") == true && SaveData.HasKey("SaveSlotTurnString1") == true)
                {
                    Debug.Log("03_セーブモード");
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
                else
                {
                    Debug.Log("エラー番号02:セーブデータがありません");
                }
            }
        }));
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
            SaveDataTurnText1.text = "累計ターン数 " + SaveData.GetInt("SaveSlotTurnString1");
            /*04キャラクターの名前*/
            SaveDataNameText1.text = "ヒュー";
            if (SaveDataNoText1.enabled == true)
            {
                SaveDataNoText1.enabled = false;
            }
            SaveSloatInitNamber = 1;
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
            SaveDataTurnText2.text = "累計ターン数 " + SaveData.GetInt("SaveSlotTurnString2");
            /*04キャラクターの名前*/
            SaveDataNameText2.text = "ヒュー";
            if (SaveDataNoText2.enabled == true)
            {
                SaveDataNoText2.enabled = false;
            }
            SaveSloatInitNamber2 = 1;
        }
    }

    public void SetFadeOut()
    {
        FindObjectOfType<Fade>().SetOutFade(true);
        FindObjectOfType<Fade>().SetSceneChangeSwitch(true);
        FindObjectOfType<Fade>().SetScene("Story");

    }
    public void SetSaveOut()
    {
        FindObjectOfType<Fade>().SetOutFade(true);
        FindObjectOfType<Fade>().SetSceneChangeSwitch(true);
        FindObjectOfType<Fade>().SetScene("SaveData");

    }
    /*ボタンが押された後に保存*/
    private void SaveDistribute()
    {
        /*ダミーボタンセット*/
        eventSystem.sendNavigationEvents = false;
        /*数値の保存*/
        /*章番号*/
        SaveData.SetInt("SaveNumber" + SaveSlotNumber, SaveNumber[SaveSlotNumber]);
        /*章の名前*/
        SaveData.SetString("SaveSlotPurpose" + SaveSlotNumber, SaveSlotPurpose[SaveSlotNumber]);
        /*章の時間*/
        SaveData.SetFloat("SaveSlotTimes" + SaveSlotNumber, SaveSlotTimes[SaveSlotNumber]);
        /*章のターン*/
        SaveData.SetInt("SaveSlotTurnString" + SaveSlotNumber, SaveSlotTurnString[SaveSlotNumber]);
        /*全体セーブ*/
        SaveData.Save();
        SaveData.Remove("GameSaveCheck");
        if (SaveData.HasKey("SaveData") == false)
        {
            Debug.Log("セーブキーが削除されました");
        }
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
    private void DataClear()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("クリア");
            SaveData.Clear();
        }
    }
    public void DeleteButton()
    {
        Debug.Log("選択時にセーブデータを削除します。");
        
        if (D_Count == 0)
        {
            Debug.Log("削除モードオン");
            D_Count = 1;
            SaveDataDeleteFlag = true;
            m_Animation3.Play();
            
        }
        else
       {
            Debug.Log("削除モードオフ");
            D_Count = 0;
            SaveDataDeleteFlag = false;
            m_Animation3.Stop();
        }
    }
}

