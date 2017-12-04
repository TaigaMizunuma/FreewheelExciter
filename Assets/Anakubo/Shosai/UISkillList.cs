using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillList : MonoBehaviour {
    public GameObject UI1;

    public void SetData(Character _chara)
    {
        UI1.GetComponent<Text>().text = "";

        var i = _chara._skillprefablist.GetComponent<SkillPrefabList>()._skillprefablist;
        for (var j = 0; j < i.Count; j++)
        {
            UI1.GetComponent<Text>().text += i[j].gameObject.name;
            UI1.GetComponent<Text>().text += "\n";
        }

    }
}
