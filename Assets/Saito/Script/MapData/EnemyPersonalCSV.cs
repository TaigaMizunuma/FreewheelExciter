using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;


public class EnemyPersonalCSV : MonoBehaviour
{
    public int e_number;

    [SerializeField]
    string c_enePerCSVName;

    TextAsset c_enePerCSVFile;
    List<string[]> c_enemyPerDatas = new List<string[]>();
    int c_enemyPerHeight = 0;

    [SerializeField]
    string e_name;
    [SerializeField]
    int e_level;
    [SerializeField]
    string e_job;
    [SerializeField]
    string e_strength;
    [SerializeField]
    string e_weapon;
    [SerializeField]
    string e_weapontype;

    [SerializeField]
    GameObject test;

    [SerializeField]
    int storyCount;

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
        e_name = c_enemyPerDatas[e_number + 1][1];
        e_level = int.Parse(c_enemyPerDatas[e_number + 1][2]);
        this.GetComponent<Character>()._level = e_level;
        e_weapon = c_enemyPerDatas[e_number + 1][5];
        e_weapontype = c_enemyPerDatas[e_number + 1][6];
        this.GetComponent<Character>()._equipment = Resources.Load("Weapon/" + e_weapontype +"/" + e_weapon,typeof(GameObject)) as GameObject;

        e_job = c_enemyPerDatas[e_number + 1][3];
        e_strength = c_enemyPerDatas[e_number + 1][4];
    }

    public string GetEnemyName()
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
}
