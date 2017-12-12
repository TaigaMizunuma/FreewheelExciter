using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Passive_Skill_List
{
    Default,        //数値上昇のみ
    Stealth,        //すり抜け移動
    Desert,         //遠隔攻撃に対して回避＋15％　       バトルマネージャー内
    Revenge,        //反撃次必殺率+20％　                バトルマネージャー内 済
    Onemore,        //行動後に余っていた分移動する　
    Charisma,       //3マス以内の味方の必殺＋10％        バトルマネージャー内 済
    LeaderJr,       //3マス以内の味方の回避、命中＋5％
    Oldsoldier,     //回復効果2倍　                      アイテム内済
    Elite,          //経験値1.5倍　                      済
    Awesomearm,     //銃装備時最大射程＋１　             済
    Leakage,        //攻撃した敵の燃料半減
    Rigidarm,       //武器の重さ無視　                   済
    Georges         //地形効果や射程で命中率が下がらない
}


public class PassiveSkill : MonoBehaviour {

    //各種基本値
    public string _name;
    public string _message;
    public int _level;
    public bool _activ = false;
    [Header ("攻撃力、力、技、速さ、運、防御、呪力、移動")]
    [Header("命中、回避、必殺、攻撃回数、最小、最大")]
    public int[] _addlist = { 0,0,0,0,0,0,0,0,0,0,0,0,0,0};//攻撃力、力、技、速さ、運、防御、呪力、移動、命中、回避、必殺、攻撃回数、最小、最大
    public Passive_Skill_List _skill_list;

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

    //public string Effect()
    //{
    //    if (_activ)
    //    {
    //        switch (_skill_list)
    //        {
    //            case Passive_Skill_List.Default:
    //                return "none";
    //            case Passive_Skill_List.Stealth:
    //                //すり抜け
    //                return "Stealth";
    //            case Passive_Skill_List.Desert:
    //                //遠隔攻撃回避＋15％
    //                return "Desert";
    //            case Passive_Skill_List.Revenge:
    //                //反撃時必殺＋20％
    //                return "Revenge";
    //            case Passive_Skill_List.Onemore:
    //                //行動後残った行動力分移動
    //                return "Onemore";
    //            case Passive_Skill_List.Charisma:
    //                //3マス以内の味方の必殺＋10％
    //                return "Charisma";
    //            case Passive_Skill_List.LeaderJr:
    //                //3マス以内の味方の回避、命中＋5％
    //                return "LeaderJr";
    //            case Passive_Skill_List.Oldsoldier:
    //                //回復効果2倍
    //                return "Oldsoldier";
    //            case Passive_Skill_List.Elite:
    //                //経験値1.5倍
    //                return "Elite";
    //            case Passive_Skill_List.Awesomearm:
    //                //銃装備時最大射程＋1
    //                return "Awesomearm";
    //            case Passive_Skill_List.Leakage:
    //                //攻撃した敵の燃料半減
    //                return "Leakage";
    //            case Passive_Skill_List.Rigidarm:
    //                //装備の重さ無視
    //                return "Rigidarm";
    //            case Passive_Skill_List.Georges:
    //                //地形効果や射程による命中の減少なし
    //                return "Georges";
    //        }
    //    }
    //    return "none";    
    //}
}
