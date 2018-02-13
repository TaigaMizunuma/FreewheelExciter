using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRepository : MonoBehaviour {

    //武器倉庫クラス
    public List<WeaponData> _weaponrepository = new List<WeaponData>();
    //倉庫id
    private int _repositoryid = 0;
    public GameObject _weaponprehub;

    public struct WeaponData
    {
        //武器名
        public string _name;
        //武器の説明
        public string _message;
        //残りの数(耐久)
        public int _stock;
        //耐久最大値
        public int _maxstock;
        //攻撃力
        public int _atk;
        //重さ
        public int _weight;
        //命中
        public int _hit;
        //必殺率
        public int _critical;
        //攻撃回数
        public int _attackcount;
        //射程(min,max)
        public int _min;
        public int _max;
        //武器種
        public string _weapontype;
        public string _weaponEtype;

        public void SetData(string name, string message, int stock, int maxstock,int atk,int weight,int hit,int critical,int count,int rangemin,int rangemax,string weapontype,string weaponEtype)
        {
            _name = name;
            _message = message;
            _stock = stock;
            _maxstock = maxstock;
            _atk = atk;
            _weight = weight;
            _hit = hit;
            _critical = critical;
            _attackcount = count;
            _min = rangemin;
            _max = rangemax;
            _weapontype = weapontype;
            _weaponEtype = weaponEtype;
        }
    }

    /// <summary>
    /// 武器を追加する
    /// </summary>
    /// <param name="name">なまえ</param>
    /// <param name="message">メッセージ</param>
    /// <param name="stock">残り使用回数</param>
    /// <param name="maxstock">耐久最大値</param>
    /// <param name="atk">攻撃力</param>
    /// <param name="weight">重さ</param>
    /// <param name="hit">命中率</param>
    /// <param name="critical">必殺率</param>
    /// <param name="count">攻撃回数</param>
    /// <param name="rangemin">最小射程</param>
    /// <param name="rangemax">最大射程</param>
    /// <param name="weapontype">武器種</param>
    /// <param name="weaponEtype">特殊効果</param>
    public void AddItem(string name, string message, int stock, int maxstock,int atk, int weight, int hit, int critical, int count, int rangemin, int rangemax,string weapontype,string weaponEtype)
    {
        var i = new WeaponData();
        i.SetData(name, message, stock, maxstock,atk,weight,hit,critical,count,rangemin,rangemax,weapontype,weaponEtype);
        _weaponrepository.Add(i);
    }

    /// <summary>
    /// 武器の取り出し
    /// </summary>
    /// <param name="_no">取り出す武器のナンバー</param>
    /// <returns></returns>
    public GameObject GetItem(int _no)
    {
        WeaponData i;
        i = _weaponrepository[_no];
        var j = Instantiate(_weaponprehub);
        j.GetComponent<Weapon>().SetStatus(_no, i._name, i._message, i._stock, i._maxstock, i._atk, i._weight, i._hit, i._critical, i._attackcount, i._min, i._max, i._weapontype, i._weaponEtype);
        //武器の削除
        _weaponrepository.RemoveAt(_no);
        return j;
    }

    /// <summary>
    /// 武器倉庫のリストを保存
    /// </summary>
    public void Save()
    {
        SaveData.SetList<WeaponData>("WeaponRepositoryList", _weaponrepository);
    }
    /// <summary>
    /// 武器倉庫のリストをロード
    /// </summary>
    public void Load()
    {
        if (SaveData.HasKey("WeaponRepositoryList") == true)
        {
            /*①ロード処理*/
            _weaponrepository = SaveData.GetList("WeaponRepositoryList", _weaponrepository);
        }
    }
}
