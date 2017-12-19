using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestListItem : MonoBehaviour
{
    /*Space押したらスタート*/
    /*①リスト保存、②クラス保存のテストコード*/
    /*①保存するリスト Public推奨*/
    public List<string> ItemNumberList = new List<string>();
    /*①ロードするリスト*/
    private List<string> ItemLoadList= new List<string>();
    /*カウント変数*/
    private int ButtonCount = 0;
    /*②クラス保存*/
    [SerializeField]
    public class Player
    {
        [SerializeField]
        public int hp;
        public float atk;
        public string name;
        public string job;
        public List<string> items;

        public Player()
        {

            items = new List<string>();
            items.Add("鉄の剣");
            items.Add("銀の剣");
            items.Add("キルソード");
            hp = 45;
            atk = 25;
            name = "オグマ";
            job = "勇者";
        }
    }

    void Start ()
    {
        /*①②開幕にロード*/ 
        /*①リストの場合*/
        /*①キーが存在しているかチェック 存在していなかったら何もしない*/
        if (SaveData.HasKey("ItemNumberList") == true)
        {
            /*①ロード処理*/
            ItemLoadList = SaveData.GetList("ItemNumberList", ItemLoadList);
            /*①デバッグ表示*/
            for (int i = 0; i < ItemLoadList.Count; i++)
            {
                Debug.Log("アイテムリスト2から" + ItemLoadList[i] + "をロードしました");
            }
        }


        /*②クラスの場合*/
        if (SaveData.HasKey("Player") == true)
        {
            /*②ロード処理*/
            Player GetPlayer = SaveData.GetClass("Player", new Player());
            /*②デバッグ表示*/
            for (int i = 0; i < GetPlayer.items.Count; i++)
            {
                Debug.Log("プレイヤークラスから" + GetPlayer.items[i] + "をロードしました");
            }
            Debug.Log("名前:" + GetPlayer.name + " 職業:" + GetPlayer.job + " HP:" + GetPlayer.hp + " 攻撃力:" + GetPlayer.atk);
        }
        Debug.Log("Spaceでアイテムを追加します。もう一度押すとアイテムをセーブします");
    }

    // Update is called once per frame
    void Update()
    {
        switch (ButtonCount)
        {

            case 0:
            if (Input.GetKeyDown(KeyCode.Space))
            {
                /*①リストにアイテム追加*/
                ItemNumberList.Add("ポーション");
                ItemNumberList.Add("ハイポーション");
                ItemNumberList.Add("エリクサー");
                ButtonCount++;
            }
            break;
            case 1:
            if (Input.GetKeyDown(KeyCode.Space))
            {
                /*①デバッグ表示*/
                for (int i = 0; i < ItemNumberList.Count; i++)
                {    
                    Debug.Log("アイテムリスト1" + ItemNumberList[i] + "をセーブしました");
                }
                /*①②セーブ処理 第1引数で指定したキーをGetで読み込むことによってロードできる*/
                SaveData.SetList<string>("ItemNumberList", ItemNumberList);
                SaveData.SetClass<Player>("Player",new Player());
                ButtonCount = 0;
            }
            break;
        }
    }
}
