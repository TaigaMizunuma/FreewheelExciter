using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class MapImporterScript : MonoBehaviour
{
    //ブロックのプレハブをここに
    public GameObject[] blocks;

    public GameObject[] start_Blocks;

    [Space(10)]

    //CSVのファイル名をここに
    public string MapCSV;

    //読み込んだCSVデータ
    TextAsset MapCSVFile;
    List<string[]> MapCSVDatas = new List<string[]>();
    int mapCSVHeight = 0;

    string[,] mapBlockPos;

    //ブロックの位置
    float mapPosX = 0;
    float mapPosZ = 0;

    [Space(10)]

    //位置の調整値
    [SerializeField]
    float AdjustmentPosX;
    [SerializeField]
    float AdjustmentPosY;
    [SerializeField]
    float AdjustmentPosZ;

    //名前用
    int mapBlockCount;

    void Awake()
    {
        MapCSVFile = Resources.Load("CSV/MapData/" + MapCSV) as TextAsset;
        StringReader reader = new StringReader(MapCSVFile.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            MapCSVDatas.Add(line.Split(','));
            mapCSVHeight++;
        }

        MapInstance();
    }

    void MapInstance()
    {
        for (int i = 0; i < MapCSVDatas.Count; i++)
        {
            for (int j = 0; j < MapCSVDatas[i].Length; j++)
            {
                int max = MapCSVDatas[i].Length;
                mapPosX = j + AdjustmentPosX;
                mapPosZ = -i + +AdjustmentPosZ;
                switch (MapCSVDatas[i][j])
                {
                    //無し
                    case "0":
                        break;
                    //土
                    case "1":
                        mapBlockCount++;
                        var mapInstance = (GameObject)Instantiate(blocks[0], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //草
                    case "2":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(blocks[1], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //水
                    case "3":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(blocks[2], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //瓦礫
                    case "4":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(blocks[3], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //瓦礫山
                    case "5":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(blocks[4], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //土嚢
                    case "6":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(blocks[5], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //森
                    case "7":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(blocks[6], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //マンホール
                    case "8":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(blocks[7], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //土
                    case "S1":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(start_Blocks[0], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //草
                    case "S2":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(start_Blocks[1], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //水
                    case "S3":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(start_Blocks[2], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //瓦礫
                    case "S4":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(start_Blocks[3], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //瓦礫山
                    case "S5":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(start_Blocks[4], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //土嚢
                    case "S6":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(start_Blocks[5], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //森
                    case "S7":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(start_Blocks[6], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //マンホール
                    case "S8":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(start_Blocks[13], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                }
            }
        }
    }
}
