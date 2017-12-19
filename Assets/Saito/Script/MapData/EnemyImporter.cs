using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;


public class EnemyImporter : MonoBehaviour {

    //エネミーのプレハブ
    [Tooltip("エネミーを種類ごとに入れてください")]
    [SerializeField]
    GameObject[] enemyObj;

    //エネミーの初期配置の書いてあるCSV
    [SerializeField]
    string c_enemyCSVName;

    //エネミーの初期配置のデータ
    TextAsset c_enemyFile;
    List<string[]> c_enemyDatas = new List<string[]>();
    int c_enemyHeight = 0;

    //エネミーを置く場所
    //YだけはInspectorで編集してください(匙加減のため)
    float e_initPosX;
    float e_initPosZ;
    [SerializeField]
    float e_initPosY;
    
    //回転、向きを変えられる
    [Tooltip("向きを変えられます")]
    [SerializeField]
    float e_initRotY;

    void Awake()
    {
        //エネミー初期配置CSVデータ読み込み
        c_enemyFile = Resources.Load("MapEnemyData/" + c_enemyCSVName) as TextAsset;

        StringReader reader = new StringReader(c_enemyFile.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            c_enemyDatas.Add(line.Split(','));
            c_enemyHeight++;
        }
    }

    void Start()
    {
        //エネミー初期配置
        for (int e_i = 0; e_i < c_enemyDatas.Count; e_i++)
        {
            for (int e_j = 0; e_j < c_enemyDatas[e_i].Length; e_j++)
            {
                //エネミー位置
                e_initPosX = e_j;
                e_initPosZ = -e_i;

                switch (c_enemyDatas[e_i][e_j])
                {
                    //何もしない
                    case "0":
                        break;
                    //ガンナー
                    case "1":
                        var charaInstance = (GameObject)Instantiate(enemyObj[0], new Vector3(e_initPosX, e_initPosY, e_initPosZ), new Quaternion(0, e_initRotY, 0, 0));
                        break;
                    //ファイター
                    case "2":
                        charaInstance = (GameObject)Instantiate(enemyObj[1], new Vector3(e_initPosX, e_initPosY, e_initPosZ), new Quaternion(0, e_initRotY, 0, 0));
                        break;
                    //ソルジャー
                    case "3":
                        charaInstance = (GameObject)Instantiate(enemyObj[2], new Vector3(e_initPosX, e_initPosY, e_initPosZ), new Quaternion(0, e_initRotY, 0, 0));
                        break;
                    //マーセナリー
                    case "4":
                        charaInstance = (GameObject)Instantiate(enemyObj[3], new Vector3(e_initPosX, e_initPosY, e_initPosZ), new Quaternion(0, e_initRotY, 0, 0));
                        break;
                    //ライダー
                    case "5":
                        charaInstance = (GameObject)Instantiate(enemyObj[4], new Vector3(e_initPosX, e_initPosY, e_initPosZ), new Quaternion(0, e_initRotY, 0, 0));
                        break;
                    //パイロット
                    case "6":
                        charaInstance = (GameObject)Instantiate(enemyObj[5], new Vector3(e_initPosX, e_initPosY, e_initPosZ), new Quaternion(0, e_initRotY, 0, 0));
                        break;
                    //スナイパー
                    case "7":
                        charaInstance = (GameObject)Instantiate(enemyObj[6], new Vector3(e_initPosX, e_initPosY, e_initPosZ), new Quaternion(0, e_initRotY, 0, 0));
                        break;
                    //マーシャル
                    case "8":
                        charaInstance = (GameObject)Instantiate(enemyObj[7], new Vector3(e_initPosX, e_initPosY, e_initPosZ), new Quaternion(0, e_initRotY, 0, 0));
                        break;
                    //ジェネラル
                    case "9":
                        charaInstance = (GameObject)Instantiate(enemyObj[8], new Vector3(e_initPosX, e_initPosY, e_initPosZ), new Quaternion(0, e_initRotY, 0, 0));
                        break;
                    //ウォーリア
                    case "A":
                        charaInstance = (GameObject)Instantiate(enemyObj[9], new Vector3(e_initPosX, e_initPosY, e_initPosZ), new Quaternion(0, e_initRotY, 0, 0));
                        break;
                    //ボス(暫定)
                    //ボスのプレハブができてから動かします
                    case "B":
                        charaInstance = (GameObject)Instantiate(enemyObj[10], new Vector3(e_initPosX, e_initPosY, e_initPosZ), new Quaternion(0, e_initRotY, 0, 0));
                        break;

                }
            }
        }
    }
}
