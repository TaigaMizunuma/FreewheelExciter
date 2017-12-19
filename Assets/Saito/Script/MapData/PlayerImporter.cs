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

    //初期配置の書いてあるCSVの名前
    [SerializeField]
    string c_playerCSVName;

    TextAsset c_playerCSVFile;
    List<string[]> c_playerCSVDatas = new List<string[]>();
    int c_playerCSVHeight = 0;

    float p_initPosX;
    float p_initPosZ;

    [SerializeField]
    float p_initPosY;

    void Awake()
    {
        c_playerCSVFile = Resources.Load("MapPlayerData/" + c_playerCSVName) as TextAsset;

        StringReader reader = new StringReader(c_playerCSVFile.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            c_playerCSVDatas.Add(line.Split(','));
            c_playerCSVHeight++;
        }
    }

    void Start()
    {
        //プレイヤー初期配置
        for (int p_i = 0; p_i < c_playerCSVDatas.Count; p_i++)
        {
            for (int p_j = 0; p_j < c_playerCSVDatas[p_i].Length; p_j++)
            {
                p_initPosX = p_j;
                p_initPosZ = -p_i;

                switch (c_playerCSVDatas[p_i][p_j])
                {
                    //何もしない
                    case "0":
                        break;
                    //ヒュー
                    case "1":
                        var charaInstance = (GameObject)Instantiate(playerObj[0], new Vector3(p_initPosX, p_initPosY, p_initPosZ), Quaternion.identity);
                        break;
                }
            }
        }
    }
}
