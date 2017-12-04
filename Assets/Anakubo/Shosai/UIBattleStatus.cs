using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleStatus : MonoBehaviour {
    public GameObject UI1;
    public GameObject[] _data = new GameObject[4];

    public void SetData(Character _chara)
    {
        UI1.GetComponent<Text>().text = "";
        _data[0].GetComponent<Text>().text = _chara._total_attack.ToString();
        _data[1].GetComponent<Text>().text = _chara._hit.ToString();
        _data[2].GetComponent<Text>().text = _chara._critical.ToString();
        _data[3].GetComponent<Text>().text = _chara._avoidance.ToString();

        var i = _chara._itemprefablist.GetComponent<ItemPrefabList>()._itemprefablist;
        for (var j = 0;j < i.Count;j++)
        {
            if (i[j].GetComponent<Item>())
            {
                
                UI1.GetComponent<Text>().text += i[j].GetComponent<Item>()._name;
                UI1.GetComponent<Text>().text += "\n";
            }
            else if (i[j].GetComponent<Weapon>())
            {
                
                UI1.GetComponent<Text>().text += i[j].GetComponent<Weapon>()._name;
                UI1.GetComponent<Text>().text += "\n";
            }
        }

    }
}
