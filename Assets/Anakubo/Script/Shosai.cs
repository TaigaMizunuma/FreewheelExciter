using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shosai : MonoBehaviour {
    public GameObject[] pages_;
    private int page_num = 0;
    private int before_num;
    public Text page_num_text;

    //.//////////////////////////////////////
    public GameObject _chara;       //対象のキャラクターオブジェクト
    public GameObject _LeftUI;      //左側のUI(顔グラ、HPなど)
    public GameObject _UI1;         //1ページ目(基礎ステータス)
    public GameObject _UI2;         //2ページ目(戦闘用ステータス)
    public GameObject _UI3;         //3ページ目(スキル一覧)
//.///////////////////////////////////////

	// Use this for initialization
	void Start () {
        PageChange();
	}
	
	// Update is called once per frame
	void Update () {
        if (_chara != null)
        {
            //////////////////////////////////////////////////////////////////////////////////////////////
            _LeftUI.GetComponent<UILeftStatus>().SetData(_chara.GetComponent<Character>());
            _UI1.GetComponent<UIStandardStatus>().SetData(_chara.GetComponent<Character>());
            _UI2.GetComponent<UIBattleStatus>().SetData(_chara.GetComponent<Character>());
            _UI3.GetComponent<UISkillList>().SetData(_chara.GetComponent<Character>());
            //////////////////////////////////////////////////////////////////////////////////////////////
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            page_num++;
            if (page_num == pages_.Length) page_num = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            page_num--;
            if (page_num < 0) page_num = pages_.Length - 1;
        }
        if (before_num != page_num) PageChange();

        if (Input.GetKeyDown(KeyCode.X))
        {
            page_num = 0;
            _chara = null;
            gameObject.SetActive(false);
        }
    }

    void PageChange()
    {
        for(int i = 0; i < pages_.Length; i++)
        {
            if (page_num == i)
            {
                pages_[i].SetActive(true);
            }
            else
            {
                pages_[i].SetActive(false);
            }
        }
        before_num = page_num;
        page_num_text.text = "ページ  "+(page_num+1)+" / "+ pages_.Length;
    }

    public void ResetPage()
    {
        page_num = 0;
    }

    public void SetChara(GameObject c)
    {
        _LeftUI.GetComponent<UILeftStatus>().SetData(c.GetComponent<Character>());
        _UI1.GetComponent<UIStandardStatus>().SetData(c.GetComponent<Character>());
        _UI2.GetComponent<UIBattleStatus>().SetData(c.GetComponent<Character>());
        _UI3.GetComponent<UISkillList>().SetData(c.GetComponent<Character>());
    }
}
