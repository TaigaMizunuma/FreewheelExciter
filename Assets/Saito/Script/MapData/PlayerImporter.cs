using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;


public class PlayerImporter : MonoBehaviour
{
    //キャラクターのプレハブをここに
    [Tooltip("プレイヤーキャラごとに入れてください")]
    [SerializeField]
    GameObject[] playerObj;

    [Space(10)]

    //初期配置の書いてあるCSVの名前
    [SerializeField]
    string c_playerCSVName;

    //プレイヤーの配置データ
    TextAsset c_playerCSVFile;
    List<string[]> c_playerCSVDatas = new List<string[]>();
    int c_playerCSVHeight = 0;

    //プレイヤーを置く場所
    float p_initPosX;
    float p_initPosZ;

    [Space  (10)]

    //調整用の値
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

    void Awake()
    {

        PlayerInstance();
    }

    void Start()
    {
    }
    void PlayerInstance()
    {
        //プレイヤー初期配置
        for (int p_i = 0; p_i < c_playerCSVDatas.Count; p_i++)
        {
            for (int p_j = 0; p_j < c_playerCSVDatas[p_i].Length; p_j++)
            {
                p_initPosX = p_j + AdjustmentPosX;
                p_initPosZ = -p_i + AdjustmentPosZ;

                switch (c_playerCSVDatas[p_i][p_j])
                {
                    //何もしない
                    case "0":
                        break;
                    //ヒュー
                    case "1":
                        var charaInstance = (GameObject)Instantiate(playerObj[0], new Vector3(p_initPosX, AdjustmentPosY, p_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.GetComponent<Move_System>().rayBox = GameObject.Find("RayBox");
                        break;
                    //キース
                    case "2":
                        charaInstance = (GameObject)Instantiate(playerObj[1], new Vector3(p_initPosX, AdjustmentPosY, p_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.GetComponent<Move_System>().rayBox = GameObject.Find("RayBox");
                        break;
                    //ジェーコブ
                    case "3":
                        charaInstance = (GameObject)Instantiate(playerObj[2], new Vector3(p_initPosX, AdjustmentPosY, p_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.GetComponent<Move_System>().rayBox = GameObject.Find("RayBox");
                        break;
                    //ケン
                    case "4":
                        charaInstance = (GameObject)Instantiate(playerObj[3], new Vector3(p_initPosX, AdjustmentPosY, p_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.GetComponent<Move_System>().rayBox = GameObject.Find("RayBox");
                        break;
                    //アイン
                    case "5":
                        charaInstance = (GameObject)Instantiate(playerObj[4], new Vector3(p_initPosX, AdjustmentPosY, p_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.GetComponent<Move_System>().rayBox = GameObject.Find("RayBox");
                        break;
                    //ゲイリー
                    case "6":
                        charaInstance = (GameObject)Instantiate(playerObj[5], new Vector3(p_initPosX, AdjustmentPosY, p_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.GetComponent<Move_System>().rayBox = GameObject.Find("RayBox");
                        break;
                    //イゴール
                    case "7":
                        charaInstance = (GameObject)Instantiate(playerObj[6], new Vector3(p_initPosX, AdjustmentPosY, p_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.GetComponent<Move_System>().rayBox = GameObject.Find("RayBox");
                        break;
                    //サイモン
                    case "8":
                        charaInstance = (GameObject)Instantiate(playerObj[7], new Vector3(p_initPosX, AdjustmentPosY, p_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.GetComponent<Move_System>().rayBox = GameObject.Find("RayBox");
                        break;
                    //ジャイロ
                    case "9":
                        charaInstance = (GameObject)Instantiate(playerObj[8], new Vector3(p_initPosX, AdjustmentPosY, p_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.GetComponent<Move_System>().rayBox = GameObject.Find("RayBox");
                        break;
                    //フィーナ
                    case "A":
                        charaInstance = (GameObject)Instantiate(playerObj[9], new Vector3(p_initPosX, AdjustmentPosY, p_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.GetComponent<Move_System>().rayBox = GameObject.Find("RayBox");
                        break;
                    //ヴィック
                    case "B":
                        charaInstance = (GameObject)Instantiate(playerObj[10], new Vector3(p_initPosX, AdjustmentPosY, p_initPosZ), new Quaternion(0, AdjustmentRotY, 0, 0));
                        charaInstance.GetComponent<Move_System>().rayBox = GameObject.Find("RayBox");
                        break;
                }
            }
        }
    }
}
