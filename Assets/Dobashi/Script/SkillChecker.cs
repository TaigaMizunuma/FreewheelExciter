using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillChecker : MonoBehaviour {

    private GameObject _skillprefablist;     //スキルの親オブジェクトの取得

    //////////////////////パッシブスキル一覧///////////////////////////
    public bool _Stealth = false;        //すり抜け移動
    public bool _Desert = false;         //遠隔攻撃に対して回避＋15％
    public bool _Revenge = false;        //反撃次必殺率+20％
    public bool _Onemore = false;        //行動後に余っていた分移動する
    public bool _Charisma = false;       //3マス以内の味方の必殺＋10％
    public bool _LeaderJr = false;       //3マス以内の味方の回避、命中＋5％
    public bool _Oldsoldier = false;     //回復効果2倍
    public bool _Elite = false;          //経験値1.5倍
    public bool _Awesomearm = false;     //銃装備時最大射程＋１
    public bool _Leakage = false;        //攻撃した敵の燃料半減
    public bool _Rigidarm = false;       //武器の重さ無視
    public bool _Georges = false;        //地形効果や射程で命中率が下がらない
    //////////////////////////////////////////////////////////


    /////////////////////ランダムスキル一覧////////////////////////////
    public bool _D_Ballet = false;               //与ダメージの半分を吸収：技％
    public bool _Cancel = false;                 //反撃無効化：技％
    public bool _SandR = false;                  //防御無視攻撃：LV％
    public bool _Smash = false;                  //攻撃力に力を加算：力％
    public bool _Destruction = false;            //一撃必殺：必殺時50％
    public bool _Counter = false;                //攻撃されたとき先制攻撃：速さ％
    public bool _D_Aggressor = false;            //受けたダメージをそのまま返す：力％
    public bool _Fortress = false;               //無敵：Lv％
    public bool _Genocide = false;               //持っている武器すべてで攻撃:力/2%
    public bool _Weapon_Destruction = false;     //敵の装備を破壊：Lv%
    public bool _Running_W = false;              //移動量*2を力に加算：移動力％
    public bool _Raid = false;                   //2回攻撃する:力％
    public bool _Shotdown = false;               //行動後、ステータス半分で再行動：Lv％
    public bool _Saving = false;                 //武器の耐久が減らない：技*2%
    public bool _NewType = false;                //3ターン全攻撃を回避:呪力％
    public bool _Praying = false;                //くいしばり:運*2%
    //////////////////////////////////////////////////////////////////

    // Use this for initialization
    void Start () {
        //自分の子にあるスキルリストを取得
        _skillprefablist = GameObject.Find(transform.name + "/SkillList");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 特殊効果ありパッシブスキルの検索及び発動
    /// </summary>
    public void PassiveSkillCheck()
    {
        var i = _skillprefablist.GetComponent<SkillPrefabList>().SearchSkill("Passive");
        for (var j = 0;j < i.Count;j++)
        {
            if(i[j].GetComponent<PassiveSkill>()._activ)
            {
                switch (i[j].GetComponent<PassiveSkill>()._skill_list)
                {
                    case Passive_Skill_List.Default:
                        break;
                    case Passive_Skill_List.Stealth:
                        //すり抜け
                        _Stealth = true;
                        break;
                    case Passive_Skill_List.Desert:
                        //遠隔攻撃回避＋15％
                        _Desert = true;
                        break;
                    case Passive_Skill_List.Revenge:
                        //反撃時必殺＋20％
                        _Revenge = true;
                        break;
                    case Passive_Skill_List.Onemore:
                        //行動後残った行動力分移動
                        _Onemore = true;
                        break;
                    case Passive_Skill_List.Charisma:
                        //3マス以内の味方の必殺＋10％
                        _Charisma = true;
                        break;
                    case Passive_Skill_List.LeaderJr:
                        //3マス以内の味方の回避、命中＋5％
                        _LeaderJr = true;
                        break;
                    case Passive_Skill_List.Oldsoldier:
                        //回復効果2倍
                        _Oldsoldier = true;
                        break;
                    case Passive_Skill_List.Elite:
                        //経験値1.5倍
                        _Elite = true;
                        break;
                    case Passive_Skill_List.Awesomearm:
                        //銃装備時最大射程＋1
                        _Awesomearm = true;
                        break;
                    case Passive_Skill_List.Leakage:
                        //攻撃した敵の燃料半減
                        _Leakage = true;
                        break;
                    case Passive_Skill_List.Rigidarm:
                        //装備の重さ無視
                        _Rigidarm = true;
                        break;
                    case Passive_Skill_List.Georges:
                        //地形効果や射程による命中の減少なし
                        _Georges = true;
                        break;
                }
            }         
        }
    }

    public void RandomSkillCheck()
    {
        var i = _skillprefablist.GetComponent<SkillPrefabList>().SearchSkill("Random");
        for (var j = 0; j < i.Count; j++)
        {
            if (i[j].GetComponent<PassiveSkill>()._activ)
            {
                switch (i[j].GetComponent<RandomSkill>()._skill_list)
                {
                    case Random_Skill_List.D_Ballet:
                        _D_Ballet = true;
                        break;
                    case Random_Skill_List.Cancel:
                        _Cancel = true;
                        break;
                    case Random_Skill_List.SandR:
                        _SandR = true;
                        break;
                    case Random_Skill_List.Smash:
                        _Smash = true;
                        break;
                    case Random_Skill_List.Destruction:
                        _Destruction = true;
                        break;
                    case Random_Skill_List.Counter:
                        _Counter = true;
                        break;
                    case Random_Skill_List.D_Aggressor:
                        _D_Aggressor = true;
                        break;
                    case Random_Skill_List.Fortress:
                        _Fortress = true;
                        break;
                    case Random_Skill_List.Genocide:
                        _Genocide = true;
                        break;
                    case Random_Skill_List.Weapon_Destruction:
                        _Weapon_Destruction = true;
                        break;
                    case Random_Skill_List.Running_W:
                        _Running_W = true;
                        break;
                    case Random_Skill_List.Raid:
                        _Raid = true;
                        break;
                    case Random_Skill_List.Shotdown:
                        _Shotdown = true;
                        break;
                    case Random_Skill_List.Saving:
                        _Saving = true;
                        break;
                    case Random_Skill_List.NewType:
                        _NewType = true;
                        break;
                    case Random_Skill_List.Praying:
                        _Praying = true;
                        break;

                }
            }           
        }
    }
}
