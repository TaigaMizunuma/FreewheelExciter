using System.Collections;
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

    TextAsset MapCSVFile;
    List<string[]> MapCSVDatas = new List<string[]>();
    int mapCSVHeight = 0;

    int mapPosX = 0;
    int mapPosZ = 0;

    void Awake()
    {
        MapCSVFile = Resources.Load("MapFile/" + MapCSV) as TextAsset;
        StringReader reader = new StringReader(MapCSVFile.text);

        while(reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            MapCSVDatas.Add(line.Split(','));
            mapCSVHeight++;
        }
    }

    void Start()
    {

        for (int i = 0; i < MapCSVDatas.Count; i++){
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
                        var mapInstance = (GameObject)Instantiate(blocks[0], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        break;
                    //草
                    case "2":
                        mapInstance = (GameObject)Instantiate(blocks[1], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        break;
                    //水
                    case "3":
                        mapInstance = (GameObject)Instantiate(blocks[2], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        break;
                    //瓦礫
                    case "4":
                        mapInstance = (GameObject)Instantiate(blocks[3], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        break;
                    //瓦礫山
                    case "5":
                        mapInstance = (GameObject)Instantiate(blocks[4], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        break;
                    //土嚢
                    case "6":
                        mapInstance = (GameObject)Instantiate(blocks[5], new Vector3(mapPosX, 0, mapPosZ), Quaternion.identity);
                        break;
                }
            }
        }
    }
}
