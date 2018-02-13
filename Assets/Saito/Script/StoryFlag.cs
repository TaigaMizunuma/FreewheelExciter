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
    int[] scnarioStartNum;

    //終了場所
    int[] scnarioEndNum;
    //ターンのカウント
    int s_turnCount;

    //バトル中に会話を始めるキャラの名前
    //近接時に会話をするキャラの名前を入れる
    string s_bossName;
    string s_playerName;

    //ストーリーのCSVリーダー
    StoryCSVReader s_reader;

	void Start ()
    {
        s_reader = FindObjectOfType<StoryCSVReader>();
	}
	
	void Update ()
    {
        StoryNumCheck();
        StoryTurn();
    }

    /// <summary>
    /// ストーリーの最初と最後の番号を合わせる
    /// </summary>
    public void StoryNumCheck()
    {
        s_reader.SetReadStartNum(scnarioStartNum[scenarioNum]);
        s_reader.SetReadEndNum(scnarioEndNum[scenarioNum]);
    }

    /// <summary>
    /// キャラクターの接近時会話判定
    /// </summary>
    void StoryCharacterCheck()
    {

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
