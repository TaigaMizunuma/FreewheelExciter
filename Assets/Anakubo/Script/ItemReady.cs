using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReady : MonoBehaviour {
    // 0は最初の画面 1はユニットを選択して交換等を選ぶ画面 2はアイテム交換 3は倉庫 4は捨てる
    private int mode_ = 0;
    // 表示するUIの親をモード毎に登録
    public GameObject[] UIs_parent;
    // カーソルの位置
    public GameObject cursor_;
    private int pos_num_x = 0;
    private int pos_num_y = 0;
    // 一番上のcanvasを登録しておく
    public GameObject parent_canvas;

    // モード0 ユニット選択
    // 選んだユニット
    private GameObject first_unit;
    // ユニットのアイテムの親
    public GameObject first_item_list;
    // ユニットのアイテム一覧
    private List<GameObject> first_items;
    // ユニットのリスト
    private List<GameObject> units_;
    // ユニットの親
    public GameObject units_parent;
    // 選んでいるユニットの番号
    private int unit_num = 0;
    
    // モード1 アイテムに関するアクションの選択
    // 操作の項目
    public GameObject[] controll_list_;
    // 選んでる項目
    private int controll_num = 0;

    // モード2 交換選択時の表示
    // 交換の時に選んだ二人目のユニット
    private GameObject second_unit;
    // 二人目ユニットのアイテムの親
    public GameObject second_item_list;
    // 二人目のユニットのアイテム一覧
    private List<GameObject> second_items;
    // それぞれの交換するアイテムを格納
    private GameObject first_unit_item = null;
    private GameObject second_unit_item = null;

    // モード3 倉庫選択時の表示
    // 倉庫のアイテムの親
    public GameObject warehouse_list;
    // 倉庫のリスト
    private List<GameObject> warehouse_items;
    // 預けるか受け取るか 0は未選択 1は預ける 2は受け取る
    private int warehouse_mode = 0;
    // 選択したアイテムを格納
    private GameObject warehouse_select_item = null;
    // 操作の選択肢を登録
    public GameObject[] warehouse_controll;

    // モード4 捨てる選択時の表示
    // 捨てるアイテムを格納
    private GameObject throw_item = null;
    // 確認のテキストの親を登録
    public GameObject[] confirmation_;
    // はい と いいえ を登録
    public GameObject[] yes_no_;

    // Use this for initialization
    void Start () {
        // モードに応じたUI表示をする
        UIChange();
        controll_list_[0].transform.parent.gameObject.SetActive(false);
        // 使えるユニットを取得
        units_ = new List<GameObject>();
        Transform obj = units_parent.GetComponentInChildren<Transform>();
        if (obj.childCount > 0)
        {
            foreach (Transform ob in obj)
            {
                units_.Add(ob.gameObject);
            }
        }
        first_items = new List<GameObject>();
        obj = first_item_list.GetComponentInChildren<Transform>();
        if (obj.childCount > 0)
        {
            foreach (Transform ob in obj)
            {
                first_items.Add(ob.gameObject);
            }
        }
        second_items = new List<GameObject>();
        obj = second_item_list.GetComponentInChildren<Transform>();
        if (obj.childCount > 0)
        {
            foreach (Transform ob in obj)
            {
                second_items.Add(ob.gameObject);
            }
        }
        warehouse_items = new List<GameObject>();
        obj = warehouse_list.GetComponentInChildren<Transform>();
        if (obj.childCount > 0)
        {
            foreach (Transform ob in obj)
            {
                warehouse_items.Add(ob.gameObject);
            }
        }
        // カーソルの位置を合わせる
        cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[0]);
    }
	
	// Update is called once per frame
	void Update () {
        switch (mode_)
        {
            case 0:
                Mode0();
                break;
            case 1:
                Mode1();
                break;
            case 2:
                Mode2();
                break;
            case 3:
                Mode3();
                break;
            case 4:
                Mode4();
                break;
            default:
                break;
        }
    }

    // アイテムの操作をしたいユニットを選ぶモード
    void Mode0()
    {
        // 下キーで１つ下に 下から２番目のときに押すとカーソルは動かずに表示されているユニットがずれる
        if (Input.GetKeyDown(KeyCode.DownArrow) && unit_num < units_.Count - 2)
        {
            if (pos_num_y == 2 && unit_num < units_.Count - 4)
            {
                Vector3 pos = units_parent.GetComponent<RectTransform>().anchoredPosition;
                pos.y += 90.0f;
                units_parent.GetComponent<RectTransform>().anchoredPosition = pos;
            }
            else
            {
                pos_num_y++;
            }
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[pos_num_y*2 + pos_num_x]);
            unit_num += 2;
        }
        // 上キーで１つ上に 上から２番目のときに押すとカーソルは動かずに表示されているユニットがずれる
        if (Input.GetKeyDown(KeyCode.UpArrow) && unit_num > 1)
        {
            if (pos_num_y == 1 && unit_num > 3)
            {
                Vector3 pos = units_parent.GetComponent<RectTransform>().anchoredPosition;
                pos.y -= 90.0f;
                units_parent.GetComponent<RectTransform>().anchoredPosition = pos;
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
            if (first_unit == null)
            {
                first_unit = units_[unit_num];
                controll_list_[0].transform.parent.gameObject.SetActive(true);
                first_items[0].transform.parent.gameObject.SetActive(false);
                mode_ = 1;
                ResetNum();
            }
            else
            {
                if (units_[unit_num] != first_unit)
                {
                    second_unit = units_[unit_num];
                    mode_ = 2;
                    UIChange();
                    ResetNum();
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (first_unit == null)
            {
                ResetNum();
                transform.parent.GetComponent<ReadyManager>().ModeChange(0);
            }
            else
            {
                mode_ = 1;
                controll_list_[0].transform.parent.gameObject.SetActive(true);
                first_items[0].transform.parent.gameObject.SetActive(false);
                ResetNum();
            }
        }
    }

    // 操作を選択するモード
    void Mode1()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            pos_num_y++;
            if (pos_num_y >= controll_list_.Length) pos_num_y = 0;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            pos_num_y--;
            if (pos_num_y < 0) pos_num_y = controll_list_.Length - 1;
        }
        cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(controll_list_[pos_num_y]);
        if (Input.GetKeyDown(KeyCode.X))
        {
            controll_list_[0].transform.parent.gameObject.SetActive(false);
            first_items[0].transform.parent.gameObject.SetActive(true);
            first_unit = null;
            mode_ = 0;
            ResetNum();
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[0]);
        }
        if (Input.GetKeyDown(KeyCode.Z)){
            switch (pos_num_y)
            {
                case 0:
                    cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[0]);
                    mode_ = 0;
                    break;
                case 1:
                    mode_ = 3;
                    cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(warehouse_controll[0]);
                    pos_num_y = 0;
                    break;
                case 2:
                    mode_ = 4;
                    confirmation_[0].SetActive(false);
                    confirmation_[1].SetActive(false);
                    yes_no_[0].transform.parent.gameObject.SetActive(false);
                    yes_no_[1].transform.parent.gameObject.SetActive(false);
                    pos_num_y = 0;
                    cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(first_items[0]);
                    break;
                default:
                    break;
            }
            controll_list_[0].transform.parent.gameObject.SetActive(false);
            first_items[0].transform.parent.gameObject.SetActive(true);
            ResetNum();
            UIChange();
        }
    }

    // 交換をするモード
    void Mode2()
    {
        if (first_unit_item == null)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                pos_num_y++;
                if (pos_num_y >= first_items.Count) pos_num_y = 0;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                pos_num_y--;
                if (pos_num_y < 0) pos_num_y = first_items.Count - 1;
            }
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(first_items[pos_num_y]);
            if (Input.GetKeyDown(KeyCode.X))
            {
                mode_ = 0;
                second_unit = null;
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[0]);
                ResetNum();
                UIChange();
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                first_unit_item = first_items[pos_num_y];
                pos_num_y = 0;
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(second_items[pos_num_y]);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                first_unit_item = null;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                pos_num_y++;
                if (pos_num_y >= second_items.Count) pos_num_y = 0;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                pos_num_y--;
                if (pos_num_y < 0) pos_num_y = second_items.Count - 1;
            }
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(second_items[pos_num_y]);
            if (Input.GetKeyDown(KeyCode.Z))
            {
                second_unit_item = second_items[pos_num_y];


                first_unit_item = null;
                second_unit_item = null;
                pos_num_y = 0;
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(first_items[pos_num_y]);
            }
        }
    }

    // 倉庫のモード
    void Mode3()
    {
        if (warehouse_mode == 0)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                pos_num_y++;
                if (pos_num_y >= warehouse_controll.Length) pos_num_y = 0;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                pos_num_y--;
                if (pos_num_y < 0) pos_num_y = warehouse_controll.Length - 1;
            }
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(warehouse_controll[pos_num_y]);
            if (Input.GetKeyDown(KeyCode.X))
            {
                mode_ = 1;
                controll_list_[0].transform.parent.gameObject.SetActive(true);
                first_items[0].transform.parent.gameObject.SetActive(false);
                ResetNum();
                UIChange();
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(controll_list_[0]);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                warehouse_mode = pos_num_y + 1;
                if (warehouse_mode == 1)
                {
                    cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(first_items[0]);
                }
                if (warehouse_mode == 2)
                {
                    cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(warehouse_items[0]);
                }
                pos_num_y = 0;
            }
        }
        else {
            if (warehouse_mode == 1)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    pos_num_y++;
                    if (pos_num_y >= first_items.Count) pos_num_y = 0;
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    pos_num_y--;
                    if (pos_num_y < 0) pos_num_y = first_items.Count - 1;
                }
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(first_items[pos_num_y]);
            }
            if (warehouse_mode == 2)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    pos_num_y++;
                    if (pos_num_y >= warehouse_items.Count) pos_num_y = 0;
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    pos_num_y--;
                    if (pos_num_y < 0) pos_num_y = warehouse_items.Count - 1;
                }
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(warehouse_items[pos_num_y]);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                warehouse_mode = 0;
                pos_num_y = 0;
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(warehouse_controll[0]);
            }
        }
    }

    // 捨てるモード
    void Mode4()
    {
        if (!confirmation_[1].activeSelf)
        {
            if (throw_item == null)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    pos_num_y++;
                    if (pos_num_y >= first_items.Count) pos_num_y = 0;
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    pos_num_y--;
                    if (pos_num_y < 0) pos_num_y = first_items.Count - 1;
                }
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(first_items[pos_num_y]);
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    throw_item = first_items[pos_num_y];
                    confirmation_[0].SetActive(true);
                    yes_no_[0].transform.parent.gameObject.SetActive(true);
                    yes_no_[1].transform.parent.gameObject.SetActive(true);
                    pos_num_x = 1;
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    mode_ = 1;
                    controll_list_[0].transform.parent.gameObject.SetActive(true);
                    first_items[0].transform.parent.gameObject.SetActive(false);
                    ResetNum();
                    UIChange();
                    cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(controll_list_[0]);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.RightArrow) && pos_num_x == 0)
                {
                    pos_num_x++;
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow) && pos_num_x == 1)
                {
                    pos_num_x--;
                }
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(yes_no_[pos_num_x]);
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (pos_num_x == 0)
                    {
                        confirmation_[0].SetActive(false);
                        confirmation_[1].SetActive(true);
                    }
                    else
                    {
                        throw_item = null;
                        confirmation_[0].SetActive(false);
                        yes_no_[0].transform.parent.gameObject.SetActive(false);
                        yes_no_[1].transform.parent.gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            if (Input.anyKeyDown)
            {
                throw_item = null;
                confirmation_[1].SetActive(false);
                yes_no_[0].transform.parent.gameObject.SetActive(false);
                yes_no_[1].transform.parent.gameObject.SetActive(false);
            }
        }
    }

    // 数値をまとめて０にする
    void ResetNum()
    {
        unit_num = 0;
        controll_num = 0;
        pos_num_x = 0;
        pos_num_y = 0;
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

    // 現在のモードに合わせてUIを表示し直す
    void UIChange()
    {
        for (int i = 0; i < UIs_parent.Length; i++)
        {
            if (mode_ == 0)
            {
                if (i == mode_) UIs_parent[i].SetActive(true);
                else UIs_parent[i].SetActive(false);
            }
            else
            {
                if (i == mode_-1) UIs_parent[i].SetActive(true);
                else UIs_parent[i].SetActive(false);
            }
        }
    }
}
