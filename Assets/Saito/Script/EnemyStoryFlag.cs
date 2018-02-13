//エネミーからストーリーフラグへ情報を送る
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStoryFlag : MonoBehaviour {

    //ストーリーに関わる敵か
    bool storyEnemy;

    //接敵したか
    bool nearPlayer;

    //自分(敵キャラ)の名前
    string enemyName;

    //接敵したプレイヤーの名前
    string playerName;

	void Start () {
        enemyName = this.gameObject.transform.name;
        storyEnemy = this.GetComponent<EnemyPersonalCSV>().GetStoryFlag();
	}
	
	void Update () {
		
	}

    /// <summary>
    /// 接敵時バトル中の会話フラグ管理クラスに情報を送る
    /// </summary>
    void SendStoryFlag()
    {
        if (storyEnemy == true && nearPlayer == true)
        {
            FindObjectOfType<StoryFlag>().SetCharacterName(enemyName, playerName);
        }
    }
}
