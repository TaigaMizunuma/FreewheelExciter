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
        if (Input.GetKeyDown(KeyCode.X))
        {
            transform.parent.GetComponent<ReadyManager>().ModeChange(0);
        }
        if (!saved_)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (pos_num == 0) pos_num = 1;
                else pos_num = 0;
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(menus_[pos_num]) + new Vector2(-100, 0);
            }
            if (Input.GetKeyDown(KeyCode.Z))
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
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
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
}
