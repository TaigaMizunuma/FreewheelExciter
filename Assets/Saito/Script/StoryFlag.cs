//戦闘中の会話フラグ制御
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryFlag : MonoBehaviour {

    //何処から読むか
    public int scenarioNum;

    //ゲームの状態
    bool gameOver;
    bool clear;

    //開始場所
    [SerializeField]
    public int scnarioStartNum;

    //終了場所
    [SerializeField]
    int[] scnarioEndNum;
    //ターンのカウント
    int s_turnCount;

    public string[] bossNameList;
    public string[] playerNameList;

    [Space(10)]

    //バトル中に会話を始めるキャラの名前
    //会話をするキャラの名前を入れる
    [SerializeField]
    string s_bossName;
    [SerializeField]
    string s_playerName;

    //ストーリーのCSVリーダー
    StoryCSVReader s_reader;

	void Start ()
    {
        s_reader = FindObjectOfType<StoryCSVReader>();
	}
	
	void Update ()
    {
        StoryCharacterCheck();
    }

    /// <summary>
    /// ストーリーの最初と最後の番号を合わせる
    /// </summary>
    public void StoryNumCheck()
    {
        s_reader.SetReadStartNum(scnarioStartNum);
        s_reader.SetReadEndNum(scnarioEndNum[scenarioNum]);
    }

    /// <summary>
    /// キャラクターの接近時会話判定
    /// </summary>
    void StoryCharacterCheck()
    {
        if (s_bossName == bossNameList[0] && s_playerName == playerNameList[0])
        {
            s_reader.battleScenarioSwitch = true;
            StoryNumCheck();
        }
        else if (s_bossName == null)
        {
            return;
        }
    }

    void StoryTurn()
    {
        if (s_turnCount != FindObjectOfType<SituationTexts>().GetTurn())
        {
            s_turnCount = FindObjectOfType<SituationTexts>().GetTurn();
        }
    }

    public void SetCharacterName(string b_name, string p_name)
    {
        s_bossName = b_name;
        s_playerName = p_name;
    }
}
