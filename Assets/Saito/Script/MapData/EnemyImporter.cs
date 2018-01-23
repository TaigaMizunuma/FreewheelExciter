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
    TextAsset c_enemyPosFile;
    List<string[]> c_enemyPosDatas = new List<string[]>();
    int c_enemyPosHeight = 0;

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

    void Start()
    {
        //エネミー初期配置CSVデータ読み込み
        c_enemyPosFile = Resources.Load("CSV/MapEnemyPlaceData/" + c_enemyPosCSVName) as TextAsset;

        StringReader reader = new StringReader(c_enemyPosFile.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            c_enemyPosDatas.Add(line.Split(','));
            c_enemyPosHeight++;
        }
        EnemyInstance();
    }
    void EnemyInstance()
    {
        //エネミー初期配置
        for (int e_i = 0; e_i < c_enemyPosDatas.Count; e_i++)
        {
            for (int e_j = 0; e_j < c_enemyPosDatas[e_i].Length; e_j++)
            {
                //エネミー位置
                e_initPosX = e_j + AdjustmentPosX;
                e_initPosZ = -e_i + AdjustmentPosZ;

                switch (c_enemyPosDatas[e_i][e_j])
                {
                    //何もなし
                    case "0":
                        break;
                    //ガンナー
                    case "1":
                        var charaInstance = (GameObject)Instantiate(enemyObj[0], new Vector3(e_initPosX, AdjustmentPosY, e_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.name = "Enemy" + eneCount;
                        charaInstance.GetComponent<EnemyPersonalCSV>().e_number = eneCount;
                        eneCount++;
                        break;
                    //ファイター
                    case "2":
                        charaInstance = (GameObject)Instantiate(enemyObj[1], new Vector3(e_initPosX, AdjustmentPosY, e_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.name = "Enemy" + eneCount;
                        charaInstance.GetComponent<EnemyPersonalCSV>().e_number = eneCount;
                        eneCount++;
                        break;
                    //ソルジャー
                    case "3":
                        charaInstance = (GameObject)Instantiate(enemyObj[2], new Vector3(e_initPosX, AdjustmentPosY, e_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.name = "Enemy" + eneCount;
                        charaInstance.GetComponent<EnemyPersonalCSV>().e_number = eneCount;
                        eneCount++;
                        break;
                    //マーセナリー
                    case "4":
                        charaInstance = (GameObject)Instantiate(enemyObj[3], new Vector3(e_initPosX, AdjustmentPosY, e_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.name = "Enemy" + eneCount;
                        charaInstance.GetComponent<EnemyPersonalCSV>().e_number = eneCount;
                        eneCount++;
                        break;
                    //ライダー
                    case "5":
                        charaInstance = (GameObject)Instantiate(enemyObj[4], new Vector3(e_initPosX, AdjustmentPosY, e_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.name = "Enemy" + eneCount;
                        charaInstance.GetComponent<EnemyPersonalCSV>().e_number = eneCount;
                        eneCount++;
                        break;
                    //パイロット
                    case "6":
                        charaInstance = (GameObject)Instantiate(enemyObj[5], new Vector3(e_initPosX, AdjustmentPosY, e_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.name = "Enemy" + eneCount;
                        charaInstance.GetComponent<EnemyPersonalCSV>().e_number = eneCount;
                        eneCount++;
                        break;
                    //レンジャー
                    case "7":
                        charaInstance = (GameObject)Instantiate(enemyObj[6], new Vector3(e_initPosX, AdjustmentPosY, e_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.name = "Enemy" + eneCount;
                        charaInstance.GetComponent<EnemyPersonalCSV>().e_number = eneCount;
                        eneCount++;
                        break;
                    //スナイパー
                    case "8":
                        charaInstance = (GameObject)Instantiate(enemyObj[7], new Vector3(e_initPosX, AdjustmentPosY, e_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.name = "Enemy" + eneCount;
                        charaInstance.GetComponent<EnemyPersonalCSV>().e_number = eneCount;
                        eneCount++;
                        break;
                    //マーシャル
                    case "9":
                        charaInstance = (GameObject)Instantiate(enemyObj[8], new Vector3(e_initPosX, AdjustmentPosY, e_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.name = "Enemy" + eneCount;
                        charaInstance.GetComponent<EnemyPersonalCSV>().e_number = eneCount;
                        eneCount++;
                        break;
                    //ウォリアー
                    case "A":
                        charaInstance = (GameObject)Instantiate(enemyObj[9], new Vector3(e_initPosX, AdjustmentPosY, e_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.name = "Enemy" + eneCount;
                        charaInstance.GetComponent<EnemyPersonalCSV>().e_number = eneCount;
                        eneCount++;
                        break;
                    //アサシン
                    case "B":
                        charaInstance = (GameObject)Instantiate(enemyObj[10], new Vector3(e_initPosX, AdjustmentPosY, e_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.name = "Enemy" + eneCount;
                        charaInstance.GetComponent<EnemyPersonalCSV>().e_number = eneCount;
                        eneCount++;
                        break;
                    //ジェネラル
                    case "C":
                        charaInstance = (GameObject)Instantiate(enemyObj[11], new Vector3(e_initPosX, AdjustmentPosY, e_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.name = "Enemy" + eneCount;
                        charaInstance.GetComponent<EnemyPersonalCSV>().e_number = eneCount;
                        eneCount++;
                        break;
                }
            }
        }
    }
}
