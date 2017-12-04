using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStandardStatus : MonoBehaviour {

    public GameObject[] _data = new GameObject[7];
    //public Character _chara;

    public void SetData(Character _chara)
    {
        _data[0].GetComponent<Text>().text = _chara._totalstr.ToString();
        _data[1].GetComponent<Text>().text = _chara._totalskl.ToString();
        _data[2].GetComponent<Text>().text = _chara._totalspd.ToString();
        _data[3].GetComponent<Text>().text = _chara._totalluk.ToString();
        _data[4].GetComponent<Text>().text = _chara._totaldef.ToString();
        _data[5].GetComponent<Text>().text = _chara._totalcur.ToString();
        _data[6].GetComponent<Text>().text = _chara._totalmove.ToString();


    }
}
