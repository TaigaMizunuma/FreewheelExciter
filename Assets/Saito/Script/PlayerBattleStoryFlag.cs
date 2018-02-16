using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleStoryFlag : MonoBehaviour {

    //ストーリーに関わる敵か
    bool storyEnemy;

    //接敵したか
    bool nearEnemy;

    //自分(敵キャラ)の名前
    string playerName;

    //接敵した敵
    [SerializeField]
    GameObject enemyObject;

    //接敵した敵の名前
    string enemyName;

    bool eneFlag;

    StoryFlag s_flag;

    void Start()
    {
        s_flag = FindObjectOfType<StoryFlag>();
        playerName = this.gameObject.GetComponent<Character>()._name;
    }

    void Update()
    {
        if(enemyObject != null)
        {
            EnemyCheck();
        }
    }

    /// <summary>
    /// 接敵時バトル中の会話フラグ管理クラスに情報を送る
    /// </summary>
    void EnemyCheck()
    {
        if (enemyObject != null)
        {
            eneFlag = enemyObject.GetComponent<EnemyPersonalCSV>().GetStoryFlag();

            if (eneFlag == true)
            {
                enemyName = enemyObject.GetComponent<Character>()._name;
                s_flag.SetCharacterName(enemyName, playerName);
            }
        }
    }
    /// <summary>
    /// ボスの名前を入れる
    /// </summary>
    /// <param name="enemy"></param>
    public void SetEnemyName(GameObject eneObj)
    {
        enemyObject = eneObj;
    }
}
