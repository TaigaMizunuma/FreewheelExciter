﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRepository : MonoBehaviour {
    //アイテム倉庫クラス
    public List<ItemData> _itemrepository = new List<ItemData>();
    //倉庫id
    private int _repositoryid = 0;
    public GameObject _itemprehub;

    public struct ItemData
    {
        //アイテムID
        public int _id;
        //アイテム名
        public string _name;
        //アイテムの説明
        public string _message;
        //回復量
        public int _recovery;
        //残りの数
        public int _stock;
        //アイテムタイプ
        public string _type;
        //アイテム効果
        public string _effect;

        public void SetData(int id, string name, string message,int recovery,int stock, string type,string effect)
        {
            _id = id;
            _name = name;
            _message = message;
            _recovery = recovery;
            _stock = stock;
            _type = type;
            _effect = effect;
        }
    }

    // Use this for initialization
    void Start () {
        AddItem("便王","くさいぞ",10,20,"Item","none");

    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown("6"))
        //{
        //    Debug.Log(_itemrepository[0]._name);
        //}
		
	}

    /// <summary>
    /// アイテムを追加する
    /// </summary>
    /// <param name="_name">名前</param>
    /// <param name="_message">表示メッセージ</param>
    /// <param name="_stock">残り使用回数</param>
    /// <param name="_type">タイプ</param>
    public void AddItem(string _name,string _message,int _recovery,int _stock,string _type,string _effect)
    {
        var i = new ItemData();
        i.SetData(_repositoryid,_name,_message,_recovery,_stock,_type,_effect);
        _itemrepository.Add(i);
        _repositoryid++;
    }

    /// <summary>
    /// アイテムの取り出し
    /// </summary>
    /// <param name="_no">取り出すアイテムのナンバー</param>
    /// <returns></returns>
    public GameObject GetItem(int _no)
    {
        var i = _itemrepository[_no];
        var j = Instantiate(_itemprehub);
        j.GetComponent<Item>().SetStatus(_no, i._name, i._message, i._recovery,i._stock, i._type,i._effect);
        //アイテムの削除
        _itemrepository.RemoveAt(_no);
        return j;
    }
}
