using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour {
    public GameObject cursor_;
    private int pos_num = 0;
    private bool saved_ = false;
    public GameObject save_q_;
    // 親のcanvasを取得
    private GameObject parent_canvas;
    public GameObject[] menus_;

    // 上下左右キーの押しっぱなしに対応
    private float up_timer = 50.0f;
    private float down_timer = 50.0f;
    private float right_timer = 50.0f;
    private float left_timer = 50.0f;
    private float key_interval = 1.0f;
    // 右 左 上 下
    bool[] key_flg_ = { false, false, false, false };
    bool[] interval_flg_ = { false, false, false, false };

    // Use this for initialization
    void Start () { 
    }

    void OnEnable()
    {
        parent_canvas = transform.parent.gameObject;
        pos_num = 0;
        save_q_.SetActive(true);
        cursor_.SetActive(true);
        for (int i = 0; i < menus_.Length; i++)
        {
            menus_[i].SetActive(true);
        }
        cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(menus_[pos_num]) + new Vector2(-100, 0);
    }
	
	// Update is called once per frame
	void Update () {
        ControllerAxis();
        if (Input.GetButtonDown("X"))
        {
            transform.parent.GetComponent<ReadyManager>().ModeChange(0);
        }
        if (!saved_)
        {
            if (key_flg_[0] || key_flg_[1])
            {
                if (pos_num == 0) pos_num = 1;
                else pos_num = 0;
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(menus_[pos_num]) + new Vector2(-100, 0);
                key_flg_[0] = false;
                key_flg_[1] = false;
            }
            if (Input.GetButtonDown("O"))
            {
                if (pos_num == 0)
                {
                    save_q_.SetActive(false);
                    cursor_.SetActive(false);
                    for (int i = 0; i < menus_.Length; i++)
                    {
                        menus_[i].SetActive(false);
                    }
                    // ここにセーブ処理

                }
                else
                {
                    transform.parent.GetComponent<ReadyManager>().ModeChange(0);
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("O") || Input.GetButtonDown("X"))
            {
                transform.parent.GetComponent<ReadyManager>().ModeChange(0);
            }
        }
    }

    Vector2 CanvasAnchoredPosition(GameObject obj)
    {
        Vector2 c_pos = new Vector2();
        GameObject o_ = obj;
        while (o_ != parent_canvas)
        {
            c_pos += o_.GetComponent<RectTransform>().anchoredPosition;
            o_ = o_.transform.parent.gameObject;
        }

        return c_pos;
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
