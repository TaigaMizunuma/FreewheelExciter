using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Command_Skill_List
{
    Doubleattack        //2連撃
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
	
	// Update is called once per frame
	void Update () {
		
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

        }
    }
}
