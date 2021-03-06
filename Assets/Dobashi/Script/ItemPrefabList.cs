﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefabList : MonoBehaviour {
    //アイテムと武器を代入する配列
    public List<GameObject> _itemprefablist = new List<GameObject>();


	// Use this for initialization
	void Start () {
        ItemAddList();

    }
	
	// Update is called once per frame
	void Update () {
        
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
        item.transform.parent = transform;
        ItemAddList();
    }

    /// <summary>
    /// アイテムを使う
    /// </summary>
    /// <param name="_chara">対象のオブジェクト</param>
    /// <param name="_Item">使うアイテムオブジェクト</param>
    public void UseItem(GameObject _chara,GameObject _Item)
    {
        _Item.GetComponent<Item>().StockDecrement(_chara);   
    }
}
