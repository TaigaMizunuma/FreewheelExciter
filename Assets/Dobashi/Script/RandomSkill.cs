using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Random_Skill_List
{
    D_Ballet,               //与ダメージの半分を吸収：技％ バトルマネージャー内：済
    Cancel,                 //反撃無効化：技％：待ち伏せには無効　バトルマネージャー内：済
    SandR,                  //防御無視攻撃：LV％
    Smash,                  //攻撃力に力を加算：力％
    Destruction,            //一撃必殺：必殺時50％
    Counter,                //攻撃されたとき先制攻撃：速さ％　バトルマネージャー内：済
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

    public string Effect()
    {
        if (_activ)
        {
            switch (_skill_list)
            {
                case Random_Skill_List.D_Ballet:
                    return "D_Ballet";
                case Random_Skill_List.Cancel:
                    return "Cancel";
                case Random_Skill_List.SandR:
                    return "SandR";
                case Random_Skill_List.Smash:
                    return "Smash";
                case Random_Skill_List.Destruction:
                    return "Destruction";
                case Random_Skill_List.Counter:
                    return "Counter";
                case Random_Skill_List.D_Aggressor:
                    return "D_Aggressor";
                case Random_Skill_List.Fortress:
                    return "Fortress";
                case Random_Skill_List.Genocide:
                    return "Genocide";
                case Random_Skill_List.Weapon_Destruction:
                    return "Weapon_Destruction";
                case Random_Skill_List.Running_W:
                    return "Running_W";
                case Random_Skill_List.Raid:
                    return "Raid";
                case Random_Skill_List.Shotdown:
                    return "Shotdown";
                case Random_Skill_List.Saving:
                    return "Saving";
                case Random_Skill_List.NewType:
                    return "NewType";
                case Random_Skill_List.Praying:
                    return "Praying";

            }
            return "none";
        }
        return "none";
    }
}
