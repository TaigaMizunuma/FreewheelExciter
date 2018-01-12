using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSave : MonoBehaviour {

    public Character _chara;

    ///保存するデータ(取得用)
    public int _id = 10;             //判別用ID
    public int _job;            //クラス
    public string _name;        //名前
    public Vector3 _pos;        //座標
    public bool _hero;          //主人公かどうか
    public int _lv;             //現在Level
    public int _totallv;        //累計Level
    public int _hp;             //HP
    public int _totalhp;        //現在HP
    public int _str;            //力
    public int _skl;            //技
    public int _spd;            //速
    public int _luk;            //運
    public int _def;            //守
    public int _cur;            //呪力
    public int _move;           //移動力
    public int _exp;            //経験値
    public int[] _addstatus;    //加算ステータスList
    public int[] _addonestatus; //1戦闘のみの加算ステータスList
    public int[] _addbuffstatus;//バフによる加算ステータス
    public List<Buff> _bufflist;//バフのリスト
    public bool _stability;     //コマンドスキルによる追撃可能かの判別
    public int _state;          //状態異常

    public CharacterSave()
    {
        _id = 2210;
        _job = (int)_chara._joblist;
        _name = _chara._name;
        _pos = _chara._position;
        _hero = _chara._hero;
        _lv = _chara._level;
        _totallv = _chara._totalLevel;
        _hp = _chara.GetStatus()[0];
        _totalhp = _chara._totalhp;
        _str = _chara.GetStatus()[1];
        _skl = _chara.GetStatus()[2];
        _spd = _chara.GetStatus()[3];
        _luk = _chara.GetStatus()[4];
        _def = _chara.GetStatus()[5];
        _cur = _chara.GetStatus()[6];
        _move = _chara.GetStatus()[7];
        _exp = _chara._exp;
        _state = (int)_chara._NowState;
        _addstatus = _chara._addstatuslist;
        _addonestatus = _chara._addonetimestatuslist;
        _addbuffstatus = _chara._addbufflist;
        _bufflist = _chara._bufflist;
    }

}
