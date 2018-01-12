using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosSort : MonoBehaviour {
    // 初期位置 出撃可能人数と同じ数にする
    public GameObject[] first_pos;
    
    // 出撃するユニットを取得する
    private GameObject[] units_;

    // Use this for initialization
    void Start () {
        SetFirstPos();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetFirstPos()
    {
        units_ = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < units_.Length; i++)
        {
            units_[i].transform.position = first_pos[i].transform.position + new Vector3(0, 1.0f, 0);
            units_[i].GetComponent<Move_System>().SetNowPos();
        }
    }

    public void ChangePos(GameObject a,GameObject b)
    {
        int num_a = GetPosNum(a);
        int num_b = GetPosNum(b);
        first_pos[num_a] = b;
        first_pos[num_b] = a;
        SetFirstPos();
    }

    public GameObject[] GetFirstPos()
    {
        return first_pos;
    }

    public int GetPosNum(GameObject fp)
    {
        for(int i = 0; i < first_pos.Length; i++)
        {
            if (fp == first_pos[i])
            {
                return i;
            }
        }
        return -1;
    }
}
