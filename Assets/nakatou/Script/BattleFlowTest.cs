using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

using System.IO;

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
    player_counter_mode,
    Exchange_mode//持ち物があふれたとき交換する
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
    public GameObject _NowChooseChar;

    // どちらの陣営のターンか表示
    public Text _TurnText;

    //BGM,SE再生
    AudioManager m_audio;

    //敵ターンに行動する敵キャラ
    public GameObject _ActionEnemy;

    //ターン出現時に操作不能にする時間
    private float waitTime = 2.0f;

    //攻撃中か？
    bool attacking = false;


    //攻撃された敵キャラ一時保存
    public GameObject _NowCounterChara;

    //ダメージ表示用の変数
    int e_dm;
    int p_dm;

    //武器、スキル、アイテム選択時のUI用
    bool once = false;
    bool choose = false;
    int count = 0;
    List<GameObject> UIs = new List<GameObject>();
    List<GameObject> ChoiceObjs = new List<GameObject>();
    GameObject SetumeiWindow;

    //HPUI用
    public GameObject PlayerRedGage;
    public GameObject EnemyRedGage;

    public GameObject Shousai;//詳細ステータス

    //ゲーム進行フラグ
    bool GameEnd = false;
    public bool GameStart = false;

    public bool Pause;//ゲーム中に一時進行ストップ

    Vector3 MoveStartPos;//キャンセル用
    GameObject battleui;//戦闘時UI;

    //アイテム交換モード用
    List<GameObject> items;

    bool PlayerTurn = false;

    //キャラ移動管理フラグ
    public bool c_moving = false;

    public GameObject Boss;//ボスキャラ

    //主人公オブジェクト
    GameObject Hero;

    void Awake()
    {
        PlayerRedGage = GameObject.Find("playerDamageGage");
        EnemyRedGage = GameObject.Find("enemyDamageGage");
        _TurnText = GameObject.Find("TurnText").GetComponent<Text>();
    }
    void Start()
    {
        FindObjectOfType<MenuManager>().SetMainControlFlag(true);
        if (GameStart) StartGame();
    }

    void Update()
    {
        if (!GameStart) return;


        //仮 勝敗判定
        if (!GameEnd)
        {
            if (Boss.GetComponent<Character>()._totalhp <= 0)
            {
                GameEnd = true;

                //StartCoroutine(DelayMethod.DelayMethodCall(3.0f, () =>
                //{
                //    FindObjectOfType<GameRequirement>().GameClear();
                //}));
            }

            else if (Hero.GetComponent<Character>()._totalhp <= 0)
            {
                GameEnd = true;

                Debug.Log("主人公が死にました...");
                StartCoroutine(DelayMethod.DelayMethodCall(3.0f, () =>
                {
                    FindObjectOfType<GameRequirement>().GameOver();
                }));
            }
        }
        

        if (GameEnd) return;//ゲーム終了

        /////////////////////////会話用ポーズ
        if (FindObjectOfType<StoryCSVReader>().battleScenarioSwitch && !FindObjectOfType<StoryCSVReader>().battleEvent)
        {
            Pause = true;
        }
        else
        {
            Pause = false;
        }

        if (Pause)
        {
            FindObjectOfType<MenuManager>().SetMainControlFlag(true);

            rayBox.GetComponent<RayBox>().move_ = false;

            return;
        }

        //FindObjectOfType<MenuManager>().SetMainControlFlag(false);
        //FindObjectOfType<EventSystem>().sendNavigationEvents = true;
        //FindObjectOfType<RayBox>().move_ = true;

        ///////////////////////////////////////end


        //カメラ追従
        if (state_ == State_.enemy_move_mode ||state_ == State_.enemy_attack_mode)
        {
            FindObjectOfType<RayBox>().SetCameraPosition(_ActionEnemy);
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

        //カーソル移動制御
        if(state_ == State_.simulation_mode || state_ == State_.move_mode)
        {
            if (!Shousai.activeInHierarchy)
            {
                if (c_moving == true) return;
                rayBox.GetComponent<RayBox>().move_ = true;
            }
            else
            {
                rayBox.GetComponent<RayBox>().move_ = false;
            }
        }
        else
        {
            rayBox.GetComponent<RayBox>().move_ = false;
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

                //持ち物が満杯だった時の交換モード
            case State_.Exchange_mode:
                ExchangeMode();

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
                    _NowChooseChar = hit.transform.gameObject;
                    MoveStartPos = _NowChooseChar.transform.position;

                    //プログラム上の問題で遅延
                    StartCoroutine(DelayMethod.DelayMethodCall(0.1f, () =>
                    {                     
                        _NowChooseChar.GetComponent<Move_System>().Retrieval();
                        _NowChooseChar.GetComponent<Animator>().CrossFade("Run", 0.0f);
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
                    Shousai.GetComponent<Shosai>().SetChara(hiton.transform.gameObject);
                    Shousai.SetActive(!Shousai.activeInHierarchy);
                    FindObjectOfType<RayBox>().move_ = false;
                    GameObject.Find("MapCursor").GetComponent<Image>().enabled = false;
                    if (Shousai.activeInHierarchy)
                    {
                        //FindObjectOfType<Shosai>()._chara = hiton.transform.gameObject;
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
        if (Input.GetKeyDown(KeyCode.X) && state_ == State_.move_mode)
        {
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Floor");
            foreach (GameObject g in obj)
            {
                g.GetComponent<Square_Info>().DecisionEnd();
            }
            c_moving = false;
            _NowChooseChar.GetComponent<Animator>().CrossFade("NoneDamy", 0.0f);
            FindObjectOfType<RayBox>().SetMovePlayer(null);
            state_ = State_.simulation_mode;
        }
    }

    /// <summary>
    /// 行動選択
    /// </summary>
    void ActionMode()
    {
        //キャンセル
        if (Input.GetKeyDown(KeyCode.X) && state_ == State_.action_mode)
        {
            _NowChooseChar.GetComponent<Move_System>().GetNowPos().GetComponent<Square_Info>().ResetChara();
            _NowChooseChar.transform.position = MoveStartPos;
            _NowChooseChar.GetComponent<Move_System>().SetNowPos();

            FindObjectOfType<RayBox>().SetMovePlayer(null);

            FindObjectOfType<RayBox>().SetCameraPosition(_NowChooseChar);

            FindObjectOfType<SubMenuRenderer>().SubMenuStart();
            FindObjectOfType<MenuManager>().SetEventSystem(false);

            _NowChooseChar = null;
            state_ = State_.simulation_mode;
        }
    }

    /// <summary>
    /// 武器選択
    /// </summary>
    void WeaponMode()
    {
        if (!once)
        {
            //説明用ウィンドウ表示
            SetumeiWindow = Instantiate(Resources.Load("Waku"),
                        GameObject.Find("Canvas1").transform.Find("Frame").transform) as GameObject;
            //アイテム所持情報取得
            var items = _NowChooseChar.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>()._itemprefablist;
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
                    if (_NowChooseChar.GetComponent<Character>().Check_Equipment(obj))
                    {
                        GameObject ui = Instantiate(
                            Resources.Load("WeaponUI"),
                            GameObject.Find("Canvas1").transform.Find("Frame").transform) as GameObject;
                        ui.transform.localPosition = new Vector3(0, 200 + count * -120, 0);
                        ui.transform.Find("Text").GetComponent<Text>().text = obj.GetComponent<Weapon>()._name;
                        ui.GetComponent<UIsText>()._name = obj.GetComponent<Weapon>()._name;
                        ui.GetComponent<UIsText>()._message =
                             "攻撃力：" + obj.GetComponent<Weapon>()._atk.ToString() +
                             "\nストック：" + obj.GetComponent<Weapon>()._stock.ToString();
                        ui.GetComponent<Image>().color = new Color32(170, 170, 170, 170);
                        ChoiceObjs.Add(obj);
                        UIs.Add(ui);
                        count++;
                    }
                }
            }
            //使える武器を持ってない
            if (count==0)
            {
                foreach (var obj in UIs)
                {
                    obj.SetActive(false);
                    Destroy(obj, 1.0f);
                }
                Destroy(SetumeiWindow);
                choose = false;
                once = false;
                count = 0;
                ChoiceObjs.Clear();
                UIs.Clear();
                state_ = State_.action_mode;
                FindObjectOfType<SubMenuRenderer>().SubMenuStart();

                return;
            }
            count = 0;
            once = true;
        }
        //武器選択中
        if (!choose)
        {
            foreach (var obj in UIs)
            {
                obj.GetComponent<Image>().color = new Color32(170, 170, 170, 170);
            }
            UIs[count].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            SetumeiWindow.transform.FindChild("Setumei").GetComponent<Text>().text =
                UIs[count].GetComponent<UIsText>()._message;

            if (Input.GetAxis("AxisY") == 1 || Input.GetAxis("Vertical") == 1)
            {
                if (count == 0) return;
                count--;
                _NowChooseChar.GetComponent<Character>().Equipment(ChoiceObjs[count]);
                _NowChooseChar.GetComponent<Character>().TotalStatus();
                _NowChooseChar.GetComponent<PlayerAttack>().RangeSerchDesp();
            }
            if (Input.GetAxis("AxisY") == -1 || Input.GetAxis("Vertical") == -1)
            {
                if (count == UIs.Count - 1) return;
                count++;
                _NowChooseChar.GetComponent<Character>().Equipment(ChoiceObjs[count]);
                _NowChooseChar.GetComponent<Character>().TotalStatus();
                _NowChooseChar.GetComponent<PlayerAttack>().RangeSerchDesp();

            }
            if (Input.GetButtonDown("O") || Input.GetKeyDown(KeyCode.Space))
            {
                choose = true;
                _NowChooseChar.GetComponent<Character>().Equipment(ChoiceObjs[count]);
                _NowChooseChar.GetComponent<Character>().TotalStatus();

                _NowChooseChar.GetComponent<PlayerAttack>().RangeSerchDespRelease();
            }

            //キャンセル
            if (Input.GetKeyDown(KeyCode.X) && state_ == State_.weapon_mode)
            {
                foreach (var obj in UIs)
                {
                    obj.SetActive(false);
                    Destroy(obj, 1.0f);
                }
                _NowChooseChar.GetComponent<PlayerAttack>().RangeSerchDespRelease();
                Destroy(SetumeiWindow);
                choose = false;
                once = false;
                count = 0;
                ChoiceObjs.Clear();
                UIs.Clear();
                state_ = State_.action_mode;
                FindObjectOfType<SubMenuRenderer>().SubMenuStart();             
            }
        }
        //選択完了
        else
        {
            foreach (var obj in UIs)
            {
                obj.SetActive(false);
                Destroy(obj, 1.0f);
            }
            Destroy(SetumeiWindow);
            choose = false;
            once = false;
            count = 0;
            ChoiceObjs.Clear();
            UIs.Clear();
            _NowChooseChar.GetComponent<PlayerAttack>().RangeSearch();
        }
    }

    void SkillMode()
    {
        if (!once)
        {
            //説明用ウィンドウ表示
            SetumeiWindow = Instantiate(Resources.Load("Waku"),
                        GameObject.Find("Canvas1").transform.Find("Frame").transform) as GameObject;
            //スキル情報取得
            var skill = _NowChooseChar.GetComponent<Character>()._skillprefablist.GetComponent<SkillPrefabList>()._skillprefablist;
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
                        ui.transform.localPosition = new Vector3(0, 200 + count * -100, 0);
                        ui.transform.Find("Text").GetComponent<Text>().text = obj.GetComponent<CommandSkill>()._name;
                        ui.GetComponent<UIsText>()._name = obj.GetComponent<CommandSkill>()._name;
                        ui.GetComponent<UIsText>()._message = obj.GetComponent<CommandSkill>()._message;
                        ui.GetComponent<Image>().color = new Color32(170, 170, 170, 170);
                        ChoiceObjs.Add(obj);
                        UIs.Add(ui);
                        count++;
                    }
                }
            }
            //使えるスキルを持ってない
            if (count == 0)
            {
                Debug.Log("使えるスキルを持っていません");
                
                Destroy(SetumeiWindow);
                choose = false;
                once = false;
                count = 0;
                ChoiceObjs.Clear();
                UIs.Clear();
                state_ = State_.action_mode;
                FindObjectOfType<SubMenuRenderer>().SubMenuStart();

                return;
            }
            count = 0;
            once = true;
        }
        if (!choose)
        {
            foreach (var obj in UIs)
            {
                obj.GetComponent<Image>().color = new Color32(170, 170, 170, 170);
            }
            UIs[count].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            SetumeiWindow.transform.FindChild("Setumei").GetComponent<Text>().text =
                UIs[count].GetComponent<UIsText>()._message;

            if (Input.GetAxis("AxisY") == 1 || Input.GetAxis("Vertical") == 1)
            {
                if (count == 0) return;
                count--;
            }
            if (Input.GetAxis("AxisY") == -1 || Input.GetAxis("Vertical") == -1)
            {
                if (count == UIs.Count - 1) return;
                count++;
            }
            if (Input.GetButtonDown("O") || Input.GetKeyDown(KeyCode.Space))
            {
                choose = true;
            }

            //キャンセル
            if (Input.GetKeyDown(KeyCode.X) && state_ == State_.skill_mode)
            {
                foreach (var obj in UIs)
                {
                    obj.SetActive(false);
                    Destroy(obj, 1.0f);
                }
                Destroy(SetumeiWindow);
                choose = false;
                once = false;
                count = 0;
                ChoiceObjs.Clear();
                UIs.Clear();
                state_ = State_.action_mode;
                FindObjectOfType<SubMenuRenderer>().SubMenuStart();
            }
        }
        else
        {
            foreach (var obj in UIs)
            {
                obj.SetActive(false);
                Destroy(obj, 1.0f);
            }
            Destroy(SetumeiWindow);
            _NowChooseChar.GetComponent<Character>()._skillprefablist.GetComponent<SkillPrefabList>().SkillEffect(_NowChooseChar, ChoiceObjs[count]);
            choose = false;
            once = false;
            ChoiceObjs.Clear();
            UIs.Clear();

            TurnEnd();
            state_ = State_.stay_mode;
        }
    }

    void ItemMode()
    {
        if (!once)
        {
            SetumeiWindow = Instantiate(Resources.Load("Waku"),
                        GameObject.Find("Canvas1").transform.Find("Frame").transform) as GameObject;

            var items = _NowChooseChar.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>()._itemprefablist;
            count = 0;
            foreach (var obj in items)
            {
                if (obj.GetComponent<Item>())
                {
                    GameObject ui = Instantiate(
                        Resources.Load("WeaponUI"),
                        GameObject.Find("Canvas1").transform.Find("Frame").transform) as GameObject;
                    ui.transform.localPosition = new Vector3(0, 200 + count * -100, 0);
                    ui.transform.Find("Text").GetComponent<Text>().text = obj.GetComponent<Item>()._name;
                    ui.GetComponent<UIsText>()._name = obj.GetComponent<Item>()._name;
                    ui.GetComponent<UIsText>()._message = obj.GetComponent<Item>()._message;
                    ui.GetComponent<Image>().color = new Color32(170, 170, 170, 170);
                    ChoiceObjs.Add(obj);
                    UIs.Add(ui);
                    count++;
                }
            }
            //使えるアイテムを持ってない
            if (count == 0)
            {
                Debug.Log("使えるアイテムを持っていません");
                
                Destroy(SetumeiWindow);
                choose = false;
                once = false;
                ChoiceObjs.Clear();
                UIs.Clear();
                state_ = State_.action_mode;
                FindObjectOfType<SubMenuRenderer>().SubMenuStart();

                return;
            }
            count = 0;
            once = true;
        }
        if (!choose)
        {
            foreach (var obj in UIs)
            {
                obj.GetComponent<Image>().color = new Color32(170, 170, 170, 170);
            }
            UIs[count].GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            SetumeiWindow.transform.FindChild("Setumei").GetComponent<Text>().text =
                UIs[count].GetComponent<UIsText>()._message;

            if (Input.GetAxis("AxisY") == 1 || Input.GetAxis("Vertical") == 1)
            {
                if (count == 0) return;
                count--;
            }
            if (Input.GetAxis("AxisY") == -1 || Input.GetAxis("Vertical") == -1)
            {
                if (count == UIs.Count - 1) return;
                count++;
            }
            if (Input.GetButtonDown("O") || Input.GetKeyDown(KeyCode.Space))
            {
                choose = true;
            }

            //キャンセル
            if (Input.GetKeyDown(KeyCode.X) && state_ == State_.item_mode)
            {
                foreach (var obj in UIs)
                {
                    obj.SetActive(false);
                    Destroy(obj, 1.0f);
                }
                Destroy(SetumeiWindow);
                choose = false;
                once = false;
                count = 0;
                ChoiceObjs.Clear();
                UIs.Clear();
                state_ = State_.action_mode;
                FindObjectOfType<SubMenuRenderer>().SubMenuStart();
            }
        }
        else
        {
            foreach (var obj in UIs)
            {
                obj.SetActive(false);
                Destroy(obj, 1.0f);
            }
            Destroy(SetumeiWindow);
            _NowChooseChar.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>().UseItem(_NowChooseChar, ChoiceObjs[count]);
            choose = false;
            once = false;
            ChoiceObjs.Clear();
            UIs.Clear();

            TurnEnd();
            state_ = State_.stay_mode;
        }
    }

    void PlayerAttackMode()
    {
        //カーソルが敵に自動で照準
        if (!attacking)
        {
            var range_enemy = _NowChooseChar.GetComponent<PlayerAttack>().GetInAttackRangeEnemy();

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
            if (Input.GetKeyDown(KeyCode.X) && !attacking)
            {
                count = 0;
                //選択キャラの攻撃可能状態を解除
                _NowChooseChar.GetComponent<PlayerAttack>().AttackRelease();

                Destroy(battleui);//戦闘時UI削除

                state_ = State_.action_mode;
                FindObjectOfType<SubMenuRenderer>().SubMenuStart();
                FindObjectOfType<RayBox>().SetCameraPosition(_NowChooseChar);
            }
        }

        Ray ray = new Ray(rayBox.transform.position, -rayBox.transform.up);
        RaycastHit hit = new RaycastHit();
        //カーソルの下になんかいて
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            if (hit.transform.tag == "Enemy")
            {
                battleui = FindObjectOfType<StatusUI>().SetBattleStatus(_NowChooseChar, hit.transform.gameObject);
            }

            //スペースまたは○ボタンが押されて && まだ攻撃してないなら
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("O")) && !attacking)
            {
                //カーソルがエネミーをさしていたら
                if (hit.transform.tag == "Enemy")
                {
                    _NowChooseChar.GetComponent<PlayerBattleStoryFlag>().SetEnemyName(hit.transform.gameObject);

                    attacking = true;
                    count = 0;

                    StartCoroutine(DelayMethod.DelayMethodCall(0.1f, () =>
                    {
                        if (FindObjectOfType<StoryCSVReader>().battleScenarioSwitch == true)
                        {
                            Debug.Log("ok");
                            //攻撃されたキャラを一時的に保存
                            _NowCounterChara = hit.transform.gameObject;

                            state_ = State_.stay_mode;

                            return;
                        }
                        else
                        {
                            PlayerAttack(hit.transform.gameObject);
                        }
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

    /// <summary>
    /// 攻撃対象が選択されたとき実行
    /// </summary>
    public void PlayerAttack(GameObject hit)
    {
        //選択キャラの攻撃可能状態を解除
        _NowChooseChar.GetComponent<PlayerAttack>().AttackRelease();

        //キャラの向き変更
        _NowChooseChar.transform.LookAt(hit.transform);

        Debug.Log("自キャラの攻撃！");
        FindObjectOfType<RayBox>().move_ = false;

        int e_nowhp = hit.transform.GetComponent<Character>()._totalhp;
        int p_nowhp = _NowChooseChar.GetComponent<Character>()._totalhp;

        //とりあえずゲージ表示
        FindObjectOfType<StatusUI>().setactive(true);

        FindObjectOfType<StatusUI>().setPlayerHpGage(
            _NowChooseChar.GetComponent<Character>()._totalMaxhp, p_nowhp);
        FindObjectOfType<StatusUI>().setEnemyHpGage(
            hit.transform.GetComponent<Character>()._totalMaxhp, e_nowhp);

        PlayerRedGage.GetComponent<DamageGage>().setdamageGage(
            _NowChooseChar.GetComponent<Character>()._totalMaxhp, p_nowhp);
        EnemyRedGage.GetComponent<DamageGage>().setdamageGage(
            hit.transform.GetComponent<Character>()._totalMaxhp, e_nowhp);

        //戦闘結果計算
        FindObjectOfType<BattleManager>().BattleSetup(_NowChooseChar, hit.transform.gameObject);

        e_dm = e_nowhp - hit.transform.GetComponent<Character>()._totalhp;//敵ダメージ算出(仮)
        p_dm = p_nowhp - _NowChooseChar.GetComponent<Character>()._totalhp;//プレイヤダメージ算出

        DamegeUI_Init(hit.transform.gameObject, e_dm);
        FindObjectOfType<StatusUI>().setEnemyDamage(e_dm);

        //攻撃されたキャラを一時的に保存
        _NowCounterChara = hit;

        //結果をUIに渡す
        FindObjectOfType<StatusUI>().setUnitStatus(
            hit.GetComponent<Character>()._name,
            hit.GetComponent<Character>()._totalhp,
            hit.GetComponent<Character>()._totalMaxhp);


        //すぐ行動しないよう間をおいて敵の反撃へ
        StartCoroutine(DelayMethod.DelayMethodCall(waitTime, () =>
        {
            state_ = State_.enemy_counter_mode;
        }));
    }

    void EnemyCounterMode()
    {
        if (attacking)
        {
            if (_NowCounterChara && _NowCounterChara.GetComponent<Character>()._HpState != Character.HP_State.Dead)
            {
                Debug.Log("敵の反撃！");

                //キャラの向き変更
                _NowCounterChara.transform.LookAt(_NowChooseChar.transform);

                DamegeUI_Init(_NowChooseChar, p_dm);

                FindObjectOfType<StatusUI>().setPlayerDamage(p_dm);

                //結果をUIに渡す
                FindObjectOfType<StatusUI>().setUnitStatus(
                    _NowChooseChar.GetComponent<Character>()._name,
                    _NowChooseChar.GetComponent<Character>()._totalhp,
                    _NowChooseChar.GetComponent<Character>()._totalMaxhp);

            }

            //すぐ行動しないように遅延
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

        SetActionEnemy();

        //敵が一体でもいれば敵の行動を開始
        _ActionEnemy.GetComponent<EnemyBase>().SetNextGoal(_ActionEnemy.GetComponent<EnemyBase>().target_square);
        state_ = State_.enemy_move_mode;
    }


    /// <summary>
    /// プレイヤーの移動終了
    /// </summary>
    public void PlayerMoveEnd()
    {
        FindObjectOfType<SubMenuRenderer>().SubMenuStart();
        _NowChooseChar.GetComponent<Animator>().CrossFade("NoneDamy", 0.0f);
        FindObjectOfType<RayBox>().move_ = false;
        c_moving = false;
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
        var ui = Instantiate(Resources.Load("LvUpWindow"),
            GameObject.Find("Canvas1").transform.Find("Frame").transform) as GameObject;
        var text = ui.transform.FindChild("ValuesText").GetComponent<Text>();
        text.text += target.GetComponent<Character>()._name + "\n";
         foreach(var st in upvalue)
        {
            text.text += st + "\n";
        }
        Destroy(ui, 5.0f);

    }

    /// <summary>
    /// 自陣ターン終了
    /// </summary>
    public void TurnEnd()
    {
        if (state_ == State_.enemy_counter_mode || state_ == State_.action_mode ||
        state_ == State_.skill_mode || state_ == State_.item_mode || state_ == State_.menu_mode || state_== State_.Exchange_mode)
        {
            //待機選択時
            if (state_ == State_.action_mode)
            {
                FindObjectOfType<SubMenuRenderer>().SubMenuStart();
                FindObjectOfType<MenuManager>().SetEventSystem(false);
            }

            state_ = State_.stay_mode;

            if (SerchFullItemChara())
            {
                PlayerTurn = true;
                state_ = State_.Exchange_mode;
                return;
            }

            //ステートを変更 & UI表示
            _TurnText.color = new Color(255, 0, 0, 255);
            _TurnText.text = "Enemy Turn";
            _TurnText.gameObject.GetComponent<Animator>().SetBool("Init", true);//ターン終了時のアニメーション
            m_audio.PlaySe("TurnStart");
            FindObjectOfType<ExpGage>().Enabled(false);
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var players = GameObject.FindGameObjectsWithTag("Player");
            for (var k = 0; k < players.Length; k++)
            {
                players[k].GetComponent<Character>().end();
            }
            var enemys = GameObject.FindGameObjectsWithTag("Enemy");
            for (var e = 0;e < enemys.Length;e++)
            {
                enemys[e].GetComponent<Character>().end();
            }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //すぐ行動しないように遅延
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
        state_ = State_.stay_mode;

        if (SerchFullItemChara())
        {
            PlayerTurn = false;
            state_ = State_.Exchange_mode;
            return;
        }

        _TurnText.color = new Color(0, 0, 255, 255);
        _TurnText.text = "Player Turn";
        m_audio.PlaySe("TurnStart");
        _TurnText.gameObject.GetComponent<Animator>().SetBool("Init", true);//ターン終了時のアニメーション
        FindObjectOfType<StatusUI>().setactive(false);
        FindObjectOfType<ExpGage>().Enabled(false);

        //すぐ行動しないように遅延
        StartCoroutine(DelayMethod.DelayMethodCall(waitTime, () => 
        {
            Turn_TextReset();
            FindObjectOfType<RayBox>().move_ = true;
            state_ = State_.simulation_mode;

            FindObjectOfType<RayBox>().SetCameraPosition(_NowChooseChar);
            
            FindObjectOfType<SituationTexts>().TurnCountUp();
        }));
    }

    public void AtkAnim(GameObject obj)
    {
        var animator = obj.GetComponent<Animator>();
        Weapon_Type type =
           animator.gameObject.GetComponent<Character>()._equipment.GetComponent<Weapon>()._weapontype;

        //ハンドガン
        if (type == Weapon_Type.Gun)
        {
            animator.CrossFade("HundGun_Atk", 0.0f);
        }
        //斧
        else if (type == Weapon_Type.Axe)
        {
            animator.CrossFade("Axe_Atk", 0.0f);
        }
        //拳
        else if (type == Weapon_Type.Fist)
        {
            animator.CrossFade("Punch_Atk", 0.0f);
        }
        //ナイフ
        else if (type == Weapon_Type.Knife)
        {
            animator.CrossFade("Knife_Atk", 0.0f);
        }
        //ライフル
        else if (type == Weapon_Type.Rifle)
        {
            animator.CrossFade("Rifle_Atk", 0.0f);
        }
        //槍
        else if (type == Weapon_Type.Spear)
        {
            animator.CrossFade("Lance_Atk", 0.0f);
        }
        //素手
        else
        {
            animator.CrossFade("Punch_Atk", 0.0f);
        }
    }

    /// <summary>
    /// アイテムがいっぱいのチャラを探す
    /// </summary>
    bool SerchFullItemChara()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(var chara in players)
        {
           if(chara.transform.FindChild("ItemList").GetComponent<ItemPrefabList>()._itemprefablist.Count > 5)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 所持アイテムが上限の時の処理
    /// </summary>
    void ExchangeMode()
    {
        //対象
        GameObject _chara = null;

        if (once == false)
        {
            Debug.Log("倉庫に送るアイテムを選択");
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var chara in players)
            {
                if (chara.transform.FindChild("ItemList").GetComponent<ItemPrefabList>()._itemprefablist.Count > 5)
                {
                    _chara = chara;
                    items = chara.transform.FindChild("ItemList").GetComponent<ItemPrefabList>()._itemprefablist;
                }
            }

            count = 0;
            foreach (var obj in items)
            {
                if (obj.GetComponent<Item>())
                {
                    GameObject ui = Instantiate(
                        Resources.Load("WeaponUI"),
                        GameObject.Find("Canvas1").transform.Find("Frame").transform) as GameObject;
                    ui.transform.localPosition = new Vector3(0, 250 + count * -100, 0);
                    ui.transform.Find("Text").GetComponent<Text>().text = obj.GetComponent<Item>()._name;
                    ui.GetComponent<UIsText>()._name = obj.GetComponent<Item>()._name;
                    ui.GetComponent<UIsText>()._message = obj.GetComponent<Item>()._message;
                    ui.GetComponent<Image>().color = new Color32(170, 170, 170, 170);
                    UIs.Add(ui);
                    count++;
                }

                if (obj.GetComponent<Weapon>())
                {
                    GameObject ui = Instantiate(
                        Resources.Load("WeaponUI"),
                        GameObject.Find("Canvas1").transform.Find("Frame").transform) as GameObject;
                    ui.transform.localPosition = new Vector3(0, 250 + count * -100, 0);
                    ui.transform.Find("Text").GetComponent<Text>().text = obj.GetComponent<Weapon>()._name;
                    ui.GetComponent<UIsText>()._name = obj.GetComponent<Weapon>()._name;
                    ui.GetComponent<UIsText>()._message = obj.GetComponent<Weapon>()._message;
                    ui.GetComponent<Image>().color = new Color32(170, 170, 170, 170);
                    ChoiceObjs.Add(obj);
                    UIs.Add(ui);
                    count++;
                }
            }

            count = 0;
            once = true;
        }

        bool end = SerchFullItemChara();
        if (end == true)
        {
            foreach (var obj in UIs)
            {
                obj.GetComponent<Image>().color = new Color32(170, 170, 170, 170);
            }
            UIs[count].GetComponent<Image>().color = new Color32(255, 255, 255, 255);


            if (Input.GetAxis("AxisY") == 1 || Input.GetAxis("Vertical") == 1)
            {
                if (count == 0) return;
                count--;
            }
            if (Input.GetAxis("AxisY") == -1 || Input.GetAxis("Vertical") == -1)
            {
                if (count == UIs.Count - 1) return;
                count++;
            }

            if (Input.GetButtonDown("O") || Input.GetKeyDown(KeyCode.Space))
            {
                FindObjectOfType<RepositoryManager>().AddItem(_chara,items[count]);

                foreach (var obj in UIs)
                {
                    obj.SetActive(false);
                    Destroy(obj, 1.0f);
                }

                items.Clear();
                UIs.Clear();

                count = 0;
                once = false;

                if(PlayerTurn)
                {
                    TurnEnd();
                }
                else
                {
                    EnemyTurnEnd();
                }
            }
        }
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
            //m_audio.PlaySe("choose");
            FindObjectOfType<SubMenuRenderer>().SubMenuStart();//サブメニュー非表示
            FindObjectOfType<MenuManager>().SetEventSystem(false);
            state_ = State_.stay_mode;

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
            //m_audio.PlaySe("choose");
            FindObjectOfType<SubMenuRenderer>().SubMenuStart();//サブメニュー非表示
            FindObjectOfType<MenuManager>().SetEventSystem(false);
            state_ = State_.stay_mode;

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
    public void AttackBt()
    {
        if(state_ == State_.action_mode)
        {
            //m_audio.PlaySe("choose");
            FindObjectOfType<SubMenuRenderer>().SubMenuStart();//サブメニュー非表示
            FindObjectOfType<MenuManager>().SetEventSystem(false);
            state_ = State_.stay_mode;

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
    public void ChangeST(State_ st)
    {
        state_ = st;
    }

    /// <summary>
    ///行動するエネミーを決定
    ///一番プレイヤーに近い敵が行動する
    /// </summary>
    public void SetActionEnemy()
    {
        var enemys = GameObject.FindGameObjectsWithTag("Enemy");
        var players = GameObject.FindGameObjectsWithTag("Player");
        GameObject minobj = null;

        foreach (var enemy in enemys)
        {
            foreach(var player in players)
            {
                if (minobj == null)
                {
                    minobj = enemy;
                    continue;
                }
                //一番近いキャラ
                if (Vector3.Distance(player.transform.position, minobj.transform.position) >
                    Vector3.Distance(player.transform.position, enemy.transform.position))
                {
                    //死んでなかったら登録
                    if (enemy.GetComponent<Character>()._HpState != Character.HP_State.Dead)
                    {
                        minobj = enemy;
                    }
                }
            }
        }
        _ActionEnemy = minobj;
    }

    /// <summary>
    /// スタートの時の処理を関数化
    /// 編成の処理のため
    /// </summary>
    public void StartGame()
    {
        /*メニュー制御不可*/
        FindObjectOfType<MenuManager>().SetMainControlFlag(true);
        FindObjectOfType<SubMenuRenderer>().GetSubControlFlag(true);
        rayBox = FindObjectOfType<RayBox>().gameObject;
        rayBox.GetComponent<RayBox>().move_ = false;

        m_audio = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();

        Shousai = Instantiate(Resources.Load("Shosai"), GameObject.Find("Canvas").transform) as GameObject;
        Shousai.SetActive(false);
        SetActionEnemy();

        _TurnText.text = "第" + FindObjectOfType<StoryCSVReader>().GetStoryNumber() + "章 \n PlayerTurn";

        foreach (var player in FindObjectsOfType<Character>())
        {
            if (player._name == "ヒュー")
            {
                player._hero = true;
                Hero = player.gameObject;
            }
            if(player.gameObject.GetComponent<EnemyPersonalCSV>())
            {
                if (player.GetComponent<EnemyPersonalCSV>().GetBossFlag())
                {
                    Boss = player.gameObject;
                }
            }
        }

        StartCoroutine(DelayMethod.DelayMethodCall(1.0f, () =>
        {
            Turn_TextReset();
            /*メニュー制御可*/
            FindObjectOfType<MenuManager>().SetMainControlFlag(false);
            FindObjectOfType<SubMenuRenderer>().GetSubControlFlag(false);
            rayBox.GetComponent<RayBox>().move_ = true;
            state_ = State_.simulation_mode;
        }));

        GameStart = true;
    }
}