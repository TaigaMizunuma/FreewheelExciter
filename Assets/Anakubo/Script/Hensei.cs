﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hensei : MonoBehaviour
{
    // 編成画面のユニットのリスト
    private List<GameObject> units_;
    // 出撃かどうか
    private bool[] sortie_;
    // ユニットの親のオブジェクトを登録する
    public GameObject unit_parent;
    // 選んでいるユニットの番号
    private int unit_num = 0;
    // カーソルの位置
    public GameObject cursor_;
    private int pos_num_x = 0;
    private int pos_num_y = 0;
    // 出撃可能人数
    public int sortie_num = 6;
    // プレイヤーを格納
    private GameObject[] players_;
    // 親のcanvasを取得
    private GameObject parent_canvas;
    // PosSortを取得
    private PosSort pos_sort;

    /////////////////////////////////////

    ////////////////////////////////////


    // Use this for initialization
    void Start()
    {
        pos_sort = GameObject.Find("ReadyCanvas").GetComponent<PosSort>();
    }

    void OnEnable()
    {
        if (units_ == null) return;
        cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[pos_num_y * 2 + pos_num_x]);
    }

    // Update is called once per frame
    void Update()
    {
        // 下キーで１つ下に 下から２番目のときに押すとカーソルは動かずに表示されているユニットがずれる
        if (Input.GetKeyDown(KeyCode.DownArrow) && unit_num < units_.Count - 2)
        {
            if (pos_num_y == 2 && unit_num < units_.Count - 4)
            {
                Vector3 pos = unit_parent.GetComponent<RectTransform>().anchoredPosition;
                pos.y += 90.0f;
                unit_parent.GetComponent<RectTransform>().anchoredPosition = pos;
            }
            else
            {
                pos_num_y++;
            }
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[pos_num_y * 2 + pos_num_x]);
            unit_num += 2;
        }
        // 上キーで１つ上に 上から２番目のときに押すとカーソルは動かずに表示されているユニットがずれる
        if (Input.GetKeyDown(KeyCode.UpArrow) && unit_num > 1)
        {
            if (pos_num_y == 1 && unit_num > 3)
            {
                Vector3 pos = unit_parent.GetComponent<RectTransform>().anchoredPosition;
                pos.y -= 90.0f;
                unit_parent.GetComponent<RectTransform>().anchoredPosition = pos;
            }
            else
            {
                pos_num_y--;
            }
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[pos_num_y * 2 + pos_num_x]);
            unit_num -= 2;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && pos_num_x == 0)
        {
            pos_num_x++;
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[pos_num_y * 2 + pos_num_x]);
            unit_num++;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && pos_num_x == 1)
        {
            pos_num_x--;
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[pos_num_y * 2 + pos_num_x]);
            unit_num--;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (sortie_[unit_num])
            {
                units_[unit_num].GetComponent<Text>().color = new Color(0.7f, 0.7f, 0.7f, 1);
                sortie_[unit_num] = false;
                sortie_num++;
                players_[unit_num].SetActive(false);
                pos_sort.SetFirstPos();
            }
            else if (sortie_num > 0)
            {
                units_[unit_num].GetComponent<Text>().color = new Color(0.2f, 0.2f, 0.2f, 1);
                sortie_[unit_num] = true;
                sortie_num--;
                players_[unit_num].SetActive(true);
                pos_sort.SetFirstPos();
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            transform.parent.GetComponent<ReadyManager>().ModeChange(0);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            parent_canvas.GetComponent<ReadyManager>().SetUnit(players_[unit_num]);
            transform.parent.GetComponent<ReadyManager>().ModeChange(6);
        }
    }

    public List<GameObject> SortieUnits()
    {
        List<GameObject> u_ = new List<GameObject>();
        for (int i = 0; i < sortie_.Length; i++)
        {
            if (sortie_[i] == true) u_.Add(unit_parent.GetComponent<UnitList>().GetPlayerModel(i));
        }
        return u_;
    }

    // Canvasから見たAnchoredPositionを算出する
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

    public void Init()
    {
        units_ = new List<GameObject>();
        Transform obj = unit_parent.GetComponentInChildren<Transform>();
        if (obj.childCount > 0)
        {
            foreach (Transform ob in obj)
            {
                units_.Add(ob.gameObject);
            }
        }
        players_ = GameObject.FindGameObjectsWithTag("Player");
        sortie_ = new bool[units_.Count];
        for (int i = 0; i < sortie_.Length; i++)
        {
            if (sortie_num > 0)
            {
                sortie_[i] = true;
                sortie_num--;
            }
            else
            {
                sortie_[i] = false;
                units_[i].GetComponent<Text>().color = new Color(0.7f, 0.7f, 0.7f, 1);
                players_[i].SetActive(false);
            }
        }
        parent_canvas = transform.parent.gameObject;
    }
}
