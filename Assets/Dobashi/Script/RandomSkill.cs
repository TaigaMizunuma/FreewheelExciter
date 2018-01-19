using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Random_Skill_List
{
    D_Ballet,               //与ダメージの半分を吸収：技％              バトルマネージャー内：済
    Cancel,                 //反撃無効化：技％：待ち伏せには無効　      バトルマネージャー内：済
    SandR,                  //防御無視攻撃：LV％                        バトルマネージャー内：済
    Smash,                  //攻撃力に力を加算：力％　                  バトルマネージャー内：済
    Destruction,            //一撃必殺：必殺時50％                      不要
    Counter,                //攻撃されたとき先制攻撃：速さ％　          バトルマネージャー内：済
    D_Aggressor,            //受けたダメージをそのまま返す：力％        不要
    Fortress,               //無敵：Lv％　                              バトルマネージャー内：済
    Genocide,               //持っている武器すべてで攻撃:力/2%        　不要
    Weapon_Destruction,     //敵の装備を破壊：Lv%                       不要
    Running_W,              //移動量*2を力に加算：移動力％              不要
    Raid,                   //2倍攻撃する:力％                          バトルマネージャー内：済
    Shotdown,               //行動後、ステータス半分で再行動：Lv％      不要
    Saving,                 //武器の耐久が減らない：技*2%               バトルマネージャー内：済
    NewType,                //3ターン全攻撃を回避:呪力％
    Praying                 //くいしばり:運*2%                          バトルマネージャー内：済
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

    void SetName()
    {
        gameObject.GetComponent<Transform>().name = _name;
    }
}
