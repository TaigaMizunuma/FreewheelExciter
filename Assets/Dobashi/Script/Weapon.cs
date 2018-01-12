using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum Weapon_Type
{
    none,       //無し
    Gun,        //銃
    Rifle,      //スナイパー専用銃
    Knife,      //短剣
    Fist,       //拳
    Spear,      //槍
    Axe         //斧
}

public enum Weapon_Effect_Type
{
    none,           //無し
    Poison,         //毒
    Paralysis,      //麻痺
    SoldierKiller,  //ソルジャー特攻
    BikeKiller      //乗り物特攻
}

public class Weapon : MonoBehaviour {

    //武器ID
    public int _id;
    //武器名
    public string _name;
    //武器の説明
    public string _message;   
    //残りの数(耐久)
    public int _stock;
    //残り回数上限
    public int _maxstock;
    //攻撃力
    public int _atk;
    //重さ
    public int _weight;
    //命中
    public int _hit;
    //必殺率
    public int _critical;
    //攻撃回数
    public int _attackcount;
    //射程(min,max)
    public int _min;
    public int _max;

    public Weapon_Type _weapontype;
    public Weapon_Effect_Type _weaponEffectType;


    // Use this for initialization
    void Start () {
        SetName();
    }
	
    /// <summary>
    /// 各種変数の代入
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="name">名前</param>
    /// <param name="message">説明</param>
    /// <param name="stock">残りの使用回数</param>
    /// <param name="maxstock">耐久最大値</param>
    /// <param name="atk">攻撃力</param>
    /// <param name="weight">重さ</param>
    /// <param name="hit">命中率</param>
    /// <param name="critical">必殺率</param>
    /// <param name="count">攻撃回数</param>
    /// <param name="rangemin">最小射程</param>
    /// <param name="rangemax">最大射程</param>
    /// <param name="weapontype">武器種</param>
    /// <param name="weaponEtype">特殊効果</param>
    public void SetStatus(int id,string name, string message, int stock, int maxstock,int atk, int weight, int hit, int critical, int count, int rangemin, int rangemax,string weapontype,string weaponEtype)
    {
        _id = id;
        _name = name;
        _message = message;
        _stock = stock;
        _maxstock = maxstock;
        _atk = atk;
        _weight = weight;
        _hit = hit;
        _critical = critical;
        _attackcount = count;
        _min = rangemin;
        _max = rangemax;
        SetName();
        _weapontype = (Weapon_Type)Enum.Parse(typeof(Weapon_Type), weapontype);
        _weaponEffectType = (Weapon_Effect_Type)Enum.Parse(typeof(Weapon_Effect_Type), weaponEtype);

    }

    void SetName()
    {
        gameObject.GetComponent<Transform>().name = _name;
    }

    /// <summary>
    /// 武器の耐久を減少させる
    /// </summary>
    public bool StockDecrement()
    {
        _stock--;
        //0になったらtrueを返す。
        if (_stock <= 0)
        {
            //Debug.Log(_name + "が壊れた…");
            Destroy(gameObject, 1.0f);
            return true;
        }
        else
        {
            return false;
        }

    }
}
