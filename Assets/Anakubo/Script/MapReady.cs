using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapReady : MonoBehaviour
{
    // メニューの項目の親を登録
    public GameObject menus_parent;
    // ↑の子供を格納するリスト
    private List<GameObject> menus_;
    // カーソルを登録
    public GameObject cursor_;
    // 選択番号
    private int menu_num;
    // 親のcanvasを取得
    private GameObject parent_canvas;
    // マップを見る状態か
    private bool map_view = false;
    // rayboxを登録
    private GameObject ray_box;
    // PosSortを取得
    private PosSort pos_sort;
    // 並び変える状態か
    private bool sort_mode = false;
    // 並び変え用
    private GameObject sort1 = null;
    private GameObject sort2 = null;
    // キャラの移動範囲を表示しているか
    private bool show_range = false;
    // 範囲表示中のキャラ
    private GameObject show_range_chara = null;

    // 準備画面の背景を取得
    public GameObject ready_back;

    // 上下左右キーの押しっぱなしに対応
    private float up_timer = 50.0f;
    private float down_timer = 50.0f;
    private float right_timer = 50.0f;
    private float left_timer = 50.0f;
    private float key_interval = 1.0f;
    bool[] key_flg_ = { false, false, false, false };
    bool[] interval_flg_ = { false, false, false, false };

    // Use this for initialization
    void Start()
    {
        menus_ = new List<GameObject>();
        Transform obj = menus_parent.GetComponentInChildren<Transform>();
        if (obj.childCount > 0)
        {
            foreach (Transform ob in obj)
            {
                menus_.Add(ob.gameObject);
            }
        }
        parent_canvas = transform.parent.gameObject;
        pos_sort = GameObject.Find("ReadyCanvas").GetComponent<PosSort>();
        ray_box = GameObject.Find("RayBox");
    }

    // Update is called once per frame
    void Update()
    {
        ControllerAxis();
        if (!map_view)
        {
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
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(menus_[menu_num]) + new Vector2(-50, -10);
            if (Input.GetButtonDown("X"))
            {
                transform.parent.GetComponent<ReadyManager>().ModeChange(0);
                ready_back.SetActive(true);
            }
            if (Input.GetButtonDown("O") && menu_num != 2)
            {
                map_view = true;
                if (menu_num == 1) sort_mode = true;
                menus_parent.SetActive(false);
                cursor_.SetActive(false);
                ray_box.GetComponent<RayBox>().move_ = true;
            }
        }
        else
        {
            if (Input.GetButtonDown("O"))
            {
                bool check = false;
                foreach (GameObject obj in pos_sort.GetFirstPos())
                {
                    if (ray_box.GetComponent<RayBox>().GetSelectSquare() == obj)
                    {
                        check = true;
                        if (sort_mode)
                        {
                            if (sort1 == null)
                            {
                                sort1 = obj;
                                break;
                            }
                            else {
                                sort2 = obj;
                                if (sort1 != sort2)
                                {
                                    pos_sort.ChangePos(sort1, sort2);
                                    sort1 = null;
                                    sort2 = null;
                                }
                                else sort2 = null;
                                break;
                            }
                        }
                        else
                        {
                            GameObject c = ray_box.GetComponent<RayBox>().GetSelectSquare().GetComponent<Square_Info>().GetChara();
                            if (!show_range)
                            {
                                if (c != null)
                                {
                                    show_range_chara = c;
                                    if (show_range_chara.transform.tag == "Player") show_range_chara.GetComponent<Move_System>().RangeDisplay();
                                    else show_range_chara.GetComponent<EnemyBase>().RangeDisplay();
                                    show_range = true;
                                }
                            }
                            else {
                                if (show_range_chara.transform.tag == "Player") show_range_chara.GetComponent<Move_System>().DisplayEnd();
                                else show_range_chara.GetComponent<EnemyBase>().DisplayEnd();
                                show_range = false;
                                show_range_chara = null;
                            }
                        }
                    }
                }
                if (!check)
                {
                    if (sort_mode)
                    {
                        if (sort1 != null) sort1 = null;
                        else {
                            menus_parent.SetActive(true);
                            cursor_.SetActive(true);
                            sort_mode = false;
                            map_view = false;
                            ray_box.GetComponent<RayBox>().move_ = false;
                        }
                    }
                    else if (show_range)
                    {
                        if (show_range_chara.transform.tag == "Player") show_range_chara.GetComponent<Move_System>().DisplayEnd();
                        else show_range_chara.GetComponent<EnemyBase>().DisplayEnd();
                        show_range = false;
                        show_range_chara = null;
                    }
                    else {
                        menus_parent.SetActive(true);
                        cursor_.SetActive(true);
                        map_view = false;
                        ray_box.GetComponent<RayBox>().move_ = false;
                    }
                }
            }
            if(Input.GetButtonDown("X"))
            {
                if (sort_mode)
                {
                    if (sort1 != null) sort1 = null;
                    else {
                        menus_parent.SetActive(true);
                        cursor_.SetActive(true);
                        sort_mode = false;
                        map_view = false;
                        ray_box.GetComponent<RayBox>().move_ = false;
                    }
                }
                else if (show_range)
                {
                    if (show_range_chara.transform.tag == "Player") show_range_chara.GetComponent<Move_System>().DisplayEnd();
                    else show_range_chara.GetComponent<EnemyBase>().DisplayEnd();
                    show_range = false;
                    show_range_chara = null;
                }
                else {
                    menus_parent.SetActive(true);
                    cursor_.SetActive(true);
                    map_view = false;
                    ray_box.GetComponent<RayBox>().move_ = false;
                }
            }
        }
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
