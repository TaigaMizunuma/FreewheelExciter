//ブロック個別に何かしたいときはここに記述します。
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStatus : MonoBehaviour
{

    //ID番号、ここに該当する数値を入れると残りの内容が反映される
    //Excel依存なので数値を変えたい場合はExcelを変えてください
    [SerializeField]
    private int mapID;
    //地形の名前
    [SerializeField]
    private string mapName;
    /// <summary>
    ///地形のコスト 
    /// </summary>
    public int mapCost;
    /// <summary>
    /// 入れるかどうか(柱などの景観マス)
    /// </summary>
    public int mapInvasion;
    /// <summary>
    /// 回避率の数値
    /// </summary>
    public int mapEvasionRate;

    /// <summary>
    /// 高低差
    /// </summary>
    public float mapHeight;

    //キャラクターのオブジェクト
    [SerializeField]
    private GameObject characterObj;

    Character s_character;

    BlockMasterScript blcMaster;

    void Start()
    {
        blcMaster = GameObject.Find("BlockMaster").GetComponent<BlockMasterScript>();

        Entity_MapStatus mapStatus = Resources.Load("Data/MapStatus") as Entity_MapStatus;

        //Excel内のデータを反映
        mapName = mapStatus.param[mapID].name;
        mapCost = mapStatus.param[mapID].cost;
        mapInvasion = mapStatus.param[mapID].invasion;
        mapEvasionRate = mapStatus.param[mapID].evasionRate;
        mapHeight = mapStatus.param[mapID].height;
    }

    void Update()
    {
        CharacterGetRay();
        if(characterObj != null)
        {
            s_character = characterObj.GetComponent<Character>();
        }
        MapStatusBuff();
    }

    void CharacterGetRay()
    {
        if (blcMaster.masterMapStatus == true && characterObj == null)
        {
            Ray map_ray = new Ray(transform.position, new Vector3(0, 1, 0));
            RaycastHit map_rayHit;
            int map_rayDistance = 5;

            if (Physics.Raycast(map_ray, out map_rayHit, map_rayDistance))
            {
                if (map_rayHit.transform.gameObject.tag == "Player" || map_rayHit.transform.gameObject.tag == "Enemy")
                {
                    characterObj = map_rayHit.transform.gameObject;
                }
            }
        }
    }

    /// <summary>
    /// マップ内の効果
    /// </summary>
    public void MapStatusBuff()
    {
        if (blcMaster.masterMapStatus == true && s_character != null)
        {
            if (mapID == 8)
            {
                //毒ガスマスの処理(毒状態の付与)
                s_character._NowState = Character.State.Poison;
                characterObj = null;
                blcMaster.masterMapStatus = false;
            }

        }
    }
}
