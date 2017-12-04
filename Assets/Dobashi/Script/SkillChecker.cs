using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillChecker : MonoBehaviour {


    //////////////////////スキル一覧///////////////////////////
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

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
