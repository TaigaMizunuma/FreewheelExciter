using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkEventChecker : MonoBehaviour
{
    public bool BattleEvent = false;

    public bool DeadEvent = false;

    public bool TurnEvent = false;

    //Battleイベント起動キャラ
    public GameObject BattleChara;

    //現在戦闘中のキャラ
    public GameObject NowBattleChara;

    //ターン数イベントの起動ターン
    public int TurnNum = 0;

    //再生するストーリーナンバー
    public int BattleStoryID = 0;

    //再生するストーリーナンバー
    public int DeadStoryID = 0;

    //再生するストーリーナンバー
    public int TurnStoryID = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(BattleEvent && NowBattleChara != null)
        {
            if(BattleChara.GetComponent<Character>()._name == NowBattleChara.GetComponent<Character>()._name)
            {
                ///会話開始
                Debug.Log("BattleEvent");
            }
        }

        if (TurnEvent)
        {
            if (FindObjectOfType<SituationTexts>().GetTurn() == TurnNum)
            {
                ///会話開
                Debug.Log("TurnEvent");
            }
        }

        if(DeadEvent)
        {
            if (GetComponent<Character>()._totalhp <= 0) 
            {
                ///会話開始
                Debug.Log("DeadEvent");

                FindObjectOfType<StoryCSVReader>().battleScenarioSwitch = true;
                FindObjectOfType<StoryCSVReader>().SetReadStartNum(DeadStoryID);
                FindObjectOfType<StoryCSVReader>().SetReadEndNum(DeadStoryID + 1);
                
            }
        }
    }
}
