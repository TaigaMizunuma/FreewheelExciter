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
    private Text SaveDataNameText;
    private Text SaveDataNameText1;
    /// <summary>
    /// セーブデータ:累計タイム
    /// </summary>
    private Text SaveDataTimeText;
    private Text SaveDataTimeText1;
    /// <summary>
    /// セーブデータ:ターン数
    /// </summary>
    private Text SaveDataTurnText;
    private Text SaveDataTurnText1;
    /// <summary>
    /// セーブデータ:勝利条件
    /// </summary>
    private Text SaveDataConditionsText;
    private Text SaveDataConditionsText1;
    /// <summary>
    /// セーブデータ:写真
    /// </summary>
    private Image SaveDataPicture;
    private Image SaveDataPicture1;
    /// <summary>
    /// 章番号
    /// </summary>
    private int[] SaveSlotNumber = { 1, 2};
    /// <summary>
    /// 章名前
    /// </summary>
    private string[] SaveSlotPurpose = {"赤光 白熱","殺人機械"};
    /// <summary>
    /// 章時間
    /// </summary>
    private float[] SaveSlotTimes = {100.0f,400.0f};
    private float SaveRenderTimes;
    /// <summary>
    /// 章累計ターン数
    /// </summary>
    private int[] SaveSlotTurnString = {1,1};
    //デバッグ用
    private int m_Debagint;
    private int m_Debagint2;
    private int m_Debagint3;
    private Text SaveRuikeiString;
    // Use this for initialization
    void Start()
    {
        //デバッグ
        SavaDataGetTexts();
        m_Debagint2 += SaveData.GetInt("Debag_int");
        SaveRuikeiString.text = "" + m_Debagint2;


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            m_Debagint2++;
            SaveRuikeiString.text = "" + m_Debagint2;
            SaveData.SetInt("Debag_int", m_Debagint2);
        }
    }
    /// <summary>
    /// TextGetComponent
    /// </summary>
    private void SavaDataGetTexts()
    {
        /////スロット1
        SaveDataConditionsText = GameObject.Find("SaveDataConditionsText").GetComponent<Text>();
        SaveDataTimeText = GameObject.Find("SaveDataTimeText").GetComponent<Text>();
        SaveDataTurnText = GameObject.Find("SaveDataTurnText").GetComponent<Text>();
        SaveDataNameText = GameObject.Find("SaveDataNameText").GetComponent<Text>();
        /////スロット2
        SaveDataConditionsText1 = GameObject.Find("SaveDataConditionsText2").GetComponent<Text>();
        SaveDataTimeText1 = GameObject.Find("SaveDataTimeText2").GetComponent<Text>();
        SaveDataTurnText1 = GameObject.Find("SaveDataTurnText2").GetComponent<Text>();
        SaveDataNameText1 = GameObject.Find("SaveDataNameText2").GetComponent<Text>();
        SaveRuikeiString = GameObject.Find("SaveRuikeiString").GetComponent<Text>();
    }

    public void GetSaveButton1()
    {
        for (int i = 0; i < 2; i++)
        {
            Debug.Log("セーブしました");
            //数値の保存
            SaveData.SetInt("" + SaveSlotNumber[i], SaveSlotNumber[i]);
            SaveData.SetString("" + SaveSlotPurpose[i], SaveSlotPurpose[i]);
            SaveData.SetFloat("" + SaveSlotTimes[i], SaveSlotTimes[i]);
            SaveData.SetInt("" + SaveSlotTurnString[i], SaveSlotTurnString[i]);

        }
        /*01タイトル*/
        SaveDataConditionsText.text = "第" + SaveData.GetInt("" + SaveSlotNumber[0]) + 
                                      "章 " + SaveData.GetString("" + SaveSlotPurpose[0]);
        /*02プレイ時間*/
        SaveRenderTimes = SaveData.GetFloat("" + SaveSlotTimes[0]);
        SaveDataTimeText.text = ("プレイ時間 " + string.Format("{1:00}:{2:00}",
                                             Mathf.Floor(SaveRenderTimes / 3600f),
                                             Mathf.Floor(SaveRenderTimes / 60f),
                                             Mathf.Floor(SaveRenderTimes % 60f),
                                             SaveRenderTimes % 1 * 99));
        /*03累計ターン数*/
        SaveDataTurnText.text = "累計ターン数 " + SaveData.GetInt("" +SaveSlotTurnString[0]);
        /*04キャラクターの名前*/
        SaveDataNameText.text = "ヒュー";
        Debug.Log("スロット1 セーブしました");
    }
    public void GetSaveButton2()
    {
        /*01タイトル*/
        SaveDataConditionsText1.text = "第" + SaveData.GetInt("" + SaveSlotNumber[1]) +
                                      "章 " + SaveData.GetString("" + SaveSlotPurpose[1]);

        /*02プレイ時間*/
        SaveRenderTimes = SaveData.GetFloat("" + SaveSlotTimes[1]);
        SaveDataTimeText1.text = ("プレイ時間 " + string.Format("{1:00}:{2:00}",
                                             Mathf.Floor(SaveRenderTimes / 3600f),
                                             Mathf.Floor(SaveRenderTimes / 60f),
                                             Mathf.Floor(SaveRenderTimes % 60f),
                                             SaveRenderTimes % 1 * 99));
        /*03累計ターン数*/
        SaveDataTurnText1.text = "累計ターン数 " + SaveData.GetInt("" + SaveSlotTurnString[1]);
        /*04キャラクターの名前*/
        SaveDataNameText1.text = "ヒュー";
        Debug.Log("スロット2 セーブしました");
    }
}

