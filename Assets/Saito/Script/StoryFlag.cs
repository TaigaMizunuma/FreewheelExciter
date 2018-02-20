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
    public int[] scnarioBattleStartNum;

    //終了場所
    public int[] scnarioEndNum;

    //ターンのカウント
    [HideInInspector]
    public int s_turnCount;

    //どのシナリオを読み終えたか
    public int[] battleStoryCount;

    [Space(10)]

    //バトル中に会話を始めるキャラの名前
    //会話をするキャラの名前を入れる
    public string s_bossName;

    public string  s_playerName;

    public GameObject g_enemyObj;

    //ストーリーのCSVリーダー
    StoryCSVReader s_reader;

    //上の配列に使用する数
    [HideInInspector]
    public int i_storyNum;

    void Start ()
    {
        s_reader = GetComponent<StoryCSVReader>();
    }	

    /// <summary>
    /// ストーリーの最初と最後の番号を合わせる
    /// </summary>
    public void StoryNumCheck()
    {
        s_reader.SetReadStartNum(scnarioBattleStartNum[i_storyNum]);
        s_reader.SetReadEndNum(scnarioEndNum[i_storyNum]);
    }

    public void StoryTurn()
    {
        if (s_turnCount != FindObjectOfType<SituationTexts>().GetTurn())
        {
            s_turnCount = FindObjectOfType<SituationTexts>().GetTurn();
        }
    }

    public void SetCharacter(string b_name, string p_name, GameObject eneObj)
    {
        s_bossName = b_name;
        s_playerName = p_name;
        g_enemyObj = eneObj;
    }
}
