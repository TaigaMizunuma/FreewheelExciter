using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    int Cost;
    public int FirstCost = 3;
    GameObject Now_pos;
    List<GameObject> attack_range = new List<GameObject>();
    // Use this for initialization
    void Start()
    {
        Cost = FirstCost;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position - new Vector3(0, -0.5f, 0), 0.2f, Vector3.down, out hit, 5.0f))
        {
            Now_pos = hit.transform.gameObject;
            Now_pos.GetComponent<Square_Info>().SetChara(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<GameObject> GetAttackRange()
    {
        return attack_range;
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
        si_.Decision();
        GameObject[] nears_ = si_.GetNear();
        foreach (GameObject n in nears_)
        {
            if (n.GetComponent<Square_Info>().GetChara() != null)
            {
                if (n.GetComponent<Square_Info>().GetChara().tag == "Enemy") continue;
            }
            Square_Info n_si_ = n.GetComponent<Square_Info>();
            if (Cost - n_si_.GetCost() >= 0 && n_si_.CanSetCost(Cost, Now_pos))
            {
                attack_range.Add(n_si_.gameObject);
                Retrieval(n, Cost);
            }
        }
    }

    void Retrieval(GameObject near, int cost_)
    {
        Square_Info si_;
        si_ = near.GetComponent<Square_Info>();
        GameObject[] nears_ = si_.GetNear();
        cost_ -= si_.GetCost();
        foreach (GameObject n in nears_)
        {
            //if (n.GetComponent<Square_Info>().GetChara() != null)
            //{
            //    if (n.GetComponent<Square_Info>().GetChara().tag == "Enemy") continue;
            //}
            Square_Info n_si_ = n.GetComponent<Square_Info>();
            if (cost_ - n_si_.GetCost() >= 0 && n_si_.CanSetCost(cost_, near))
            {
                attack_range.Add(n_si_.gameObject);
                Retrieval(n, cost_);
            }
        }
    }
}
