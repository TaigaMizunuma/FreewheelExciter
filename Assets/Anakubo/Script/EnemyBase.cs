using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {
    // 目的地
    public GameObject target_square;
    // 移動するルートを登録
    private List<GameObject> route_;
    // 動いているか
    private bool moving_ = false;
    // 今いるマスを取得する用
    private GameObject now_pos;
    // 経路探索用の数
    private int cost = 99;
    // 実際のコスト
    public int first_cost = 3;
    // 現在使ったコスト
    private int now_cost = 0;
    // 移動時のListの番号用
    private int move_list_num = 0;
    // 移動のLerp用
    private Vector3 start_pos;
    private Vector3 goal_pos;
    // Lerpのタイマー
    private float move_timer = 0.0f;
    // 移動速度
    public float move_speed = 2.0f;
    // コストが余っている状態か
    private bool cost_remainder = false;

    // 攻撃範囲(距離)
    public int attak_range = 1;
    // 攻撃範囲(直線か円範囲か) falseだと円範囲
    public bool line_range = true;
    // プレイヤーキャラが攻撃できる位置にいるか
    private bool player_in_range = false;
    // 攻撃範囲のマスを登録する
    private List<GameObject> a_range;

    private GameObject in_range_player;

    // Use this for initialization
    void Awake () {
        route_ = new List<GameObject>();
        a_range = new List<GameObject>();
        RaycastHit hit;
        if (Physics.SphereCast(transform.position - new Vector3(0, -0.5f, 0), 0.2f, Vector3.down, out hit, 5.0f))
        {
            now_pos = hit.transform.gameObject;
            now_pos.GetComponent<Square_Info>().SetChara(gameObject);
        }
        attak_range = GetComponent<Character>()._range[1];
    }
	
	// Update is called once per frame
	void Update () {
        // 移動
        if (moving_) Move();
        else if (Input.GetKeyDown(KeyCode.RightShift)) SetNextGoal(target_square);
    }

    public void SetNextGoal(GameObject next)
    {
        player_in_range = false;
        if (next == now_pos)
        {
            cost_remainder = true;
            return;
        }
        
        target_square = next;
        RangeSearch();
        Retrieval();
        RushAI();
        Search();
        DisplayEnd();
        cost_remainder = false;
        Debug.Log(target_square.name);
    }

    void Retrieval()
    {
        Square_Info si_;
        si_ = now_pos.GetComponent<Square_Info>();
        si_.Decision();
        GameObject[] nears_ = si_.GetNear();
        foreach (GameObject n in nears_)
        {
            if (n.GetComponent<Square_Info>().GetChara() != null)
            {
                if (n.GetComponent<Square_Info>().GetChara().tag == "Player") continue;
            }
            Square_Info n_si_ = n.GetComponent<Square_Info>();
            if (cost - n_si_.GetCost() >= 0 && n_si_.CanSetCost(cost, now_pos) && first_cost >= n_si_.GetCost())
            {
                if (n.GetComponent<Square_Info>().GetChara() == null) n_si_.Decision();

                Retrieval(n, cost);
            }
        }
    }

    void Retrieval(GameObject near, int cost_)
    {
        Square_Info si_;
        si_ = near.GetComponent<Square_Info>();
        GameObject[] nears_ = si_.GetNear();
        cost_ -= si_.GetCost();
        foreach (GameObject n in nears_)
        {
            if (n.GetComponent<Square_Info>().GetChara() != null)
            {
                if (n.GetComponent<Square_Info>().GetChara().tag == "Player") continue;
            }
            Square_Info n_si_ = n.GetComponent<Square_Info>();
            if (cost_ - n_si_.GetCost() >= 0 && n_si_.CanSetCost(cost_, near)&&first_cost>= n_si_.GetCost())
            {
                if (n.GetComponent<Square_Info>().GetChara() == null) n_si_.Decision();
                Retrieval(n, cost_);
            }
        }
    }

    void Search()
    {
        if (target_square == now_pos)
        {
            if(player_in_range)
            {
                GetComponent<CharaStatus>().attack_to_player(in_range_player);
            }
            return;
        }
        if (target_square.GetComponent<Square_Info>().GetChara() != null)
        {
            if(target_square.GetComponent<Square_Info>().GetChara().tag == "Player")
            {
                GameObject[] g_near = target_square.GetComponent<Square_Info>().GetNear();
                target_square = null;
                foreach (GameObject g in g_near)
                {
                    if (g.GetComponent<Square_Info>().GetChara() != null) continue;
                    if (target_square == null) target_square = g;
                    else if (target_square.GetComponent<Square_Info>().GetMaxCost() < g.GetComponent<Square_Info>().GetMaxCost())
                    {
                        target_square = g;
                    }
                }
            }
        }
        route_.Add(target_square);
        while (route_[route_.Count - 1] != now_pos)
        {
            //if(route_[route_.Count - 1].GetComponent<Square_Info>().SearchRoute().GetComponent<Square_Info>().GetCost()<=first_cost)
            route_.Add(route_[route_.Count - 1].GetComponent<Square_Info>().SearchRoute());
        }
        if (route_.Count > 0)
        {
            route_.Reverse();
            while (route_[route_.Count - 1].GetComponent<Square_Info>().GetChara() != null)
            {
                route_.RemoveAt(route_.Count - 1);
            }
            SetPos(move_list_num);
            moving_ = true;
            now_cost += route_[move_list_num + 1].GetComponent<Square_Info>().GetCost();
            now_pos.GetComponent<Square_Info>().ResetChara();
        }
    }

    void SetPos(int num)
    {
        start_pos = route_[num].transform.position;
        goal_pos = route_[num + 1].transform.position;
        start_pos.y = transform.position.y;
        goal_pos.y = transform.position.y;
    }

    void Move()
    {
        move_timer += move_speed * Time.deltaTime;
        transform.position = Vector3.Lerp(start_pos, goal_pos, move_timer);
        if (move_timer > 1.0f)
        {
            move_list_num++;
            if (move_list_num < route_.Count - 1 && now_cost + route_[move_list_num + 1].GetComponent<Square_Info>().GetCost() <= first_cost)
            {
                SetPos(move_list_num);
                now_cost += route_[move_list_num + 1].GetComponent<Square_Info>().GetCost();
            }
            else
            {
                moving_ = false;
                RaycastHit hit;
                if (Physics.SphereCast(transform.position - new Vector3(0, -0.5f, 0), 0.2f, Vector3.down, out hit, 5.0f))
                {
                    now_pos = hit.transform.gameObject;
                    now_pos.GetComponent<Square_Info>().SetChara(gameObject);
                }
                
                move_list_num = 0;
                route_.Clear();
                if (now_cost < first_cost && !player_in_range)
                {
                    cost_remainder = true;
                }
                else
                {
                    now_cost = 0;
                }
            }
            move_timer = 0;
            if (moving_ == false && cost_remainder == false)
            {
                FindObjectOfType<BattleFlowTest>().EnemyTurnEnd();
            }
        }
    }

    public bool IsRemainder()
    {
        return cost_remainder;
    }

    public GameObject GetNowPos()
    {
        return now_pos;
    }

    void RangeSearch()
    {
        int move_cost = first_cost;
        Square_Info si_;
        si_ = now_pos.GetComponent<Square_Info>();
        si_.SetNowPosCost(move_cost);
        si_.Decision();
        GameObject[] nears_ = si_.GetNear();
        foreach (GameObject n in nears_)
        {
            if (n.GetComponent<Square_Info>().GetChara() != null)
            {
                if (n.GetComponent<Square_Info>().GetChara().tag == "Player")
                {
                    player_in_range = true;
                    target_square = now_pos;
                    in_range_player = n.GetComponent<Square_Info>().GetChara();
                    break;
                }
            }
        }

        if (!player_in_range)
        {
            foreach (GameObject n in nears_)
            {
                
                Square_Info n_si_ = n.GetComponent<Square_Info>();
                if (move_cost - n_si_.GetCost() >= 0 && n_si_.CanSetCost(move_cost, now_pos))
                {
                    n_si_.Decision();

                    RangeSearch(n, move_cost + attak_range);

                }
            }
            PlayerSearch();
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
        if (a_range != null)
        {
            foreach (GameObject r in a_range)
            {
                if (r == range_) exist_ = true;
            }
        }
        if (!exist_) a_range.Add(range_);
    }

    void PlayerSearch()
    {
        foreach (GameObject r in a_range)
        {
            if (r.GetComponent<Square_Info>().GetChara()!=null)
            {
                if (r.GetComponent<Square_Info>().GetChara().tag == "Player")
                {
                    player_in_range = true;
                    target_square = r;
                    in_range_player = r.GetComponent<Square_Info>().GetChara();
                    WaitEnemy we_ = GetComponent<WaitEnemy>();
                    if(we_!=null)we_.RushStart();
                    break;
                }
            }
        }
        if (player_in_range)
        {
            
            GameObject[] g_near = target_square.GetComponent<Square_Info>().GetNear();
            target_square = null;
            foreach (GameObject g in g_near)
            {
                if (g.GetComponent<Square_Info>().GetChara() != null) continue;
                if (target_square == null) target_square = g;
                else if (target_square.GetComponent<Square_Info>().GetMaxCost() < g.GetComponent<Square_Info>().GetMaxCost())
                {
                    target_square = g;
                }
            }
        }
        a_range.Clear();
    }

    void RushAI()
    {
        if (player_in_range) return;
        RushEnemy re_ = GetComponent<RushEnemy>();
        if (re_ != null && !player_in_range) target_square = re_.SetTarget();
        LowHPSnipeEnemy lhs_ = GetComponent<LowHPSnipeEnemy>();
        if (lhs_ != null && !player_in_range) target_square = lhs_.SetTarget();
        WaitEnemy we_ = GetComponent<WaitEnemy>();
        if (we_ != null && !player_in_range) target_square = we_.SetTarget();
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

                RangeSearch(n, move_cost + attak_range);
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
}
