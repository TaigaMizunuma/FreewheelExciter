using UnityEngine;

/// <summary>
/// アニメーションとか死んだときの処理とかクラス
/// </summary>
public class CharaStatus : MonoBehaviour
{
    //public Vector3 cameraPosOffset; // fps用

    Animator _anim;

    Character _charaStatus;

    public bool _deadFlag = false;


    void Start()
    {
        _anim = GetComponent<Animator>();
        _charaStatus = GetComponent<Character>();
    }

    void Update()
    {
        //エネミーだけアニメーション同期可能(仮)
        if(gameObject.tag == "Enemy")
        {
            
        }

        //死んだとき
        if (_charaStatus._totalhp <= 0)
        {
            _deadFlag = true;
            gameObject.tag = "Dead";
            if (FindObjectOfType<BattleFlowTest>()._randamEnemy == this.gameObject)
            {
                //敵の行動予定キャラならリセット
                FindObjectOfType<BattleFlowTest>()._randamEnemy = null;
            }

            StartCoroutine(DelayMethod.DelayMethodCall(1.5f, () =>
            {
                _anim.SetBool("damage", true);
                Destroy(gameObject, 3.0f);
            }));
        }
    }

    /// <summary>
    /// 敵の攻撃用
    /// </summary>
    /// <param name="target"></param>
    public void attack_to_player(GameObject target)
    {
        FindObjectOfType<BattleFlowTest>().state_ = State_.enemy_attack_mode;

        Debug.Log("敵の攻撃！");
        var effect = Instantiate(Resources.Load("Eff_Hit_6"), target.transform.position, Quaternion.identity);
        FindObjectOfType<AudioManager>().PlaySe("GunShot");
        Destroy(effect, 1.0f);

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

        FindObjectOfType<BattleFlowTest>().DamegeUIInit(target, p_dm);
        FindObjectOfType<StatusUI>().setPlayerDamage(p_dm);

        FindObjectOfType<StatusUI>().setUnitStatus(
                                target.GetComponent<Character>()._name,
                                target.GetComponent<Character>()._totalhp,
                                target.GetComponent<Character>()._totalMaxhp);

        //演出上の間
        StartCoroutine(DelayMethod.DelayMethodCall(1.0f, () =>
        {
            FindObjectOfType<BattleFlowTest>().EnemyAttackEnd(gameObject, target, dm);
        }));
    }
}
