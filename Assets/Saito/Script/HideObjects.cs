//ユニットの視界に影響されるものすべてに付けてください
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjects : MonoBehaviour
{

    // 敵の視界外にいるかそうでないかの判定
    //どちらも視界エリアにいることを前提に
    // Oneが遠くにいるとき(敵から遠いもの)
    // Twoが近くにいるとき(敵から近いもの)
    bool objHideFlagOne;
    bool objHideFlagTwo;

    //通常のマテリアル
    Material startMaterial;
    //視界の外にある時の真っ黒マテリアル
    [SerializeField]
    Material m_blackMat;
    //透明のマテリアル
    [SerializeField]
    Material m_transparent;

    [SerializeField]
    HideObjectSelect h_objSelect;

    MeshRenderer hideObjectMaterial;

    BattleFlowTest btl_test;

    BlockMasterScript blcMas;

    public enum HideObjectSelect
    {
        Field,
        Unit,
        Object,
        Debug
    }

    void Start()
    {
        startMaterial = this.gameObject.GetComponent<MeshRenderer>().material;
        hideObjectMaterial = this.gameObject.GetComponent<MeshRenderer>();
        blcMas = GameObject.Find("BlockMaster").GetComponent<BlockMasterScript>();
        btl_test = GameObject.Find("GameManager").GetComponent<BattleFlowTest>();
    }

    void Update()
    {
        if (btl_test.state_ != State_.move_mode)
        {
            VisionFlag();
        }
    }
    //視界に対しての処理
    void VisionFlag()
    {
        if (blcMas.HideObjectOnOff == true)
        {
            if (objHideFlagOne == true)
            {
                if (h_objSelect == HideObjectSelect.Field)
                {
                    hideObjectMaterial.material = startMaterial;
                }
                if (objHideFlagTwo == true)
                {
                    hideObjectMaterial.material = startMaterial;
                }
            }

        }
    }
}