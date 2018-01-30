using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILeftStatus : MonoBehaviour {
    public GameObject[] _data = new GameObject[4];

    public void SetData(Character _chara)
    {
        _data[0].GetComponent<Text>().text = _chara._level.ToString();
        _data[2].GetComponent<Text>().text = _chara._totalMaxhp.ToString();
        _data[1].GetComponent<Text>().text = _chara._exp.ToString();
        _data[3].GetComponent<Image>().sprite = _chara._faceimage;
    }
}
