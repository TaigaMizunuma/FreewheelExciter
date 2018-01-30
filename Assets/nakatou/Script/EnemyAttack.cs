using UnityEngine;
using System.Collections.Generic;


public class EnemyAttack : MonoBehaviour
{
    private int MinCost = 1;//最小攻撃範囲
    private int MaxCost = 1;//最大攻撃範囲
    GameObject Now_pos;

    List<GameObject> attack_range = new List<GameObject>();//攻撃範囲のマス取得
    List<GameObject> InAttackRange_Player = new List<GameObject>();//攻撃範囲内のエネミー

    public bool range_line = false;//キャラクターの攻撃範囲が直線かそうじゃないか


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


    void Update()
    {
        
    }

    /// <summary>
    /// 攻撃準備状態
    /// </summary>
    public void AttackReady()
    {
        var count = 0;
        InAttackRange_Player.Clear();

        foreach (GameObject atk in attack_range)
        {
            atk.GetComponent<Square_Info>().AttackRange();

            //攻撃範囲に何かいる時攻撃選択
            if (atk.GetComponent<Square_Info>().GetChara())
            {
                if (atk.GetComponent<Square_Info>().GetChara().tag == "Player")
                {
                    Debug.Log("プレイヤーが攻撃範囲内");
                    InAttackRange_Player.Add(atk.GetComponent<Square_Info>().GetChara());
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
                FindObjectOfType<BattleFlowTest>().EnemyTurnEnd();
                AttackRelease();
            }));
            count = 0;
        }
        else
        {
            //一番近いプレイヤーを攻撃対象にセット
            GameObject minobj = null;
            foreach(var obj in InAttackRange_Player)
            {
                if(minobj==null)
                {
                    minobj = obj;
                    continue;
                }
                if(Vector3.Distance(gameObject.transform.position, minobj.transform.position) > 
                    Vector3.Distance(gameObject.transform.position, obj.transform.position))
                {
                    minobj = obj;
                }
            }
            AttackToPlayer(minobj); 
        }
    }

    /// <summary>
    /// 攻撃準備状態解除
    /// </summary>
    public void AttackRelease()
    {
        foreach (GameObject atk in attack_range)
        {
            atk.GetComponent<Square_Info>().DecisionEnd();
        }
    }

    /// <summary>
    /// エネミーの攻撃時、攻撃範囲を調べる
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
    /// エネミーの反撃時　反撃できるか判定
    /// </summary>
    /// <param name="target_player">攻撃してきたキャラ</param>
    public bool CounterRangeSearch(GameObject target_player)
    {
        Character c = gameObject.GetComponent<Character>();
        MinCost = c._range[0];
        MaxCost = c._range[1];

        Retrieval();
        RetrievalRelease();

        foreach (var atk in attack_range)
        {
            if (atk.GetComponent<Square_Info>().GetChara())
            {
                //自分の攻撃範囲内にいるキャラが攻撃してきたプレイヤーなら反撃可能
                if (atk.GetComponent<Square_Info>().GetChara() == target_player)
                {
                    return true;
                }
            }
        }
        return false;
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
        for (int i = 0; i < nears_.Length; i++)
        {
            if (nears_[i] == null) continue;
            if (nears_[i].GetComponent<Square_Info>().GetChara() != null)
            {
                if (nears_[i].GetComponent<Square_Info>().GetChara().tag == "Enemy")
                {
                    continue;
                }
            }
            Square_Info n_si_ = nears_[i].GetComponent<Square_Info>();
            if (1 <= MaxCost)
            {
                if (!attack_range.Contains(n_si_.gameObject))
                {
                    attack_range.Add(n_si_.gameObject);
                }
            }
            if (1 < MaxCost)
            {
                if (!range_line) Retrieval(nears_[i], 2);
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
                if (n.GetComponent<Square_Info>().GetChara().tag == "Enemy")
                {
                    continue;
                }
            }
            Square_Info n_si_ = n.GetComponent<Square_Info>();

            if (cost_ <= MaxCost)
            {
                if (!attack_range.Contains(n_si_.gameObject))
                {
                    attack_range.Add(n_si_.gameObject);
                }
            }
            if (cost_ < MaxCost) Retrieval(n, cost_ + 1);
        }
    }

    void Retrieval(GameObject near, int cost_, int num)
    {
        Square_Info si_;
        si_ = near.GetComponent<Square_Info>();
        GameObject near_ = si_.GetNear()[num];
        if (near_.GetComponent<Square_Info>().GetChara() != null)
        {
            if (near_.GetComponent<Square_Info>().GetChara().tag == "Enemy")
            {
                return;
            }
        }
        Square_Info n_si_ = near_.GetComponent<Square_Info>();

        if (cost_ <= MaxCost)
        {
            if (!attack_range.Contains(n_si_.gameObject))
            {
                attack_range.Add(n_si_.gameObject);
            }
        }
        if (cost_ < MaxCost) Retrieval(near_, cost_ + 1, num);
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
                if (n.GetComponent<Square_Info>().GetChara().tag == "Enemy") continue;
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
                if (n.GetComponent<Square_Info>().GetChara().tag == "Enemy") continue;
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
    /// 敵の攻撃用
    /// </summary>
    /// <param name="target"></param>
    public void AttackToPlayer(GameObject target)
    {
        FindObjectOfType<BattleFlowTest>().state_ = State_.enemy_attack_mode;

        Debug.Log("敵の攻撃！");
        //var effect = Instantiate(Resources.Load("Eff_Hit_6"), target.transform.position, Quaternion.identity);
        //FindObjectOfType<AudioManager>().PlaySe("GunShot");
        //Destroy(effect, 1.0f);

        //キャラの向き変更
        transform.LookAt(target.transform);

        int nowhp = GetComponent<Character>()._totalhp;
        int p_nowhp = target.GetComponent<Character>()._totalhp;
        //とりあえずHPゲージ表示
        FindObjectOfType<StatusUI>().setactive(true);
        FindObjectOfType<StatusUI>().setPlayerHpGage(target.GetComponent<Character>()._totalMaxhp, p_nowhp);
        FindObjectOfType<StatusUI>().setEnemyHpGage(GetComponent<Character>()._totalMaxhp, nowhp);

        FindObjectOfType<BattleFlowTest>().PlayerRedGage.GetComponent<DamageGage>().setdamageGage(target.GetComponent<Character>()._totalMaxhp, p_nowhp);
        FindObjectOfType<BattleFlowTest>().EnemyRedGage.GetComponent<DamageGage>().setdamageGage(GetComponent<Character>()._totalMaxhp, nowhp);

        FindObjectOfType<BattleManager>().BattleSetup(gameObject, target);

        int dm = nowhp - GetComponent<Character>()._totalhp;
        int p_dm = p_nowhp - target.GetComponent<Character>()._totalhp;

        FindObjectOfType<BattleFlowTest>().DamegeUI_Init(target, p_dm);
        FindObjectOfType<StatusUI>().setPlayerDamage(p_dm);

        FindObjectOfType<StatusUI>().setUnitStatus(
                                target.GetComponent<Character>()._name,
                                target.GetComponent<Character>()._totalhp,
                                target.GetComponent<Character>()._totalMaxhp);

        //すぐに行動しないように遅延
        StartCoroutine(DelayMethod.DelayMethodCall(1.0f, () =>
        {
            EnemyAttackEnd(gameObject, target, dm);
            AttackRelease();
        }));
    }

    /// <summary>
    /// 敵の攻撃終了で攻撃された味方キャラの反撃
    /// </summary>
    /// <param name="enemy">攻撃した敵</param>
    /// <param name="target">攻撃された味方</param>
    /// <param name="damage">反撃時のダメージ(ui用)</param>
    public void EnemyAttackEnd(GameObject enemy, GameObject target, int damage)
    {
        if (target.GetComponent<Character>()._HpState != Character.HP_State.Dead)
        {
            Debug.Log("自キャラの反撃！");
            //var effect = Instantiate(Resources.Load("Eff_Hit_6"), enemy.transform.position, Quaternion.identity);
            //FindObjectOfType<AudioManager>().PlaySe("GunShot");
            //Destroy(effect, 1.0f);

            //キャラの向き変更
            target.transform.LookAt(enemy.transform);

            FindObjectOfType<BattleFlowTest>().DamegeUI_Init(enemy, damage);

            FindObjectOfType<StatusUI>().setUnitStatus(
                                    enemy.GetComponent<Character>()._name,
                                    enemy.GetComponent<Character>()._totalhp,
                                    enemy.GetComponent<Character>()._totalMaxhp);

            FindObjectOfType<StatusUI>().setEnemyDamage(damage);
        }

        //すぐに行動しないように遅延
        StartCoroutine(DelayMethod.DelayMethodCall(1.0f, () =>
        {
            FindObjectOfType<BattleFlowTest>().EnemyTurnEnd();
        }));
    }
}
