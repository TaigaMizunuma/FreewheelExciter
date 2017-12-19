using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusSave : MonoBehaviour {

    /*①保存するリスト Public推奨*/
    public List<string> CharastatusList = new List<string>();
    /*①ロードするリスト*/
    private List<string> CharastatusLoadList = new List<string>();
    //キャラクタースクリプト
    private Character _chara;

    // Use this for initialization
    void Start () {
        _chara = GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// ステータスをセーブ
    /// </summary>
    public void SaveStatus()
    {
        //CharastatusList.Add();

        SaveData.SetList<string>("ItemNumberList", CharastatusList);
    }
}
