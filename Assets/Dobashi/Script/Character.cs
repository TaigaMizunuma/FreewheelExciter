using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Character : MonoBehaviour {

    private int _id;                            //Characterリストのナンバー
    private int _classid;                       //classリストのナンバー
    public GameObject _equipment;               //装備アイテムのID

    public Image _faceimage;    //フォアグラ

    public string _name;        //名前
    public string _class;       //職業
    public Vector3 _position;   //座標
    public bool _hero = false;  //主人公かどうか
    private bool _reset = false;

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

    public int _exp;                     //経験値

    public int[] _addstatuslist = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };                 //定数増加量配列
    public int[] _addonetimestatuslist = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };          //戦闘時ステータス増加量配列


    public int _total_attack = 0;           //最終的な攻撃力(装備が銃なら技を参照する)
    public int _attack_count = 1;           //攻撃回数(勇者系などの複数回攻撃武器で使用)
    public int _attack_speed = 0;           //攻撃速度
    public float _hit = 0;                  //命中率
    public float _avoidance = 0;            //回避率(地形補正抜き)
    public float _critical = 0;             //必殺率

    public int[] _range = {0,0};            //攻撃範囲(配列の0番目が最小,1番目が最大)

    public GameObject _itemprefablist;      //アイテムの親オブジェクトの取得
    public GameObject _skillprefablist;     //スキルの親オブジェクトの取得
    public SkillChecker _skillchecker;      //スキルチェッカースクリプト

    //キャラリスト読み込み
    Entity_CharaList charaList;
    //キャラ成長率リスト読み込み
    Entity_Chara_rateList chara_rateList;
    //クラスリスト読み込み
    Entity_ClassList classList;
    //武器リスト読み込み
    Entity_WeaponList weaponList;

    //現在のHP状態
    public HP_State _HpState;
    //現在の状態異常
    public State _NowState;

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
        Poison = 2      //毒
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
        //武器リストを読み込み
        weaponList = Resources.Load("Data/WeaponList") as Entity_WeaponList;
        //自分の子にあるアイテムリストを取得
        _itemprefablist = GameObject.Find( transform.name + "/ItemList");
        //自分の子にあるスキルリストを取得
        _skillprefablist = GameObject.Find(transform.name + "/SkillList");
        _skillchecker = GetComponent<SkillChecker>();

        //キャラの初期化
        if (!_reset)
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
                    //クラス(職業)の読み込み
                    _class = charaList.param[_id].job;

                }
            }

            _reset = true;
        }
        ClassChange(_class);
        TotalStatus();
        FullRecoveryHP();
    }
	// Update is called once per frame
	void Update () {
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
        
        //exp100でレベルアップ
        if (_exp >= 100)
        {
            LevelUp();
        }

        //AddStatus();
        TotalStatus();
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    void Dead()
    {
        //Debug.Log("死:" + _name);
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
        _addonetimestatuslist = new int[14] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
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
    /// 装備のstockの取得
    /// </summary>
    /// <returns>ストック数</returns>
    public int GetStock()
    {
        return _equipment.GetComponent<Weapon>()._stock;
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
                Debug.Log(_equipment.transform.name + "が壊れた…");
            }
        }
        
    }

    /// <summary>
    /// パッシブスキルの合計を計算
    /// </summary>
    public int[] AddStatus()
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
    ///ステータス計算
    ///各補正値と基本ステータスを足す。
    ///攻撃速度、命中、回避、必殺率の計算も行う
    /// </summary>
    void TotalStatus()
    {
        _addstatuslist = AddStatus();

        _totalMaxhp = _hp + classList.param[_classid].hp;     

        _totalstr = _str + classList.param[_classid].str + _addstatuslist[1] + _addonetimestatuslist[1];
        _totalskl = _skl + classList.param[_classid].skl + _addstatuslist[2] + _addonetimestatuslist[2];
        _totalspd = _spd + classList.param[_classid].spd + _addstatuslist[3] + _addonetimestatuslist[3];
        _totalluk = _luk + classList.param[_classid].luk + _addstatuslist[4] + _addonetimestatuslist[4];
        _totaldef = _def + classList.param[_classid].def + _addstatuslist[5] + _addonetimestatuslist[5];
        _totalcur = _cur + classList.param[_classid].cur + _addstatuslist[6] + _addonetimestatuslist[6];
        _totalmove = _move + classList.param[_classid].move + _addstatuslist[7] + _addonetimestatuslist[7];
  
        //攻撃回数(基本は1)
        _attack_count = _equipment.GetComponent<Weapon>()._attackcount + _addonetimestatuslist[11];

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
        _hit = _equipment.GetComponent<Weapon>()._hit + _totalskl + (_totalluk / 2) + _addonetimestatuslist[8];
        //回避率(速さ + 運)
        _avoidance = _totalspd + _totalluk + _addonetimestatuslist[9];
        //必殺率(武器の必殺率 + 技/2)
        _critical = _equipment.GetComponent<Weapon>()._critical + (_totalskl / 2) + _addonetimestatuslist[10];
        //攻撃範囲のセット
        _range[0] = _equipment.GetComponent<Weapon>()._min + _addonetimestatuslist[12];
        _range[1] = _equipment.GetComponent<Weapon>()._max + _addonetimestatuslist[13];

        //攻撃力計算
        //装備が銃だったら技を参照にする。
        if (_equipment.GetComponent<Weapon>()._type == "gun")
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
    /// クラスチェンジ
    /// </summary>
    /// <param name="_classname">チェンジ先のクラス名</param>
    public void ClassChange(string _classname)
    {
        //クラスを名前で検索
        for (var i = 0; i < classList.param.Count; i++)
        {
            if (classList.param[i].name == _classname)
            {
                //クラスID上書き
                _classid = classList.param[i].id;
                //レベルの初期化
                _level = 1;
               
            }
        }
    }

    /// <summary>
    /// HPの全回復
    /// </summary>
    public void FullRecoveryHP()
    {
        _totalhp = _totalMaxhp;
    }

    /// <summary>
    /// ターン終了時の処理
    /// </summary>
    public void end()
    {
        if (_NowState == State.Poison)
        {
            _totalhp -= _totalMaxhp / 5;
        }
    }

    /// <summary>
    /// Stateの変更
    /// </summary>
    /// <param name="_statename">変更するState名</param>
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
        bool _loop = true;
        _level++;
        _totalLevel++;
        _exp -= 100;       
        //各能力が上昇するかの抽選
        if (UnityEngine.Random.Range(0, 100) <= _hprate)
        {
            Debug.Log("HP + 1");
            _hp++;
            _loop = false;
        }
        if (UnityEngine.Random.Range(0, 100) <= _strrate)
        {
            Debug.Log("力 + 1");
            _str++;
            _loop = false;
        }
        if (UnityEngine.Random.Range(0, 100) <= _sklrate)
        {
            Debug.Log("技 + 1");
            _skl++;
            _loop = false;
        }
        if (UnityEngine.Random.Range(0, 100) <= _spdrate)
        {
            Debug.Log("速さ + 1");
            _spd++;
            _loop = false;
        }
        if (UnityEngine.Random.Range(0, 100) <= _lukrate)
        {
            Debug.Log("幸運 + 1");
            _luk++;
            _loop = false;
        }
        if (UnityEngine.Random.Range(0, 100) <= _defrate)
        {
            Debug.Log("守備 + 1");
            _def++;
            _loop = false;
        }
        if (UnityEngine.Random.Range(0, 100) <= _currate)
        {
            Debug.Log("呪力 + 1");
            _cur++;
            _loop = false;
        }
        if (UnityEngine.Random.Range(0, 100) <= _moverate)
        {
            Debug.Log("移動力 + 1");
            _move++;
            _loop = false;
        }

        //能力が一切上がらなかった場合５０％の確率で再抽選
        if(_loop || UnityEngine.Random.Range(0, 2) == 0)
        {
            if (UnityEngine.Random.Range(0, 100) <= _hprate)
            {
                Debug.Log("HP + 1");
                _hp++;
            }
            if (UnityEngine.Random.Range(0, 100) <= _strrate)
            {
                Debug.Log("力 + 1");
                _str++;
            }
            if (UnityEngine.Random.Range(0, 100) <= _sklrate)
            {
                Debug.Log("技 + 1");
                _skl++;
            }
            if (UnityEngine.Random.Range(0, 100) <= _spdrate)
            {
                Debug.Log("速さ + 1");
                _spd++;
            }
            if (UnityEngine.Random.Range(0, 100) <= _lukrate)
            {
                Debug.Log("幸運 + 1");
                _luk++;
            }
            if (UnityEngine.Random.Range(0, 100) <= _defrate)
            {
                Debug.Log("守備 + 1");
                _def++;
            }
            if (UnityEngine.Random.Range(0, 100) <= _currate)
            {
                Debug.Log("呪力 + 1");
                _cur++;
            }
            if (UnityEngine.Random.Range(0, 100) <= _moverate)
            {
                Debug.Log("移動力 + 1");
                _move++;
            }
        }

        _skillprefablist.GetComponent<SkillPrefabList>().CheckSkillLevel(_totalLevel);

    }
}
