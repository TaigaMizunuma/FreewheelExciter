using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemPrefabList : MonoBehaviour {
    //アイテムと武器を代入する配列
    public List<GameObject> _itemprefablist = new List<GameObject>();


	// Use this for initialization
	void Start () {
        ItemAddList();

    }

    void ItemAddList()
    {
        //子オブジェクト(アイテム)を配列に代入
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            _itemprefablist.Add(gameObject.transform.GetChild(i).gameObject);

        }
    }

    /// <summary>
    /// アイテムまたは武器の追加
    /// </summary>
    /// <param name="item">追加するオブジェクト</param>
    public void AddItem(GameObject item)
    {
        if (_itemprefablist.Count >= 5)
        {
            //Debug.Log("アイテムがいっぱいです");
            item.transform.parent = transform;
            _itemprefablist.Add(item);
        }
        else
        {
            item.transform.parent = transform;
            _itemprefablist.Add(item);
        }
    }

    /// <summary>
    /// 子のアイテムをすべてセーブする
    /// </summary>
    /// <param name="name">親から受け取る名前</param>
    public void SaveItems(string name)
    {
        var i = GetComponent<ItemSave>();
        for (var j = 0; j < _itemprefablist.Count;j++)
        {
            i.ItemDataSave(name + "item" + j,_itemprefablist[j]);
        }
    }
    /// <summary>
    /// 子のアイテムをロードする
    /// </summary>
    public void LoadItem(string name)
    {
        var i = GetComponent<ItemSave>();
        for (var j = 0; j < 5;j++)
        {
            if (i.LoadStatus(name + "item" + j))
            {
                if (i.r_category == "item")
                {
                    var it = new GameObject("Enpty");
                    //var it = Instantiate(obj);
                    it.AddComponent<Item>();
                    it.GetComponent<Item>()._id = i.r_id;
                    it.GetComponent<Item>()._name = i.r_name;
                    it.GetComponent<Item>()._message = i.r_message;
                    it.GetComponent<Item>()._recovery = i.r_recovery;
                    it.GetComponent<Item>()._stock = i.r_stock;
                    it.GetComponent<Item>()._type = i.r_type;
                    it.GetComponent<Item>()._effect = (Effect_Type)Enum.Parse(typeof(Effect_Type), i.r_effect);
                    AddItem(it);
                }
                else if (i.r_category == "weapon")
                {
                    var it = new GameObject("Enpty");
                    //var it = Instantiate(obj);
                    it.AddComponent<Weapon>();
                    it.GetComponent<Weapon>()._id = i.r_id;
                    it.GetComponent<Weapon>()._name = i.r_name;
                    it.GetComponent<Weapon>()._stock = i.r_stock;
                    it.GetComponent<Weapon>()._maxstock = i.r_maxstock;
                    it.GetComponent<Weapon>()._atk = i.r_atk;
                    it.GetComponent<Weapon>()._hit = i.r_hit;
                    it.GetComponent<Weapon>()._weight = i.r_weight;
                    it.GetComponent<Weapon>()._critical = i.r_cri;
                    it.GetComponent<Weapon>()._attackcount = i.r_attackcount;
                    it.GetComponent<Weapon>()._min = i.r_min;
                    it.GetComponent<Weapon>()._max = i.r_max;
                    it.GetComponent<Weapon>()._message = i.r_message;
                    it.GetComponent<Weapon>()._weapontype = (Weapon_Type)Enum.Parse(typeof(Weapon_Type), i.r_weapontype);
                    it.GetComponent<Weapon>()._weaponEffectType = (Weapon_Effect_Type)Enum.Parse(typeof(Weapon_Effect_Type), i.r_weaponEtype);
                    AddItem(it);
                }
            }
        }
    }

    public void DeleteItem()
    {
        //子オブジェクト(アイテム)を配列に代入
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            _itemprefablist.Add(gameObject.transform.GetChild(i).gameObject);

        }
        for (int i = 0; i < _itemprefablist.Count; i++)
        {
            Destroy(_itemprefablist[i]);
        }
        _itemprefablist.Clear();
    }

    /// <summary>
    /// アイテムを渡す
    /// </summary>
    /// <param name="givitem">渡すアイテム</param>
    public void GiveItem(GameObject givitem)
    {
        _itemprefablist.Remove(givitem);
    }

    /// <summary>
    /// アイテム交換用
    /// </summary>
    /// <param name="getitem">もらいもの</param>
    /// <param name="givitem">あげるもの</param>
    public void ChangeItem(GameObject getitem,GameObject givitem)
    {
        AddItem(getitem);
        _itemprefablist.Remove(givitem);
    }


    /// <summary>
    /// 不要アイテムの削除
    /// </summary>
    public void RemoveItem()
    {
        for (var i = 0; i < _itemprefablist.Count; i++)
        {
            if (_itemprefablist[i] == null)
            {
                _itemprefablist.RemoveAt(i);
            }
            if (_itemprefablist[i].GetComponent<Weapon>())
            {
                if (_itemprefablist[i].GetComponent<Weapon>()._stock <= 0 &&
                    _itemprefablist[i].GetComponent<Weapon>()._weapontype != Weapon_Type.Gun && _itemprefablist[i].GetComponent<Weapon>()._weapontype != Weapon_Type.Rifle)
                {
                    _itemprefablist.Remove(_itemprefablist[i]);
                }
            }
            if (_itemprefablist[i].GetComponent<Item>())
            {
                if (_itemprefablist[i].GetComponent<Item>()._stock <= 0)
                {
                    _itemprefablist.Remove(_itemprefablist[i]);
                }
            }
        }
    }

    /// <summary>
    /// アイテムを使う
    /// </summary>
    /// <param name="_chara">対象のオブジェクト</param>
    /// <param name="_Item">使うアイテムオブジェクト</param>
    public void UseItem(GameObject _chara,GameObject _Item)
    {
        if (_Item.GetComponent<Item>().StockDecrement(_chara))
        {
            RemoveItem();
        }
        
    }
}
