using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosSort : MonoBehaviour {
    // 初期位置
    private GameObject[] floor_;
    private List<GameObject> first_pos = new List<GameObject>();
    private GameObject[] f_pos;
    
    // 出撃するユニットを取得する
    private GameObject[] units_;

    // Use this for initialization
    void Start () {
        //SetFirstPos();
        bool set_ = false;
        floor_ = GameObject.FindGameObjectsWithTag("Floor");
        foreach(GameObject f in floor_)
        {
            if (f.GetComponent<MapStatus>().startPosition)
            {
                first_pos.Add(f);
                set_ = true;
                Debug.Log("get");
            }
        }
        if (set_) SetFirstPos();
    }
	
	// Update is called once per frame
	void Update () {
		if(first_pos == null)
        {
            bool set_ = false;
            floor_ = GameObject.FindGameObjectsWithTag("Floor");
            foreach (GameObject f in floor_)
            {
                if (f.GetComponent<MapStatus>().startPosition)
                {
                    first_pos.Add(f);
                    set_ = true;
                    Debug.Log("get");
                }
            }
            if (set_) SetFirstPos();
        }
	}

    public void SetFirstPos()
    {
        if (first_pos == null) return;
        units_ = GameObject.FindGameObjectsWithTag("Player");
        int min = Mathf.Min(units_.Length, first_pos.Count);
        for (int i = 0; i < min; i++)
        {
            units_[i].transform.position = first_pos[i].transform.position + new Vector3(0, 1.0f, 0);
            units_[i].GetComponent<Move_System>().SetNowPos();
        }
    }

    public void ChangePos(GameObject a,GameObject b)
    {
        if (first_pos == null) return;
        int num_a = GetPosNum(a);
        int num_b = GetPosNum(b);
        first_pos[num_a] = b;
        first_pos[num_b] = a;
        SetFirstPos();
    }

    public GameObject[] GetFirstPos()
    {
        if (first_pos == null) return null;
        f_pos = new GameObject[first_pos.Count];
        for (int i = 0; i < first_pos.Count; i++)
        {
            f_pos[i] = first_pos[i];
        }
        return f_pos;
    }

    public int GetPosNum(GameObject fp)
    {
        if (first_pos == null) return -1;
        for (int i = 0; i < first_pos.Count; i++)
        {
            if (fp == first_pos[i])
            {
                return i;
            }
        }
        return -1;
    }
}
