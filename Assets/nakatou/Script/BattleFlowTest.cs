using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// 現在のステート
/// </summary>
public enum State_
{
    simulation_mode,
    menu_mode,
    move_mode,
    action_mode,
    weapon_mode,//武器選択
    player_attack_mode,
    enemy_counter_mode,
    skill_mode,
    item_mode,
    stay_mode,
    enemy_turn,
    enemy_move_mode,
    enemy_attack_mode,
    player_counter_mode
}

/// <summary>
/// バトルの大まかな流れを制御
/// </summary>
public class BattleFlowTest : MonoBehaviour
{
    //ステート遷移
    public State_ state_;

    //ターゲットカーソル
    GameObject rayBox;

    //現在のカーソルの下にいるキャラ
    public GameObject _nowChooseChar;

    // どちらの陣営のターンか表示
    public Text _TurnText;

    //BGM,SE再生
    AudioManager m_audio;

    //敵ターンに行動する敵キャラ
    public GameObject _randamEnemy;

    //ターン出現時に操作不能にする時間
    [SerializeField]
    private float waitTime = 1.0f;

    //攻撃中か？
    bool attacking = false;

    //攻撃中キャラ
    GameObject _nowAttackChara;
    //反撃するキャラ
    GameObject _nowCounterChara;

    //ダメージ表示用の変数
    int e_dm;
    int p_dm;

    //仮
    bool once = false;
    bool choose = false;
    int count = 0;
    List<GameObject> weaponUIs = new List<GameObject>();
    List<GameObject> weapons = new List<GameObject>();

    public GameObject PlayerRedGage;
    public GameObject EnemyRedGage;

    public GameObject Shousai;

    bool GameEnd = false;


    void Awake()
    {
        PlayerRedGage = GameObject.Find("playerDamageGage");
        EnemyRedGage = GameObject.Find("enemyDamageGage");
    }
    void Start()
    {
        /*メニュー制御不可*/
        FindObjectOfType<MenuManager>().GetMainControlFlag(true);
        FindObjectOfType<SubMenuRenderer>().GetSubControlFlag(true);
        rayBox = FindObjectOfType<RayBox>().gameObject;
        rayBox.GetComponent<RayBox>().move_ = false;
        m_audio = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        Shousai = Instantiate(Resources.Load("Shosai"), GameObject.Find("Canvas").transform) as GameObject;
        Shousai.SetActive(false);
        
        _TurnText.text = "第１章 \n PlayerTurn";

        m_audio.PlayBgm("battle1");

        StartCoroutine(DelayMethod.DelayMethodCall(1.0f, () =>
        {
            Turn_TextReset();
            /*メニュー制御可*/
            FindObjectOfType<MenuManager>().GetMainControlFlag(false);
            FindObjectOfType<SubMenuRenderer>().GetSubControlFlag(false);
            rayBox.GetComponent<RayBox>().move_ = true;
        }));
    }

    void Update()
    {
        //仮 勝敗判定
        if(!GameObject.FindGameObjectWithTag("Enemy"))
        {
            _TurnText.text = "GameClear!!";
            GameEnd = true;
            StartCoroutine(DelayMethod.DelayMethodCall(3.0f, () =>
            {
                SceneManager.LoadScene("Story");
            }));
        }
        else if(!GameObject.FindGameObjectWithTag("Player"))
        {
            _TurnText.text = "GameOver...";
            GameEnd = true;
            StartCoroutine(DelayMethod.DelayMethodCall(3.0f, () =>
            {
                SceneManager.LoadScene("GameOver");
            }));
        }
        if (GameEnd) return;

        //遷移
        switch(state_)
        {
            //キャラ選択
            case State_.simulation_mode:

                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("O")) && !Shousai.activeInHierarchy)//○ボタン予定
                {
                    StartCoroutine(DelayMethod.DelayMethodCall(0.1f, () =>
                     {
                         ChooseChara();
                     }));
                }

                if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetButtonDown("R1"))
                {
                    Ray serch = new Ray(rayBox.transform.position, -rayBox.transform.up);
                    RaycastHit hiton = new RaycastHit();
                    //カーソルの下になんかいて
                    if (Physics.Raycast(serch, out hiton, 1000.0f))
                    {
                        if (hiton.transform.tag == "Player")
                        {
                            Shousai.SetActive(!Shousai.activeInHierarchy);
                            FindObjectOfType<RayBox>().move_ = false;
                            FindObjectOfType<MenuManager>().GetMainControlFlag(true);
                            if (Shousai.activeInHierarchy)
                            {
                                FindObjectOfType<Shosai>()._chara = hiton.transform.gameObject;
                            }
                            else
                            {
                                FindObjectOfType<RayBox>().move_ = true;
                                FindObjectOfType<MenuManager>().GetMainControlFlag(false);
                            }
                        }
                    }
                }
                break;

              //キャラがいないマスならメニュー開く(使わない)
            case State_.menu_mode:

                break;

                //移動中
            case State_.move_mode:
                //アニメ再生とか入れる予定
                break;

                //移動後の行動選択
            case State_.action_mode:
                //現状何もなし
                break;

            //武器選択
            case State_.weapon_mode:
                if (!once)
                {
                    var items = _nowChooseChar.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>()._itemprefablist;
                    count = 0;
                    if (items == null)
                    {
                        once = true;
                        choose = true;
                    }
                    foreach (var obj in items)
                    {
                        if (obj.GetComponent<Weapon>())
                        {
                            GameObject ui = Instantiate(
                                Resources.Load("WeaponUI"),
                                GameObject.Find("Canvas1").transform.Find("Frame").transform) as GameObject;
                            ui.transform.localPosition = new Vector3(0, count * -100, 0);
                            ui.transform.Find("Text").GetComponent<Text>().text = obj.GetComponent<Weapon>()._name;
                            ui.GetComponent<Image>().color = new Color32(170, 170, 170, 170);
                            weapons.Add(obj);
                            weaponUIs.Add(ui);
                            count++;
                        }
                    }
                    count = 0;
                    once = true;
                }
                if(!choose)
                {
                    foreach(var obj in weaponUIs)
                    {
                        obj.GetComponent<Image>().color = new Color32(170, 170, 170, 170);
                    }
                    weaponUIs[count].GetComponent<Image>().color = new Color32(255, 255, 255, 255);

                    if(Input.GetAxis("AxisY") == 1 || Input.GetAxis("Vertical") == 1)
                    {
                        if (count == 0) return;
                        count--;
                        _nowChooseChar.GetComponent<Character>().Equipment(weapons[count]);
                    }
                    if (Input.GetAxis("AxisY") == -1 || Input.GetAxis("Vertical") == -1)
                    {
                        if (count == weaponUIs.Count - 1) return;
                        count++;
                        _nowChooseChar.GetComponent<Character>().Equipment(weapons[count]);
                    }
                    if(Input.GetButtonDown("O") || Input.GetKeyDown(KeyCode.Space))
                    {
                        choose = true;
                    }
                }
                else
                {
                    foreach (var obj in weaponUIs)
                    { 
                        obj.SetActive(false);
                    }
                    
                    _nowChooseChar.GetComponent<Character>().Equipment(weapons[count]);
                    choose = false;
                    once = false;
                    count = 0;
                    weapons.Clear();
                    weaponUIs.Clear();
                    _nowChooseChar.GetComponent<PlayerAttack>().RangeSearch();
                }

                break;

            case State_.skill_mode:
                if (!once)
                {
                    var skill = _nowChooseChar.GetComponent<Character>()._skillprefablist.GetComponent<SkillPrefabList>()._skillprefablist;
                    count = 0;
                    if (skill == null)
                    {
                        once = true;
                        choose = true;
                    }
                    foreach (var obj in skill)
                    {
                        if (obj.GetComponent<CommandSkill>())
                        {
                            if (obj.GetComponent<CommandSkill>()._activ)
                            {
                                GameObject ui = Instantiate(
                                    Resources.Load("WeaponUI"),
                                    GameObject.Find("Canvas1").transform.Find("Frame").transform) as GameObject;
                                ui.transform.localPosition = new Vector3(0, count * -100, 0);
                                ui.transform.Find("Text").GetComponent<Text>().text = obj.GetComponent<CommandSkill>()._name;
                                ui.GetComponent<Image>().color = new Color32(170, 170, 170, 170);
                                weapons.Add(obj);
                                weaponUIs.Add(ui);
                                count++;
                            }
                        }
                    }
                    count = 0;
                    once = true;
                }
                if (!choose)
                {
                    foreach (var obj in weaponUIs)
                    {
                        obj.GetComponent<Image>().color = new Color32(170, 170, 170, 170);
                    }
                    weaponUIs[count].GetComponent<Image>().color = new Color32(255, 255, 255, 255);

                    if (Input.GetAxis("AxisY") == 1 || Input.GetAxis("Vertical") == 1)
                    {
                        if (count == 0) return;
                        count--;
                    }
                    if (Input.GetAxis("AxisY") == -1 || Input.GetAxis("Vertical") == -1)
                    {
                        if (count == weaponUIs.Count - 1) return;
                        count++;
                    }
                    if (Input.GetButtonDown("O") || Input.GetKeyDown(KeyCode.Space))
                    {
                        choose = true;
                    }
                }
                else
                {
                    foreach (var obj in weaponUIs)
                    {
                        obj.SetActive(false);
                    }

                    _nowChooseChar.GetComponent<Character>()._skillprefablist.GetComponent<SkillPrefabList>().SkillEffect(_nowChooseChar,weapons[count]);
                    choose = false;
                    once = false;
                    weapons.Clear();
                    weaponUIs.Clear();
                    TurnEnd();
                    state_ = State_.stay_mode;
                }
                break;

                //道具選択
            case State_.item_mode:
                if (!once)
                {
                    var items = _nowChooseChar.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>()._itemprefablist;
                    count = 0;
                    foreach (var obj in items)
                    {
                        if (obj.GetComponent<Item>())
                        {
                            GameObject ui = Instantiate(
                                Resources.Load("WeaponUI"),
                                GameObject.Find("Canvas1").transform.Find("Frame").transform) as GameObject;
                            ui.transform.localPosition = new Vector3(0, count * -100, 0);
                            ui.transform.Find("Text").GetComponent<Text>().text = obj.GetComponent<Item>()._name;
                            ui.GetComponent<Image>().color = new Color32(170, 170, 170, 170);
                            weapons.Add(obj);
                            weaponUIs.Add(ui);
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        Debug.Log("使えるアイテムを持っていません");
                        FindObjectOfType<SubMenuRenderer>().SubMenuStart();
                        choose = false;
                        once = false;
                        weapons.Clear();
                        weaponUIs.Clear();
                        state_ = State_.action_mode;
                    }
                    count = 0;
                    once = true;
                }
                if (!choose)
                {
                    foreach (var obj in weaponUIs)
                    {
                        obj.GetComponent<Image>().color = new Color32(170, 170, 170, 170);
                        weaponUIs[count].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    }

                    if (Input.GetAxis("AxisY") == 1 || Input.GetAxis("Vertical") == 1)
                    {
                        if (count == 0) return;
                        count--;
                    }
                    if (Input.GetAxis("AxisY") == -1 || Input.GetAxis("Vertical") == -1)
                    {
                        if (count == weaponUIs.Count - 1) return;
                        count++;
                    }
                    if (Input.GetButtonDown("O") || Input.GetKeyDown(KeyCode.Space))
                    {
                        choose = true;
                    }
                }
                else
                {
                    foreach (var obj in weaponUIs)
                    {
                        obj.SetActive(false);
                    }
                    
                    _nowChooseChar.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>().UseItem(_nowChooseChar,weapons[count]);
                    choose = false;
                    once = false;
                    weapons.Clear();
                    weaponUIs.Clear();
                    TurnEnd();
                    state_ = State_.stay_mode;
                }
                //仕様未定
                break;

                //待機選択
            case State_.stay_mode:
                //ターン終了

                break;

            //攻撃対象の選択
            case State_.player_attack_mode:
                //仮実装 カーソルが敵に自動で照準
                if (!attacking)
                {
                    var range_enemy = _nowChooseChar.GetComponent<PlayerAttack>().GetInAttackRangeEnemy();
                    if (Input.GetAxis("AxisY") == 1 || Input.GetAxis("Vertical") == 1 || Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        if (count == 0)
                        {
                            count = range_enemy.Count - 1;
                        }
                        else
                        {
                            count--;
                        }
                    }
                    if (Input.GetAxis("AxisY") == -1 || Input.GetAxis("Vertical") == -1 || Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        if (count == range_enemy.Count - 1)
                        {
                            count = 0;
                        }
                        else
                        {
                            count++;
                        }
                    }
                    var pos = range_enemy[count].transform.position;
                    pos.y = rayBox.transform.position.y;
                    rayBox.transform.position = pos;
                }

                Ray ray = new Ray(rayBox.transform.position, -rayBox.transform.up);
                RaycastHit hit = new RaycastHit();
                //カーソルの下になんかいて
                if (Physics.Raycast(ray, out hit, 1000.0f))
                {
                    if(hit.transform.tag == "Enemy")
                    {
                        FindObjectOfType<StatusUI>().setBattleStatus(
                            _nowChooseChar.GetComponent<Character>()._name,
                            _nowChooseChar.GetComponent<Character>()._totalhp,
                            _nowChooseChar.GetComponent<Character>()._totalMaxhp,
                            _nowChooseChar.GetComponent<Character>()._total_attack,
                            _nowChooseChar.GetComponent<Character>()._hit,
                            _nowChooseChar.GetComponent<Character>()._critical,
                            hit.transform.GetComponent<Character>()._name,
                            hit.transform.GetComponent<Character>()._totalhp,
                            hit.transform.GetComponent<Character>()._totalMaxhp,
                            hit.transform.GetComponent<Character>()._total_attack,
                            hit.transform.GetComponent<Character>()._hit,
                            hit.transform.GetComponent<Character>()._critical);
                    }

                    //スペース(仮)が押されて && まだ攻撃してないなら
                    if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("O")) && !attacking)
                    {
                        //カーソルがエネミーをさしていたら
                        if (hit.transform.tag == "Enemy")
                        {
                            attacking = true;
                            count = 0;

                            Debug.Log("自キャラの攻撃！");
                            FindObjectOfType<RayBox>().move_ = false;

                            //とりあえずのエフェクト表示&SE
                            Instantiate(Resources.Load("Eff_Hit_6"), hit.transform.position, Quaternion.identity);
                            m_audio.PlaySe("GunShot");
                           
                            int e_nowhp = hit.transform.GetComponent<Character>()._totalhp;
                            int p_nowhp = _nowChooseChar.GetComponent<Character>()._totalhp;

                            //とりあえずゲージ表示
                            FindObjectOfType<StatusUI>().setactive(true);

                            FindObjectOfType<StatusUI>().setPlayerHpGage(
                                _nowChooseChar.GetComponent<Character>()._totalMaxhp, p_nowhp);
                            FindObjectOfType<StatusUI>().setEnemyHpGage(
                                hit.transform.GetComponent<Character>()._totalMaxhp, e_nowhp);
                            
                            PlayerRedGage.GetComponent<DamageGage>().setdamageGage(
                                _nowChooseChar.GetComponent<Character>()._totalMaxhp, p_nowhp);
                            EnemyRedGage.GetComponent<DamageGage>().setdamageGage(
                                hit.transform.GetComponent<Character>()._totalMaxhp, e_nowhp);

                            //戦闘結果計算
                            FindObjectOfType<BattleManager>().BattleSetup(_nowChooseChar, hit.transform.gameObject);

                            e_dm = e_nowhp - hit.transform.GetComponent<Character>()._totalhp;//敵ダメージ算出(仮)
                            p_dm = p_nowhp - _nowChooseChar.GetComponent<Character>()._totalhp;//プレイヤダメージ算出

                            DamegeUIInit(hit.transform.gameObject, e_dm);
                            FindObjectOfType<StatusUI>().setEnemyDamage(e_dm);

                            //攻撃キャラを一時的に保存
                            _nowCounterChara = hit.collider.gameObject;
                            _nowAttackChara = _nowChooseChar;

                            //結果をUIに渡す
                            FindObjectOfType<StatusUI>().setUnitStatus(
                                hit.collider.GetComponent<Character>()._name,
                                hit.collider.GetComponent<Character>()._totalhp,
                                hit.collider.GetComponent<Character>()._totalMaxhp);

                            //選択キャラの攻撃可能状態を解除
                            _nowChooseChar.GetComponent<PlayerAttack>().AttackRelease();

                            //演出上の間をおいてから敵の反撃へ
                            StartCoroutine(DelayMethod.DelayMethodCall(waitTime, () => 
                            {
                                state_ = State_.enemy_counter_mode;
                            }));
                        }
                        //プレイヤー
                        else if(hit.transform.tag == "Player")
                        {
                            //何もしない
                        }
                        //カーソルがエネミー以外のとこにあると
                        else
                        {
                            Debug.Log("そのマスには何もいません");
                        }
                    }
                }
                
                break;

            case State_.enemy_counter_mode:
                if (attacking)
                {
                    Debug.Log("敵の反撃！");
                    //とりあえずの反撃エフェクト表示&SE
                    Instantiate(Resources.Load("Eff_Hit_6"), _nowAttackChara.transform.position, Quaternion.identity);
                    m_audio.PlaySe("GunShot");
                    DamegeUIInit(_nowAttackChara, p_dm);

                    FindObjectOfType<StatusUI>().setPlayerDamage(p_dm);

                    //結果をUIに渡す
                    FindObjectOfType<StatusUI>().setUnitStatus(
                        _nowAttackChara.GetComponent<Character>()._name,
                        _nowAttackChara.GetComponent<Character>()._totalhp,
                        _nowAttackChara.GetComponent<Character>()._totalMaxhp);

                    //演出上の遅延
                    StartCoroutine(DelayMethod.DelayMethodCall(waitTime, () =>
                    {
                        TurnEnd();
                    }));
                   attacking = false;
                }              
                break;
            
                //敵ターン
            case State_.enemy_turn:
                FindObjectOfType<StatusUI>().setactive(false);
                //行動予定エネミーのHPが０なら
                if (_randamEnemy == null)
                {
                    //別のエネミーに
                    _randamEnemy = GameObject.FindGameObjectWithTag("Enemy");
                }

                //敵が一体でもいれば
                if (_randamEnemy)
                {
                    //敵の行動を開始
                    _randamEnemy.GetComponent<EnemyBase>().SetNextGoal(_randamEnemy.GetComponent<EnemyBase>().target_square);
                    FindObjectOfType<RayBox>().SetCameraPosition(_randamEnemy);
                    state_ = State_.enemy_move_mode;
                }
                break;

                //敵の移動
            case State_.enemy_move_mode:
                //敵のアニメーションなどを入れる予定
                
                break;

                //敵の攻撃
            case State_.enemy_attack_mode:
                //敵のアニメーションなどを入れる予定
                
                break;

                //プレイヤーの反撃(いらないかも)
            case State_.player_counter_mode:

                break;

            default:
                break;
        }
    }


    /// <summary>
    /// 行動キャラ選択
    /// </summary>
    public void ChooseChara()
    {
        m_audio.PlaySe("choose");//se

        Ray ray = new Ray(FindObjectOfType<RayBox>().transform.gameObject.transform.position, -transform.up);
        RaycastHit hit = new RaycastHit();
        //カーソルの場所になにがあるか
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            //プレイヤーだったとき移動開始
            if (hit.collider.tag == "Player")
            {
                //カーソル固定
                FindObjectOfType<RayBox>().move_ = false;

                _nowChooseChar = hit.transform.gameObject;
                _nowChooseChar.GetComponent<Move_System>().Retrieval();

                state_ = State_.move_mode;
            }
            //何もいなかった時メニュー表示
            //else if (hit.collider.tag == "Floor")
            //{
            //    FindObjectOfType<RayBox>().move_ = false;
            //    //FindObjectOfType<MenuManager>().IniMainMenu();
            //    state_ = State_.menu_mode;
            //}
        }
    }

    public void moveEnd()
    {
        FindObjectOfType<SubMenuRenderer>().SubMenuStart();
        FindObjectOfType<RayBox>().move_ = false;
        state_ = State_.action_mode;
    }

    /// <summary>
    /// 敵の攻撃終了で攻撃された味方キャラの反撃
    /// </summary>
    /// <param name="enemy">攻撃した敵</param>
    /// <param name="target">攻撃された味方</param>
    /// <param name="damage">反撃時のダメージ(ui用)</param>
    public void EnemyAttackEnd(GameObject enemy, GameObject target, int damage)
    {
        Debug.Log("自キャラの反撃！");
        Instantiate(Resources.Load("Eff_Hit_6"), enemy.transform.position, Quaternion.identity);
        m_audio.PlaySe("GunShot");
        DamegeUIInit(enemy, damage);

        FindObjectOfType<StatusUI>().setUnitStatus(
                                enemy.GetComponent<Character>()._name,
                                enemy.GetComponent<Character>()._totalhp,
                                enemy.GetComponent<Character>()._totalMaxhp);

        FindObjectOfType<StatusUI>().setEnemyDamage(damage);

        //演出上の間をおいてからプレイヤーのターンへ
        StartCoroutine(DelayMethod.DelayMethodCall(waitTime, () =>
        {
            EnemyTurnEnd();
        }));
    }

    /// <summary>
    /// 頭の上にUIでダメージを表示
    /// </summary>
    /// <param name="target">頭にだすキャラ</param>
    /// <param name="damage">ダメージ</param>
    public void DamegeUIInit(GameObject target, int damage)
    {
        //ダメージUI表示
        GameObject text = Instantiate(
            Resources.Load("DamageTxt"),
            GameObject.Find("Canvas1").transform.Find("frame")) as GameObject;
        //ワールド座標をスクリーン座標に変換
        var p = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);
        var retPosition = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GameObject.Find("Canvas1").GetComponent<RectTransform>(),
            p,
            Camera.main,
            out retPosition
        );
        text.transform.localPosition = retPosition;
        text.GetComponent<DamegeUI>().setDamegeTxt(damage);
    }

    /// <summary>
    /// 自陣ターン終了
    /// </summary>
    public void TurnEnd()
    {
        if (state_ == State_.enemy_counter_mode || state_ == State_.action_mode || state_ == State_.skill_mode || state_ == State_.item_mode)
        {
            //待機選択時
            if (state_ == State_.action_mode) FindObjectOfType<SubMenuRenderer>().SubMenuStart();
            
            //ステートを変更 & UI表示
            _TurnText.color = new Color(255, 0, 0, 255);
            _TurnText.text = "Enemy Turn";
            m_audio.PlaySe("TurnStart");
            FindObjectOfType<ExpGage>().Enabled(false);

            //演出上の遅延
            StartCoroutine(DelayMethod.DelayMethodCall(waitTime, () =>
            {
                Turn_TextReset();
                state_ = State_.enemy_turn;
            }));
        }
    }

    /// <summary>
    /// 敵陣ターン終了
    /// </summary>
    public void EnemyTurnEnd()
    {
        _TurnText.color = new Color(0, 0, 255, 255);
        _TurnText.text = "Player Turn";
        m_audio.PlaySe("TurnStart");

        FindObjectOfType<StatusUI>().setactive(false);
        FindObjectOfType<ExpGage>().Enabled(false);

        //演出上の遅延
        StartCoroutine(DelayMethod.DelayMethodCall(waitTime, () => 
        {
            Turn_TextReset();
            FindObjectOfType<RayBox>().move_ = true;
            state_ = State_.simulation_mode;
        }));
    }

    /// <summary>
    /// ターン表示を初期化
    /// </summary>
    void Turn_TextReset()
    {
        _TurnText.text = "";
    }


    //特技ボタン用
    public void SkillBt()
    {
        if (state_ == State_.action_mode)
        {
            m_audio.PlaySe("choose");
            FindObjectOfType<SubMenuRenderer>().SubMenuStart();//サブメニュー非表示

            //プログラムの問題で遅延
            StartCoroutine(DelayMethod.DelayMethodCall(0.5f, () =>
            {
                state_ = State_.skill_mode;
            }));
        }
    }

    public void ItemBt()
    {
        if (state_ == State_.action_mode)
        {
            m_audio.PlaySe("choose");
            FindObjectOfType<SubMenuRenderer>().SubMenuStart();//サブメニュー非表示

            //プログラムの問題で遅延
            StartCoroutine(DelayMethod.DelayMethodCall(0.5f, () =>
            {
                state_ = State_.item_mode;
            }));
        }
    }

    //攻撃ボタン用
    public void attackBt()
    {
        if(state_ == State_.action_mode)
        {
            m_audio.PlaySe("choose");
            FindObjectOfType<SubMenuRenderer>().SubMenuStart();//サブメニュー非表示
            
            //プログラムの問題で遅延
            StartCoroutine(DelayMethod.DelayMethodCall(0.5f, () =>
            {
                state_ = State_.weapon_mode;
            }));
        }
    }

    //メニュー閉じる(未使用)
    public void MenuEnd()
    {
        if(state_ == State_.menu_mode)
        {
            m_audio.PlaySe("choose");
            _nowChooseChar = null;//選んでたキャラリセット
            FindObjectOfType<RayBox>().move_ = true;
            //プログラム上の問題で遅延
            StartCoroutine(DelayMethod.DelayMethodCall(0.5f, () =>
            {
                changeST(State_.simulation_mode);
            }));
        }
    }

    /// <summary>
    /// 任意のステートに変更
    /// </summary>
    /// <param name="st">State_.name</param>
    public void changeST(State_ st)
    {
        state_ = st;
    }

    /// <summary>
    /// 仮
    /// </summary>
    /// <param name="obj"></param>
    public void setRandamEnemy(GameObject obj)
    {
        _randamEnemy = obj;
    }
}