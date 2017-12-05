using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_UnitList : MonoBehaviour {
    public GameObject[] players_;
    public GameObject unit_;
    private GameObject n_unit_;

    // Use this for initialization
    void Awake () {
        unit_.GetComponent<Text>().text = players_[0].name;
        for (int i = 1; i < players_.Length; i++)
        {
            n_unit_ = Instantiate(unit_);
            n_unit_.transform.parent = gameObject.transform;
            n_unit_.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            Vector3 pos = n_unit_.GetComponent<RectTransform>().anchoredPosition;
            pos.x = unit_.GetComponent<RectTransform>().anchoredPosition.x + (175.0f * (i % 2));
            pos.y = unit_.GetComponent<RectTransform>().anchoredPosition.y - ((float)(90 * (i / 2)));
            n_unit_.GetComponent<RectTransform>().anchoredPosition = pos;
            n_unit_.GetComponent<Text>().text = players_[i].name;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
