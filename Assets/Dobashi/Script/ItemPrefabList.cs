using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
