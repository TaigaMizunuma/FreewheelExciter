using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyManager : MonoBehaviour {

    private int now_mode=0;

    public GameObject[] menu_item;

    private GameObject select_unit;

    private BattleFlowTest battle_flow;

    private GameObject ready_back;

	// Use this for initialization
	void Start () {
        ModeChange(0);
        battle_flow = GameObject.Find("GameManager").GetComponent<BattleFlowTest>();
        ready_back = GameObject.Find("ReadyBack");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ModeChange(int num)
    {
        if (num == 5)
        {
            battle_flow.StartGame();
            gameObject.SetActive(false);
        }
        else {
            now_mode = num;
            for (int i = 0; i < menu_item.Length; i++)
            {
                if (now_mode == i)
                {
                    menu_item[i].SetActive(true);
                    if (now_mode == 3) ready_back.SetActive(false);
                }
                else
                {
                    menu_item[i].SetActive(false);
                }
            }
        }
    }

    public void SetUnit(GameObject u)
    {
        select_unit = u;
    }

    public GameObject GetUnit()
    {
        return select_unit;
    }
}
