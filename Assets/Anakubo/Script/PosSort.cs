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
        }
    }

    public void ChangePos(int a,int b)
    {
        GameObject a_pos = first_pos[a];
        GameObject b_pos = first_pos[b];
        first_pos[a] = b_pos;
        first_pos[b] = a_pos;
    }
}
