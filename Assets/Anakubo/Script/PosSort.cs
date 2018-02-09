﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosSort : MonoBehaviour {
    // 初期位置
    private GameObject[] floor_;
    private List<GameObject> first_pos = new List<GameObject>();
    private GameObject[] f_pos;
    
    // 出撃するユニットを取得する
    private GameObject[] units_;

    // 全てのプレイヤーキャラを取得
    private GameObject[] p_;
    // プレイヤーをID順に入れる
    private List<GameObject> players_ = new List<GameObject>();

    // Use this for initialization
    void Start () {
        p_ = GameObject.FindGameObjectsWithTag("Player");
        //SetFirstPos();
        bool set_ = false;
        floor_ = GameObject.FindGameObjectsWithTag("Floor");
        foreach(GameObject f in floor_)
        {
            if (f.GetComponent<MapStatus>().startPosition)
            {
                first_pos.Add(f);
                set_ = true;
            }
        }
        //if (set_) SetFirstPos();
    }
	
	// Update is called once per frame
	void Update () {
        if(p_ != null)
        {
            for (int i = 0; i < p_.Length; i++)
            {
                foreach (GameObject ply in p_)
                {
                    if (ply.GetComponent<Character>()._id == i) players_.Add(ply);
                }
            }
            p_ = null;
        }
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
                }
            }
            if (set_) SetFirstPos();
        }
	}

    public void SetFirstPos()
    {
        if (first_pos == null) return;
        units_ = GameObject.FindGameObjectsWithTag("Player");
        int num = 0;
        for (int i = 0; i < first_pos.Count; i++)
        {
            if (i + num < players_.Count)
            {
                while (!players_[i + num].activeSelf)
                {
                    if (i + num == players_.Count-1)
                    {
                        //Debug.Log("aa");
                        num++;
                        break;
                    }
                    else
                    {
                        num++;
                    }
                }
            }
            if (i + num < players_.Count)
            {
                players_[i + num].transform.position = first_pos[i].transform.position + new Vector3(0, 1.0f, 0);
                players_[i + num].GetComponent<Move_System>().SetNowPos();
            }
            else
            {
                first_pos[i].GetComponent<Square_Info>().ResetChara();
            }
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

    public GameObject[] GetPlayers()
    {
        if (p_ != null) return null;
        GameObject[] pls = new GameObject[players_.Count];
        for (int i = 0; i < pls.Length; i++)
        {
            pls[i] = players_[i];
        }
        return pls;
    }
}
