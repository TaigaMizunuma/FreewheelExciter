using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;


public class EnemyImporter : MonoBehaviour
{

    //エネミーのプレハブ
    [Tooltip("エネミーを種類ごとに入れてください")]
    [SerializeField]
    GameObject[] enemyObj;

    [Space(10)]

    //エネミーの初期配置の書いてあるCSV
    [SerializeField]
    string c_enemyPosCSVName;

    //エネミーの初期配置のデータ
    TextAsset c_enemyFile;
    List<string[]> c_enemyDatas = new List<string[]>();
    int c_enemyHeight = 0;

    //エネミーを置く場所
    float e_initPosX;
    float e_initPosZ;

    [Space(10)]

    //位置の調整値
    [SerializeField]
    float AdjustmentPosX;
    [SerializeField]
    float AdjustmentPosY;
    [SerializeField]
    float AdjustmentPosZ;

    [Space(10)]

    //回転、向きを変えられる
    [Tooltip("向きを変えられます")]
    [SerializeField]
    float AdjustmentRotY;

    //エネミーの名前用カウント
    //末尾にこの数値を付ける予定
    [SerializeField]
    int eneCount;

    int e_number;

    void Awake()
    {
        //エネミー初期配置CSVデータ読み込み
        c_enemyFile = Resources.Load("MapEnemyData/" + c_enemyPosCSVName) as TextAsset;

        StringReader reader = new StringReader(c_enemyFile.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            c_enemyDatas.Add(line.Split(','));
            c_enemyHeight++;
        }
        EnemyInstance();
    }

    void Start()
    {
    }
    void EnemyInstance()
    {
        //エネミー初期配置
        for (int e_i = 0; e_i < c_enemyDatas.Count; e_i++)
        {
            for (int e_j = 0; e_j < c_enemyDatas[e_i].Length; e_j++)
            {
                //エネミー位置
                e_initPosX = e_j + AdjustmentPosX;
                e_initPosZ = -e_i + AdjustmentPosZ;
                e_number = int.Parse(c_enemyDatas[e_i][e_j]);

                if (c_enemyDatas[e_i][e_j] != "0")
                {
                    var charaInstance = (GameObject)Instantiate(enemyObj[0], new Vector3(e_initPosX, AdjustmentPosY, e_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                    charaInstance.name = "Enemy" + eneCount;
                    eneCount++;
                }
            }
        }
    }
}
