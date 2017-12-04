//ブロック全体に何かしたいときはここに記述します。
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMasterScript : MonoBehaviour {

    /// <summary>
    /// 床の色付けスイッチ(全体)
    /// これがfalseだとMapHiLightのs_mapHilightがtrueでも光らないので注意
    /// </summary>
    public bool masterMapHighLightSwitch;

    /// <summary>
    /// マップの効果を発揮させるスイッチ
    /// これをtrueにすれば効果を与えることができます
    /// 現状は毒だけです
    /// </summary>
    public bool masterMapStatus;

    /// <summary>
    /// デバッグ用
    /// falseにするとブロックを黒くする機能がオフになります
    /// </summary>
    [Tooltip("デバッグ用です")]
    public bool HideObjectOnOff;

	void Start () {
        masterMapHighLightSwitch = true;
        masterMapStatus = false;
	}
}
