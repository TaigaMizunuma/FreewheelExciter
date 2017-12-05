using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaManager : MonoBehaviour {
    public GameObject[] _charas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //キャラクターの検索
    public void CharacterSearch()
    {

    }

    //全キャラクターの取得
    public void MaxCharacter()
    {
        _charas = GameObject.FindGameObjectsWithTag("Player");
    }

    //キャラリストを1つずつ取り出す
    public void DisplayChara()
    {
        for (var i = 0;i < _charas.Length;i++)
        {
            var c = _charas[i].GetComponent<Character>();
            Debug.Log(c);
        }
    }

    /// <summary>
    /// アイテム交換
    /// </summary>
    /// <param name="_chara">キャラ1</param>
    /// <param name="_chara2">キャラ2</param>
    /// <param name="_item">キャラ1の渡すアイテム</param>
    /// <param name="_item2">キャラ2の渡すアイテム</param>
    public void ChangeItem(GameObject _chara, GameObject _chara2, GameObject _item, GameObject _item2)
    {
        var c1 = _chara.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>();
        var c2 = _chara2.GetComponent<Character>()._itemprefablist.GetComponent<ItemPrefabList>();

        c1.AddItem(_item2);
        c2.AddItem(_item);

        Destroy(_item);
        Destroy(_item2);
    }

    /// <summary>
    /// アイテムを捨てる
    /// </summary>
    public void DiscardItem(GameObject _chara)
    {

    }
}
