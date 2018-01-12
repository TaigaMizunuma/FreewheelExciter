using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleManager : MonoBehaviour {

    public GameObject _AttackSide;
    public GameObject _DefenseSide;

    private bool _battleend = false;

    //public Skill_Effect_State _skill_effect_state;

    //private int _atk1,_atk2;                    //攻撃の威力
    //private float _accuracy1, _accuracy2;       //命中率
    //private int attack_speed1, attack_speed2;   //攻撃速度
    //private float avoidance1, avoidance2;       //回避率
    //private float critical1, critical2;         //必殺率
    //private float _luk1, _luk2;                 //必殺回避率

    public struct Attack_manager
    {
        public GameObject _obj;
        public Character _chara;
        public int _atk;
        public float _Totalhit;
        public float _critical;
        public int _getexp;
        public List<string> _skilleffect;
        public int _stock;
        public int _exavo;

        public bool _drain;                 //吸収
        public bool _fortress;              //無敵
        public bool _prediction;            //未来予知
        public bool _counter;               //反撃先制
        public int _revenge;                //反撃時必殺20%アップ
        public bool _fightingspirit;        //致死量ダメージ時HP1で耐える


        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            _drain = false;
            _fortress = false;
            _counter = false;
            _prediction = false;
            _revenge = 0;
            _fightingspirit = false;
            _getexp = 5;
            
        }

        /// <summary>
        /// Characterのオブジェクトとクラス、武器のstockをセット
        /// </summary>
        /// <param name="_thisobj">セットするオブジェクト</param>
        public void set_chara(GameObject _thisobj)
        {
            _obj = _thisobj;
            _chara = _thisobj.GetComponent<Character>();
            if (_chara._skillchecker._Revenge)
            {
                _revenge = 20;
            }
            _stock = _chara.GetStock();

            _exavo = 0;
            if (_obj.transform.tag == "Player")
            {
                var i = _obj.GetComponent<PlayerAttack>().RangeSerch2(3);
                foreach (GameObject j in i)
                {
                    if (j.GetComponent<SkillChecker>()._LeaderJr)
                    {
                        _exavo += 5;
                    }
                }

                var i2 = _obj.GetComponent<PlayerAttack>().RangeSerch3(1);
                if(i2.Count <= 0 && _obj.GetComponent<SkillChecker>()._Desert)
                {
                    _exavo += 15;
                } 
            }
            
            
        }

        /// <summary>
        /// 攻撃の威力をセット
        /// </summary>
        /// <param name="_target_def">攻撃対象の守備力</param>
        /// <param name="_rate">倍率</param>
        public void set_atk(int _target_def,int _rate)
        {
            _atk = (_chara._total_attack * _rate) - _target_def;
            if(_atk < 0)
            {
                _atk = 0;
            }
        }

        /// <summary>
        /// 命中率をセット
        /// </summary>
        /// <param name="_target_avoidance">敵の回避率</param>
        public void set_hit(float _target_avoidance)
        {
            var ex = 0;

            if (_obj.transform.tag == "Player")
            {
                var i = _obj.GetComponent<PlayerAttack>().RangeSerch2(3);
                foreach (GameObject j in i)
                {
                    if (j.GetComponent<SkillChecker>()._LeaderJr)
                    {
                        ex = 5;
                    }
                }
            }
            
             _Totalhit =ex + _chara._hit - _target_avoidance;
            //命中判定 50以上だった場合少し確立を上げる
            if (_Totalhit >= 50)
            {
                _Totalhit += 10.0f;
                //100以上だったら100に固定する
                if(_Totalhit >= 100)
                {
                    _Totalhit = 100;
                }
            }
            //命中が0以下だった場合1にする
            else if (_Totalhit<=0)
            {
                _Totalhit = 1;
            }
        }

        /// <summary>
        /// 必殺率をセット
        /// </summary>
        /// <param name="target_luk">敵の運</param>
        public void set_critical(int _target_luk)
        {
            int ex = 0;
            if (_obj.transform.tag == "Player")
            {
                var bn = _obj.GetComponent<Move_System>().GetNowPos();
                var i = bn.GetComponent<Square_Info>().GetNear();                
                for (var j = 0; j < i.Length; j++)
                {
                    var k = i[j].GetComponent<Square_Info>();
                    if (k.GetChara())
                    {
                        if (k.GetChara().GetComponent<SkillChecker>())
                        {
                            if (k.GetChara().GetComponent<SkillChecker>()._Charisma)
                            {
                                ex = 10;
                            }
                        }
                    }
                }
            }            
            _critical = ex + _chara._critical - _target_luk;

        }

        /// <summary>
        /// 勝利時経験値取得
        /// </summary>
        /// <param name="_lv">相手の合計Level</param>
        public int addexpwin(int _lv)
        {
            var i = 30 + (3 * (_lv - _chara._totalLevel));
            return i;
        }
        /// <summary>
        /// 膠着経験値取得
        /// </summary>
        /// <param name="_lv">相手の合計Level</param>
        /// <param name="_damage">与ダメ</param>
        public int addexpdraw(int _lv,int _damage)
        {
            var i = 3 + (_damage / 4);
            return i;
        }

        /// <summary>
        /// 経験値の加算
        /// </summary>
        public void addexp(int exp)
        {
            if (_getexp <= 0)
            {
                _getexp = 1;
            }
            _chara.GetExp(exp);
        }

    }

    /// <summary>
    /// 戦闘準備
    /// お互いの与ダメージと
    /// オブジェクト、Characterスクリプト、命中率のセット
    /// </summary>
    /// <param name="_attack">攻撃側のオブジェクト</param>
    /// <param name="_interceptor">迎撃側のオブジェクト</param>
    public void BattleSetup(GameObject _attack,GameObject _interceptor)
    {

        Attack_manager _attacker = new Attack_manager();
        Attack_manager _def = new Attack_manager();

        _battleend = false;

        //初期化
        _attacker.Initialize();
        _def.Initialize();

        //クラスセット
        _attacker.set_chara(_attack);
        _def.set_chara(_interceptor);

        //与ダメセット
        _attacker.set_atk(_def._chara._totaldef,1);
        _def.set_atk(_attacker._chara._totaldef,1);

        //命中率セット
        _attacker.set_hit(_def._chara._avoidance + _def._exavo);
        _def.set_hit(_attacker._chara._avoidance + _attacker._exavo);

        //必殺率セット
        _attacker.set_critical(_def._chara._totalluk);
        _def.set_critical(_attacker._chara._totalluk);

        //戦闘処理開始
        Battle(_attacker,_def);
    }

    /// <summary>
    /// 戦闘詳細表示用
    /// 戻り値[A:HP,A:威力,A:命中,A:必殺率,D:HP,D:威力,D:命中,D:必殺率]
    /// </summary>
    /// <param name="_attack">攻撃側のオブジェクト</param>
    /// <param name="_interceptor">迎撃側のオブジェクト</param>
    /// <returns>表示用配列</returns>
    public int[] GetBattleState(GameObject _attack, GameObject _interceptor)
    {
        int[] i = { 0, 0, 0, 0, 0, 0, 0, 0 };
        Attack_manager _attacker = new Attack_manager();
        Attack_manager _def = new Attack_manager();

        _battleend = false;

        //初期化
        _attacker.Initialize();
        _def.Initialize();

        //クラスセット
        _attacker.set_chara(_attack);
        _def.set_chara(_interceptor);

        //与ダメセット
        _attacker.set_atk(_def._chara._totaldef, 1);
        _def.set_atk(_attacker._chara._totaldef, 1);

        //命中率セット
        _attacker.set_hit(_def._chara._avoidance);
        _def.set_hit(_attacker._chara._avoidance);

        //必殺率セット
        _attacker.set_critical(_def._chara._totalluk);
        _def.set_critical(_attacker._chara._totalluk);

        i[0] = _attacker._chara._totalhp;
        i[1] = _attacker._atk;
        i[2] = (int)_attacker._Totalhit;
        i[3] = (int)_attacker._critical;

        i[4] = _def._chara._totalhp;
        i[5] = _def._atk;
        i[6] = (int)_def._Totalhit;
        i[7] = (int)_def._critical;

        return i;
    }

    /// <summary>
    /// 与ダメージ
    /// </summary>
    /// <param name="_attack">攻撃側のオブジェクト</param>
    /// <param name="_interceptor">攻撃される側のオブジェクト</param>
    public int GetDamage(GameObject _attack, GameObject _interceptor)
    {
        Attack_manager _attacker = new Attack_manager();
        Attack_manager _def = new Attack_manager();

        //与ダメセット
        _attacker.set_atk(_def._chara._totaldef,1);
        return _attacker._atk;
        
    }

    /// <summary>
    /// 戦闘処理
    /// </summary>
    /// <param name="_atkside">攻撃側</param>
    /// <param name="_defside">迎撃側</param>
    /// <param name="_addcri">必殺補正</param>
    /// <param name="weapon_stock">今の戦闘での武器の消耗</param>
    /// <returns>経験値</returns>
    int BattleSystem(Attack_manager _atkside, Attack_manager _defside,int _addcri,int weapon_stock)
    {
        if (!_battleend && _atkside._stock - weapon_stock >= 1)
        {
            //攻撃側の攻撃
            //命中判定
            if (Random.Range(0, 100) <= _atkside._Totalhit)
            {
                //命中
                //無敵判定
                if (_defside._fortress == true && Random.Range(0, 101) < _defside._chara._level)
                {
                    Debug.Log(_defside._chara._name + ":無敵!");
                }
                else
                {

                    var exrate = 1;
                    //特攻判定
                    if ((_atkside._chara._equipment.GetComponent<Weapon>()._weaponEffectType == Weapon_Effect_Type.SoldierKiller && (_defside._chara._joblist == Joblist.Soldier || _defside._chara._joblist == Joblist.General))
                        || (_atkside._chara._equipment.GetComponent<Weapon>()._weaponEffectType == Weapon_Effect_Type.BikeKiller && (_defside._chara._joblist == Joblist.Rider || _defside._chara._joblist == Joblist.Pilot || _defside._chara._joblist == Joblist.Motorhead || _defside._chara._joblist == Joblist.Captain)))
                    {
                        exrate = 2;
                    }
                    //必殺判定
                    if (Random.Range(0, 100) <= _atkside._critical + _addcri)
                    {
                        //防御無視判定
                        if (_atkside._chara.GetComponent<SkillChecker>()._SandR && (Random.Range(0, 101) < _atkside._chara._totalLevel))
                        {
                            //攻撃力に力を加算
                            if (_atkside._chara.GetComponent<SkillChecker>()._Smash && (Random.Range(0, 101) < _atkside._chara._totalstr))
                            {
                                //必殺発動+防御無視+撃殺
                                //力を2倍+敵の防御0+攻撃力に力を加算して攻撃力を再計算
                                Debug.Log(_atkside._chara._name + ":防御無視+撃殺!");
                                //キラーの場合倍率にさらに2をかける
                                _atkside.set_atk(0 - _atkside._chara._totalstr, 2 * exrate);
                            }
                            else
                            {
                                //必殺発動+防御無視
                                //力を2倍+敵の防御0にして攻撃力を再計算
                                Debug.Log(_atkside._chara._name + ":防御無視!");
                                _atkside.set_atk(0, 2 * exrate);
                            }

                            _defside._chara._totalhp -= _atkside._atk;
                        }
                        else
                        {
                            //攻撃力に力を加算
                            if (_atkside._chara.GetComponent<SkillChecker>()._Smash && (Random.Range(0, 101) < _atkside._chara._totalstr))
                            {
                                //必殺発動+撃殺
                                //力を2倍+攻撃力に力を加算して攻撃力を再計算
                                Debug.Log(_atkside._chara._name + ":撃殺!");
                                _atkside.set_atk(_defside._chara._totaldef - _atkside._chara._totalstr, 2 * exrate);
                            }
                            else
                            {                               
                                //力を2倍にして攻撃力を再計算
                                _atkside.set_atk(_defside._chara._totaldef, 2 * exrate);
                            }

                            _defside._chara._totalhp -= _atkside._atk;
                        }
                        Debug.Log(_atkside._chara._name + ":必殺!" + _atkside._atk);
                        //吸収判定
                        if (_atkside._chara.GetComponent<SkillChecker>()._D_Ballet && (Random.Range(0, 101) < _atkside._chara._totalskl))
                        {
                            Debug.Log(_atkside._chara._name + ":ダメージ吸収!");
                            _atkside._chara._totalhp += (_atkside._atk) / 2;
                        }
                        _atkside.set_atk(_defside._chara._totaldef, 1 * exrate);
                    }
                    else
                    {

                        //防御無視判定
                        if (_atkside._chara.GetComponent<SkillChecker>()._SandR && (Random.Range(0, 101) < _atkside._chara._totalLevel))
                        {
                            //攻撃力に力を加算
                            if (_atkside._chara.GetComponent<SkillChecker>()._Smash && (Random.Range(0, 101) < _atkside._chara._totalstr))
                            {
                                //防御無視+撃殺
                                //力を2倍+敵の防御0+攻撃力に力を加算して攻撃力を再計算
                                Debug.Log(_atkside._chara._name + ":防御無視+撃殺!");
                                _atkside.set_atk(0 - _atkside._chara._totalstr, 1 * exrate);
                            }
                            else
                            {
                                //防御無視
                                //力を2倍+敵の防御0にして攻撃力を再計算
                                Debug.Log(_atkside._chara._name + ":防御無視!");
                                _atkside.set_atk(0, 1 * exrate);
                            }

                            _defside._chara._totalhp -= _atkside._atk;
                        }
                        else
                        {
                            //攻撃力に力を加算
                            if (_atkside._chara.GetComponent<SkillChecker>()._Smash && (Random.Range(0, 101) < _atkside._chara._totalstr))
                            {
                                //撃殺
                                //攻撃力に力を加算して攻撃力を再計算
                                Debug.Log(_atkside._chara._name + ":撃殺!");
                                _atkside.set_atk(_defside._chara._totaldef - _atkside._chara._totalstr, 1 * exrate);
                            }
                            else
                            {
                                //通常攻撃
                                _atkside.set_atk(_defside._chara._totaldef, 1 * exrate);
                            }

                            _defside._chara._totalhp -= _atkside._atk;
                        }
                        Debug.Log(_atkside._chara._name + ":命中!" + _atkside._atk);
                        //吸収判定
                        if (_atkside._chara.GetComponent<SkillChecker>()._D_Ballet && (Random.Range(0, 101) < _atkside._chara._totalskl))
                        {
                            Debug.Log(_atkside._chara._name + ":ダメージ吸収!");
                            _atkside._chara._totalhp += _atkside._atk / 2;
                        }
                    }

                    //毒や麻痺の判定
                    if (_atkside._chara._equipment.GetComponent<Weapon>()._weaponEffectType == Weapon_Effect_Type.Poison)
                    {
                        //相手を毒にする
                        _defside._chara.ChangeState("Poison");
                    }
                    else if (_atkside._chara._equipment.GetComponent<Weapon>()._weaponEffectType == Weapon_Effect_Type.Paralysis)
                    {
                        //相手を麻痺にする
                        _defside._chara.ChangeState("Paralysis");
                    }

                    if (_defside._chara._totalhp <= 0)
                    {
                        //根性判定
                        if (_defside._fightingspirit && Random.Range(0, 101) < _defside._chara._totalluk)
                        {
                            Debug.Log(_defside._chara._name + ":根性!");
                            _defside._chara._totalhp = 1;
                        }
                        else
                        {
                            _battleend = true;
                            //戦闘終了処理
                            return _atkside.addexpwin(_defside._chara._totalLevel);                            
                        }
                    }
                    else
                    {
                        return _atkside.addexpdraw(_defside._chara._totalLevel,_atkside._atk);
                    }
                }
            }
            else
            {
                //外れ
                Debug.Log(_atkside._chara._name + ":ミス");
                return 0;
            }
        }
        return 0;
    }


    /// <summary>
    /// 戦闘実行
    /// </summary>
    /// <param name="_attacker">攻撃側の構造体</param>
    /// <param name="_def">防御側の構造体</param>
    void Battle(Attack_manager _attacker, Attack_manager _def)
    {
        //それぞれの武器の耐久減少量
        var atk_stock = 0;
        var def_stock = 0;
        //それぞれの取得経験値
        var atk_exp = 0;
        var def_exp = 0;
        //反撃封じ発動確認
        var _Cancel = false;
        //攻撃回数増加用変数
        var _count_plus = 1;
        //カウンター時。反撃封じは無効
        if (_def._chara.GetComponent<SkillChecker>()._Counter && (Random.Range(0, 101) < _def._chara._totalspd))
        {
            Debug.Log("待ち伏せ!");
            //攻撃回数2倍
            if(_def._chara.GetComponent<SkillChecker>()._Raid && (Random.Range(0, 101) < _def._chara._totalstr))
            {
                Debug.Log("攻撃回数2倍!");
                _count_plus = 2;
            }
            for (var i = 0; i < _def._chara._attack_count * _count_plus; i++)
            {

                //迎撃側の攻撃
                def_exp += BattleSystem(_def,_attacker,_def._revenge,def_stock);
                if(!_def._chara.GetComponent<SkillChecker>()._Saving || (Random.Range(0, 101) > _def._chara._totalskl * 2)) def_stock++;
                if (_battleend) break;
            }
            _count_plus = 1;


            //攻撃回数2倍
            if (_attacker._chara.GetComponent<SkillChecker>()._Raid && (Random.Range(0, 101) < _attacker._chara._totalstr))
            {
                Debug.Log("攻撃回数2倍!");
                _count_plus = 2;
            }
            for (var i = 0; i < _attacker._chara._attack_count * _count_plus; i++)
            {
                //攻撃側の攻撃
                atk_exp += BattleSystem(_attacker, _def, 0,atk_stock);
                if (!_attacker._chara.GetComponent<SkillChecker>()._Saving || (Random.Range(0, 101) > _attacker._chara._totalskl * 2)) atk_stock++;
                if (_battleend) break;
            }
            _count_plus = 1;
        }
        else
        {
            //攻撃回数2倍
            if (_attacker._chara.GetComponent<SkillChecker>()._Raid && (Random.Range(0, 101) < _attacker._chara._totalstr))
            {
                Debug.Log("攻撃回数2倍!");
                _count_plus = 2;
            }
            for (var i = 0; i < _attacker._chara._attack_count * _count_plus; i++)
            {
                //攻撃側の攻撃
                atk_exp+= BattleSystem(_attacker, _def, 0,atk_stock);
                if (!_attacker._chara.GetComponent<SkillChecker>()._Saving || (Random.Range(0, 101) > _attacker._chara._totalskl * 2)) atk_stock++;
                if (_battleend) break;
            }
            _count_plus = 1;


            //攻撃回数2倍
            if (_def._chara.GetComponent<SkillChecker>()._Raid && (Random.Range(0, 101) < _def._chara._totalstr))
            {
                Debug.Log("攻撃回数2倍!");
                _count_plus = 2;
            }
            //反撃封じ判定
            if (_attacker._chara.GetComponent<SkillChecker>()._Cancel && (Random.Range(0, 101) < _attacker._chara._totalskl)){
                _Cancel = true;
                Debug.Log("反撃封じ!");
            }
            else
            {                
                for (var i = 0; i < _def._chara._attack_count * _count_plus; i++)
                {
                    //迎撃側の攻撃
                    def_exp += BattleSystem(_def, _attacker, _def._revenge, def_stock);
                    if (!_def._chara.GetComponent<SkillChecker>()._Saving || (Random.Range(0, 101) > _def._chara._totalskl * 2)) def_stock++;
                    if (_battleend) break;
                }
            }
            _count_plus = 1;
            
        }

        //安定スキル発動時は追撃しない
        if (!_attacker._chara._stability && !_def._chara._stability)
        {
            //追撃判定
            if ((_attacker._chara._attack_speed > _def._chara._attack_speed + 3) && (_attacker._chara._totalskl > _def._chara._totalskl + 3) && _battleend == false)
            {
                //攻撃回数2倍
                if (_attacker._chara.GetComponent<SkillChecker>()._Raid && (Random.Range(0, 101) < _attacker._chara._totalstr))
                {
                    Debug.Log("攻撃回数2倍!");
                    _count_plus = 2;
                }
                for (var i = 0; i < _attacker._chara._attack_count * _count_plus; i++)
                {
                    //攻撃側の追撃
                    atk_exp += BattleSystem(_attacker, _def, 0, atk_stock);
                    if (!_attacker._chara.GetComponent<SkillChecker>()._Saving || (Random.Range(0, 101) > _attacker._chara._totalskl * 2)) atk_stock++;
                    if (_battleend) break;
                }
                _count_plus = 1;
            }
            //反撃無効時は追撃もしない
            else if ((_attacker._chara._attack_speed + 3 < _def._chara._attack_speed) &&
                (_attacker._chara._totalskl + 3 < _def._chara._totalskl) && _battleend == false && !_Cancel)
            {
                //攻撃回数2倍
                if (_def._chara.GetComponent<SkillChecker>()._Raid && (Random.Range(0, 101) < _def._chara._totalstr))
                {
                    Debug.Log("攻撃回数2倍!");
                    _count_plus = 2;
                }
                for (var i = 0; i < _def._chara._attack_count * _count_plus; i++)
                {
                    //迎撃側の追撃
                    def_exp += BattleSystem(_def, _attacker, _def._revenge, def_stock);
                    if (!_def._chara.GetComponent<SkillChecker>()._Saving || (Random.Range(0, 101) > _def._chara._totalskl * 2)) def_stock++;
                    if (_battleend) break;
                }
                _count_plus = 1;
            }
        }
        
        //経験値加算
        _attacker.addexp(atk_exp);
        _def.addexp(def_exp);
        //装備の耐久を使った分だけ減らす
        _attacker._chara.UseEquipment(atk_stock);
        _def._chara.UseEquipment(def_stock);

        _attacker._chara.BattleEnd();
        _def._chara.BattleEnd();
    }
}
