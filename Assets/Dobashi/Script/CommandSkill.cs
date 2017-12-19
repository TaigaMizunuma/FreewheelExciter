using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Command_Skill_List
{
    Doubleattack,        //2連撃
    Berserk,             //次の攻撃時必殺＋15%命中-15%
    Destruction,         //次の攻撃時攻撃+10守備-10
    Stability,           //お互いに追撃不可
    Emblemburst,         //HP-5して3ターンの間力に呪力を追加
    Emblemawakening      //Hp-10して5ターンの間、命中と回避と必殺を(10 + Lv)プラスする
}

public class CommandSkill : MonoBehaviour {

    public Command_Skill_List _skill_list;
    public string _name;
    public string _message;
    public int _level;
    public bool _activ = false;

    // Use this for initialization
    void Start () {
        SetName();
    }
	
    void SetName()
    {
        gameObject.GetComponent<Transform>().name = _name;
    }

    public void Effect(GameObject _chara)
    {
        switch (_skill_list)
        {
            case Command_Skill_List.Doubleattack:
                Debug.Log("次の攻撃時2回攻撃");
                _chara.GetComponent<Character>()._totalhp -= 5;
                _chara.GetComponent<Character>().OneButtleStatus(0,0,0,0,0,0,0,0,0,0,0,1,0,0);
                break;
            case Command_Skill_List.Berserk:
                Debug.Log("次の攻撃時必殺+15％、命中-15％");
                _chara.GetComponent<Character>()._totalhp -= 10;
                _chara.GetComponent<Character>().OneButtleStatus(0, 0, 0, 0, 0, 0, 0, 0, 0, -15, 15, 0, 0, 0);
                break;
            case Command_Skill_List.Destruction:
                Debug.Log("次の攻撃時力+10、守備-10");
                _chara.GetComponent<Character>()._totalhp -= 10;
                _chara.GetComponent<Character>().OneButtleStatus(0, 10, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                break;
            case Command_Skill_List.Stability:
                Debug.Log("次の攻撃時お互いに追撃不可");
                _chara.GetComponent<Character>()._stability = true;
                break;
            case Command_Skill_List.Emblemburst:
                Debug.Log("3ターンの間、力に呪力を追加");
                _chara.GetComponent<Character>()._totalhp -= 5;
                _chara.GetComponent<Character>().AddBuff(_chara.GetComponent<Character>()._totalcur,0,0,0,0,0,0,0,0,0,0,0,0,3);
                break;
            case Command_Skill_List.Emblemawakening:
                Debug.Log("5ターンの間、命中と回避と必殺を(10 + Lv)プラスする");
                _chara.GetComponent<Character>()._totalhp -= 10;
                _chara.GetComponent<Character>().AddBuff(0,0,0,0,0,0,0,10 + _chara.GetComponent<Character>()._totalLevel, 10 + _chara.GetComponent<Character>()._totalLevel, 10 + _chara.GetComponent<Character>()._totalLevel, 0,0,0,5);
                break;

        }
    }
}
