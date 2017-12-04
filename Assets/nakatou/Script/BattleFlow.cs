//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

///// <summary>
///// バトルの大まかな流れを制御  バックアップ
///// </summary>
//public class BattleFlow : MonoBehaviour
//{
//    //ステート遷移
//    public State_ state_;

//    //ターゲットカーソル
//    GameObject rayBox;

//    //現在のカーソルの下にいるキャラ
//    public GameObject _nowChooseChar;

//    // どちらの陣営のターンか表示
//    public Text _TurnText;

//    //BGM,SE再生
//    AudioManager m_audio;

//    //遅延メソッド
//    DelayMethod _delayMethod;

//    //メニューキャンバス
//    public GameObject _canvas;

//    //敵ターンに行動する敵キャラ
//    public GameObject _randamEnemy;


//    void Start()
//    {
//        rayBox = FindObjectOfType<RayBox>().gameObject;
//        m_audio = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
//        _delayMethod = FindObjectOfType<DelayMethod>().GetComponent<DelayMethod>();

//        _TurnText.text = "第１章 \n PlayerTurn";

//        m_audio.PlayBgm("battle1");
//        //Debug.Log("キャラ選択");

//        StartCoroutine(_delayMethod.DelayMethodCall(1.0f, () =>
//        {
//            Turn_TextReset();
//        }));
//    }

//    void Update()
//    {
//        //仮 勝敗判定
//        if (!GameObject.FindGameObjectWithTag("Enemy"))
//        {
//            _TurnText.text = "GameClear!!";
//            StartCoroutine(_delayMethod.DelayMethodCall(2.0f, () =>
//            {
//                SceneManager.LoadScene(0);
//            }));
//        }
//        else if (!GameObject.FindGameObjectWithTag("Player"))
//        {
//            _TurnText.text = "GameOver...";
//            StartCoroutine(_delayMethod.DelayMethodCall(2.0f, () =>
//            {
//                SceneManager.LoadScene(0);
//            }));
//        }

//        //遷移
//        switch (state_)
//        {
//            //キャラ選択
//            case State_.simulation_mode:
//                if (Input.GetKeyDown(KeyCode.Space))
//                {
//                    ChooseChara();
//                }
//                break;

//            //キャラ選択中
//            case State_.menu_mode:
//                if (Input.GetKeyDown(KeyCode.M))
//                {
//                    MenuEnd();
//                }
//                break;

//            //移動中
//            case State_.move_mode:

//                break;

//            //攻撃中
//            case State_.player_attack_mode:

//                Ray ray = new Ray(rayBox.transform.position, -rayBox.transform.up);
//                RaycastHit hit = new RaycastHit();
//                //カーソルの下になんかいて
//                if (Physics.Raycast(ray, out hit, 1000.0f))
//                {
//                    //スペース(仮)が押されて
//                    if (Input.GetKeyDown(KeyCode.Space))
//                    {
//                        //カーソルがエネミーをさしていたら
//                        if (hit.transform.tag == "Enemy")
//                        {
//                            Debug.Log("自キャラの攻撃！");

//                            //とりあえずのエフェクト表示&SE
//                            Instantiate(Resources.Load("Eff_Hit_6"), hit.transform.position, Quaternion.identity);
//                            m_audio.PlaySe("GunShot");

//                            //戦闘結果計算
//                            FindObjectOfType<BattleManager>().BattleSetup(_nowChooseChar, hit.transform.gameObject);

//                            //とりあえず反撃エフェクト
//                            Instantiate(Resources.Load("Eff_Hit_6"), _nowChooseChar.transform.position, Quaternion.identity);
//                            m_audio.PlaySe("GunShot");

//                            //結果をUIに渡す
//                            FindObjectOfType<StatusUI>().setUnitStatus(
//                                hit.collider.GetComponent<Character>()._name,
//                                hit.collider.GetComponent<Character>()._totalhp,
//                                hit.collider.GetComponent<Character>()._totalMaxhp);

//                            //選択キャラの攻撃可能状態を解除
//                            _nowChooseChar.GetComponent<Move_System>().AttackRelease();
//                            //自分ターン終了
//                            TurnEnd();
//                        }
//                        //カーソルがエネミー以外のとこにあると
//                        else
//                        {
//                            Debug.Log("何もいません");
//                        }
//                    }
//                }

//                break;

//            //敵ターン
//            case State_.enemy_turn:

//                break;

//            default:
//                break;
//        }
//    }

//    /// <summary>
//    /// 自陣ターン終わり
//    /// </summary>
//    public void TurnEnd()
//    {
//        if (FindObjectOfType<MenuManager>().MenuCount == 1)
//        {
//            FindObjectOfType<MenuManager>().IniMainMenu();
//            FindObjectOfType<RayBox>().move_ = true;
//        }
//        //ステートを変更 & UI表示
//        state_ = State_.enemy_turn;
//        _TurnText.color = new Color(255, 0, 0, 255);
//        _TurnText.text = "Enemy Turn";
//        m_audio.PlaySe("TurnStart");

//        //敵の行動をすぐ実行すると分かりにくいので遅延させる
//        StartCoroutine(_delayMethod.DelayMethodCall(1.0f, () =>
//        {
//            //行動予定エネミーのHPが０なら
//            if (_randamEnemy == null)
//            {
//                //別のエネミーに
//                _randamEnemy = GameObject.FindGameObjectWithTag("Enemy");
//            }

//            //敵が一体でもいれば
//            if (_randamEnemy)
//            {
//                //敵の行動を開始
//                _randamEnemy.GetComponent<EnemyBase>().SetNextGoal(_randamEnemy.GetComponent<EnemyBase>().target_square);
//            }
//        }));
//    }

//    /// <summary>
//    /// 敵陣ターン終了
//    /// </summary>
//    public void EnemyTurnEnd()
//    {
//        _TurnText.color = new Color(0, 0, 255, 255);
//        _TurnText.text = "Player Turn";
//        m_audio.PlaySe("TurnStart");

//        //すぐ実行すると分かりにくいので遅延させる
//        StartCoroutine(_delayMethod.DelayMethodCall(1.0f, () =>
//        {
//            Turn_TextReset();
//            state_ = State_.simulation_mode;
//        }));
//    }

//    /// <summary>
//    /// ターン表示を初期化
//    /// </summary>
//    void Turn_TextReset()
//    {
//        _TurnText.text = "";
//    }

//    /// <summary>
//    /// 行動キャラ選択
//    /// </summary>
//    public void ChooseChara()
//    {
//        Ray ray = new Ray(FindObjectOfType<RayBox>().transform.gameObject.transform.position, -transform.up);
//        RaycastHit hit = new RaycastHit();
//        //カーソルの場所になにがあるか
//        if (Physics.Raycast(ray, out hit, 1000.0f))
//        {
//            //カーソル固定
//            FindObjectOfType<RayBox>().move_ = false;

//            _nowChooseChar = hit.transform.gameObject;

//            //メインメニュー表示 
//            FindObjectOfType<MenuManager>().GetComponent<MenuManager>().IniMainMenu();
//            //se
//            m_audio.PlaySe("choose");
//            state_ = State_.menu_mode;
//        }
//    }


//    //移動ボタン用
//    public void moveBt()
//    {
//        if (state_ == State_.menu_mode)
//        {
//            m_audio.PlaySe("choose");
//            _nowChooseChar.GetComponent<Move_System>().Retrieval();
//            FindObjectOfType<MenuManager>().GetComponent<MenuManager>().IniMainMenu();//メインメニュー非表示
//            state_ = State_.move_mode;
//        }
//    }

//    //攻撃ボタン用
//    public void attackBt()
//    {
//        if (state_ == State_.menu_mode)
//        {
//            m_audio.PlaySe("choose");
//            _nowChooseChar.GetComponent<Move_System>().AttackReady();
//            FindObjectOfType<MenuManager>().GetComponent<MenuManager>().IniMainMenu();//メインメニュー非表示
//        }
//    }

//    //メニュー閉じる
//    public void MenuEnd()
//    {
//        if (state_ == State_.menu_mode)
//        {
//            m_audio.PlaySe("choose");
//            _nowChooseChar = null;//選んでたキャラリセット
//            FindObjectOfType<MenuManager>().GetComponent<MenuManager>().IniMainMenu();//メインメニュー非表示
//            FindObjectOfType<RayBox>().move_ = true;
//            //プログラム上の問題で遅延
//            StartCoroutine(_delayMethod.DelayMethodCall(0.5f, () =>
//            {
//                changeST(State_.simulation_mode);
//            }));
//        }
//    }

//    /// <summary>
//    /// 任意のステートに変更
//    /// </summary>
//    /// <param name="st">State_.name</param>
//    public void changeST(State_ st)
//    {
//        state_ = st;
//    }

//    /// <summary>
//    /// 仮
//    /// </summary>
//    /// <param name="obj"></param>
//    public void setRandamEnemy(GameObject obj)
//    {
//        _randamEnemy = obj;
//    }
//}