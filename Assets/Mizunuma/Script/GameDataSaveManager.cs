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
    /// <summary>
    /// セーブデータ:累計タイム
    /// </summary>
    private Text SaveDataTimeText;
    /// <summary>
    /// セーブデータ:ターン数
    /// </summary>
    private Text SaveDataTurnText;
    /// <summary>
    /// セーブデータ:勝利条件
    /// </summary>
    private Text SaveDataConditionsText;

    // Use this for initialization
    void Start()
    {
        SavaDataGetTexts();
        SaveDataConditionsText.text = "第2章 旅支度";
        SaveDataTimeText.text = "00:04";
        SaveDataTurnText.text = "現在のターン数:07";
        SaveDataNameText.text = "ヒュー";
        //Game_turn = SaveData.GetInt("turn");

    }

    // Update is called once per frame
    void Update()
    {

         //-= Time.deltaTime;
        
        //GetComponent<Text>().text =
        //    (string.Format("{1:00}:{2:00}",
        //    Mathf.Floor(starttimer / 3600f),
        //    Mathf.Floor(starttimer / 60f),
        //    Mathf.Floor(starttimer % 60f),
        //    starttimer % 1 * 99));
        //if (Input.GetKeyDown(KeyCode.P))
        //{
            
        //    //SaveData.SetInt("turn", Game_turn);
        //    //SaveData.Save();
        //}
    }
    /// <summary>
    /// TextGetComponent
    /// </summary>
    private void SavaDataGetTexts()
    {
        SaveDataConditionsText = GameObject.Find("SaveDataConditionsText").GetComponent<Text>();
        SaveDataTimeText = GameObject.Find("SaveDataTimeText").GetComponent<Text>();
        SaveDataTurnText = GameObject.Find("SaveDataTurnText").GetComponent<Text>();
        SaveDataNameText = GameObject.Find("SaveDataNameText").GetComponent<Text>();
    }

    public void GetSaveButton()
    {
        SaveData.GetString("");
    } 
}

