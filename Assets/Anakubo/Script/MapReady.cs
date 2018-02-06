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
        if (!map_view)
        {
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
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(menus_[menu_num]) + new Vector2(-50, -10);
            if (Input.GetKeyDown(KeyCode.X))
            {
                transform.parent.GetComponent<ReadyManager>().ModeChange(0);
                ready_back.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Z) && menu_num != 2)
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
            if (Input.GetKeyDown(KeyCode.Z))
            {
                bool check = false;
                foreach (GameObject obj in pos_sort.GetFirstPos())
                {
                    if (ray_box.GetComponent<RayBox>().GetSelectSquare() == obj)
                    {
                        Debug.Log(ray_box.GetComponent<RayBox>().GetSelectSquare());
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
            if(Input.GetKeyDown(KeyCode.X))
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
}
