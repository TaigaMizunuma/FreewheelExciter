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
    private int row;
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
    private List<GameObject>[] warehouse_items;
    // 武器倉庫かアイテム倉庫か
    private int repository_num = 0;
    // 預けるか受け取るか 0は未選択 1は預ける 2は受け取る
    private int warehouse_mode = 0;
    // 選択したアイテムを格納
    private GameObject warehouse_select_item = null;
    // 操作の選択肢を登録
    public GameObject[] warehouse_controll;

    // モード4 捨てる選択時の表示
    // 捨てるアイテムを格納
    private int throw_item_num = -1;
    // 確認のテキストの親を登録
    public GameObject[] confirmation_;
    // はい と いいえ を登録
    public GameObject[] yes_no_;

    // 詳細ウィンドウを取得
    public GameObject frame_left;
    // プレイヤーを取得
    private GameObject[] players_;
    // 渡すを選んだ状態か
    private bool isGive = false;

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
    }

    void OnEnable()
    {
        if (units_ == null) return;
        cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[pos_num_y * 2 + pos_num_x]);
        parent_canvas.GetComponent<ReadyManager>().SetUnit(players_[unit_num]);
    }
	
	// Update is called once per frame
	void Update () {
        if (players_ == null) return;
        ControllerAxis();
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
            case 5:
                Mode5();
                break;
            default:
                break;
        }
    }

    // アイテムの操作をしたいユニットを選ぶモード
    void Mode0()
    {
        // 下キーで１つ下に 下から２番目のときに押すとカーソルは動かずに表示されているユニットがずれる
        if (key_flg_[3] && unit_num < units_.Count - 2 && unit_num/2<row-1)
        {
            if (pos_num_y == 2 && unit_num / 2 <row - 2)
            {
                Vector3 pos = units_parent.GetComponent<RectTransform>().anchoredPosition;
                pos.y += 90.0f;
                units_parent.GetComponent<RectTransform>().anchoredPosition = pos;
            }
            else
            {
                pos_num_y++;
            }
            unit_num += 2;
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[unit_num]);
            ShosaiUpdate();
            key_flg_[3] = false;
        }
        // 上キーで１つ上に 上から２番目のときに押すとカーソルは動かずに表示されているユニットがずれる
        if (key_flg_[2] && unit_num > 1)
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
            ShosaiUpdate();
            key_flg_[2] = false;
        }

        if (key_flg_[0] && pos_num_x == 0)
        {
            pos_num_x++;
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[pos_num_y * 2 + pos_num_x]);
            unit_num++;
            ShosaiUpdate();
            key_flg_[0] = false;
        }
        if (key_flg_[1] && pos_num_x == 1)
        {
            pos_num_x--;
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[pos_num_y * 2 + pos_num_x]);
            unit_num--;
            ShosaiUpdate();
            key_flg_[1] = false;
        }

        if (Input.GetButtonDown("O"))
        {
            if (first_unit == null)
            {
                first_unit = players_[unit_num];
                controll_list_[0].transform.parent.gameObject.SetActive(true);
                first_item_list.SetActive(false);
                mode_ = 1;
                ResetNum();
            }
            else
            {
                if (players_[unit_num] != first_unit)
                {

                    second_unit = players_[unit_num];
                    if (!isGive) mode_ = 2;
                    else mode_ = 5;
                    UIChange();
                    ResetNum();
                    DisplayUpdate();
                }
            }
        }
        
        if (Input.GetButtonDown("X"))
        {
            if (first_unit == null)
            {
                ResetNum();
                transform.parent.GetComponent<ReadyManager>().ModeChange(0);
            }
            else
            {
                mode_ = 1;
                isGive = false;
                controll_list_[0].transform.parent.gameObject.SetActive(true);
                first_item_list.SetActive(false);
                ResetNum();
                for(int i = 0; i < players_.Length; i++)
                {
                    if (players_[i] == first_unit) unit_num = i;
                }
                ShosaiUpdate();
            }
        }
    }

    // 操作を選択するモード
    void Mode1()
    {
        if (key_flg_[3])
        {
            pos_num_y++;
            if (pos_num_y >= controll_list_.Length) pos_num_y = 0;
            key_flg_[3] = false;
        }
        if (key_flg_[2])
        {
            pos_num_y--;
            if (pos_num_y < 0) pos_num_y = controll_list_.Length - 1;
            key_flg_[2] = false;
        }
        cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(controll_list_[pos_num_y]);
        if (Input.GetButtonDown("X"))
        {
            controll_list_[0].transform.parent.gameObject.SetActive(false);
            first_item_list.SetActive(true);
            first_unit = null;
            mode_ = 0;
            ResetNum();
            ShosaiUpdate();
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[0]);
        }
        if (Input.GetButtonDown("O")){
            switch (pos_num_y)
            {
                case 0:
                    cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[0]);
                    mode_ = 0;
                    break;
                case 1:
                    cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[0]);
                    mode_ = 0;
                    isGive = true;
                    break;
                case 2:
                    mode_ = 3;
                    cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(warehouse_controll[0]);
                    first_items = frame_left.GetComponent<UIItemList>().GetItemTexts();
                    warehouse_items[repository_num] = UIs_parent[2].GetComponent<UIRepository>().GetItemTexts(repository_num);
                    pos_num_y = 0;
                    break;
                case 3:
                    mode_ = 4;
                    confirmation_[0].SetActive(false);
                    confirmation_[1].SetActive(false);
                    yes_no_[0].transform.parent.gameObject.SetActive(false);
                    yes_no_[1].transform.parent.gameObject.SetActive(false);
                    pos_num_y = 0;
                    first_items = frame_left.GetComponent<UIItemList>().GetItemTexts();
                    cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(first_items[0]);
                    break;
                default:
                    break;
            }
            controll_list_[0].transform.parent.gameObject.SetActive(false);
            first_item_list.SetActive(true);
            ResetNum();
            UIChange();
            if(mode_ != 0)
            {
                for (int i = 0; i < players_.Length; i++)
                {
                    if (players_[i] == first_unit) unit_num = i;
                }
            }
            ShosaiUpdate();
        }
    }

    // 交換をするモード
    void Mode2()
    {
        if (first_items.Count == 0)
        {
            first_items = frame_left.GetComponent<UIItemList>().GetItemTexts();
        }
        if(second_items.Count == 0)
        {
            second_items = UIs_parent[1].GetComponent<UIItemList>().GetItemTexts();
        }

        if (first_unit_item == null)
        {
            if (key_flg_[3])
            {
                pos_num_y++;
                if (pos_num_y >= first_items.Count) pos_num_y = 0;
                key_flg_[3] = false;
            }
            if (key_flg_[2])
            {
                pos_num_y--;
                if (pos_num_y < 0) pos_num_y = first_items.Count - 1;
                key_flg_[2] = false;
            }
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(first_items[pos_num_y]);
            if (Input.GetButtonDown("X"))
            {
                mode_ = 0;
                second_unit = null;
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[0]);
                ResetNum();
                UIChange();
                ShosaiUpdate();
            }
            if (Input.GetButtonDown("O"))
            {
                first_unit_item = first_unit.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>()._itemprefablist[pos_num_y];
                pos_num_y = 0;
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(second_items[pos_num_y]);
            }
        }
        else
        {
            if (Input.GetButtonDown("X"))
            {
                first_unit_item = null;
            }
            if (key_flg_[3])
            {
                pos_num_y++;
                if (pos_num_y >= second_items.Count) pos_num_y = 0;
                key_flg_[3] = false;
            }
            if (key_flg_[2])
            {
                pos_num_y--;
                if (pos_num_y < 0) pos_num_y = second_items.Count - 1;
                key_flg_[2] = false;
            }
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(second_items[pos_num_y]);
            if (Input.GetButtonDown("O"))
            {
                second_unit_item = second_unit.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>()._itemprefablist[pos_num_y];

                ChangeItem(first_unit, second_unit, first_unit_item, second_unit_item);


                first_unit_item = null;
                second_unit_item = null;
                DisplayUpdate();
                first_items = frame_left.GetComponent<UIItemList>().GetItemTexts();
                second_items = UIs_parent[1].GetComponent<UIItemList>().GetItemTexts();
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
            if (key_flg_[3])
            {
                pos_num_y++;
                if (pos_num_y >= warehouse_controll.Length) pos_num_y = 0;
                key_flg_[3] = false;
            }
            if (key_flg_[2])
            {
                pos_num_y--;
                if (pos_num_y < 0) pos_num_y = warehouse_controll.Length - 1;
                key_flg_[2] = false;
            }
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(warehouse_controll[pos_num_y]);
            if (Input.GetButtonDown("X"))
            {
                mode_ = 1;
                controll_list_[0].transform.parent.gameObject.SetActive(true);
                first_item_list.SetActive(false);
                ResetNum();
                for (int i = 0; i < players_.Length; i++)
                {
                    if (players_[i] == first_unit) unit_num = i;
                }
                UIChange();
                ShosaiUpdate();
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(controll_list_[0]);
            }
            if (Input.GetButtonDown("O"))
            {
                warehouse_mode = pos_num_y + 1;
                if (warehouse_mode == 1)
                {
                    if(first_items.Count>0)
                    cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(first_items[0]);
                }
                
                pos_num_y = 0;
            }
        }
        else {
            if (warehouse_mode == 1)
            {
                if (first_items.Count <= 0)
                {
                    warehouse_mode = 0;
                    pos_num_y = 0;
                    cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(warehouse_controll[0]);
                    return;
                }
                if (key_flg_[3])
                {
                    pos_num_y++;
                    if (pos_num_y >= first_items.Count) pos_num_y = 0;
                    key_flg_[3] = false;
                }
                if (key_flg_[2])
                {
                    pos_num_y--;
                    if (pos_num_y < 0) pos_num_y = first_items.Count - 1;
                    key_flg_[2] = false;
                }
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(first_items[pos_num_y]);
                if (Input.GetButtonDown("O"))
                {
                    GameObject.Find("RepositoryManager").GetComponent<RepositoryManager>().AddItem(first_unit,first_unit.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>()._itemprefablist[pos_num_y]);
                    first_unit.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>().RemoveItem();
                    DisplayUpdate();
                    UIs_parent[2].GetComponent<UIRepository>().RepositoryUpdate();
                    first_items = frame_left.GetComponent<UIItemList>().GetItemTexts();
                    pos_num_y = 0;
                    if(first_items.Count > 0)
                    cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(first_items[pos_num_y]);
                }
            }
            if (warehouse_mode == 2)
            {
                if (warehouse_items[repository_num].Count == 0)
                {
                    if (repository_num == 0) repository_num = 1;
                    else repository_num = 0;
                    warehouse_items[repository_num] = UIs_parent[2].GetComponent<UIRepository>().GetItemTexts(repository_num);
                    UIs_parent[2].GetComponent<UIRepository>().DisplayChange();
                    if(warehouse_items[repository_num].Count == 0)
                    {
                        if(repository_num==1) UIs_parent[2].GetComponent<UIRepository>().DisplayChange();
                        warehouse_mode = 0;
                        repository_num = 0;
                        pos_num_y = 0;
                        cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(warehouse_controll[0]);
                        return;
                    }
                }
                if(key_flg_[0] || key_flg_[1])
                {
                    if (repository_num == 0)
                    {
                        warehouse_items[1] = UIs_parent[2].GetComponent<UIRepository>().GetItemTexts(1);
                        if (warehouse_items[1].Count > 0)
                        {
                            repository_num = 1;
                            pos_num_y = 0;
                            UIs_parent[2].GetComponent<UIRepository>().DisplayChange();
                        }
                    }
                    else {
                        warehouse_items[0] = UIs_parent[2].GetComponent<UIRepository>().GetItemTexts(0);
                        if (warehouse_items[0].Count > 0)
                        {
                            repository_num = 0;
                            pos_num_y = 0;
                            UIs_parent[2].GetComponent<UIRepository>().DisplayChange();
                        }
                    }
                    key_flg_[0] = false;
                    key_flg_[1] = false;
                }
                if (key_flg_[3])
                {
                    pos_num_y++;
                    if (pos_num_y >= warehouse_items[repository_num].Count) pos_num_y = 0;
                    key_flg_[3] = false;
                }
                if (key_flg_[2])
                {
                    pos_num_y--;
                    if (pos_num_y < 0) pos_num_y = warehouse_items[repository_num].Count - 1;
                    key_flg_[2] = false;
                }
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(warehouse_items[repository_num][pos_num_y]);
                if (Input.GetButtonDown("O"))
                {
                    if(repository_num==0)
                        GameObject.Find("RepositoryManager").GetComponent<RepositoryManager>().GetItem(first_unit, pos_num_y, "Item");
                    else
                        GameObject.Find("RepositoryManager").GetComponent<RepositoryManager>().GetItem(first_unit, pos_num_y, "Weapon");
                    DisplayUpdate();
                    UIs_parent[2].GetComponent<UIRepository>().RepositoryUpdate();
                    first_items = frame_left.GetComponent<UIItemList>().GetItemTexts();
                    warehouse_items[repository_num] = UIs_parent[2].GetComponent<UIRepository>().GetItemTexts(repository_num);
                    pos_num_y = 0;
                    if (warehouse_items[repository_num].Count > 0)
                        cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(warehouse_items[repository_num][pos_num_y]);
                }
            }
            if (Input.GetButtonDown("X"))
            {
                warehouse_mode = 0;
                repository_num = 0;
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
            if (throw_item_num < 0)
            {
                if (key_flg_[3])
                {
                    pos_num_y++;
                    if (pos_num_y >= first_items.Count) pos_num_y = 0;
                    key_flg_[3] = false;
                }
                if (key_flg_[2])
                {
                    pos_num_y--;
                    if (pos_num_y < 0) pos_num_y = first_items.Count - 1;
                    key_flg_[2] = false;
                }
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(first_items[pos_num_y]);
                if (Input.GetButtonDown("O"))
                {
                    throw_item_num = pos_num_y;
                    confirmation_[0].SetActive(true);
                    yes_no_[0].transform.parent.gameObject.SetActive(true);
                    yes_no_[1].transform.parent.gameObject.SetActive(true);
                    pos_num_x = 1;
                }
                if (Input.GetButtonDown("X"))
                {
                    mode_ = 1;
                    controll_list_[0].transform.parent.gameObject.SetActive(true);
                    first_item_list.SetActive(false);
                    ResetNum();
                    for (int i = 0; i < players_.Length; i++)
                    {
                        if (players_[i] == first_unit) unit_num = i;
                    }
                    UIChange();
                    ShosaiUpdate();
                    cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(controll_list_[0]);
                }
            }
            else
            {
                if (key_flg_[0] && pos_num_x == 0)
                {
                    pos_num_x++;
                    key_flg_[0] = false;
                }
                if (key_flg_[1] && pos_num_x == 1)
                {
                    pos_num_x--;
                    key_flg_[1] = false;
                }
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(yes_no_[pos_num_x]);
                if (Input.GetButtonDown("O"))
                {
                    if (pos_num_x == 0)
                    {
                        confirmation_[0].SetActive(false);
                        confirmation_[1].SetActive(true);
                        yes_no_[0].transform.parent.gameObject.SetActive(false);
                        yes_no_[1].transform.parent.gameObject.SetActive(false);
                        cursor_.SetActive(false);
                    }
                    else
                    {
                        throw_item_num = -1;
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
                first_unit.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>().GiveItem(first_unit.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>()._itemprefablist[throw_item_num]);
                first_unit.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>().RemoveItem();
                DisplayUpdate();
                first_items = frame_left.GetComponent<UIItemList>().GetItemTexts();
                pos_num_y = 0;
                cursor_.SetActive(true);
                cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(first_items[pos_num_y]);
                throw_item_num = -1;
                confirmation_[1].SetActive(false);
            }
        }
    }

    void Mode5()
    {
        if (first_items.Count == 0)
        {
            first_items = frame_left.GetComponent<UIItemList>().GetItemTexts();
        }
        if (second_items.Count == 0)
        {
            second_items = UIs_parent[1].GetComponent<UIItemList>().GetItemTexts();
        }

        if (key_flg_[3])
        {
            pos_num_y++;
            if (pos_num_y >= first_items.Count) pos_num_y = 0;
            key_flg_[3] = false;
        }
        if (key_flg_[2])
        {
            pos_num_y--;
            if (pos_num_y < 0) pos_num_y = first_items.Count - 1;
            key_flg_[2] = false;
        }
        cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(first_items[pos_num_y]);
        if (Input.GetButtonDown("X"))
        {
            mode_ = 0;
            second_unit = null;
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[0]);
            ResetNum();
            UIChange();
            ShosaiUpdate();
        }
        if (Input.GetButtonDown("O"))
        {
            GiveItem(second_unit, first_unit, first_unit.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>()._itemprefablist[pos_num_y]);
            DisplayUpdate();
            first_items = frame_left.GetComponent<UIItemList>().GetItemTexts();
            second_items = UIs_parent[1].GetComponent<UIItemList>().GetItemTexts();
            pos_num_y = 0;
            cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(first_items[pos_num_y]);
        }
    }

    // 数値をまとめて０にする
    void ResetNum()
    {
        unit_num = 0;
        pos_num_x = 0;
        pos_num_y = 0;
    }

    void DisplayUpdate()
    {
        parent_canvas.GetComponent<ReadyManager>().SetUnit(first_unit);
        frame_left.GetComponent<UILeftStatus>().SetData(first_unit.GetComponent<Character>());
        frame_left.GetComponent<UIItemList>().SetData(first_unit.GetComponent<Character>(), first_item_list);
        if (second_unit == null) return;
        parent_canvas.GetComponent<ReadyManager>().SetUnit(second_unit);
        UIs_parent[1].GetComponent<UILeftStatus>().SetData(second_unit.GetComponent<Character>());
        UIs_parent[1].GetComponent<UIItemList>().SetData(second_unit.GetComponent<Character>(), second_item_list);
        
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
            else if(mode_ == 5)
            {
                if (i == 1) UIs_parent[i].SetActive(true);
                else UIs_parent[i].SetActive(false);
            }
            else
            {
                if (i == mode_-1) UIs_parent[i].SetActive(true);
                else UIs_parent[i].SetActive(false);
            }
        }
    }

    public void Init()
    {
        // モードに応じたUI表示をする
        UIChange();
        controll_list_[0].transform.parent.gameObject.SetActive(false);
        // 使えるユニットを取得
        units_ = new List<GameObject>();
        units_ = units_parent.GetComponent<Item_UnitList>().GetUnitTexts();
        first_items = new List<GameObject>();
        second_items = new List<GameObject>();
        warehouse_items = new List<GameObject>[2];
        for (int i = 0; i < 2; i++)
        {
            warehouse_items[i] = new List<GameObject>();
        }
        // カーソルの位置を合わせる
        cursor_.GetComponent<RectTransform>().anchoredPosition = CanvasAnchoredPosition(units_[0]);

        players_ = units_parent.GetComponent<Item_UnitList>().GetPlayers();
        parent_canvas.GetComponent<ReadyManager>().SetUnit(players_[unit_num]);
        ShosaiUpdate();
        row = (units_.Count + 1) / 2;
    }

    void ShosaiUpdate()
    {
        parent_canvas.GetComponent<ReadyManager>().SetUnit(players_[unit_num]);
        frame_left.GetComponent<UILeftStatus>().SetData(players_[unit_num].GetComponent<Character>());
        frame_left.GetComponent<UIItemList>().SetData(players_[unit_num].GetComponent<Character>(),first_item_list);
    }

    public void ChangeItem(GameObject _chara, GameObject _chara2, GameObject _item, GameObject _item2)
    {
        var c1 = _chara.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>();
        var c2 = _chara2.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>();

        //Destroy(_item);
        //Destroy(_item2);

        //c1.AddItem(_item2);
        //c2.AddItem(_item);

        c1.ChangeItem(_item2,_item);
        c2.ChangeItem(_item, _item2);

        c1.RemoveItem();
        c2.RemoveItem();
    }

    /// <summary>
    /// アイテムを渡す
    /// </summary>
    /// <param name="_givechara">アイテムをもらうキャラ</param>
    /// <param name="_getchara">アイテムを渡すキャラ</param>
    /// <param name="_item">渡すアイテム</param>
    public void GiveItem(GameObject _givechara,GameObject _getchara,GameObject _item)
    {

        var c1 = _givechara.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>();
        var c2 = _getchara.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>();

        //アイテムに空きがあった場合実行
        if (c1._itemprefablist.Count < 5)
        {
            c1.AddItem(_item);
            c2.GiveItem(_item);

            c1.RemoveItem();
            c2.RemoveItem();
        }
        else
        {
            //もらう側のアイテムがいっぱいのとき
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
