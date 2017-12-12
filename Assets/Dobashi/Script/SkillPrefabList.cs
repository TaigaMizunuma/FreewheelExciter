using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPrefabList : MonoBehaviour {

    //スキルを代入する配列
    public List<GameObject> _skillprefablist = new List<GameObject>();
    //上昇するステータスの合計
    public int[] _addlist = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//攻撃力、力、技、速さ、運、防御、呪力、移動、命中、回避、必殺、攻撃回数、最小、最大

    // Use this for initialization
    void Start () {
        SkillAddList();
    }

    void SkillAddList()
    {
        //子オブジェクト(スキル)を配列に代入
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            _skillprefablist.Add(gameObject.transform.GetChild(i).gameObject);
            if(gameObject.transform.GetChild(i).gameObject.GetComponent<PassiveSkill>())
            {
                if (transform.parent.gameObject.GetComponent<Character>()._totalLevel >=
                    gameObject.transform.GetChild(i).gameObject.GetComponent<PassiveSkill>()._level)
                {
                    gameObject.transform.GetChild(i).gameObject.GetComponent<PassiveSkill>()._activ = true;
                }                
            }
            else if (gameObject.transform.GetChild(i).gameObject.GetComponent<RandomSkill>())
            {
                if (transform.parent.gameObject.GetComponent<Character>()._totalLevel >= gameObject.transform.GetChild(i).gameObject.GetComponent<RandomSkill>()._level)
                {
                    gameObject.transform.GetChild(i).gameObject.GetComponent<RandomSkill>()._activ = true;
                }
                
            }
            else if (gameObject.transform.GetChild(i).gameObject.GetComponent<CommandSkill>())
            {
                if (transform.parent.gameObject.GetComponent<Character>()._totalLevel >= gameObject.transform.GetChild(i).gameObject.GetComponent<CommandSkill>()._level)
                {
                    gameObject.transform.GetChild(i).gameObject.GetComponent<CommandSkill>()._activ = true;
                }

            }
        }
    }

    /// <summary>
    /// スキルの追加
    /// </summary>
    /// <param name="skill">追加するオブジェクト</param>
    public void AddItem(GameObject skill)
    {
        skill.transform.parent = transform;
        SkillAddList();
    }


    /// <summary>
    /// スキル習得チェック
    /// レベルアップ時に呼ぶ
    /// </summary>
    public void CheckSkillLevel(int level)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.GetComponent<PassiveSkill>())
            {
                if (transform.parent.gameObject.GetComponent<Character>()._totalLevel >= gameObject.transform.GetChild(i).gameObject.GetComponent<PassiveSkill>()._level)
                {
                    gameObject.transform.GetChild(i).gameObject.GetComponent<PassiveSkill>()._activ = true;
                }
            }
            else if (gameObject.transform.GetChild(i).gameObject.GetComponent<RandomSkill>())
            {
                if (transform.parent.gameObject.GetComponent<Character>()._totalLevel >= gameObject.transform.GetChild(i).gameObject.GetComponent<RandomSkill>()._level)
                {
                    gameObject.transform.GetChild(i).gameObject.GetComponent<RandomSkill>()._activ = true;
                }

            }
            else if (gameObject.transform.GetChild(i).gameObject.GetComponent<CommandSkill>())
            {
                if (transform.parent.gameObject.GetComponent<Character>()._totalLevel >= gameObject.transform.GetChild(i).gameObject.GetComponent<CommandSkill>()._level)
                {
                    gameObject.transform.GetChild(i).gameObject.GetComponent<CommandSkill>()._activ = true;
                }

            }
        }
    }

    /// <summary>
    /// スキルを発動する
    /// </summary>
    /// <param name="chara">発動者のオブジェクト</param>
    /// <param name="skill">発動するスキル</param>
    public void SkillEffect(GameObject chara,GameObject skill)
    {
        
        if (skill.GetComponent<CommandSkill>())
        {
            var i = skill.GetComponent<CommandSkill>();
            i.Effect(chara);
        }
    }


    /// <summary>
    /// 指定のタイプのスキル一覧を返す
    /// </summary>
    /// <param name="type">スキルタイプ</param>
    public List<GameObject> SearchSkill(string type)
    {
        List<GameObject> _skill = new List<GameObject>();
        for (int i = 0; i < _skillprefablist.Count; i++)
        {
            if ((_skillprefablist[i].GetComponent<PassiveSkill>() && type == "Passive") ||
                (_skillprefablist[i].GetComponent<RandomSkill>() && type == "Random") ||
                (_skillprefablist[i].GetComponent<CommandSkill>() && type == "Command"))
            {
                _skill.Add(_skillprefablist[i]);              
            }
        }
        return _skill;
    }
}
