using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyMenu : MonoBehaviour {
    // 項目のリスト
    private List<GameObject> menus_;
    // 項目の親を登録
    public GameObject menu_parent;
    // 選んでいる項目の番号
    private int menu_num = 0;
    // カーソル
    public GameObject cursor_;

    // Use this for initialization
    void Start () {
        menus_ = new List<GameObject>();
        Transform obj = menu_parent.GetComponentInChildren<Transform>();
        if (obj.childCount > 0)
        {
            foreach (Transform ob in obj)
            {
                menus_.Add(ob.gameObject);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            menu_num++;
            if (menu_num == menus_.Count) menu_num = 0;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            menu_num--;
            if (menu_num < 0) menu_num = menus_.Count - 1;
        }
        cursor_.GetComponent<RectTransform>().anchoredPosition = menus_[menu_num].GetComponent<RectTransform>().anchoredPosition + new Vector2(-50, 0);
        if (Input.GetKeyDown(KeyCode.Z))
        {
            transform.parent.GetComponent<ReadyManager>().ModeChange(menu_num+1);
        }
	}
}
