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

    //武器、スキル、アイテム選択時のUI用
    bool once = false;
    bool choose = false;
    int count = 0;
    List<GameObject> weaponUIs = new List<GameObject>();
    List<GameObject> weapons = new List<GameObject>();

    //hpui用
    public GameObject PlayerRedGage;
    public GameObject EnemyRedGage;

    public GameObject Shousai;//詳細ステータス

    bool GameEnd = false;//仮

    Vector3 MoveStartPos;//キャンセル用
    GameObject battleui;//戦闘時UI;


    void Awake()
    {
        PlayerRedGage = GameObject.Find("playerDamageGage");
        EnemyRedGage = GameObject.Find("enemyDamageGage");
    }
    void Start()
    {
        /*メニュー制御不可*/
        FindObjectOfType<MenuManager>().SetMainControlFlag(true);
        FindObjectOfType<SubMenuRenderer>().GetSubControlFlag(true);
        rayBox = FindObjectOfType<RayBox>().gameObject;
        rayBox.GetComponent<RayBox>().move_ = false;
        m_audio = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        Shousai = Instantiate(Resources.Load("Shosai"), GameObject.Find("Canvas").transform) as GameObject;
        Shousai.SetActive(false);
        
        _TurnText.text = "第１章 \n PlayerTurn";

        m_audio.PlayBgm("battle1");

        // ボタンが押された時の処理を登録
        //GameObject.Find("Menu5_Attack").GetComponent<Button>().onClick.AddListener(() => attackBt());
        //GameObject.Find("Menu6_Skill").GetComponent<Button>().onClick.AddListener(() => SkillBt());
        //GameObject.Find("Menu7_Item").GetComponent<Button>().onClick.AddListener(() => ItemBt());
        //GameObject.Find("Menu8_Return").GetComponent<Button>().onClick.AddListener(() => TurnEnd());

        StartCoroutine(DelayMethod.DelayMethodCall(1.0f, () =>
        {
            Turn_TextReset();
            /*メニュー制御可*/
            FindObjectOfType<MenuManager>().SetMainControlFlag(false);
            FindObjectOfType<SubMenuRenderer>().GetSubControlFlag(false);
            rayBox.GetComponent<RayBox>().move_ = true;
        }));
    }

    void Update()
    {
        //仮 勝敗判定
        if (!GameObject.FindGameObjectWithTag("Enemy"))
        {
            _TurnText.text = "GameClear!!";
            GameEnd = true;
            StartCoroutine(DelayMethod.DelayMethodCall(3.0f, () =>
            {
                FindObjectOfType<Fade>().SetOutFade(true);
                FindObjectOfType<Fade>().SetSceneChangeSwitch(true);
                FindObjectOfType<Fade>().SetScene("Title");
            }));
        }
        else if(!GameObject.FindGameObjectWithTag("Player"))
        {
            _TurnText.text = "GameOver...";
            GameEnd = true;
            StartCoroutine(DelayMethod.DelayMethodCall(3.0f, () =>
            {
                
                FindObjectOfType<Fade>().SetOutFade(true);
                FindObjectOfType<Fade>().SetSceneChangeSwitch(true);
                FindObjectOfType<Fade>().SetScene("GameOver");
            }));
        }
        if (GameEnd) return;//ゲーム終了

        //カメラ追従
        if (state_ == State_.enemy_move_mode ||state_ == State_.enemy_attack_mode)
        {
            if (!_randamEnemy) setRandamEnemy(GameObject.FindGameObjectWithTag("Enemy"));
            FindObjectOfType<RayBox>().SetCameraPosition(_randamEnemy);
        }

        //メニューフラグ制御
        if ((state_ == State_.simulation_mode || state_ == State_.menu_mode) && !Shousai.activeInHierarchy)
        {
            FindObjectOfType<MenuManager>().SetMainControlFlag(false);
        }
        else
        {
            FindObjectOfType<MenuManager>().SetMainControlFlag(true);
        }

        //遷移
        switch (state_)
        {
            //キャラ選択
            case State_.simulation_mode:
                SimulationMode();
                break;

              //メインメニューを開いた状態
            case State_.menu_mode:
                //今のところ何もなし
                break;

                //移動中
            case State_.move_mode:
                //アニメ再生とか入れる予定
                MoveMode();

                break;

                //移動後の行動選択
            case State_.action_mode:
                ActionMode();
                
                break;

            //武器選択
            case State_.weapon_mode:
                WeaponMode();

                break;

                //スキル選択
            case State_.skill_mode:
                SkillMode();

                break;

                //道具選択
            case State_.item_mode:
                ItemMode();
                
                break;

                //待機選択
            case State_.stay_mode:
                //ターン終了　何もしない

                break;

            //攻撃対象の選択
            case State_.player_attack_mode:
                PlayerAttackMode();           

                break;

                //エネミーの反撃
            case State_.enemy_counter_mode:
                EnemyCounterMode();

                break;
            
                //敵ターン
            case State_.enemy_turn:
                EnemyTurnMode();

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
    /// キャラ選択モード
    /// </summary>
    void SimulationMode()
    {
        //行動キャラ選択
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("O")) && !Shousai.activeInHierarchy)//○ボタン予定
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
                    MoveStartPos = _nowChooseChar.transform.position;

                    //プログラム上の問題で遅延
                    StartCoroutine(DelayMethod.DelayMethodCall(0.1f, () =>
                    {                     
                        _nowChooseChar.GetComponent<Move_System>().Retrieval();

                        state_ = State_.move_mode;
                    }));
                }
            }
        }

        //詳細ステータス表示
        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetButtonDown("R1"))
        {
            Ray serch = new Ray(rayBox.transform.position, -rayBox.transform.up);
            RaycastHit hiton = new RaycastHit();
            //カーソルの下になんかいて
            if (Physics.Raycast(serch, out hiton, 1000.0f))
            {
                if (hiton.transform.tag == "Player" || hiton.transform.tag == "Enemy")
                {
                    Shousai.SetActive(!Shousai.activeInHierarchy);
                    FindObjectOfType<RayBox>().move_ = false;
                    GameObject.Find("MapCursor").GetComponent<Image>().enabled = false;
                    if (Shousai.activeInHierarchy)
                    {
                        FindObjectOfType<Shosai>()._chara = hiton.transform.gameObject;
                    }
                    else
                    {
                        FindObjectOfType<RayBox>().move_ = true;
                        GameObject.Find("MapCursor").GetComponent<Image>().enabled = true;
                    }
                }
            }
        }
    }
   
    void MoveMode()
    {
        //キャンセル
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Floor");
            foreach (GameObject g in obj)
            {
                g.GetComponent<Square_Info>().DecisionEnd();
            }
            FindObjectOfType<RayBox>().SetMovePlayer(null);
            state_ = State_.simulation_mode;
        }
    }

    void ActionMode()
    {
        //キャンセル
        if (Input.GetKeyDown(KeyCode.X))
        {
            _nowChooseChar.GetComponent<Move_System>().GetNowPos().GetComponent<Square_Info>().ResetChara();
            _nowChooseChar.transform.position = MoveStartPos;
            _nowChooseChar.GetComponent<Move_System>().SetNowPos();

            FindObjectOfType<RayBox>().SetMovePlayer(null);


            FindObjectOfType<RayBox>().SetCameraPosition(_nowChooseChar);

            FindObjectOfType<SubMenuRenderer>().SubMenuStart();
            FindObjectOfType<RayBox>().move_ = true;

            _nowChooseChar = null;
            state_ = State_.simulation_mode;
        }
    }

    void WeaponMode()
    {
        if (!once)
        {
            //アイテム所持情報取得
            var items = _nowChooseChar.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>()._itemprefablist;
            count = 0;
            //なんもないとき
            if (items == null)
            {
                once = true;
                choose = true;
            }
            //アイテムの中から武器を取得描画
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
        //武器選択中
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
                _nowChooseChar.GetComponent<Character>().Equipment(weapons[count]);
            }
            if (Input.GetAxis("AxisY") == -1 || Input.GetAxis("Vertical") == -1)
            {
                if (count == weaponUIs.Count - 1) return;
                count++;
                _nowChooseChar.GetComponent<Character>().Equipment(weapons[count]);
            }
            if (Input.GetButtonDown("O") || Input.GetKeyDown(KeyCode.Space))
            {
                choose = true;
                _nowChooseChar.GetComponent<Character>().Equipment(weapons[count]);
            }

            //キャンセル
            if (Input.GetKeyDown(KeyCode.X))
            {
                foreach (var obj in weaponUIs)
                {
                    obj.SetActive(false);
                    Destroy(obj, 1.0f);
                }

                choose = false;
                once = false;
                count = 0;
                weapons.Clear();
                weaponUIs.Clear();
                state_ = State_.action_mode;
                FindObjectOfType<SubMenuRenderer>().SubMenuStart();
            }
        }
        //選択完了
        else
        {
            foreach (var obj in weaponUIs)
            {
                obj.SetActive(false);
                Destroy(obj, 1.0f);
            }

            choose = false;
            once = false;
            count = 0;
            weapons.Clear();
            weaponUIs.Clear();
            _nowChooseChar.GetComponent<PlayerAttack>().RangeSearch();
        }
    }

    void SkillMode()
    {
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

            //キャンセル
            if (Input.GetKeyDown(KeyCode.X))
            {
                foreach (var obj in weaponUIs)
                {
                    obj.SetActive(false);
                    Destroy(obj, 1.0f);
                }

                choose = false;
                once = false;
                count = 0;
                weapons.Clear();
                weaponUIs.Clear();
                state_ = State_.action_mode;
                FindObjectOfType<SubMenuRenderer>().SubMenuStart();
            }
        }
        else
        {
            foreach (var obj in weaponUIs)
            {
                obj.SetActive(false);
                Destroy(obj, 1.0f);
            }

            _nowChooseChar.GetComponent<Character>()._skillprefablist.GetComponent<SkillPrefabList>().SkillEffect(_nowChooseChar, weapons[count]);
            choose = false;
            once = false;
            weapons.Clear();
            weaponUIs.Clear();

            TurnEnd();
            state_ = State_.stay_mode;
        }
    }

    void ItemMode()
    {
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

            //キャンセル
            if (Input.GetKeyDown(KeyCode.X))
            {
                foreach (var obj in weaponUIs)
                {
                    obj.SetActive(false);
                    Destroy(obj, 1.0f);
                }

                choose = false;
                once = false;
                count = 0;
                weapons.Clear();
                weaponUIs.Clear();
                state_ = State_.action_mode;
                FindObjectOfType<SubMenuRenderer>().SubMenuStart();
            }
        }
        else
        {
            foreach (var obj in weaponUIs)
            {
                obj.SetActive(false);
                Destroy(obj, 1.0f);
            }

            _nowChooseChar.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>().UseItem(_nowChooseChar, weapons[count]);
            choose = false;
            once = false;
            weapons.Clear();
            weaponUIs.Clear();

            TurnEnd();
            state_ = State_.stay_mode;
        }
    }

    void PlayerAttackMode()
    {
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

            //キャンセル
            if (Input.GetKeyDown(KeyCode.X))
            {
                count = 0;
                //選択キャラの攻撃可能状態を解除
                _nowChooseChar.GetComponent<PlayerAttack>().AttackRelease();

                Destroy(battleui);//戦闘時UI削除

                state_ = State_.action_mode;
                FindObjectOfType<SubMenuRenderer>().SubMenuStart();
                FindObjectOfType<RayBox>().SetCameraPosition(_nowChooseChar);
            }
        }

        Ray ray = new Ray(rayBox.transform.position, -rayBox.transform.up);
        RaycastHit hit = new RaycastHit();
        //カーソルの下になんかいて
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            if (hit.transform.tag == "Enemy")
            {
                battleui = FindObjectOfType<StatusUI>().SetBattleStatus(_nowChooseChar, hit.transform.gameObject);
            }

            //スペース(仮)が押されて && まだ攻撃してないなら
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("O")) && !attacking)
            {
                //カーソルがエネミーをさしていたら
                if (hit.transform.tag == "Enemy")
                {
                    attacking = true;
                    count = 0;
                    //選択キャラの攻撃可能状態を解除
                    _nowChooseChar.GetComponent<PlayerAttack>().AttackRelease();

                    //キャラの向き変更
                    _nowChooseChar.transform.LookAt(hit.transform);

                    Debug.Log("自キャラの攻撃！");
                    FindObjectOfType<RayBox>().move_ = false;

                    //とりあえずのエフェクト表示&SE
                    var effect = Instantiate(Resources.Load("Eff_Hit_6"), hit.transform.position, Quaternion.identity);
                    m_audio.PlaySe("GunShot");
                    Destroy(effect, 1.0f);

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

                    DamegeUI_Init(hit.transform.gameObject, e_dm);
                    FindObjectOfType<StatusUI>().setEnemyDamage(e_dm);

                    //攻撃キャラを一時的に保存
                    _nowCounterChara = hit.collider.gameObject;
                    _nowAttackChara = _nowChooseChar;

                    //結果をUIに渡す
                    FindObjectOfType<StatusUI>().setUnitStatus(
                        hit.collider.GetComponent<Character>()._name,
                        hit.collider.GetComponent<Character>()._totalhp,
                        hit.collider.GetComponent<Character>()._totalMaxhp);


                    //すぐ行動しないよう間をおいて敵の反撃へ
                    StartCoroutine(DelayMethod.DelayMethodCall(waitTime, () =>
                    {
                        state_ = State_.enemy_counter_mode;
                    }));
                }
                //プレイヤー
                else if (hit.transform.tag == "Player")
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
    }

    void EnemyCounterMode()
    {
        if (attacking)
        {
            if (_nowCounterChara && _nowCounterChara.tag != "Dead")
            {
                Debug.Log("敵の反撃！");
                //とりあえずの反撃エフェクト表示&SE
                var effect = Instantiate(Resources.Load("Eff_Hit_6"), _nowAttackChara.transform.position, Quaternion.identity);
                m_audio.PlaySe("GunShot");
                Destroy(effect, 1.0f);

                //キャラの向き変更
                _nowCounterChara.transform.LookAt(_nowAttackChara.transform);

                DamegeUI_Init(_nowAttackChara, p_dm);

                FindObjectOfType<StatusUI>().setPlayerDamage(p_dm);

                //結果をUIに渡す
                FindObjectOfType<StatusUI>().setUnitStatus(
                    _nowAttackChara.GetComponent<Character>()._name,
                    _nowAttackChara.GetComponent<Character>()._totalhp,
                    _nowAttackChara.GetComponent<Character>()._totalMaxhp);

            }

            //演出上の遅延
            StartCoroutine(DelayMethod.DelayMethodCall(waitTime, () =>
            {
                Destroy(battleui);
                TurnEnd();
            }));
            attacking = false;
        }
    }

    void EnemyTurnMode()
    {
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
            state_ = State_.enemy_move_mode;
        }
    }


    /// <summary>
    /// プレイヤーの移動終了
    /// </summary>
    public void PlayerMoveEnd()
    {
        FindObjectOfType<SubMenuRenderer>().SubMenuStart();
        FindObjectOfType<RayBox>().move_ = false;
        state_ = State_.action_mode;
    }


    /// <summary>
    /// 頭の上にUIでダメージを表示
    /// </summary>
    /// <param name="target">頭にだすキャラ</param>
    /// <param name="damage">ダメージ</param>
    public void DamegeUI_Init(GameObject target, int damage)
    {
        //ダメージUI表示
        GameObject text = Instantiate(
            Resources.Load("DamageTxt"),
            GameObject.Find("Canvas1").transform.Find("Frame")) as GameObject;
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
    /// レベルアップしたときに上昇値を表示
    /// </summary>
    /// <param name="target"></param>
    /// <param name="upvalue"></param>
    public void LevelUpUI_Init(GameObject target, List<string> upvalue)
    {
        var i = 2;
        StartCoroutine(DelayMethod.DelayMethodCall(i, () =>
        {
            //UI表示
            GameObject text = Instantiate(
                 Resources.Load("DamageTxt"),
                 GameObject.Find("Canvas1").transform.Find("Frame")) as GameObject;
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
            text.GetComponent<DamegeUI>().setDamegeTxt("LevelUp!");
        }));

        i++;
      
        foreach (var value in upvalue)
        {
            StartCoroutine(DelayMethod.DelayMethodCall(i, () =>
            {
                GameObject text2 = Instantiate(
                Resources.Load("DamageTxt"),
                GameObject.Find("Canvas1").transform.Find("Frame")) as GameObject;
                //ワールド座標をスクリーン座標に変換
                var p2 = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);
                var retPosition2 = Vector2.zero;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    GameObject.Find("Canvas1").GetComponent<RectTransform>(),
                    p2,
                    Camera.main,
                    out retPosition2
                );
                text2.transform.localPosition = retPosition2;
                text2.GetComponent<DamegeUI>().setDamegeTxt(value);
            }));
            i++;
        }
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


    /// <summary>
    /// 特技ボタン
    /// </summary>
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

    /// <summary>
    /// アイテムボタン
    /// </summary>
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


    /// <summary>
    /// 攻撃ボタン
    /// </summary>
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

    /// <summary>
    /// 任意のステートに変更
    /// </summary>
    /// <param name="st">State_.name</param>
    public void changeST(State_ st)
    {
        state_ = st;
    }

    /// <summary>
    /// 仮 行動するエネミーを決定
    /// </summary>
    /// <param name="obj"></param>
    public void setRandamEnemy(GameObject obj)
    {
        _randamEnemy = obj;
    }
}