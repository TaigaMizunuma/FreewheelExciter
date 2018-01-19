using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyShosai : MonoBehaviour {
    public GameObject[] pages_;
    private int page_num = 0;
    private int before_num;
    public Text page_num_text;
    // 親のcanvasを取得
    private GameObject parent_canvas;

    //.//////////////////////////////////////
    public GameObject _chara;       //対象のキャラクターオブジェクト
    public GameObject _LeftUI;      //左側のUI(顔グラ、HPなど)
    public GameObject _UI1;         //1ページ目(基礎ステータス)
    public GameObject _UI2;         //2ページ目(戦闘用ステータス)
    public GameObject _UI3;         //3ページ目(スキル一覧)
                                    //.///////////////////////////////////////

    // Use this for initialization
    void Start()
    {
        PageChange();
        parent_canvas = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (_chara == null)
        {
            _chara = parent_canvas.GetComponent<ReadyManager>().GetUnit();
            return;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        _LeftUI.GetComponent<UILeftStatus>().SetData(_chara.GetComponent<Character>());
        _UI1.GetComponent<UIStandardStatus>().SetData(_chara.GetComponent<Character>());
        _UI2.GetComponent<UIBattleStatus>().SetData(_chara.GetComponent<Character>());
        _UI3.GetComponent<UISkillList>().SetData(_chara.GetComponent<Character>());
        //////////////////////////////////////////////////////////////////////////////////////////////

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
            parent_canvas.GetComponent<ReadyManager>().SetUnit(null);
            parent_canvas.GetComponent<ReadyManager>().ModeChange(1);
        }
    }

    void PageChange()
    {
        for (int i = 0; i < pages_.Length; i++)
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
        page_num_text.text = "ページ  " + (page_num + 1) + " / " + pages_.Length;
    }
}
