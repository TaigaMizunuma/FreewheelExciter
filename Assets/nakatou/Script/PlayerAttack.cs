using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private int MinCost = 1;//最小攻撃範囲
    private int MaxCost = 1;//最大攻撃範囲
    GameObject Now_pos;

    List<GameObject> attack_range = new List<GameObject>();//攻撃範囲のマス取得
    List<GameObject> serch_range = new List<GameObject>();//攻撃範囲のマス取得

    public bool range_line = false;//キャラクターの攻撃範囲が直線かそうじゃないか


    List<GameObject> InAttackRange_Enemy = new List<GameObject>();//攻撃範囲内のエネミー

    List<GameObject> InRange_Enemy = new List<GameObject>();//指定範囲内のエネミー

    List<GameObject> InRange_Player = new List<GameObject>();//指定範囲内のプレイヤー取得

    // Use this for initialization
    void Start()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position - new Vector3(0, -0.5f, 0), 0.2f, Vector3.down, out hit, 5.0f))
        {
            Now_pos = hit.transform.gameObject;
            Now_pos.GetComponent<Square_Info>().SetChara(gameObject);
        }
        Character c = gameObject.GetComponent<Character>();
        MinCost = c._range[0];
        MaxCost = c._range[1];
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void AttackReady()
    {
        var count = 0;
        InAttackRange_Enemy.Clear();

        foreach (GameObject atk in attack_range)
        {
            atk.GetComponent<Square_Info>().AttackRange();

            //攻撃範囲に何かいる時攻撃選択
            if (atk.GetComponent<Square_Info>().GetChara())
            {
                if (atk.GetComponent<Square_Info>().GetChara().tag == "Enemy")
                {
                    Debug.Log("エネミーが攻撃範囲内");
                    InAttackRange_Enemy.Add(atk.GetComponent<Square_Info>().GetChara());
                    FindObjectOfType<BattleFlowTest>().state_ = State_.player_attack_mode;
                }
                else
                {
                    count++;
                }
            }
            else
            {
                count++;
            }
        }

        //攻撃範囲内に何もいないとき攻撃キャンセル
        if (count == attack_range.Count)
        {
            Debug.Log("攻撃範囲内には何もいない");
            StartCoroutine(DelayMethod.DelayMethodCall(0.5f, () =>
            {
                AttackRelease();
            }));
            FindObjectOfType<SubMenuRenderer>().SubMenuStart();
            FindObjectOfType<BattleFlowTest>().state_ = State_.action_mode;
            count = 0;
        }
    }

    public void AttackRelease()
    {
        foreach (GameObject atk in attack_range)
        {
            atk.GetComponent<Square_Info>().DecisionEnd();
        }
        InAttackRange_Enemy.Clear();
    }


    /// <summary>
    ///ゲッター 攻撃範囲に入ってるエネミー
    /// </summary>
    /// <returns>攻撃範囲に入ってるエネミーのリスト</returns>
    public List<GameObject> GetInAttackRangeEnemy()
    {
        return InAttackRange_Enemy;
    }

    /// <summary>
    /// プレイヤーの攻撃時、攻撃範囲を調べる
    /// </summary>
    public void RangeSearch()
    {
        Character c = gameObject.GetComponent<Character>();
        MinCost = c._range[0];
        MaxCost = c._range[1];

        Retrieval();
        RetrievalRelease();

        AttackReady();
    }

    /// <summary>
    /// スキル用 周囲のマスを調べてプレイヤーがいたら返す
    /// </summary>
    /// <param name="cost">調べるマスの範囲</param>
    /// <returns></returns>
    public List<GameObject> RangeSerch2(int cost)
    {
        MinCost = 1;
        MaxCost = cost;

        InRange_Player.Clear();

        Retrieval2();

        return InRange_Player;
    }

    /// <summary>
    /// スキル用 指定範囲内のマスを調べてエネミーを返す
    /// </summary>
    /// <param name="cost">マス範囲指定</param>
    /// <returns></returns>
    public List<GameObject> RangeSerch3(int cost)
    {
        MinCost = 1;
        MaxCost = cost;
       
        InRange_Enemy.Clear();

        Retrieval2();

        foreach(GameObject obj in serch_range)
        {
            if (obj.GetComponent<Square_Info>().GetChara() && obj.GetComponent<Square_Info>().GetChara().tag == "Enemy")
            {
                InRange_Enemy.Add(obj.GetComponent<Square_Info>().GetChara());
            }
        }

        return InRange_Enemy;
    }

    /// <summary>
    /// 攻撃用　サーチ
    /// </summary>
    public void Retrieval()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position - new Vector3(0, -0.5f, 0), 0.2f, Vector3.down, out hit, 5.0f))
        {
            Now_pos = hit.transform.gameObject;
            Now_pos.GetComponent<Square_Info>().SetChara(gameObject);
        }
        attack_range.Clear();

        Square_Info si_;
        si_ = Now_pos.GetComponent<Square_Info>();
        GameObject[] nears_ = si_.GetNear();
        for (int i=0;i<nears_.Length;i++)
        {
            if (nears_[i] == null) continue;
            if (nears_[i].GetComponent<Square_Info>().GetChara() != null)
            {
                if (nears_[i].GetComponent<Square_Info>().GetChara().tag == "Player")
                {   
                    continue;
                } 
            }
            Square_Info n_si_ = nears_[i].GetComponent<Square_Info>();
            if (1 <= MaxCost)
            {
                //n_si_.AttackRange();
                if (!attack_range.Contains(n_si_.gameObject))
                {
                    attack_range.Add(n_si_.gameObject);
                }
            }
            if (1 < MaxCost)
            {
                if(!range_line)Retrieval(nears_[i], 2);
                else
                {
                    Retrieval(nears_[i], 2, i);
                }
            }
        }
    }

    void Retrieval(GameObject near, int cost_)
    {
        Square_Info si_;
        si_ = near.GetComponent<Square_Info>();
        GameObject[] nears_ = si_.GetNear();
        foreach (GameObject n in nears_)
        {
            if (n.GetComponent<Square_Info>().GetChara() != null)
            {
                if (n.GetComponent<Square_Info>().GetChara().tag == "Player")
                {
                    continue;
                }
            }
            Square_Info n_si_ = n.GetComponent<Square_Info>();

            if (cost_ <= MaxCost)
            {
                //n_si_.AttackRange();
                if (!attack_range.Contains(n_si_.gameObject))
                {
                    attack_range.Add(n_si_.gameObject);
                }
            }
            if (cost_ < MaxCost) Retrieval(n, cost_ + 1);
        }
    }

    void Retrieval(GameObject near, int cost_,int num)
    {
        Square_Info si_;
        si_ = near.GetComponent<Square_Info>();
        GameObject near_ = si_.GetNear()[num];
        if (near_.GetComponent<Square_Info>().GetChara() != null)
        {
            if (near_.GetComponent<Square_Info>().GetChara().tag == "Player")
            {
                return;
            }
        }
        Square_Info n_si_ = near_.GetComponent<Square_Info>();

        if (cost_ <= MaxCost)
        {
            //n_si_.AttackRange();
            if (!attack_range.Contains(n_si_.gameObject))
            {
                attack_range.Add(n_si_.gameObject);
            }
        }
        if (cost_ < MaxCost) Retrieval(near_, cost_ + 1,num);
    }

    void RetrievalRelease()
    {
        Square_Info si_;
        si_ = Now_pos.GetComponent<Square_Info>();
        GameObject[] nears_ = si_.GetNear();
        foreach (GameObject n in nears_)
        {
            if (n.GetComponent<Square_Info>().GetChara() != null)
            {
                if (n.GetComponent<Square_Info>().GetChara().tag == "Player") continue;
            }
            Square_Info n_si_ = n.GetComponent<Square_Info>();
            if (1 < MinCost)
            {
                n_si_.DecisionEnd();
                if (!n_si_.GetRange()) attack_range.Remove(n_si_.gameObject);
                RetrievalRelease(n, 2);
            }
        }
    }

    void RetrievalRelease(GameObject near, int cost_)
    {
        Square_Info si_;
        si_ = near.GetComponent<Square_Info>();
        GameObject[] nears_ = si_.GetNear();
        foreach (GameObject n in nears_)
        {
            if (n.GetComponent<Square_Info>().GetChara() != null)
            {
                if (n.GetComponent<Square_Info>().GetChara().tag == "Player") continue;
            }
            Square_Info n_si_ = n.GetComponent<Square_Info>();

            if (cost_ < MinCost)
            {
                n_si_.DecisionEnd();
                if (!n_si_.GetRange()) attack_range.Remove(n_si_.gameObject);
                RetrievalRelease(n, cost_ + 1);
            }
        }
    }

    /// <summary>
    /// スキル用　サーチ
    /// </summary>
    public void Retrieval2()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position - new Vector3(0, -0.5f, 0), 0.2f, Vector3.down, out hit, 5.0f))
        {
            Now_pos = hit.transform.gameObject;
            Now_pos.GetComponent<Square_Info>().SetChara(gameObject);
        }

        serch_range.Clear();

        Square_Info si_;
        si_ = Now_pos.GetComponent<Square_Info>();
        GameObject[] nears_ = si_.GetNear();
        for (int i = 0; i < nears_.Length; i++)
        {
            if (nears_[i] == null) continue;
            if (nears_[i].GetComponent<Square_Info>().GetChara() != null)
            {
                if (nears_[i].GetComponent<Square_Info>().GetChara().tag == "Player")
                {
                    var obj = nears_[i].gameObject.GetComponent<Square_Info>().GetChara();
                    if ((!InRange_Player.Contains(obj)) && (obj != gameObject))
                    {
                        InRange_Player.Add(nears_[i].GetComponent<Square_Info>().GetChara());
                    }
                    continue;
                }
            }
            Square_Info n_si_ = nears_[i].GetComponent<Square_Info>();
            if (1 <= MaxCost)
            {
                //n_si_.AttackRange();
                if (!serch_range.Contains(n_si_.gameObject))
                {
                    serch_range.Add(n_si_.gameObject);
                }
            }
            if (1 < MaxCost)
            {
                if (!range_line) Retrieval2(nears_[i], 2);
                else
                {
                    Retrieval2(nears_[i], 2, i);
                }
            }
        }
    }

    void Retrieval2(GameObject near, int cost_)
    {
        Square_Info si_;
        si_ = near.GetComponent<Square_Info>();
        GameObject[] nears_ = si_.GetNear();
        foreach (GameObject n in nears_)
        {
            if (n.GetComponent<Square_Info>().GetChara() != null)
            {
                if (n.GetComponent<Square_Info>().GetChara().tag == "Player")
                {
                    var obj = n.gameObject.GetComponent<Square_Info>().GetChara();
                    if ((!InRange_Player.Contains(obj)) && (gameObject != obj))
                    {
                        InRange_Player.Add(n.GetComponent<Square_Info>().GetChara());
                    }
                    continue;
                }
            }
            Square_Info n_si_ = n.GetComponent<Square_Info>();

            if (cost_ <= MaxCost)
            {
                //n_si_.AttackRange();
                if (!serch_range.Contains(n_si_.gameObject))
                {
                    serch_range.Add(n_si_.gameObject);
                }
            }
            if (cost_ < MaxCost) Retrieval2(n, cost_ + 1);
        }
    }

    void Retrieval2(GameObject near, int cost_, int num)
    {
        Square_Info si_;
        si_ = near.GetComponent<Square_Info>();
        GameObject near_ = si_.GetNear()[num];
        if (near_.GetComponent<Square_Info>().GetChara() != null)
        {
            if (near_.GetComponent<Square_Info>().GetChara().tag == "Player")
            {
                var obj = near_.gameObject.GetComponent<Square_Info>().GetChara();
                if ( (!InRange_Player.Contains(obj)) && (gameObject != obj) )
                {
                    InRange_Player.Add(near_.GetComponent<Square_Info>().GetChara());
                }
                return;
            }
        }
        Square_Info n_si_ = near_.GetComponent<Square_Info>();

        if (cost_ <= MaxCost)
        {
            //n_si_.AttackRange();
            if (!serch_range.Contains(n_si_.gameObject))
            {
                serch_range.Add(n_si_.gameObject);
            }
        }
        if (cost_ < MaxCost) Retrieval2(near_, cost_ + 1, num);
    }

}
