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

    // 上下左右キーの押しっぱなしに対応
    private float up_timer = 50.0f;
    private float down_timer = 50.0f;
    private float right_timer = 50.0f;
    private float left_timer = 50.0f;
    private float key_interval = 1.0f;
    bool[] key_flg_ = { false, false, false, false };
    bool[] interval_flg_ = { false, false, false, false };

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
        ControllerAxis();
        if (key_flg_[3])
        {
            menu_num++;
            if (menu_num == menus_.Count) menu_num = 0;
            key_flg_[3] = false;
        }
        if (key_flg_[2])
        {
            menu_num--;
            if (menu_num < 0) menu_num = menus_.Count - 1;
            key_flg_[2] = false;
        }
        cursor_.GetComponent<RectTransform>().anchoredPosition = menus_[menu_num].GetComponent<RectTransform>().anchoredPosition + new Vector2(-50, 0);
        if (Input.GetButtonDown("O"))
        {
            transform.parent.GetComponent<ReadyManager>().ModeChange(menu_num+1);
        }
	}
    void ControllerAxis()
    {

        if (Input.GetAxis("AxisX") == 1)
        {
            right_timer += Time.deltaTime;
            if (right_timer > key_interval)
            {
                if (right_timer < 10)
                {
                    interval_flg_[0] = true;
                }
                right_timer = 0;
                key_flg_[0] = true;
            }
            else if (right_timer > key_interval / 5.0f && interval_flg_[0])
            {
                right_timer = 0;
                key_flg_[0] = true;
            }
        }
        else
        {
            right_timer = 50;
            interval_flg_[0] = false;
            key_flg_[0] = false;
        }
        if (Input.GetAxis("AxisX") == -1)
        {
            left_timer += Time.deltaTime;
            if (left_timer > key_interval)
            {
                if (left_timer < 10)
                {
                    interval_flg_[1] = true;
                }
                left_timer = 0;
                key_flg_[1] = true;
            }
            else if (left_timer > key_interval / 5.0f && interval_flg_[1])
            {
                left_timer = 0;
                key_flg_[1] = true;
            }
        }
        else
        {
            left_timer = 50;
            interval_flg_[1] = false;
            key_flg_[1] = false;
        }
        if (Input.GetAxis("AxisY") == 1)
        {
            up_timer += Time.deltaTime;
            if (up_timer > key_interval)
            {
                if (up_timer < 10)
                {
                    interval_flg_[2] = true;
                }
                up_timer = 0;
                key_flg_[2] = true;
            }
            else if (up_timer > key_interval / 5.0f && interval_flg_[2])
            {
                up_timer = 0;
                key_flg_[2] = true;
            }
        }
        else
        {
            up_timer = 50;
            interval_flg_[2] = false;
            key_flg_[2] = false;
        }
        if (Input.GetAxis("AxisY") == -1)
        {
            down_timer += Time.deltaTime;
            if (down_timer > key_interval)
            {
                if (down_timer < 10)
                {
                    interval_flg_[3] = true;
                }
                down_timer = 0;
                key_flg_[3] = true;
            }
            if (down_timer > key_interval / 5.0f && interval_flg_[3])
            {
                down_timer = 0;
                key_flg_[3] = true;
            }
        }
        else
        {
            down_timer = 50.0f;
            interval_flg_[3] = false;
            key_flg_[3] = false;
        }
    }
}
