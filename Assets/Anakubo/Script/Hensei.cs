using System.Collections;
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
    private int row;
    // 出撃可能人数
    public int sortie_num = 6;
    // プレイヤーを格納
    private GameObject[] players_;
    // 親のcanvasを取得
    private GameObject parent_canvas;
    // PosSortを取得
    private PosSort pos_sort;
    // Shosaiを取得
    public GameObject shosai_;

    // 上下左右キーの押しっぱなしに対応
    private float up_timer = 50.0f;
    private float down_timer = 50.0f;
    private float right_timer = 50.0f;
    private float left_timer = 50.0f;
    private float key_interval = 1.0f;
    // 右 左 上 下
    bool[] key_flg_ = { false, false, false, false };
    bool[] interval_flg_ = { false, false, false, false };

    /////////////////////////////////////

    ////////////////////////////////////


    // Use this for initialization
    void Start()
    {
        pos_sort = GameObject.Find("ReadyCanvas").GetComponent<PosSort>();
        sortie_num = pos_sort.GetFirstPos().Length;
    }

    void OnEnable()
    {
        if (units_ == null) return;
        cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[pos_num_y * 2 + pos_num_x]);
        parent_canvas.GetComponent<ReadyManager>().SetUnit(players_[unit_num]);
        shosai_.GetComponent<ReadyShosai>().SetUnit();
    }

    // Update is called once per frame
    void Update()
    {
        if (parent_canvas == null) return;
        if (shosai_.activeSelf == false)
        {
            shosai_.SetActive(true);
            shosai_.GetComponent<ReadyShosai>().Hensei_Display();
        }
        ControllerAxis();
        // 下キーで１つ下に 下から２番目のときに押すとカーソルは動かずに表示されているユニットがずれる
        if (key_flg_[3] && unit_num/2 < row-1 && unit_num < units_.Count - 2)
        {
            if (pos_num_y == 2 && unit_num / 2 < row-2)
            {
                Vector3 pos = unit_parent.GetComponent<RectTransform>().anchoredPosition;
                pos.y += 90.0f;
                unit_parent.GetComponent<RectTransform>().anchoredPosition = pos;
            }
            else
            {
                pos_num_y++;
            }
            unit_num += 2;
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[unit_num]);
            parent_canvas.GetComponent<ReadyManager>().SetUnit(players_[unit_num]);
            shosai_.GetComponent<ReadyShosai>().SetUnit();
            key_flg_[3] = false;
        }
        // 上キーで１つ上に 上から２番目のときに押すとカーソルは動かずに表示されているユニットがずれる
        if (key_flg_[2]&& unit_num > 1)
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
            unit_num -= 2;
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[unit_num]);
            parent_canvas.GetComponent<ReadyManager>().SetUnit(players_[unit_num]);
            shosai_.GetComponent<ReadyShosai>().SetUnit();
            key_flg_[2] = false;
        }

        if (key_flg_[0] && pos_num_x == 0 && unit_num < units_.Count - 1)
        {
            pos_num_x++;
            unit_num++;
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[unit_num]);
            parent_canvas.GetComponent<ReadyManager>().SetUnit(players_[unit_num]);
            shosai_.GetComponent<ReadyShosai>().SetUnit();
            key_flg_[0] = false;
        }
        if (key_flg_[1] && pos_num_x == 1)
        {
            pos_num_x--;
            unit_num--;
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[unit_num]);
            parent_canvas.GetComponent<ReadyManager>().SetUnit(players_[unit_num]);
            shosai_.GetComponent<ReadyShosai>().SetUnit();
            key_flg_[1] = false;
        }

        if (Input.GetButtonDown("O"))
        {
            if (sortie_[unit_num])
            {
                if (players_[unit_num].GetComponent<Character>()._hero) return;
                units_[unit_num].GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                sortie_[unit_num] = false;
                sortie_num++;
                players_[unit_num].SetActive(false);
                pos_sort.SetFirstPos();
            }
            else if (sortie_num > 0)
            {
                units_[unit_num].GetComponent<Text>().color = new Color(1.0f, 1.0f, 1.0f, 1);
                sortie_[unit_num] = true;
                sortie_num--;
                players_[unit_num].SetActive(true);
                pos_sort.SetFirstPos();
            }
        }
        if (Input.GetButtonDown("X"))
        {
            transform.parent.GetComponent<ReadyManager>().ModeChange(0);
        }
        if (Input.GetButtonDown("△"))
        {
            shosai_.GetComponent<ReadyShosai>().Hensei_Display_End();
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
        units_ = unit_parent.GetComponent<UnitList>().GetTexts();
        players_ = unit_parent.GetComponent<UnitList>().GetPlayers();
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
                units_[i].GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                players_[i].SetActive(false);
            }
        }
        parent_canvas = transform.parent.gameObject;
        cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[unit_num]);
        parent_canvas.GetComponent<ReadyManager>().SetUnit(players_[unit_num]);
        shosai_.GetComponent<ReadyShosai>().SetUnit();
        pos_sort.SetFirstPos();
        row = (units_.Count+1) / 2;
    }

    void ControllerAxis()
    {
        
        if(Input.GetAxis("AxisX") == 1)
        {
            right_timer += Time.deltaTime;
            if (right_timer > key_interval)
            {
                if (right_timer < 10) {
                    interval_flg_[0] = true;
                }
                right_timer = 0;
                key_flg_[0] = true;
            }
            else if(right_timer>key_interval / 5.0f && interval_flg_[0])
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
