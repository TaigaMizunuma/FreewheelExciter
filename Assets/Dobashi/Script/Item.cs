using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum Effect_Type
{
    none,               //特になし(回復のみ)
    staterecovery,      //状態異常回復
    healplus,           //回復量増加
    regeneration,       //リジェネ
    classchange,        //クラスチェンジ
    bullet              //弾
}

public class Item : MonoBehaviour {

    //アイテムID
    public int _id;
    //アイテム名
    public string _name;
    //アイテムの説明
    public string _message;
    //回復量
    public int _recovery;
    //残りの数
    public int _stock;
    //アイテムタイプ
    public string _type;
    //効果の種類
    public Effect_Type _effect;

    // Use this for initialization
    void Start () {
        SetName();
	}
	
	// Update is called once per frame
	void Update () {
	}

    /// <summary>
    /// 各種変数の代入
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="name">名前</param>
    /// <param name="message">説明</param>
    /// <param name="recovery">回復量</param>
    /// <param name="stock">残りの数</param>
    /// <param name="type">タイプ</param>
    /// <param name="effect">効果</param>
    public void SetStatus(int id,string name,string message,int recovery,int stock,string type,string effect)
    {
        _id = id;
        _name = name;
        _message = message;
        _recovery = recovery;
        _stock = stock;
        _type = type;
        SetName();
        SetEnum(effect);
    }


    /// <summary>
    /// 名前をセット
    /// </summary>
    void SetName()
    {
        gameObject.GetComponent<Transform>().name = _name;
    }

    /// <summary>
    /// 文字列をenumに変換
    /// </summary>
    /// <param name="s">文字列</param>
    void SetEnum(string s)
    {
        _effect = (Effect_Type)Enum.Parse(typeof(Effect_Type), s);
    }
    
    /// <summary>
    /// アイテムを使用し、数を減少させる
    /// </summary>
    /// <param name="chara">使用者</param>
    public bool StockDecrement(GameObject chara)
    {
        UseItem(chara);
        _stock--;
        //0になったらtrueを返す。
        if (_stock <= 0)
        {
            Debug.Log(_name + "がなくなった…");
            //Destroy(gameObject,1.0f);
            return true;        
        }
        else
        {
            return false;
        }

    }

    //アイテム使用
    void UseItem(GameObject chara)
    {
        var i = chara.GetComponent<Character>();
        if (i._skillchecker._Oldsoldier)
        {
            //HP回復2倍
            i.RecoveryHP(_recovery * 2);
        }
        else
        {
            //HP回復
            i.RecoveryHP(_recovery);
        }
        
        switch (_effect)
        {
            case Effect_Type.staterecovery:
                //状態回復
                i.ChangeState("Default");
                break;
            case Effect_Type.healplus:
                //回復量増加
                i._yani = true;
                i._yaniTurn = 5;
                break;
            case Effect_Type.regeneration:
                //リジェネ
                i._healplus = true;
                i._healplusturn = 3;
                break;
            case Effect_Type.classchange:
                //クラスチェンジ
                //ガンナー
                if (chara.GetComponent<Character>()._joblist == Joblist.Gunner)
                {
                    var job = 0;
                    if (job == 0)
                    {
                        chara.GetComponent<Character>().ClassChange(Joblist.Sniper);
                    }
                    else if (job == 2)
                    {
                        chara.GetComponent<Character>().ClassChange(Joblist.Assassin);
                    }
                    else if (job == 1)
                    {
                        chara.GetComponent<Character>().ClassChange(Joblist.Rogue);
                    }
                }
                //ソルジャー
                else if (chara.GetComponent<Character>()._joblist == Joblist.Soldier)
                {
                    chara.GetComponent<Character>().ClassChange(Joblist.General);
                }
                //ファイター
                else if (chara.GetComponent<Character>()._joblist == Joblist.Fighter)
                {
                    chara.GetComponent<Character>().ClassChange(Joblist.Marshall);
                }
                //マーセナリー
                else if (chara.GetComponent<Character>()._joblist == Joblist.Mercenary)
                {
                    chara.GetComponent<Character>().ClassChange(Joblist.Warrior);
                }
                //ライダー
                else if (chara.GetComponent<Character>()._joblist == Joblist.Rider)
                {
                    chara.GetComponent<Character>().ClassChange(Joblist.Motorhead);
                }
                //パイロット
                else if (chara.GetComponent<Character>()._joblist == Joblist.Pilot)
                {
                    chara.GetComponent<Character>().ClassChange(Joblist.Captain);
                }
                //レンジャー
                else if (chara.GetComponent<Character>()._joblist == Joblist.Ranger)
                {
                    chara.GetComponent<Character>().ClassChange(Joblist.Load);
                }
                break;
            case Effect_Type.bullet:
                //弾
                if (i._equipment.GetComponent<Weapon>()._weapontype == Weapon_Type.Gun || i._equipment.GetComponent<Weapon>()._weapontype == Weapon_Type.Rifle)
                {
                    i._equipment.GetComponent<Weapon>()._stock = i._equipment.GetComponent<Weapon>()._maxstock;
                }
                break;
        }
    }
}
