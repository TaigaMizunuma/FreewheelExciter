using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyManager : MonoBehaviour {

    private int now_mode=0;

    public GameObject[] menu_item;

	// Use this for initialization
	void Start () {
        ModeChange(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ModeChange(int num)
    {
        now_mode = num;
        for(int i = 0; i < menu_item.Length; i++)
        {
            if (now_mode == i)
            {
                menu_item[i].SetActive(true);
            }
            else
            {
                menu_item[i].SetActive(false);
            }
        }
    }
}
