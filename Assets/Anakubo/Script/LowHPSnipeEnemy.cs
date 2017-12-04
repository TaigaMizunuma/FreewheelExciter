using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowHPSnipeEnemy : MonoBehaviour {
    private GameObject target_;
    private GameObject[] players_;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<EnemyBase>().IsRemainder())
        {
            GetComponent<EnemyBase>().SetNextGoal(SetTarget());
        }
    }

    public GameObject SetTarget()
    {
        players_ = GameObject.FindGameObjectsWithTag("Player");
        target_ = null;
        GameObject target_player = null;
        foreach (GameObject p in players_)
        {
            if (target_player == null || target_player.GetComponent<Character>()._totalhp > p.GetComponent<Character>()._totalhp)
            {
                target_player = p;
            }
        }
        target_ = target_player.GetComponent<Move_System>().GetNowPos();
        players_ = null;
        return target_;
    }
}
