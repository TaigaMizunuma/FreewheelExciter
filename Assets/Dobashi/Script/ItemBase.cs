using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour {

    //アイテムリスト読み込み
    Entity_ItemList itemList;
    
    // Use this for initialization
    void Start () {
        //アイテムリスト読み込み
        itemList = Resources.Load("Data/ItemList") as Entity_ItemList;
    }
	
	// Update is called once per frame
	void Update () {
	}

    /// <summary>
    /// アイテム検索
    /// 名前とメッセージを返す
    /// </summary>
    /// <param name="_id">アイテムID</param>
    public string[] ItemSearch(int _id)
    {
        string[] ms = { "", ""};
        if(_id != 0)
        {
            ms[0] = itemList.param[_id].name;
            ms[1] = itemList.param[_id].message;
        }
        return ms;

    }


    /// <summary>
    /// アイテム使用
    /// </summary>
    /// <param name="_id">アイテムID</param>
    /// <param name="_obj">効果を適用するオブジェクト</param>
    public void ItemEffect(int _id,GameObject _obj)
    {
        var _character = _obj.GetComponent<Character>();

        //アイテムの回復(もしくはダメージ)処理
        //Recoveryの数値分回復(-の場合はダメージ)
        _character._totalhp += itemList.param[_id].recovery;
        Debug.Log(_character._name + "は" + itemList.param[_id].recovery + "回復!");

        //良い効果の適用
        if (itemList.param[_id].good_effect != "none")
        {
            // 必要な変数を宣言する
            string stCsvData = itemList.param[_id].good_effect;

            // カンマ区切りで分割して配列に格納する
            string[] stArrayData = stCsvData.Split(',');

            // データを確認する
            foreach (string stData in stArrayData)
            {
                switch (stData)
                {
                    case "毒":
                        
                        break;
                }
            }
        }

        if (itemList.param[_id].bad_effect != "none")
        {
            // 必要な変数を宣言する
            string stCsvData = itemList.param[_id].bad_effect;

            // カンマ区切りで分割して配列に格納する
            string[] stArrayData = stCsvData.Split(',');

            // データを確認する
            foreach (string stData in stArrayData)
            {
                Debug.Log(stData);
            }
        }

    }
}
