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

    // 上下左右キーの押しっぱなしに対応
    private float up_timer = 50.0f;
    private float down_timer = 50.0f;
    private float right_timer = 50.0f;
    private float left_timer = 50.0f;
    private float key_interval = 1.0f;
    // 右 左 上 下
    bool[] key_flg_ = { false, false, false, false };
    bool[] interval_flg_ = { false, false, false, false };

    // Use this for initialization
    void Start () {
        PageChange();
	}
	
	// Update is called once per frame
	void Update () {
        ControllerAxis();
        if (_chara != null)
        {
            //////////////////////////////////////////////////////////////////////////////////////////////
            _LeftUI.GetComponent<UILeftStatus>().SetData(_chara.GetComponent<Character>());
            _UI1.GetComponent<UIStandardStatus>().SetData(_chara.GetComponent<Character>());
            _UI2.GetComponent<UIBattleStatus>().SetData(_chara.GetComponent<Character>());
            _UI3.GetComponent<UISkillList>().SetData(_chara.GetComponent<Character>());
            //////////////////////////////////////////////////////////////////////////////////////////////
        }

        if (key_flg_[0])
        {
            page_num++;
            if (page_num == pages_.Length) page_num = 0;
            key_flg_[0] = false;
        }
        if (key_flg_[1])
        {
            page_num--;
            if (page_num < 0) page_num = pages_.Length - 1;
            key_flg_[1] = false;
        }
        if (before_num != page_num) PageChange();

        if (Input.GetButtonDown("X"))
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

    void ControllerAxis()
    {

        if (Input.GetAxis("AxisX") == 1)
        {
            right_timer += Time.deltaTime;
            if (right_timer > key_interval)
            {
                if (right_timer < 10)
                {
                    interval_flg_[0] = true;
                }
                right_timer = 0;
                key_flg_[0] = true;
            }
            else if (right_timer > key_interval / 5.0f && interval_flg_[0])
            {
                right_timer = 0;
                key_flg_[0] = true;
            }
        }
        else
        {
            right_timer = 50;
            interval_flg_[0] = false;
            key_flg_[0] = false;
        }
        if (Input.GetAxis("AxisX") == -1)
        {
            left_timer += Time.deltaTime;
            if (left_timer > key_interval)
            {
                if (left_timer < 10)
                {
                    interval_flg_[1] = true;
                }
                left_timer = 0;
                key_flg_[1] = true;
            }
            else if (left_timer > key_interval / 5.0f && interval_flg_[1])
            {
                left_timer = 0;
                key_flg_[1] = true;
            }
        }
        else
        {
            left_timer = 50;
            interval_flg_[1] = false;
            key_flg_[1] = false;
        }
        if (Input.GetAxis("AxisY") == 1)
        {
            up_timer += Time.deltaTime;
            if (up_timer > key_interval)
            {
                if (up_timer < 10)
                {
                    interval_flg_[2] = true;
                }
                up_timer = 0;
                key_flg_[2] = true;
            }
            else if (up_timer > key_interval / 5.0f && interval_flg_[2])
            {
                up_timer = 0;
                key_flg_[2] = true;
            }
        }
        else
        {
            up_timer = 50;
            interval_flg_[2] = false;
            key_flg_[2] = false;
        }
        if (Input.GetAxis("AxisY") == -1)
        {
            down_timer += Time.deltaTime;
            if (down_timer > key_interval)
            {
                if (down_timer < 10)
                {
                    interval_flg_[3] = true;
                }
                down_timer = 0;
                key_flg_[3] = true;
            }
            if (down_timer > key_interval / 5.0f && interval_flg_[3])
            {
                down_timer = 0;
                key_flg_[3] = true;
            }
        }
        else
        {
            down_timer = 50.0f;
            interval_flg_[3] = false;
            key_flg_[3] = false;
        }
    }
}
