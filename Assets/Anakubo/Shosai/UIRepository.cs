using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRepository : MonoBehaviour {
    public GameObject _data;
    private GameObject _n_text;
    private List<GameObject>[] _texts_;
    private ItemRepository i_repository_;
    private WeaponRepository w_repository_;
    public GameObject i_parent;
    public GameObject w_parent;

    // Use this for initialization
    void Start () {
        _texts_ = new List<GameObject>[2];
        for (int i = 0; i < 2; i++)
        {
            _texts_[i] = new List<GameObject>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (_texts_[0].Count == 0)
        {
            i_repository_ = GameObject.Find("RepositoryManager").GetComponent<ItemRepository>();
            w_repository_ = GameObject.Find("RepositoryManager").GetComponent<WeaponRepository>();
            for (int i = 0; i < i_repository_._itemrepository.Count; i++)
            {
                _n_text = Instantiate(_data);
                _n_text.SetActive(true);
                _n_text.transform.SetParent(i_parent.transform);
                _n_text.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                Vector3 pos = _n_text.GetComponent<RectTransform>().anchoredPosition;
                pos.x = _data.GetComponent<RectTransform>().anchoredPosition.x;
                pos.y = _data.GetComponent<RectTransform>().anchoredPosition.y - (25 * i);
                _n_text.GetComponent<RectTransform>().anchoredPosition = pos;
                _n_text.GetComponent<Text>().text = i_repository_._itemrepository[i]._name;
                _texts_[0].Add(_n_text);
            }
            for(int i = 0; i < w_repository_._weaponrepository.Count; i++)
            {
                _n_text = Instantiate(_data);
                _n_text.SetActive(true);
                _n_text.transform.SetParent(w_parent.transform);
                _n_text.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                Vector3 pos = _n_text.GetComponent<RectTransform>().anchoredPosition;
                pos.x = _data.GetComponent<RectTransform>().anchoredPosition.x;
                pos.y = _data.GetComponent<RectTransform>().anchoredPosition.y - (25 * i);
                _n_text.GetComponent<RectTransform>().anchoredPosition = pos;
                _n_text.GetComponent<Text>().text = w_repository_._weaponrepository[i]._name;
                _texts_[1].Add(_n_text);
            }
            w_parent.SetActive(false);
        }
	}

    public List<GameObject> GetItemTexts(int num)
    {
        return _texts_[num];
    }

    public void DisplayChange()
    {
        if (i_parent.activeSelf)
        {
            i_parent.SetActive(false);
            w_parent.SetActive(true);
        }
        else
        {
            i_parent.SetActive(true);
            w_parent.SetActive(false);
        }
    }
}
