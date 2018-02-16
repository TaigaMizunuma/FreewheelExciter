using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_System : MonoBehaviour
{
    // 移動コスト
    private int cost;
    // 初期コスト
    private int first_cost;
    // 今いるマスを取得する用
    private GameObject now_pos;
    // 移動するルートを登録
    List<GameObject> route_;
    // 移動のLerp用
    private Vector3 start_pos;
    private Vector3 goal_pos;
    // 動いているか
    private bool moving_ = false;
    // Lerpのタイマー
    private float move_timer = 0.0f;
    // 移動時のListの番号用
    private int move_list_num = 0;
    // 移動速度
    public float move_speed = 2.0f;

    // 攻撃しようとしているか
    private bool attack_mode = false;
    // 攻撃範囲のマス
    GameObject[] atk_range;
    List<GameObject> _range = new List<GameObject>();

    public GameObject rayBox;// 10/27 追加

    // Use this for initialization
    void Start ()
    {
        rayBox = FindObjectOfType<RayBox>().gameObject;
        first_cost = GetComponent<Character>()._totalmove;
        cost = first_cost;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position - new Vector3(0,-0.5f,0), 0.2f, Vector3.down, out hit, 5.0f))
        {
            now_pos = hit.transform.gameObject;
            now_pos.GetComponent<Square_Info>().SetChara(gameObject);
        }
        route_ = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        first_cost = GetComponent<Character>()._totalmove;
        cost = first_cost;
        // 選択したマスが移動可能なマスであれば移動を開始する
        if ((Input.GetKeyDown(KeyCode.Return)|| Input.GetButtonDown("O") || Input.GetKeyDown(KeyCode.Space)) && !moving_)
        {
            if (FindObjectOfType<BattleFlowTest>().state_ != State_.move_mode ||
                FindObjectOfType<BattleFlowTest>()._NowChooseChar != gameObject) return;// 10/31 追加


            Ray ray = new Ray(rayBox.transform.position, -rayBox.transform.up);// 10/27 追加
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.transform.tag == "Floor" && hit.transform.GetComponent<Square_Info>().move_cost < 100)
                {
                    Square_Info a = hit.transform.GetComponent<Square_Info>();
                    if (a.IsDecision() == false)
                    {
                        return;
                    }


                    FindObjectOfType<BattleFlowTest>().c_moving = true;
                    FindObjectOfType<RayBox>().move_ = false;


                    Search(hit.transform.gameObject);
                    GameObject[] obj = GameObject.FindGameObjectsWithTag("Floor");
                    foreach (GameObject g in obj)
                    {
                        g.GetComponent<Square_Info>().DecisionEnd();
                    }

                }
                else if (hit.transform.gameObject == FindObjectOfType<BattleFlowTest>()._NowChooseChar)
                {
                    FindObjectOfType<RayBox>().move_ = false;
                    GameObject[] obj = GameObject.FindGameObjectsWithTag("Floor");
                    foreach (GameObject g in obj)
                    {
                        g.GetComponent<Square_Info>().DecisionEnd();
                    }
                    FindObjectOfType<BattleFlowTest>().PlayerMoveEnd();
                }
            }
        }

        // 移動
        if (moving_) Move();
    }

    public void Retrieval() // 10/27 publicに変更
    {
        FindObjectOfType<RayBox>().move_ = true;
        Square_Info si_;
        si_ = now_pos.GetComponent<Square_Info>();
        si_.Decision();
        GameObject[] nears_ = si_.GetNear();
        foreach (GameObject n in nears_)
        {
            if (n.GetComponent<Square_Info>().GetChara() != null)
            {
                if (n.GetComponent<Square_Info>().GetChara().tag == "Enemy") continue;
            }
            Square_Info n_si_ = n.GetComponent<Square_Info>();
            if (cost - n_si_.GetCost() >= 0 && n_si_.CanSetCost(cost, now_pos)) 
            {
                if (n.GetComponent<Square_Info>().GetChara() == null) n_si_.Decision();

                Retrieval(n, cost);  
            }
        }
        rayBox.GetComponent<RayBox>().SetMovePlayer(gameObject);
    }

    void Retrieval(GameObject near,int cost_)
    {
        Square_Info si_;
        si_ = near.GetComponent<Square_Info>();
        GameObject[] nears_ = si_.GetNear();
        cost_ -= si_.GetCost();
        foreach (GameObject n in nears_)
        {
            if (n.GetComponent<Square_Info>().GetChara() != null)
            {
                if (n.GetComponent<Square_Info>().GetChara().tag == "Enemy") continue;
            }
            Square_Info n_si_ = n.GetComponent<Square_Info>();
            if (cost_ - n_si_.GetCost() >= 0 && n_si_.CanSetCost(cost_, near))
            {
                if (n.GetComponent<Square_Info>().GetChara() == null) n_si_.Decision();
                Retrieval(n, cost_);
            }
        }
    }

    void Search(GameObject hit)
    {
        route_.Add(hit);
        while (route_[route_.Count - 1] != now_pos)
        {
            route_.Add(route_[route_.Count - 1].GetComponent<Square_Info>().SearchRoute());
        }
        if (route_.Count > 0)
        {
            route_.Reverse();
            SetPos(move_list_num);
            moving_ = true;
            now_pos.GetComponent<Square_Info>().ResetChara();
            rayBox.GetComponent<RayBox>().SetMovePlayer(null);
        }
    }

    void Move()
    {
        move_timer += move_speed * Time.deltaTime;
        transform.position = Vector3.Lerp(start_pos, goal_pos, move_timer);
        if(move_timer > 1.0f)
        {
            move_list_num++;
            if (move_list_num < route_.Count -1)
            {
                SetPos(move_list_num);
            }
            else
            {
                moving_ = false;
                route_.Clear();
                SetNowPos();
                cost = first_cost;
                move_list_num = 0;
                FindObjectOfType<BattleFlowTest>().PlayerMoveEnd();// 10/31 追加
            }
            move_timer = 0;
        }
    }

    void SetPos(int num)
    {
        start_pos = route_[num].transform.position;
        goal_pos = route_[num + 1].transform.position;
        start_pos.y = transform.position.y;
        goal_pos.y = transform.position.y;
        transform.LookAt(goal_pos);
    }

    //public void AttackReady()
    //{
    //    var count = 0;
    //    //atk_range = now_pos.GetComponent<Square_Info>().GetNear();
    //    GetComponent<PlayerAttack>().Retrieval();
    //    _range = GetComponent<PlayerAttack>().GetAttackRange();
        
        
    //    foreach(GameObject atk in _range)
    //    {
    //        atk.GetComponent<Square_Info>().AttackRange();
            
    //        //攻撃範囲に何かいる時攻撃選択
    //        if(atk.GetComponent<Square_Info>().GetChara())
    //        {
    //            if(atk.GetComponent<Square_Info>().GetChara().tag == "Enemy")
    //            {
    //                Debug.Log("エネミーが攻撃範囲内");
    //                FindObjectOfType<RayBox>().move_ = true;
    //                FindObjectOfType<BattleFlowTest>().state_ = State_.player_attack_mode;
    //            }
    //            else
    //            {
    //                count++;

    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("攻撃範囲内には何もいない");
    //            count++;
    //        }
    //    }
        
    //    //攻撃範囲内に何もいないとき攻撃キャンセル
    //    if (count == _range.Count)
    //    {
    //        StartCoroutine(DelayMethod.DelayMethodCall(0.5f, () =>
    //        {
    //            AttackRelease();
    //        }));
    //        FindObjectOfType<SubMenuRenderer>().SubMenuStart();
    //        FindObjectOfType<BattleFlowTest>().state_ = State_.action_mode;
    //        count = 0;
    //    }
    //    attack_mode = true;
    //}

    //public void AttackRelease()
    //{
    //    foreach(GameObject atk in _range)
    //    {
    //        atk.GetComponent<Square_Info>().DecisionEnd();
    //    }
    //    _range.Clear();
    //    attack_mode = false;
    //}

    public GameObject GetNowPos()
    {
        return now_pos;
    }

    public void LineRend(GameObject square_)
    {
        if (!square_.GetComponent<Square_Info>().IsDecision()) return;

        List<GameObject> r_ = new List<GameObject>();
        r_.Add(square_);
        while (r_[r_.Count - 1] != now_pos)
        {
            r_.Add(r_[r_.Count - 1].GetComponent<Square_Info>().SearchRoute());
        }
        if (r_.Count > 0)
        {
            r_.Reverse();
        }
        GameObject.FindGameObjectWithTag("lRend").GetComponent<RouteLine>().LineRend(r_);
    }

    public void SetNowPos()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position - new Vector3(0, -0.5f, 0), 0.2f, Vector3.down, out hit, 5.0f))
        {
            now_pos = hit.transform.gameObject;
            now_pos.GetComponent<Square_Info>().SetChara(gameObject);
        }
    }

    public void RangeDisplay()
    {
        int move_cost = first_cost;
        Square_Info si_;
        si_ = now_pos.GetComponent<Square_Info>();
        si_.SetNowPosCost(move_cost);
        si_.Decision();
        GameObject[] nears_ = si_.GetNear();
        foreach (GameObject n in nears_)
        {
            Square_Info n_si_ = n.GetComponent<Square_Info>();
            if (move_cost - n_si_.GetCost() >= 0 && n_si_.CanSetCost(move_cost, now_pos))
            {
                n_si_.Decision();

                RangeSearch(n, move_cost);
            }
        }
    }

    public void DisplayEnd()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Floor");
        foreach (GameObject g in obj)
        {
            g.GetComponent<Square_Info>().DecisionEnd();
        }
    }

    void RangeSearch(GameObject near, int cost_)
    {
        AddRangeList(near);
        Square_Info si_;
        si_ = near.GetComponent<Square_Info>();
        GameObject[] nears_ = si_.GetNear();
        cost_ -= si_.GetCost();
        foreach (GameObject n in nears_)
        {

            Square_Info n_si_ = n.GetComponent<Square_Info>();
            if (cost_ - n_si_.GetCost() >= 0 && n_si_.CanSetCost(cost_, near))
            {
                n_si_.Decision();
                RangeSearch(n, cost_);
            }
        }
    }

    void AddRangeList(GameObject range_)
    {
        bool exist_ = false;
        if (_range != null)
        {
            foreach (GameObject r in _range)
            {
                if (r == range_) exist_ = true;
            }
        }
        if (!exist_) _range.Add(range_);
    }
}
