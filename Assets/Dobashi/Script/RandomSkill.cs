using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Random_Skill_List
{
    D_Ballet,               //与ダメージの半分を吸収：技％
    Cancel,                 //反撃無効化：技％
    SandR,                  //防御無視攻撃：LV％
    Smash,                  //攻撃力に力を加算：力％
    Destruction,            //一撃必殺：必殺時50％
    Counter,                //攻撃されたとき先制攻撃：速さ％
    D_Aggressor,            //受けたダメージをそのまま返す：力％
    Fortress,               //無敵：Lv％
    Genocide,               //持っている武器すべてで攻撃:力/2%
    Weapon_Destruction,     //敵の装備を破壊：Lv%
    Running_W,              //移動量*2を力に加算：移動力％
    Raid,                   //2回攻撃する:力％
    Shotdown,               //行動後、ステータス半分で再行動：Lv％
    Saving,                 //武器の耐久が減らない：技*2%
    NewType,                //3ターン全攻撃を回避:呪力％
    Praying                 //くいしばり:運*2%
}

public class RandomSkill : MonoBehaviour {

    public string _name;
    public string _message;
    public int _level;
    public bool _activ = false;
    public Random_Skill_List _skill_list;
    // Use this for initialization
    void Start () {
        SetName();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetName()
    {
        gameObject.GetComponent<Transform>().name = _name;
    }

    public void Effect(GameObject _chara)
    {

    }
}
