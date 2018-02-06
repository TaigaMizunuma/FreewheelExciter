using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSave : MonoBehaviour {

    //アイテムプレハブスクリプト
    private ItemPrefabList _itemp;
    //ロード時の数値
    public int r_id;
    public int r_job;
    public string r_name;
    public Vector3 r_pos;
    public bool r_hero;
    public int r_lv;
    public int r_totallv;
    public int r_hp;
    public int r_totalhp;
    public int r_str;
    public int r_skl;
    public int r_spd;
    public int r_luk;
    public int r_def;
    public int r_cur;
    public int r_move;
    public int r_exp;
    public int[] r_addstatus;
    public int[] r_addonestatus;
    public int[] r_addbuffstatus;
    public bool r_stability;
    public int r_state;
    public bool r_isDead;

    [SerializeField]
    public class DataSave
    {
        [SerializeField]
        public int _id;             //判別用ID
        public string _type;        //ItemかWeapon
        public string _name;        //名前
        public int _stock;          //耐久値
        public int _maxstock;       //最大耐久値
        public string _message;     //説明
        public int _recovery;       //回復量

        public DataSave()
        {
            
        }

    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
