using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHighLight : MonoBehaviour
{
    /// <summary>
    /// 床の色付けスイッチ(個別)
    /// true=色を付ける
    /// false=元の色に戻す
    /// </summary>
    public bool s_mapHighLight;
    /// <summary>
    /// マップを何色に変えるか
    /// 0=赤
    /// 1=緑
    /// 2=青
    /// </summary>
    public int MapColorChangeNum;

    /// <summary>
    /// ブロック全体のスクリプト
    /// </summary>
    public BlockMasterScript blockMas;
    //レンダラー
    private new Renderer renderer;

    //ブロックの挙動全体スクリプトを早めに読み込んでおきます
    void Awake()
    {
        blockMas = GameObject.Find("MapMaster").GetComponent<BlockMasterScript>();
    }

    void Start()
    {
        renderer = this.gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        MapColorChange();
    }


    /// <summary>
    /// マップの色変え処理
    /// </summary>
    void MapColorChange()
    {
        if (blockMas.masterMapHighLightSwitch != false)
        {
            if (s_mapHighLight == true)
            {
                if (MapColorChangeNum == 0)
                {
                    renderer.material.EnableKeyword("_EMISSION");
                    renderer.material.color = new Color(0.5f, 0.5f, 0.5f);
                    renderer.material.SetColor("_EmissionColor", new Color(1, 0, 0));
                }
                else if (MapColorChangeNum == 1)
                {
                    renderer.material.EnableKeyword("_EMISSION");
                    renderer.material.color = new Color(0.5f, 0.5f, 0.5f);
                    renderer.material.SetColor("_EmissionColor", new Color(0, 1, 0));
                }
                else if (MapColorChangeNum == 2)
                {
                    renderer.material.EnableKeyword("_EMISSION");
                    renderer.material.color = new Color(0.5f, 0.5f, 0.5f);
                    renderer.material.SetColor("_EmissionColor", new Color(0, 0, 1));
                }
            }

        }
        else if (blockMas.masterMapHighLightSwitch == false)
        {
            if (s_mapHighLight == false)
            {
                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.color = new Color(1,1,1);
                renderer.material.SetColor("_EmissionColor", new Color(1, 1, 1));
            }
        }
    }
}
