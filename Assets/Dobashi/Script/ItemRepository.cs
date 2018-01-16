using System.Collections;
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

        public void SetData(string name, string message,int recovery,int stock, string type,string effect)
        {
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
        //AddItem("きずぐすり","HPを10回復",10,10,"Item","none");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// アイテムを追加する
    /// </summary>
    /// <param name="_name">名前</param>
    /// <param name="_message">表示メッセージ</param>
    /// <param name="_recovery">回復量</param>
    /// <param name="_stock">残り使用回数</param>
    /// <param name="_type">タイプ</param>
    /// <param name="_effect">特殊効果</param>
    public void AddItem(string _name,string _message,int _recovery,int _stock,string _type,string _effect)
    {
        var i = new ItemData();
        i.SetData(_name,_message,_recovery,_stock,_type,_effect);
        _itemrepository.Add(i);
    }

    /// <summary>
    /// アイテムの取り出し
    /// </summary>
    /// <param name="_no">取り出すアイテムのナンバー</param>
    /// <returns>アイテムオブジェクト</returns>
    public GameObject GetItem(int _no)
    {
        var i = _itemrepository[_no];
        var j = Instantiate(_itemprehub);
        j.GetComponent<Item>().SetStatus(_no, i._name, i._message, i._recovery,i._stock, i._type,i._effect);
        //アイテムの削除
        _itemrepository.RemoveAt(_no);
        return j;
    }

    /// <summary>
    /// アイテム倉庫のリストを保存
    /// </summary>
    public void Save()
    {
        SaveData.SetList<ItemData>("ItemRepositoryList", _itemrepository);
    }
    /// <summary>
    /// アイテム倉庫のリストをセーブ
    /// </summary>
    public void Load()
    {
        if (SaveData.HasKey("ItemRepositoryList") == true)
        {
            /*①ロード処理*/
            _itemrepository = SaveData.GetList("ItemRepositoryList", _itemrepository);
        }
    }
}
