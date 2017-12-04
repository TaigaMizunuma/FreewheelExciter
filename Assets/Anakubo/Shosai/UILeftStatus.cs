using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILeftStatus : MonoBehaviour {
    public Image _faceimage;
    public GameObject[] _data = new GameObject[3];


    public void SetData(Character _chara)
    {
        _data[0].GetComponent<Text>().text = _chara._level.ToString();
        _data[2].GetComponent<Text>().text = _chara._totalMaxhp.ToString();
        _data[1].GetComponent<Text>().text = _chara._exp.ToString();
    }
}
