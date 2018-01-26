using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_UnitList : MonoBehaviour {
    private GameObject[] players_;
    public GameObject unit_;
    private GameObject n_unit_;
    private List<GameObject> texts_ = new List<GameObject>();

    private PosSort pos_sort;

    // Use this for initialization
    void Awake () {
        pos_sort = GameObject.Find("ReadyCanvas").GetComponent<PosSort>();
    }
	
	// Update is called once per frame
	void Update () {
        if (players_ == null)
        {
            players_ = pos_sort.GetPlayers();
            Init();
        }
	}

    public GameObject[] GetPlayers()
    {
        return players_;
    }

    void Init()
    {
        unit_.GetComponent<Text>().text = players_[0].GetComponent<Character>()._name;
        texts_.Add(unit_);
        for (int i = 1; i < players_.Length; i++)
        {
            if (players_[i].GetComponent<Character>()._isDead) continue;
            n_unit_ = Instantiate(unit_);
            n_unit_.transform.SetParent(gameObject.transform);
            n_unit_.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            Vector3 pos = n_unit_.GetComponent<RectTransform>().anchoredPosition;
            pos.x = unit_.GetComponent<RectTransform>().anchoredPosition.x + (145.0f * (i % 2));
            pos.y = unit_.GetComponent<RectTransform>().anchoredPosition.y - ((float)(90 * (i / 2)));
            n_unit_.GetComponent<RectTransform>().anchoredPosition = pos;
            n_unit_.GetComponent<Text>().text = players_[i].GetComponent<Character>()._name;
            texts_.Add(n_unit_);
        }
        GameObject.Find("Item").GetComponent<ItemReady>().Init();
    }

    public List<GameObject> GetUnitTexts()
    {
        return texts_;
    }
}
