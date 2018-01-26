using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemList : MonoBehaviour {
    public GameObject _data;
    private GameObject _n_text;
    private List<GameObject> _texts_ = new List<GameObject>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetData(Character _chara,GameObject list_parent)
    {
        for (int i = 0; i < _texts_.Count; i++)
        {
            Destroy(_texts_[i]);
        }
        _texts_.Clear();
        List<GameObject> items_ = new List<GameObject>();
        items_ = _chara._itemprefablist.GetComponent<ItemPrefabList>()._itemprefablist;
        //_data.GetComponent<Text>().text = items_[0].name;
        for (int i = 0; i < items_.Count; i++)
        {
            _n_text = Instantiate(_data);
            _n_text.SetActive(true);
            _n_text.transform.SetParent(list_parent.transform);
            _n_text.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            Vector3 pos = _n_text.GetComponent<RectTransform>().anchoredPosition;
            pos.x = _data.GetComponent<RectTransform>().anchoredPosition.x;
            pos.y = _data.GetComponent<RectTransform>().anchoredPosition.y - (25 * i);
            _n_text.GetComponent<RectTransform>().anchoredPosition = pos;
            _n_text.GetComponent<Text>().text = items_[i].name;
            _texts_.Add(_n_text);
        }
    }

    public List<GameObject> GetItemTexts()
    {
        return _texts_;
    }
}
