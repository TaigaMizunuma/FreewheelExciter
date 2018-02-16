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

    //接敵した敵の名前
    [SerializeField]
    GameObject enemyName;

    void Start()
    {
        playerName = this.gameObject.transform.name;
    }

    void Update()
    {

    }

    /// <summary>
    /// 接敵時バトル中の会話フラグ管理クラスに情報を送る
    /// </summary>
    void SendStoryFlag()
    {
        if (storyEnemy == true && nearEnemy == true)
        {
            FindObjectOfType<StoryFlag>().SetCharacterName(playerName, playerName);
        }
    }

    void PlayerStoryPlay()
    {

    }

    /// <summary>
    /// ボスの名前を入れる
    /// </summary>
    /// <param name="enemy"></param>
    public void SetEnemyName(GameObject eneObj)
    {
        enemyName = eneObj;
    }
}
