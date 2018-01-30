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
        //死んだとき
        if (_charaStatus._totalhp <= 0)
        {
            _deadFlag = true;
           
            if (FindObjectOfType<BattleFlowTest>()._ActionEnemy == gameObject)
            {
                //敵の行動予定キャラならリセット
                FindObjectOfType<BattleFlowTest>()._ActionEnemy = null;
            }

            StartCoroutine(DelayMethod.DelayMethodCall(1.5f, () =>
            {
                _anim.CrossFade("Dead", 0.0f);
                Destroy(gameObject, 3.0f);
            }));
        }
    }
}
