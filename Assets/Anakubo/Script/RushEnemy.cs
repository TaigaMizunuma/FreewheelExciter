using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEnemy : MonoBehaviour {
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
        List<GameObject> p_ = new List<GameObject>();
        for(int i = 0; i < players_.Length; i++)
        {
            p_.Add(players_[i]);
        }
        for(int i = 0; i < p_.Count; i++)
        {
            for(int j = 0; j < p_.Count - 1; j++)
            {
                if(p_[j].GetComponent<Square_Info>().GetRushCost() > p_[j + 1].GetComponent<Square_Info>().GetRushCost())
                {
                    GameObject obj = p_[j];
                    p_[j] = p_[j + 1];
                    p_[j + 1] = obj;
                }
            }
        }
        //target_ = null;
        //foreach (GameObject p in players_)
        //{
        //    GameObject p_pos = p.GetComponent<Move_System>().GetNowPos();
        //    if (target_ == null || target_.GetComponent<Square_Info>().GetRushCost() < p_pos.GetComponent<Square_Info>().GetRushCost())
        //    {
        //        target_ = p_pos;
        //    }
        //}
        
        GameObject target_pos = null;
        //foreach (GameObject t in target_.GetComponent<Square_Info>().GetNear())
        //{
        //    if (t.GetComponent<Square_Info>().GetChara() != null || t.GetComponent<Square_Info>().GetCost()>GetComponent<EnemyBase>().first_cost) continue;
        //    if (target_pos == null) target_pos = t;
        //    else if (t.GetComponent<Square_Info>().GetChara() == null)
        //    {
        //        if (target_pos.GetComponent<Square_Info>().GetMaxCost() < t.GetComponent<Square_Info>().GetMaxCost())
        //        {
        //            target_pos = t;
        //        }
        //    }
        //}
        for(int i = 0; i < p_.Count; i++)
        {
            foreach(GameObject t in p_[i].GetComponent<Square_Info>().GetNear())
            {
                if (t.GetComponent<Square_Info>().GetChara() != null || t.GetComponent<Square_Info>().GetCost() > GetComponent<EnemyBase>().first_cost) continue;
                if (target_pos == null) target_pos = t;
                else
                {
                    if (target_pos.GetComponent<Square_Info>().GetMaxCost() < t.GetComponent<Square_Info>().GetMaxCost())
                    {
                        target_pos = t;
                    }
                }
            }
            if (target_pos != null) break;
        }
        players_ = null;
        return target_pos;
    }
}
