using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square_Info : MonoBehaviour {
    public int move_cost = 1;
    private GameObject[] near_squares;
    private bool isDecisioned = false;
    private int max_cost = -1;
    private GameObject before_point;
    public LayerMask mask;
    private List<GameObject> nears_ = new List<GameObject>();

    public GameObject ride_chara;

    private bool atk_range = false;

    private bool[] existNextSquare = { false, false, false, false };

    // Use this for initialization
    void Awake () {
        SearchNearSquare();
        near_squares = new GameObject[nears_.Count];
        for(int i = 0; i < nears_.Count; i++)
        {
            near_squares[i] = nears_[i];
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetCost()
    {
        return move_cost;
    }

    public GameObject[] GetNear()
    {
        return near_squares;
    }

    public void Decision()
    {
        if (isDecisioned) return;
        isDecisioned = true;
        GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 1);
    }

    public void DecisionEnd()
    {
        isDecisioned = false;
        atk_range = false;
        GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1);
        max_cost = -1;
    }

    public bool IsDecision()
    {
        return isDecisioned;
    }

    public bool CanSetCost(int c,GameObject b)
    {
        if (before_point != null)
        {
            if (b.GetComponent<Square_Info>().GetChara() == null && before_point.GetComponent<Square_Info>().GetChara() != null)
            {
                if (c == max_cost)
                {
                    max_cost = c;
                    before_point = b;
                    return true;
                }
            }
        }
        if (c > max_cost)
        {
            max_cost = c;
            before_point = b;
            return true;
        }
        return false;
    }

    public void SetNowPosCost(int c)
    {
        max_cost = c;
    }

    public GameObject SearchRoute()
    {
        return before_point;
    }

    private void SearchNearSquare()
    {
        Vector3[] direction_ = new Vector3[]
        {
            new Vector3(0,0,1),
            new Vector3(1,0,0),
            new Vector3(0,0,-1),
            new Vector3(-1,0,0)
        };
        for (int i = 0; i < 4; i++)
        {
            Ray ray = new Ray(transform.position, direction_[i]);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 2.0f, mask))
            {
                nears_.Add(hit.transform.gameObject);
                existNextSquare[i] = true;
            }
        }
    }

    public void SetChara(GameObject cha)
    {
        ride_chara = cha;
    }

    public GameObject GetChara()
    {
        if (ride_chara == null) return null;
        return ride_chara;
    }

    public void ResetChara()
    {
        ride_chara = null;
    }

    public void AttackRange()
    {
        GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
        atk_range = true;
    }

    public bool GetRange()
    {
        return atk_range;
    }

    public int GetMaxCost()
    {
        if (ride_chara != null) return -1;
        return max_cost;
    }

    public int GetRushCost()
    {
        int m_cos = -1;
        for(int i = 0; i < nears_.Count; i++)
        {
            if (m_cos < 0 || m_cos > nears_[i].GetComponent<Square_Info>().GetMaxCost())
            {
                m_cos = nears_[i].GetComponent<Square_Info>().GetMaxCost();
            }
        }
        return m_cos;
    }
    
    public bool ExistNextSquare(int num)
    {
        return existNextSquare[num];
    }
}
