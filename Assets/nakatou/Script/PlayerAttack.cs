using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private int MinCost = 1;
    private int MaxCost = 1;
    GameObject Now_pos;
    List<GameObject> attack_range = new List<GameObject>();
    List<GameObject> judged_range = new List<GameObject>();
    public bool range_line = false;
    // Use this for initialization
    void Start()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position - new Vector3(0, -0.5f, 0), 0.2f, Vector3.down, out hit, 5.0f))
        {
            Now_pos = hit.transform.gameObject;
            Now_pos.GetComponent<Square_Info>().SetChara(gameObject);
        }
        Character c = gameObject.GetComponent<Character>();
        MinCost = c._range[0];
        MaxCost = c._range[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            RangeSearch();   
        }
    }

    public List<GameObject> GetAttackRange()
    {
        return attack_range;
    }

    public void RangeSearch()
    {
        Retrieval();
        RetrievalRelease();
    }

    public void Retrieval()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position - new Vector3(0, -0.5f, 0), 0.2f, Vector3.down, out hit, 5.0f))
        {
            Now_pos = hit.transform.gameObject;
            Now_pos.GetComponent<Square_Info>().SetChara(gameObject);
        }
        attack_range.Clear();
        Square_Info si_;
        si_ = Now_pos.GetComponent<Square_Info>();
        GameObject[] nears_ = si_.GetNear();
        for (int i=0;i<nears_.Length;i++)
        {
            if (nears_[i] == null) continue;
            if (nears_[i].GetComponent<Square_Info>().GetChara() != null)
            {
                if (nears_[i].GetComponent<Square_Info>().GetChara().tag == "Player") continue;
            }
            Square_Info n_si_ = nears_[i].GetComponent<Square_Info>();
            if (1 <= MaxCost)
            {
                n_si_.AttackRange();
                attack_range.Add(n_si_.gameObject);
            }
            if (1 < MaxCost)
            {
                if(!range_line)Retrieval(nears_[i], 2);
                else
                {
                    Retrieval(nears_[i], 2, i);
                }
            }
        }
    }

    void Retrieval(GameObject near, int cost_)
    {
        Square_Info si_;
        si_ = near.GetComponent<Square_Info>();
        GameObject[] nears_ = si_.GetNear();
        foreach (GameObject n in nears_)
        {
            if (n.GetComponent<Square_Info>().GetChara() != null)
            {
                if (n.GetComponent<Square_Info>().GetChara().tag == "Player") continue;
            }
            Square_Info n_si_ = n.GetComponent<Square_Info>();

            if (cost_ <= MaxCost)
            {
                n_si_.AttackRange();
                attack_range.Add(n_si_.gameObject);
            }
            if (cost_ < MaxCost) Retrieval(n, cost_ + 1);
        }
    }

    void Retrieval(GameObject near, int cost_,int num)
    {
        Square_Info si_;
        si_ = near.GetComponent<Square_Info>();
        GameObject near_ = si_.GetNear()[num];
        if (near_.GetComponent<Square_Info>().GetChara() != null)
        {
            if (near_.GetComponent<Square_Info>().GetChara().tag == "Player") return;
        }
        Square_Info n_si_ = near_.GetComponent<Square_Info>();

        if (cost_ <= MaxCost)
        {
            n_si_.AttackRange();
            attack_range.Add(n_si_.gameObject);
        }
        if (cost_ < MaxCost) Retrieval(near_, cost_ + 1,num);
    }

    void RetrievalRelease()
    {
        Square_Info si_;
        si_ = Now_pos.GetComponent<Square_Info>();
        GameObject[] nears_ = si_.GetNear();
        foreach (GameObject n in nears_)
        {
            if (n.GetComponent<Square_Info>().GetChara() != null)
            {
                if (n.GetComponent<Square_Info>().GetChara().tag == "Player") continue;
            }
            Square_Info n_si_ = n.GetComponent<Square_Info>();
            if (1 < MinCost)
            {
                n_si_.DecisionEnd();
                if (n_si_.GetRange()) attack_range.Remove(n_si_.gameObject);
                RetrievalRelease(n, 2);
            }
        }
    }

    void RetrievalRelease(GameObject near, int cost_)
    {
        Square_Info si_;
        si_ = near.GetComponent<Square_Info>();
        GameObject[] nears_ = si_.GetNear();
        foreach (GameObject n in nears_)
        {
            if (n.GetComponent<Square_Info>().GetChara() != null)
            {
                if (n.GetComponent<Square_Info>().GetChara().tag == "Player") continue;
            }
            Square_Info n_si_ = n.GetComponent<Square_Info>();

            if (cost_ < MinCost)
            {
                n_si_.DecisionEnd();
                if (n_si_.GetRange()) attack_range.Remove(n_si_.gameObject);
                RetrievalRelease(n, cost_ + 1);
            }
        }
    }
}
