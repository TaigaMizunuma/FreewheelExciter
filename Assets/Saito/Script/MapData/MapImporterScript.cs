﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class MapImporterScript : MonoBehaviour
{
    //ブロックのプレハブをここに
    public GameObject[] blocks;

    //CSVのファイル名をここに
    public string MapCSV;

    //読み込んだCSVデータ
    TextAsset MapCSVFile;
    List<string[]> MapCSVDatas = new List<string[]>();
    int mapCSVHeight = 0;

    string[,] mapBlockPos;

    //ブロックの位置
    int mapPosX = 0;
    int mapPosZ = 0;

    //名前用
    int mapBlockCount;

    void Awake()
    {
        MapCSVFile = Resources.Load("MapData/" + MapCSV) as TextAsset;
        StringReader reader = new StringReader(MapCSVFile.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            MapCSVDatas.Add(line.Split(','));
            mapCSVHeight++;
        }
    }

    void Start()
    {
        for (int i = 0; i < MapCSVDatas.Count; i++)
        {
            for (int j = 0; j < MapCSVDatas[i].Length; j++)
            {
                
                mapPosX = j;
                mapPosZ = -i;
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
                    //瓦礫
                    case "7":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(blocks[6], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //毒ガス
                    case "8":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(blocks[7], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //床・階段
                    case "9":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(blocks[8], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //柱系
                    case "A":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(blocks[9], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //扉
                    case "B":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(blocks[10], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //宝
                    case "C":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(blocks[11], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;
                    //マンホール
                    case "D":
                        mapBlockCount++;
                        mapInstance = (GameObject)Instantiate(blocks[12], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        mapInstance.name = "block" + mapBlockCount;
                        break;



                }
            }
        }
    }
}
