using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitEnemy : MonoBehaviour {
    private GameObject target_;
    private GameObject[] players_;
    private bool rush_ = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject SetTarget()
    {
        if(!rush_)return GetComponent<EnemyBase>().GetNowPos();
        players_ = GameObject.FindGameObjectsWithTag("Player");
        target_ = null;
        foreach (GameObject p in players_)
        {
            GameObject p_pos = p.GetComponent<Move_System>().GetNowPos();
            if (target_ == null || target_.GetComponent<Square_Info>().GetRushCost() < p_pos.GetComponent<Square_Info>().GetRushCost())
            {
                target_ = p_pos;
            }
        }
        players_ = null;
        return target_;
    }

    public void RushStart()
    {
        rush_ = true;
    }
}
