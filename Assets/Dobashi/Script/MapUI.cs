using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour {

    Text Map1;
    Text Map2;

	// Use this for initialization
	void Start () {
        Map1 = transform.Find("MapText").GetComponent<Text>();
        Map2 = transform.Find("MapText2").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 地形情報をセット
    /// </summary>
    /// <param name="mapstatus"></param>
    public void SetMapStatus(MapStatus mapstatus)
    {
        Map1.text = mapstatus.GetMapName();
        Map2.text = "回避:" + mapstatus.GetMapEvasionRate();
    }
}
