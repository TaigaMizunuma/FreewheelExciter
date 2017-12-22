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
}
