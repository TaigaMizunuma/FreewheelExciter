using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusSave : MonoBehaviour {

    /*①保存するリスト Public推奨*/
    public List<CharacterSave> CharastatusSaveList = new List<CharacterSave>();
    /*①ロードするリスト*/
    public List<CharacterSave> CharastatusLoadList = new List<CharacterSave>();
    //キャラクタースクリプト
    private Character _chara;
    //ロード時の数値
    public int r_id;
    public int r_job;
    public string r_name;
    public Vector3 r_pos;
    public bool r_hero;
    public int r_lv;
    public int r_totallv;
    public int r_hp;
    public int r_totalhp;
    public int r_str;
    public int r_skl;
    public int r_spd;
    public int r_luk;
    public int r_def;
    public int r_cur;
    public int r_move;
    public int r_exp;
    public int[] r_addstatus;
    public int[] r_addonestatus;
    public int[] r_addbuffstatus;
    public bool r_stability;
    public int r_state;
    public bool r_isDead;

    [SerializeField]
    public class DataSave
    {
        [SerializeField]
        public int _id;             //判別用ID
        public int _job;            //クラス
        public string _name;        //名前
        public Vector3 _pos;        //座標
        public bool _hero;          //主人公かどうか
        public int _lv;             //現在Level
        public int _totallv;        //累計Level
        public int _hp;             //HP
        public int _totalhp;        //現在HP
        public int _str;            //力
        public int _skl;            //技
        public int _spd;            //速
        public int _luk;            //運
        public int _def;            //守
        public int _cur;            //呪力
        public int _move;           //移動力
        public int _exp;            //経験値
        public int[] _addstatus;    //加算ステータスList
        public int[] _addonestatus; //1戦闘のみの加算ステータスList
        public int[] _addbuffstatus;//バフによる加算ステータス
        public bool _stability;     //コマンドスキルによる追撃可能かの判別
        public int _state;          //状態異常
        public bool _isDead;        //死亡済みか

        public DataSave()
        {
            _id = 0;
            _job = 0;
            _name = "";
            _pos = new Vector3(0,0,0);
            _hero = false;
            _lv = 1;
            _totallv =1;
            _hp = 1;
            _totalhp = 1;
            _str = 1;
            _skl = 1;
            _spd = 1;
            _luk = 1;
            _def = 1;
            _cur = 1;
            _move = 1;
            _exp = 1;
            _addstatus = new int[14];
            _addonestatus = new int[14];
            _addbuffstatus = new int[14];
            _stability = false;
            _state = 0;
            _isDead = false;
        }

    }


    // Use this for initialization
    void Start () {
        _chara = GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown("h"))
        //{
        //    Debug.Log("Characterデータセーブ");
        //    SaveStatus(NewData());
        //}
        //if (Input.GetKeyDown("f"))
        //{
        //    Debug.Log("オブジェクトロードテスト");
        //    LoadStatus();
        //    Instantiate(r_itemobj);
        //    Instantiate(r_skillobj);
        //}
        
    }

    /// <summary>
    /// キャラクターステータスをセーブするときに呼ぶ
    /// </summary>
    public void CharactorDataSave(string key)
    {
        SaveStatus(NewData(),key);
    }

    /// <summary>
    /// ステータスをセーブ
    /// </summary>
    public void SaveStatus(DataSave i,string key)
    {
        SaveData.SetClass<DataSave>(key, i);
    }
    /// <summary>
    /// ステータスをロードし、渡すための変数に代入
    /// </summary>
    public bool LoadStatus()
    {
        if (SaveData.HasKey(GetComponent<Character>()._name) == true)
        {
            DataSave gs = SaveData.GetClass(GetComponent<Character>()._name, new DataSave());
            r_id = gs._id;
            r_job = gs._job;
            r_name = gs._name;
            r_pos = gs._pos;
            r_hero = gs._hero;
            r_lv = gs._lv;
            r_totallv = gs._totallv;
            r_hp = gs._hp;
            r_totalhp = gs._totalhp;
            r_str = gs._str;
            r_skl = gs._skl;
            r_spd = gs._spd;
            r_luk = gs._luk;
            r_def = gs._def;
            r_cur = gs._cur;
            r_move = gs._move;
            r_exp = gs._exp;
            r_addstatus = gs._addstatus;
            r_addonestatus = gs._addonestatus;
            r_addbuffstatus = gs._addbuffstatus;
            r_state = gs._state;
            r_isDead = gs._isDead;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 保存データの設定
    /// </summary>
    DataSave NewData()
    {
        var i = new DataSave();
        i._id = 0;
        i._job = (int)_chara._joblist;
        i._name = _chara._name;
        i._pos = _chara._position;
        i._hero = _chara._hero;
        i._lv = _chara._level;
        i._totallv = _chara._totalLevel;
        i._hp = _chara.GetStatus()[0];
        i._totalhp = _chara._totalhp;
        i._str = _chara.GetStatus()[1];
        i._skl = _chara.GetStatus()[2];
        i._spd = _chara.GetStatus()[3];
        i._luk = _chara.GetStatus()[4];
        i._def = _chara.GetStatus()[5];
        i._cur = _chara.GetStatus()[6];
        i._move = _chara.GetStatus()[7];
        i._exp = _chara._exp;
        i._addstatus = _chara._addstatuslist;
        i._addonestatus = _chara._addonetimestatuslist;
        i._addbuffstatus = _chara._addbufflist;
        i._state = (int)_chara._NowState;
        i._isDead = _chara._isDead;
        
        return i;
    }
}
