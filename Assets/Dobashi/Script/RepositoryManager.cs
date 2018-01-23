using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositoryManager : MonoBehaviour {

    //倉庫クラス
    public ItemRepository _itemrepository;
    public WeaponRepository _weaponrepository;

    public GameObject testc;
    public GameObject testc2;
    public int id;

	// Use this for initialization
	void Start () {
        _itemrepository = GetComponent<ItemRepository>();
        _weaponrepository = GetComponent<WeaponRepository>();
        _itemrepository.Load();
        _weaponrepository.Load();
	}
	
	// Update is called once per frame
	void Update () {
        //交換テスト
        //if (Input.GetKeyDown("5"))
        //{
        //    ChangeItem(testc, testc2, id,"Item");
        //}
		
	}

    /// <summary>
    /// 倉庫の内容をセーブする
    /// </summary>
    public void SaveRepository()
    {
        _itemrepository.Save();
        _weaponrepository.Save();
    }

    /// <summary>
    /// アイテムを倉庫にしまう
    /// </summary>
    /// <param name="obj">しまうアイテム</param>
    public void AddItem(GameObject obj)
    {
        //種類の選別
        if (obj.GetComponent<Item>())
        {
            var i = obj.GetComponent<Item>();
            _itemrepository.AddItem(i._name,i._message,i._recovery,i._stock,i._type,i._effect.ToString());
            Destroy(obj);
        }
        else if (obj.GetComponent<Weapon>())
        {
            var i = obj.GetComponent<Weapon>();
            _weaponrepository.AddItem(i._name,i._message,i._stock,i._maxstock,i._atk,i._weight,i._hit,i._critical,i._attackcount,i._min,i._max,i._weapontype.ToString(), i._weaponEffectType.ToString());
            Destroy(obj);
        }
    }

    /// <summary>
    /// アイテムを取り出す
    /// </summary>
    /// <param name="chara">受け取るキャラクターオブジェクト</param>
    /// <param name="id">取り出すアイテムID</param>
    public void GetItem(GameObject chara, int id,string type)
    {
        GameObject j = null;
        if (type == "Item")
        {
            j = _itemrepository.GetItem(id);
        }
        else if (type == "Weapon")
        {
            j = _weaponrepository.GetItem(id);
        }
        if(j == null)
        {
            Debug.Log("アイテムがありません");
        }
        else
        {
            Debug.Log(j.name + "を取り出しました");
            GameObject k = GameObject.Find(chara.transform.name + "/ItemList");
            k.GetComponent<ItemPrefabList>().AddItem(j);
        }
    }



    /// <summary>
    /// アイテム交換実行
    /// </summary>
    /// <param name="chara">交換するキャラクター</param>
    /// <param name="item">差し出すアイテム</param>
    /// <param name="id">取り出すアイテムid</param>
    /// <param name="type">受け取るアイテムの種類("Item"or"Weapon")</param>
    public void ChangeItem(GameObject chara,GameObject item,int id,string type)
    {
        var i = item;
        //しまうアイテムの選別
        if (i.GetComponent<Item>())
        {
            var itemscript = i.GetComponent<Item>();
            _itemrepository.AddItem(itemscript._name, itemscript._message, itemscript._recovery,itemscript._stock, itemscript._type,itemscript._effect.ToString());
            
        }
        else if (i.GetComponent<Weapon>())
        {
            var itemscript = i.GetComponent<Weapon>();
            _weaponrepository.AddItem(itemscript._name,itemscript._message,itemscript._stock,itemscript._maxstock,
                itemscript._atk,itemscript._weight,itemscript._hit,
                itemscript._critical,itemscript._attackcount,itemscript._min,itemscript._max,itemscript._weapontype.ToString(),itemscript._weaponEffectType.ToString());
        }

        //受け取るアイテムの選別
        if (type == "Item")
        {
            var j = _itemrepository.GetItem(id);

            GameObject k = GameObject.Find(chara.transform.name + "/ItemList");
            k.GetComponent<ItemPrefabList>().AddItem(j);
        }
        else if (type == "Weapon")
        {
            var j = _weaponrepository.GetItem(id);

            GameObject k = GameObject.Find(chara.transform.name + "/ItemList");
            k.GetComponent<ItemPrefabList>().AddItem(j);
        }

        //倉庫にしまったアイテムをプレイヤーのアイテムリストから削除
        Destroy(i);

    }
}
