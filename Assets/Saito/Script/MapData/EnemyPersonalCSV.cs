using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;


public class EnemyPersonalCSV : MonoBehaviour
{
    //CSVから読み込む際、縦列に値する数値
    public int e_number;

    //CSVファイル関連(消さないで！)
    TextAsset c_enePerCSVFile;
    List<string[]> c_enemyPerDatas = new List<string[]>();
    int c_enemyPerHeight = 0;

    /*CSVから読み込んだデータ群*/
    //敵の名前
    string e_name;
    //敵のレベル
    int e_level;
    //敵の職業
    string e_job;
    //敵の強さ(？)
    string e_strength;
    //敵の武器名
    string e_weapon;
    //敵の武器タイプ
    string e_weapontype;
    //敵のアイテムドロップ判定
    int e_drop;
    //ストーリーに関わる敵の判定
    int storyCount;
    //アイテムリスト、自分のものが入る
    [SerializeField]
    GameObject itemList;

    //ドロップ判定をbool化したもの
    bool e_dropFlag;

    //ストーリーに関わる敵の判定をbool化したもの
    bool e_storyFlag;


    void Awake()
    {
        storyCount = FindObjectOfType<StoryCSVReader>().GetStoryNumber();

        //エネミー初期パラメータCSVデータ読み込み
        c_enePerCSVFile = Resources.Load("CSV/MapEnemyPersonalData/" + "Stage" + storyCount + "_EnemyPersonalData") as TextAsset;

        StringReader reader = new StringReader(c_enePerCSVFile.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            c_enemyPerDatas.Add(line.Split(','));
            c_enemyPerHeight++;
        }
    }

    void Start()
    {
        GameObject e_weapon_p;
        GameObject e_equip_p;

        Character s_character = this.GetComponent<Character>();

        e_name = c_enemyPerDatas[e_number + 1][1];
        this.transform.name = e_name;

        e_level = int.Parse(c_enemyPerDatas[e_number + 1][2]);

        s_character._name = e_name;
        s_character._level = e_level;

        e_weapon = c_enemyPerDatas[e_number + 1][5];
        e_weapontype = c_enemyPerDatas[e_number + 1][6];

        if (e_weapontype != null || e_weapon != null)
        {
            e_weapon_p = Resources.Load("Weapon/" + e_weapontype + "/" + e_weapon, typeof(GameObject)) as GameObject;
            e_equip_p = Instantiate(e_weapon_p, this.transform.position, Quaternion.identity);
            e_equip_p.transform.name = e_weapon;
            e_equip_p.transform.parent = itemList.transform;
            s_character._equipment = e_weapon_p;
        }

        e_job = c_enemyPerDatas[e_number + 1][3];
        e_strength = c_enemyPerDatas[e_number + 1][4];

        e_drop = int.Parse(c_enemyPerDatas[e_number + 1][7]);

        if(e_drop == 0)
        {
            e_dropFlag = false;
        }
        else
        {
            e_dropFlag = true;
        }

        storyCount = int.Parse(c_enemyPerDatas[e_number + 1][8]);

        if(storyCount == 0)
        {
            e_storyFlag = false;
        }
        else
        {
            e_storyFlag = true;
        }
            
        s_character.Enemy_Init();
        
    }

    public string GetName()
    {
        return e_name;
    }

    public string GetJob()
    {
        return e_job;
    }

    public string GetStrength()
    {
        return e_strength;
    }

    public bool GetDropFlag()
    {
        return e_dropFlag;
    }

    public bool GetStoryFlag()
    {
        return e_storyFlag;
    }
}
