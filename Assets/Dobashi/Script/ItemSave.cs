using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSave : MonoBehaviour {

    //アイテムプレハブスクリプト
    private ItemPrefabList _itemp;
    //ロード時の数値
    public int r_id;             //判別用ID
    public string r_category;        //武器かアイテムか
    public string r_type;         //Itemタイプ
    public string r_name;        //名前
    public int r_stock;          //耐久値
    public int r_maxstock;       //最大耐久値
    public string r_message;     //説明
    public int r_recovery;       //回復量
    public string r_effect;      //特殊効果

    public int r_atk;            //攻撃力
    public int r_weight;         //重さ
    public int r_hit;            //命中
    public int r_cri;            //必殺
    public int r_attackcount;    //攻撃回数
    public int r_min;            //最小射程
    public int r_max;            //最大射程
    public string r_weapontype;  //武器タイプ
    public string r_weaponEtype; //特殊効果

    [SerializeField]
    public class DataSave
    {
        [SerializeField]
        public int _id;             //判別用ID
        public string _category;       //武器かアイテムか
        public string _type;        //Itemタイプ
        public string _name;        //名前
        public int _stock;          //耐久値
        public int _maxstock;       //最大耐久値
        public string _message;     //説明
        public int _recovery;       //回復量
        public string _effect;      //特殊効果

        public int _atk;            //攻撃力
        public int _weight;         //重さ
        public int _hit;            //命中
        public int _cri;            //必殺
        public int _attackcount;    //攻撃回数
        public int _min;            //最小射程
        public int _max;            //最大射程
        public string _weapontype;  //武器タイプ
        public string _weaponEtype; //特殊効果

        public DataSave()
        {
            _id = 0;
            _category = "";
            _type = "";
            _name = "";
            _stock = 0;
            _maxstock = 0;
            _message = "";
            _recovery = 0;
            _effect = "";

            _atk = 0;
            _weight = 0;
            _hit = 0;
            _cri = 0;
            _attackcount = 1;
            _min = 0;
            _max = 0;
            _weapontype = "";
            _weaponEtype = "";

        }
    }


    // Use this for initialization
    void Start () {
		
	}

    /// <summary>
    /// アイテムをセーブするときに呼ぶ
    /// </summary>
    public void ItemDataSave(string key,GameObject item)
    {
        SaveStatus(NewData(item), key);
    }

    /// <summary>
    /// アイテムをセーブ
    /// </summary>
    public void SaveStatus(DataSave i, string key)
    {
        SaveData.SetClass<DataSave>(key, i);
    }

    /// <summary>
    /// ステータスをロードし、渡すための変数に代入
    /// </summary>
    public bool LoadStatus(string name)
    {
        if (SaveData.HasKey(name) == true)
        {
            DataSave gs = SaveData.GetClass(name, new DataSave());
            r_id = gs._id;
            r_category = gs._category;
            r_type = gs._type;
            r_name = gs._name;
            r_stock = gs._stock;
            r_maxstock = gs._maxstock;
            r_message = gs._message;
            r_recovery = gs._recovery;
            r_effect = gs._effect;

            r_atk = gs._atk;
            r_weight = gs._weight;
            r_hit = gs._hit;
            r_cri = gs._cri;
            r_attackcount = gs._attackcount;
            r_min = gs._min;
            r_max = gs._max;
            r_weapontype = gs._weapontype;
            r_weaponEtype = gs._weaponEtype;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 保存データの設定
    /// </summary>
    DataSave NewData(GameObject _item)
    {
        var i = new DataSave();
        if (_item.GetComponent<Item>())
        {
            var sc = _item.GetComponent<Item>();
            i._id = sc._id;
            i._category = "item";
            i._type = sc._type;
            i._name = sc._name;
            i._stock = sc._stock;
            i._maxstock = 0;
            i._message = sc._message;
            i._recovery = sc._recovery;
            i._effect = sc._effect.ToString();

            i._atk = 0;
            i._weight = 0;
            i._hit = 0;
            i._cri = 0;
            i._attackcount = 0;
            i._min = 0;
            i._max = 0;
            i._weapontype = "";
            i._weaponEtype = "";
        }
        else if (_item.GetComponent<Weapon>())
        {
            var sc = _item.GetComponent<Weapon>();
            i._id = sc._id;
            i._category = "weapon";
            i._type = "";
            i._name = sc._name;
            i._stock = sc._stock;
            i._maxstock = sc._maxstock;
            i._message = sc._message;
            i._recovery = 0;
            i._effect = "";

            i._atk = sc._atk;
            i._weight = sc._weight;
            i._hit = sc._hit;
            i._cri = sc._critical;
            i._attackcount = sc._attackcount;
            i._min = sc._min;
            i._max = sc._max;
            i._weapontype = sc._weapontype.ToString();
            i._weaponEtype = sc._weaponEffectType.ToString();
        }
        

        return i;
    }

}
