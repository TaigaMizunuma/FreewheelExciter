using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitList : MonoBehaviour {
    public GameObject[] players_;
    public GameObject unit_;
    private GameObject n_unit_;
    private List<GameObject> unit_texts_ = new List<GameObject>();

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	}

    public GameObject GetPlayerModel(int a)
    {
        return players_[a];
    }

    public List<GameObject> GetTexts()
    {
        return unit_texts_;
    }

    public GameObject[] GetPlayers()
    {
        return players_;
    }

    public void Init()
    {
        players_ = GameObject.Find("ReadyCanvas").GetComponent<PosSort>().GetPlayers();
        unit_.GetComponent<Text>().text = players_[0].GetComponent<Character>()._name;
        unit_texts_.Add(unit_);
        for (int i = 1; i < players_.Length; i++)
        {
            if (players_[i].GetComponent<Character>()._isDead) continue;
            n_unit_ = Instantiate(unit_);
            n_unit_.transform.SetParent(gameObject.transform);
            n_unit_.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            Vector3 pos = n_unit_.GetComponent<RectTransform>().anchoredPosition;
            pos.x = unit_.GetComponent<RectTransform>().anchoredPosition.x + (100.0f * (i % 2));
            pos.y = unit_.GetComponent<RectTransform>().anchoredPosition.y - ((float)(90 * (i / 2)));
            n_unit_.GetComponent<RectTransform>().anchoredPosition = pos;
            n_unit_.GetComponent<Text>().text = players_[i].GetComponent<Character>()._name;
            unit_texts_.Add(n_unit_);
        }
        GameObject.Find("Hensei").GetComponent<Hensei>().Init();
    }
}
