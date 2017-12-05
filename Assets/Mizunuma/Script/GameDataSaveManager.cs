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
    /// <summary>
    /// セーブデータ:写真
    /// </summary>
    private Image SaveDataPicture;

    // Use this for initialization
    void Start()
    {
        SavaDataGetTexts();
        SaveDataConditionsText.text = "第" + "2" + "章" + " 旅支度";
        SaveDataTimeText.text = "プレイ時間:"+ "08:00";
        SaveDataTurnText.text = "累計ターン数:" + "18";
        SaveDataNameText.text = "ヒュー";
        //Game_turn = SaveData.GetInt("turn");

    }

    // Update is called once per frame
    void Update()
    {
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
        Debug.Log("セーブしました");
    } 
}

