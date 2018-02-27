﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//クラスリスト
public enum Joblist
{
    Gunner = 0,
    Sniper,
    Rogue,
    Assassin,
    Fighter,
    Marshall,
    Soldier,
    General,
    Mercenary,
    Warrior,
    Rider,
    Motorhead,
    Pilot,
    Captain,
    Ranger,
    Load,
    Thief,
    Android,
    Hacker,
    Singer,
    Valkyrie,
    Paladin,
    Desperado,
    EarthDragon
}

public struct Buff
{
    public int[] _datalist;

    //バフスキル追加
    public void SetData(int[] data)
    {
        _datalist = data;
    }
    //バフスキルのターン経過
    //ターン0になったらバフを削除
    public bool AddTurn()
    {
        _datalist[13] -= 1;
        if (_datalist[13] <= 0) return true;
        return false;
    }
}

public class Character : MonoBehaviour {

    public int _id;                 //Characterリストのナンバー
    private int _classid;           //classリストのナンバー
    public GameObject _equipment;   //装備アイテムのオブジェクト

    public Sprite _faceimage;       //フォアグラ

    public string _name;            //名前
    public Vector3 _position;       //座標
    public bool _hero = false;      //主人公かどうか
    private bool _reset = false;
    public bool _dropitem = false;  //アイテムドロップの有無(敵専用)

    [Header("レベル")]
    public int _level;          //現在のLv
    public int _totalLevel;     //今までの合計Level

    private int _hp;            //HP
    [Header("MaxHP")]
    public int _totalMaxhp;     //最終的な最大HP
    [Header("HP")]
    public int _totalhp;        //最終的なHP
    public int _hprate;         //HP成長率

    private int _str;           //力
    [Header("力")]
    public int _totalstr;       //最終的なSTR
    public int _strrate;        //力成長率

    private int _skl;           //技
    [Header("技")]
    public int _totalskl;       //最終的なSKL
    public int _sklrate;        //技成長率

    private int _spd;           //速さ
    [Header("速さ")]
    public int _totalspd;       //最終的なSPD
    public int _spdrate;        //速さ成長率

    private int _luk;           //幸運
    [Header("幸運")]
    public int _totalluk;       //最終的なLUK
    public int _lukrate;        //幸運成長率

    private int _def;           //守備力
    [Header("守備")]
    public int _totaldef;       //最終的なDEF
    public int _defrate;        //守備成長率

    private int _cur;           //呪力
    [Header("呪力")]
    public int _totalcur;       //最終的なCUR
    public int _currate;        //呪力成長率
    
    private int _move;          //移動力   
    [Header("移動力")]
    public int _totalmove;      //最終的なMOVE
    public int _moverate;       //移動力成長率

    public int _exp;            //経験値

    public int[] _addstatuslist = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };                 //定数増加量配列
    public int[] _addonetimestatuslist = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };          //戦闘時ステータス増加量配列
    public int[] _addbufflist = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };                   //ターン制限付きバフステータス増加量配列
                                                                                                //上限値以下かの判定
    public int[] def_st = { 0, 0, 0, 0, 0, 0, 0, 0 };
    public List<Buff> _bufflist = new List<Buff>();


    public int _total_attack = 0;           //最終的な攻撃力(装備が銃なら技を参照する)
    public int _attack_count = 1;           //攻撃回数(勇者系などの複数回攻撃武器で使用)
    public int _attack_speed = 0;           //攻撃速度
    public float _hit = 0;                  //命中率
    public float _avoidance = 0;            //回避率(地形補正抜き)
    public float _critical = 0;             //必殺率

    public int[] _range = {0,0};            //攻撃範囲(配列の0番目が最小,1番目が最大)

    public bool _stability = false;         //追撃できないかどうか
    public bool _georges = false;           //ジョルジュの有無
    public bool _healplus = false;          //回復量増加
    public bool _yani = false;              //ヤニの有無
    public int _yaniTurn = 0;               //ヤニの残りターン
    public int _healplusturn = 0;           //回復量増加残りターン
    public bool _isDead = false;            //死亡済みか

    public GameObject _itemprefablist;      //アイテムの親オブジェクトの取得
    public GameObject _skillprefablist;     //スキルの親オブジェクトの取得
    public SkillChecker _skillchecker;      //スキルチェッカースクリプト
    public StatusSave _statussave;
    private LogDisplay _logDisplay;

    //キャラリスト読み込み
    Entity_CharaList charaList;
    //キャラ成長率リスト読み込み
    Entity_Chara_rateList chara_rateList;
    //クラスリスト読み込み
    Entity_ClassList classList;
    //クラス基本値リスト読み込み
    Entity_ClassDefaultStatus class_def_list;

    //現在のHP状態
    public HP_State _HpState;
    //現在の状態異常
    public State _NowState;
    //クラスリスト
    public Joblist _joblist;

    //Hpの状態管理
    public enum HP_State
    {
        Fine,       //元気
        Bleeding,   //出血
        Severe,     //重症
        Dying,      //瀕死
        Dead        //死亡
    }
    //状態異常
    public enum State
    {
        Default = 0,    //特になし
        Paralysis = 1,  //麻痺
        Poison = 2,     //毒
        Fire = 3,       //やけど
        Sleep = 4,      //睡眠
        Confusion = 5   //混乱
    }


    // Use this for initialization
    void Start()
    {
        _HpState = HP_State.Fine;
        _NowState = State.Default;
        //キャラリスト読み込み
        charaList = Resources.Load("Data/CharaList") as Entity_CharaList;
        //キャラ成長率リスト読み込み
        chara_rateList = Resources.Load("Data/Chara_rateList") as Entity_Chara_rateList;
        //クラスリスト読み込み
        classList = Resources.Load("Data/ClassList") as Entity_ClassList;
        //クラス基本値リスト読み込み
        class_def_list = Resources.Load("Data/ClassDefaultStatus") as Entity_ClassDefaultStatus;
        //自分の子にあるアイテムリストを取得
        _itemprefablist = GameObject.Find( transform.name + "/ItemList");
        //自分の子にあるスキルリストを取得
        _skillprefablist = GameObject.Find(transform.name + "/SkillList");
        _skillchecker = GetComponent<SkillChecker>();
        _statussave = GetComponent<StatusSave>();
        Initialize();
        _logDisplay = FindObjectOfType<LogDisplay>();

    }
	// Update is called once per frame
	void Update () {
        if (!_equipment)
        {
            _equipment = Resources.Load("Unarmed") as GameObject;
        }
        //HPの状態による分岐
        switch (_HpState)
        {
            //HP100~80%
            case HP_State.Fine:
                if (_totalhp <= _totalMaxhp*0.8)
                {
                    _HpState = HP_State.Bleeding;
                }
                break;
            //HP80~50%
            case HP_State.Bleeding:
                if(_totalhp <= _totalMaxhp * 0.5)
                {
                    _HpState = HP_State.Severe;
                }
                break;
            //HP50~20%
            case HP_State.Severe:
                if (_totalhp <= _totalMaxhp * 0.2)
                {
                    _HpState = HP_State.Dying;
                }
                break;
            //HP20~0%
            case HP_State.Dying:
                if (_totalhp <= 0)
                {
                    _HpState = HP_State.Dead;
                }
                break;
            //死
            case HP_State.Dead:
                _totalhp = 0;
                Dead();
                break;

        }

        //if (Input.GetKeyDown("j") && transform.tag == "Player")
        //{
        //    SaveStatus();
        //    Debug.Log("Save" + _name);

        //}
        //if (Input.GetKeyDown("k") && transform.tag == "Player" && _statussave.LoadStatus())
        //{
        //    LoadData();
        //    _itemprefablist.GetComponent<ItemPrefabList>().DeleteItem();
        //    _itemprefablist.GetComponent<ItemPrefabList>().LoadItem(_name);
        //    _itemprefablist.GetComponent<ItemPrefabList>().RemoveItem();
        //}
        //if (transform.tag != "Enemy")
        //{
        //    if (_statussave.LoadStatus())
        //    {
        //        LoadData();
        //    }
        //}


        //exp100でレベルアップ
        if (_exp >= 100)
        {
            //_exp = 0;
            LevelUp();
        }
        else if(_exp < 0)
        {
            _exp = 0;
        }

        //TotalStatus();
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    void Dead()
    {
        _isDead = true;
    }

    /// <summary>
    /// スキルによる1戦闘のみのステータス上昇を代入
    /// </summary>
    /// <param name="_atk">攻撃力</param>
    /// <param name="_str">力</param>
    /// <param name="_skl">技</param>
    /// <param name="_spd">速さ</param>
    /// <param name="_luk">運</param>
    /// <param name="_def">防御</param>
    /// <param name="_cur">呪力</param>
    /// <param name="_move">移動</param>
    /// <param name="_hit">命中</param>
    /// <param name="_avo">回避</param>
    /// <param name="_cri">必殺</param>
    /// <param name="_count">攻撃回数</param>
    /// <param name="_min">最小範囲</param>
    /// <param name="_max">最大範囲</param>
    public void OneButtleStatus(int _atk,int _str,int _skl,int _spd,int _luk,int _def,int _cur,int _move,int _hit,int _avo,int _cri,int _count,int _min,int _max)
    {
        _addonetimestatuslist[0] += _atk;
        _addonetimestatuslist[1] += _str;
        _addonetimestatuslist[2] += _skl;
        _addonetimestatuslist[3] += _spd;
        _addonetimestatuslist[4] += _luk;
        _addonetimestatuslist[5] += _def;
        _addonetimestatuslist[6] += _cur;
        _addonetimestatuslist[7] += _move;
        _addonetimestatuslist[8] += _hit;
        _addonetimestatuslist[9] += _avo;
        _addonetimestatuslist[10] += _cur;
        _addonetimestatuslist[11] += _count;
        _addonetimestatuslist[12] += _min;
        _addonetimestatuslist[13] += _max;
    }

    /// <summary>
    /// 自分の戦闘終了時の処理
    /// </summary>
    public void BattleEnd()
    {
        _stability = false;
        _georges = false;
        //1戦闘限りのバフを初期化
        _addonetimestatuslist = new int[14] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    }

    /// <summary>
    /// hp,str,skl,spd,luk,def,cur,move
    /// </summary>
    /// <returns></returns>
    public List<int> GetStatus()
    {
        List<int> i = new List<int>();
        i.Add(_hp);
        i.Add(_str);
        i.Add(_skl);
        i.Add(_spd);
        i.Add(_luk);
        i.Add(_def);
        i.Add(_cur);
        i.Add(_move);
        return i;
    }

    /// <summary>
    /// ロードしたデータを適用
    /// </summary>
    public void LoadData()
    {
        //_statussave.LoadStatus();
        _id = _statussave.r_id;
        _joblist = (Joblist)Enum.ToObject(typeof(Joblist), _statussave.r_job);
        _name = _statussave.r_name;
        _position = _statussave.r_pos;
        _hero = _statussave.r_hero;
        _level = _statussave.r_lv;
        _totalLevel = _statussave.r_totallv;
        _hp = _statussave.r_hp;
        _totalhp = _statussave.r_totalhp;
        _str = _statussave.r_str;
        _skl = _statussave.r_skl;
        _spd = _statussave.r_spd;
        _luk = _statussave.r_luk;
        _def = _statussave.r_def;
        _cur = _statussave.r_cur;
        _move = _statussave.r_move;
        _exp = _statussave.r_exp;
        _addstatuslist = _statussave.r_addstatus;
        _addonetimestatuslist = _statussave.r_addonestatus;
        _addbufflist = _statussave.r_addbuffstatus;
        _stability = _statussave.r_stability;
        _isDead = _statussave.r_isDead;     
        //_statussave.r_itemobj.transform.parent = transform;
        //_itemprefablist.GetComponent<ItemPrefabList>()._itemprefablist = _statussave.r_skillobj.GetComponent<ItemPrefabList>()._itemprefablist;
    }


    /// <summary>
    /// キャラクターのデータをセーブ
    /// </summary>
    public void SaveStatus()
    {
        GetComponent<StatusSave>().CharactorDataSave(_name);
        _itemprefablist.GetComponent<ItemPrefabList>().SaveItems(_name);
    }

    /// <summary>
    /// 初期化
    /// Excelから初期値を読み込む
    /// </summary>
    public void Initialize()
    {
        //プレイヤー初期化
        if (transform.tag == "Player")
        {
            for (var i = 0; i < charaList.param.Count; i++)
            {
                if (charaList.param[i].name == _name)
                {
                    //idの上書き
                    _id = i;
                    //各能力値と成長率の読み込み
                    _level = charaList.param[_id].lv;
                    _totalLevel = _level;
                    _hp = charaList.param[_id].hp;
                    _hprate = chara_rateList.param[_id].hp;
                    _str = charaList.param[_id].str;
                    _strrate = chara_rateList.param[_id].str;
                    _skl = charaList.param[_id].skl;
                    _sklrate = chara_rateList.param[_id].skl;
                    _spd = charaList.param[_id].spd;
                    _spdrate = chara_rateList.param[_id].spd;
                    _luk = charaList.param[_id].luk;
                    _lukrate = chara_rateList.param[_id].luk;
                    _def = charaList.param[_id].def;
                    _defrate = chara_rateList.param[_id].def;
                    _cur = charaList.param[_id].cur;
                    _currate = chara_rateList.param[_id].cur;
                    _move = charaList.param[_id].move;
                    _moverate = chara_rateList.param[_id].move;

                }
            }
            //if (_statussave.LoadStatus())
            //{
            //    LoadData();
            //    Debug.Log("データロード" + transform.name);
            //}
            //else
            //{
            //    Debug.Log("セーブデータなし");
            //}
        }
        
        
        _skillprefablist.GetComponent<SkillPrefabList>().CheckSkillLevel(_totalLevel);
        TotalStatus();
        RecoveryHP(100);
    }

    /// <summary>
    /// 装備変更
    /// </summary>
    /// <param name="weapon">装備するアイテム</param>
    public void Equipment(GameObject weapon)
    {
        _equipment = weapon;
    }

    /// <summary>
    /// 装備できるかチェック
    /// </summary>
    /// <param name="weapon">装備する予定の武器</param>
    /// <returns>装備できるかどうか</returns>
    public bool Check_Equipment(GameObject weapon)
    {
        if (
            (weapon.GetComponent<Weapon>()._weapontype == Weapon_Type.Gun && classList.param[(int)_joblist].gun == 1) ||
            (weapon.GetComponent<Weapon>()._weapontype == Weapon_Type.Rifle && classList.param[(int)_joblist].rifle == 1) ||
            (weapon.GetComponent<Weapon>()._weapontype == Weapon_Type.Knife && classList.param[(int)_joblist].knife == 1) ||
            (weapon.GetComponent<Weapon>()._weapontype == Weapon_Type.Fist && classList.param[(int)_joblist].fist == 1) ||
            (weapon.GetComponent<Weapon>()._weapontype == Weapon_Type.Spear && classList.param[(int)_joblist].spear == 1) ||
            (weapon.GetComponent<Weapon>()._weapontype == Weapon_Type.Axe && classList.param[(int)_joblist].axe == 1)
            )
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// エネミー用initialize
    /// </summary>
    public void Enemy_Init()
    {
        if (transform.tag == "Enemy")
        {
            //クラス(ジョブ)検索
            for (var j = 0; j < class_def_list.param.Count; j++)
            {
                //一致したクラスの初期値を代入
                if (class_def_list.param[j].name == _joblist.ToString())
                {
                    _hp = class_def_list.param[j].hp;
                    _hprate = class_def_list.param[j].hp_r;
                    _str = class_def_list.param[j].str;
                    _strrate = class_def_list.param[j].str_r;
                    _skl = class_def_list.param[j].skl;
                    _sklrate = class_def_list.param[j].skl_r;
                    _spd = class_def_list.param[j].spd;
                    _spdrate = class_def_list.param[j].spd_r;
                    _luk = class_def_list.param[j].luk;
                    _lukrate = class_def_list.param[j].luk_r;
                    _def = class_def_list.param[j].def;
                    _defrate = class_def_list.param[j].def_r;
                    _cur = class_def_list.param[j].cur;
                    _currate = class_def_list.param[j].cur_r;
                    _move = class_def_list.param[j].move;
                    _moverate = class_def_list.param[j].move_r;
                    var lv = _level;
                    if (lv <= 0) lv = 1;
                    _totalLevel += 1;
                    //初期Level分レベルアップ
                    for (var k = 1; k < lv; k++)
                    {
                        LevelUp();
                        _level--;
                    }
                }
            }
            _skillprefablist.GetComponent<SkillPrefabList>().CheckSkillLevel(_totalLevel);
            TotalStatus();
            RecoveryHP(100);
        }
    }

    /// <summary>
    /// 装備のstockの取得
    /// </summary>
    /// <returns>ストック数</returns>
    public int GetStock()
    {
        if (!_equipment)
        {
            return 0;
        }
        else
        {
            return _equipment.GetComponent<Weapon>()._stock;
        }
    }


    /// <summary>
    /// 武器の耐久を減らす
    /// 戦闘終了時に呼ぶ
    /// </summary>
    /// <param name="i">使用回数</param>
    public void UseEquipment(int i)
    {
        for (var j = 0; j < i;j++)
        {
            if (_equipment.GetComponent<Weapon>().StockDecrement())
            {
                //Debug.Log(_equipment.transform.name + "が壊れた…");
                _itemprefablist.GetComponent<ItemPrefabList>().RemoveItem();
            }
        }
        
    }

    /// <summary>
    /// パッシブスキルの合計を計算
    /// </summary>
    public int[] AddPassiveStatus()
    {
        int[] _list = new int[14];
        var _skillList = _skillprefablist.GetComponent<SkillPrefabList>();

        if (_skillList.SearchSkill("Passive").Count >1)
        {
            for (var i = 0; i < _skillList.SearchSkill("Passive").Count; i++)
            {
                for (var j = 0; j < _list.Length; j++)
                {
                    _list[j] += _skillList.SearchSkill("Passive")[i].GetComponent<PassiveSkill>()._addlist[j];
                }
            }
        }
        return _list;
    }

    /// <summary>
    /// バフスキルの合計を計算
    /// </summary>
    /// <returns></returns>
    public int[] AddBuffStatus()
    {
        int[] _list = new int[14];
        for (var i = 0;i < _bufflist.Count;i++)
        {
            for (var j = 0; j < _bufflist[i]._datalist.Length;j++)
            {
                _list[j] += _bufflist[i]._datalist[j];
            }
            
        }
        return _list;
    }

    /// <summary>
    ///ステータス計算
    ///各補正値と基本ステータスを足す。
    ///攻撃速度、命中、回避、必殺率の計算も行う
    /// </summary>
    public void TotalStatus()
    {
        _addstatuslist = AddPassiveStatus();
        _addbufflist = AddBuffStatus();
        _itemprefablist.GetComponent<ItemPrefabList>().RemoveItem();

        if (!_equipment)
        {
            _equipment = Resources.Load("Unarmed") as GameObject;
        }

        RevisionStatus();

        _totalMaxhp = def_st[0];
        _totalstr = def_st[1] + _addstatuslist[1] + _addbufflist[0] + _addonetimestatuslist[1];
        _totalskl = def_st[2] + _addstatuslist[2] + _addbufflist[1] + _addonetimestatuslist[2];
        _totalspd = def_st[3] + _addstatuslist[3] + _addbufflist[2] + _addonetimestatuslist[3];
        _totalluk = def_st[4] + _addstatuslist[4] + _addbufflist[3] + _addonetimestatuslist[4];
        _totaldef = def_st[5] + _addstatuslist[5] + _addbufflist[4] + _addonetimestatuslist[5];
        _totalcur = def_st[6] + _addstatuslist[6] + _addbufflist[5] + _addonetimestatuslist[6];
        _totalmove = def_st[7] + _addstatuslist[7] + _addbufflist[6] + _addonetimestatuslist[7];
        
        //攻撃回数(基本は1)
        _attack_count = _equipment.GetComponent<Weapon>()._attackcount + _addbufflist[10] + _addonetimestatuslist[11];

        //重さ無視
        if (_skillchecker._Rigidarm)
        {
            //攻撃速度(力)
            _attack_speed = _totalstr;
        }
        else
        {
            //攻撃速度(力 − 武器の重さ)
            _attack_speed = -_equipment.GetComponent<Weapon>()._weight + _totalstr;
        }
        //命中率(武器の命中率 + 技 + 運/2)
        _hit = _equipment.GetComponent<Weapon>()._hit + _totalskl + (_totalluk / 2) + _addbufflist[7] + _addonetimestatuslist[8];

        //回避率計算
        //足元のマスがとれない場合地形補正はなしにする
        if (transform.tag == "Player")
        {
            if (GetComponent<Move_System>().GetNowPos() && !_georges)
            {
                //回避率(速さ + 運 + 地形補正)
                _avoidance = _totalspd + _totalluk + _addbufflist[8] + _addonetimestatuslist[9] + GetComponent<Move_System>().GetNowPos().GetComponent<MapStatus>().GetMapEvasionRate();
            }
            else
            {
                //回避率(速さ + 運)
                _avoidance = _totalspd + _totalluk + _addbufflist[8] + _addonetimestatuslist[9];
            }
        }
        else if (transform.tag == "Enemy")
        {
            if (GetComponent<EnemyBase>().GetNowPos() && !_georges)
            {
                //回避率(速さ + 運 + 地形補正)
                _avoidance = _totalspd + _totalluk + _addbufflist[8] + _addonetimestatuslist[9] + GetComponent<EnemyBase>().GetNowPos().GetComponent<MapStatus>().GetMapEvasionRate();
            }
            else
            {
                //回避率(速さ + 運 + 地形補正)
                _avoidance = _totalspd + _totalluk + _addbufflist[8] + _addonetimestatuslist[9];
            }
        }
        
        //必殺率(武器の必殺率 + 技/2)
        _critical = _equipment.GetComponent<Weapon>()._critical + (_totalskl / 2) + _addbufflist[9] + _addonetimestatuslist[10];
        //攻撃範囲のセット
        _range[0] = _equipment.GetComponent<Weapon>()._min + _addbufflist[11] + _addonetimestatuslist[12];
        _range[1] = _equipment.GetComponent<Weapon>()._max + _addbufflist[12] + _addonetimestatuslist[13];

        //攻撃力計算
        //装備が銃だったら技を参照にする。
        if (_equipment.GetComponent<Weapon>()._weapontype == Weapon_Type.Gun || _equipment.GetComponent<Weapon>()._weapontype == Weapon_Type.Rifle)
        {
            //射程プラス1
            if (_skillchecker._Awesomearm)
            {
                _range[1] += 1;
            }
            _total_attack = _totalskl + _equipment.GetComponent<Weapon>()._atk + _addonetimestatuslist[0];
        }
        else
        {
            _total_attack = _totalstr + _equipment.GetComponent<Weapon>()._atk + _addonetimestatuslist[0];
        }


        //麻痺の場合移動力半減
        if (_NowState == State.Paralysis)
        {
            _totalmove = _totalmove / 2;
        }
        else if (_NowState == State.Sleep)
        {
            _totalmove = 0;
            _avoidance = 0;
        }

        //最大値以上or0以下にならないように補正
        if (_totalMaxhp < _totalhp)
        {
            _totalhp = _totalMaxhp;
        }
        else if (_totalhp < 0)
        {
            _totalhp = 0;
        }
        if(_attack_speed < 0)
        {
            _attack_speed = 0;
        }
    }

    /// <summary>
    /// ステータスの上限値を超えないように補正
    /// </summary>
    void RevisionStatus()
    {
        //上限値以下かの判定
        //def_st = new int[8];
        def_st[0] = _hp + classList.param[(int)_joblist].hp;
        def_st[1] = _str + classList.param[(int)_joblist].str;
        def_st[2] = _skl + classList.param[(int)_joblist].skl;
        def_st[3] = _spd + classList.param[(int)_joblist].spd;
        def_st[4] = _luk + classList.param[(int)_joblist].luk;
        def_st[5] = _def + classList.param[(int)_joblist].def;
        def_st[6] = _cur + classList.param[(int)_joblist].cur;
        def_st[7] = _move + classList.param[(int)_joblist].move;
        if (class_def_list.param[(int)_joblist].hp_max < _hp + classList.param[(int)_joblist].hp)
        {
            def_st[0] = class_def_list.param[(int)_joblist].hp_max;
        }
        if (class_def_list.param[(int)_joblist].str_max < _str + classList.param[(int)_joblist].str)
        {
            def_st[1] = class_def_list.param[(int)_joblist].str_max;
        }
        if (class_def_list.param[(int)_joblist].skl_max < _skl + classList.param[(int)_joblist].skl)
        {
            def_st[2] = class_def_list.param[(int)_joblist].skl_max;
        }
        if (class_def_list.param[(int)_joblist].spd_max < _spd + classList.param[(int)_joblist].spd)
        {
            def_st[3] = class_def_list.param[(int)_joblist].spd_max;
        }
        if (class_def_list.param[(int)_joblist].luk_max < _luk + classList.param[(int)_joblist].luk)
        {
            def_st[4] = class_def_list.param[(int)_joblist].luk_max;
        }
        if (class_def_list.param[(int)_joblist].def_max < _def + classList.param[(int)_joblist].def)
        {
            def_st[5] = class_def_list.param[(int)_joblist].def_max;
        }
        if (class_def_list.param[(int)_joblist].cur_max < _cur + classList.param[(int)_joblist].cur)
        {
            def_st[6] = class_def_list.param[(int)_joblist].cur_max;
        }
        if (class_def_list.param[(int)_joblist].move_max < _move + classList.param[(int)_joblist].move)
        {
            def_st[7] = class_def_list.param[(int)_joblist].move_max;
        }
    }

    /// <summary>
    /// クラスチェンジ
    /// </summary>
    /// <param name="_classname">チェンジ先のクラス名</param>
    public void ClassChange(Joblist _classname)
    {
        _joblist = _classname;
        //レベルの初期化
        _level = 1;
    }

    /// <summary>
    /// HPの回復
    /// </summary>
    public void RecoveryHP(int _heal)
    {
        int i = 0;
        i = _heal;
        //回復量増加状態
        if (_healplus)
        {
            i = (int)(i * 1.5f);
        }
        _totalhp += i;
        if (_totalhp > _totalMaxhp)
        {
            _totalhp = _totalMaxhp;
        }
        TotalStatus();
    }

    /// <summary>
    /// バフスキル追加
    /// </summary>
    /// <param name="str">力</param>
    /// <param name="skl">技</param>
    /// <param name="spd">速</param>
    /// <param name="luk">運</param>
    /// <param name="def">守</param>
    /// <param name="cur">呪</param>
    /// <param name="move">移動</param>
    /// <param name="hit">命中</param>
    /// <param name="avo">回避</param>
    /// <param name="cri">必殺</param>
    /// <param name="count">攻撃回数</param>
    /// <param name="min">最小</param>
    /// <param name="max">最大</param>
    /// <param name="turn">持続ターン</param>
    public void AddBuff(int str, int skl, int spd, int luk, int def, int cur, int move, int hit, int avo, int cri, int count, int min, int max,int turn)
    {
        int[] i = new int[14];
        i[0] = str;
        i[1] = skl;
        i[2] = spd;
        i[3] = luk;
        i[4] = def;
        i[5] = cur;
        i[6] = move;
        i[7] = hit;
        i[8] = avo;
        i[9] = cri;
        i[10] = count;
        i[11] = min;
        i[12] = max;
        i[13] = turn;
        var b = new Buff();
        b.SetData(i);
        _bufflist.Add(b);
    }

    /// <summary>
    /// ターン終了時の処理
    /// </summary>
    public void end()
    {
        //ターン制限付きのスキルのターンを1経過させる
        for(int i = 0; i < _bufflist.Count;i++)
        {
            //ターン0のバフは削除
            if (_bufflist[i].AddTurn())
            {
                _bufflist.RemoveAt(i);
            }
        }
        //毒の場合最大Hpの20％ダメージ
        if (_NowState == State.Poison)
        {
            Debug.Log("毒ダメージ");
            _totalhp -= _totalMaxhp / 5;
        }
        else if (_NowState == State.Fire)
        {
            Debug.Log("やけどダメージ");
            _totalhp -= (int)(_totalMaxhp * 0.4f);
        }
        if (_yani)
        {
            Debug.Log("タバコで回復");
            RecoveryHP((int)(_totalMaxhp * 0.1f));
            _yaniTurn--;
            if (_yaniTurn <= 0)
            {
                _yani = false;
            }
        }
        if (_healplus)
        {
            _healplusturn--;
            if (_healplusturn <= 0)
            {
                _healplus = false;
            }
        }
        TotalStatus();
    }

    /// <summary>
    /// Stateの変更
    /// </summary>
    /// <param name="_statename">変更するState名:Poison:Paralysis:Default</param>
    public void ChangeState(string _statename)
    {
        _NowState = (State)Enum.Parse(typeof(State), _statename);
    }

    /// <summary>
    /// 経験値取得
    /// </summary>
    /// <param name="_getexp">入手経験値</param>
    public void GetExp(int _getexp)
    {
        var rate = 1.0f;
        //Eliteの場合経験値1.5倍
        if (_skillchecker._Elite) rate = 1.5f;
        if (transform.tag != "Enemy")
        {
            FindObjectOfType<ExpGage>().SetExpGage(_exp, _exp + (int)(_getexp * rate));
            _exp += (int)(_getexp * rate);
        }
        
    }

    /// <summary>
    /// レベルアップ
    /// </summary>
    void LevelUp()
    {
        List<string> lvup = new List<string>();
        bool _loop = true;
        _level++;
        _totalLevel++;
        _exp -= 100;
        //各能力が上昇するかの抽選
        if (UnityEngine.Random.Range(0, 100) <= _hprate)
        {
            lvup.Add("HP + 1");
            _hp++;
            _loop = false;
        }
        if (UnityEngine.Random.Range(0, 100) <= _strrate)
        {
            lvup.Add("力 + 1");
            _str++;
            _loop = false;
        }
        if (UnityEngine.Random.Range(0, 100) <= _sklrate)
        {
            lvup.Add("技 + 1");
            _skl++;
            _loop = false;
        }
        if (UnityEngine.Random.Range(0, 100) <= _spdrate)
        {
            lvup.Add("速 + 1");
            _spd++;
            _loop = false;
        }
        if (UnityEngine.Random.Range(0, 100) <= _lukrate)
        {
            lvup.Add("運 + 1");
            _luk++;
            _loop = false;
        }
        if (UnityEngine.Random.Range(0, 100) <= _defrate)
        {
            lvup.Add("守 + 1");
            _def++;
            _loop = false;
        }
        if (UnityEngine.Random.Range(0, 100) <= _currate)
        {
            lvup.Add("呪 + 1");
            _cur++;
            _loop = false;
        }
        if (UnityEngine.Random.Range(0, 100) <= _moverate)
        {
            lvup.Add("移動 + 1");
            _move++;
            _loop = false;
        }

        //能力が一切上がらなかった場合５０％の確率で再抽選
        if(_loop && UnityEngine.Random.Range(0, 2) == 0)
        {
            if (UnityEngine.Random.Range(0, 100) <= _hprate)
            {
                lvup.Add("HP + 1");
                _hp++;
                _loop = false;
            }
            if (UnityEngine.Random.Range(0, 100) <= _strrate)
            {
                lvup.Add("力 + 1");
                _str++;
                _loop = false;
            }
            if (UnityEngine.Random.Range(0, 100) <= _sklrate)
            {
                lvup.Add("技 + 1");
                _skl++;
                _loop = false;
            }
            if (UnityEngine.Random.Range(0, 100) <= _spdrate)
            {
                lvup.Add("速 + 1");
                _spd++;
                _loop = false;
            }
            if (UnityEngine.Random.Range(0, 100) <= _lukrate)
            {
                lvup.Add("運 + 1");
                _luk++;
                _loop = false;
            }
            if (UnityEngine.Random.Range(0, 100) <= _defrate)
            {
                lvup.Add("守 + 1");
                _def++;
                _loop = false;
            }
            if (UnityEngine.Random.Range(0, 100) <= _currate)
            {
                lvup.Add("呪 + 1");
                _cur++;
                _loop = false;
            }
            if (UnityEngine.Random.Range(0, 100) <= _moverate)
            {
                lvup.Add("移動 + 1");
                _move++;
                _loop = false;
            }
        }

        _skillprefablist.GetComponent<SkillPrefabList>().CheckSkillLevel(_totalLevel);
        if (transform.tag == "Player")
        {
            FindObjectOfType<BattleFlowTest>().LevelUpUI_Init(gameObject, lvup);
        }
        
    }
}
