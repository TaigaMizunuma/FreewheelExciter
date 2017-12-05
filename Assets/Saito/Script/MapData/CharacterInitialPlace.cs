using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CharacterInitialPlace : MonoBehaviour
{

    //キャラクターのプレハブをここに
    [SerializeField]
    GameObject[] characterObj;

    //初期配置の書いてあるcsvの名前
    [SerializeField]
    string c_initCSVName;

    TextAsset c_initCSVFile;
    List<string[]> c_initCSVDatas = new List<string[]>();
    int c_initCSVHeight = 0;

    int c_initPosX;
    int c_initPosZ;

    void Awake()
    {
        c_initCSVFile = Resources.Load("MapFile/" + c_initCSVName) as TextAsset;
        StringReader reader = new StringReader(c_initCSVFile.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            c_initCSVDatas.Add(line.Split(','));
            c_initCSVHeight++;
        }
    }

    void Start()
    {
        for (int i = 0; i < c_initCSVDatas.Count; i++)
        {
            for (int j = 0; j < c_initCSVDatas[i].Length; j++)
            {
                c_initPosX = j;
                c_initPosZ = -i;

                switch(c_initCSVDatas[i][j])
                {
                    case "0":
                        break;

                    case "1":
                        var c_initInstance = (GameObject)Instantiate(characterObj[0], new Vector3(c_initPosX, 1, c_initPosZ), Quaternion.identity);
                        break;

                    case "2":
                        c_initInstance = (GameObject)Instantiate(characterObj[0], new Vector3(c_initPosX, 1, c_initPosZ), Quaternion.identity);
                        break;
                }
            }
        }
    }
}
