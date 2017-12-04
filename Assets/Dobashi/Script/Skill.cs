using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Skill_Type
{
    Status = 0,     //ステータス定数上昇スキル
    Passive = 1,    //パッシブスキル
    Battle = 2      //戦闘時発動スキル
}

public class Skill : MonoBehaviour {

    //スキルクラス
    //キャラクターにアタッチして使う
    [Header("スキル名")]
    public string _name = "";
    [Header("スキルタイプ")]
    public Skill_Type _skilltype;
    [Header("hp、力、技、速、幸運、守備、呪力、移動力")]
    public int[] _statuslist = { 0, 0, 0, 0, 0, 0, 0, 0 };
    public int[] _statusratelist = { 0, 0, 0, 0, 0, 0 };

    private Character _chara;

    // Use this for initialization
    void Start()
    {
        if (GetComponent<Character>() != null)
        {
            _chara = GetComponent<Character>();
        }

        switch (_skilltype)
        {
            //定数上昇スキルの処理
            case Skill_Type.Status:
                break;

            //パッシブスキルの効果
            case Skill_Type.Passive:
                PassiveEffect(_name);
                break;

            //確立発動系スキルの処理
            case Skill_Type.Battle:

                break;
        }

    }

    void PassiveEffect(string _name)
    {
        switch (_name)
        {
            case "Elite":
                //経験値1.5倍
                _chara._exp_rate = 1.5f;
                break;
            case "RangePlus":
                //最大射程+1
                _chara._range[1] += 1;
                break;
            case "LeaderJr":
                //周囲3マスの味方の命中と回避+5%

                break;
            case "Stealth":
                //敵すり抜け

                break;
            case "Prediction":
                //3ターン無敵

                break;
            case "Berserk":
                //必殺+15%,命中-15%

                break;
            case "Acrobatics":
                //地形無視

                break;
            case "FirstAid":
                //回復薬の効果2倍

                break;
        }
    }


}
