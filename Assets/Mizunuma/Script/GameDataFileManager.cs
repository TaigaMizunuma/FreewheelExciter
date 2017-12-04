using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDataFileManager : MonoBehaviour
{
    //開始 // 
    string key = "SavedText";
    //********** 終了 **********// 
    string str;
    public InputField inputField;
    public Text text;

    //********** 開始 **********// 
    void Start()
    {
        //保存キー「SavedText」で保存されたstring型のデータがあればそれを、
        //無ければブランクを取得
        text.text = PlayerPrefs.GetString(key, "");
        Debug.Log("前のログを取得しました" + " " + text.text);
        //********** 終了 **********// 
    }

    public void SaveText()
    {
        str = inputField.text;
        //********** 開始 **********//
        //保存キー「SavedText」で入力文字を保存
        PlayerPrefs.SetString(key, str);
        PlayerPrefs.Save();
        //********** 終了 **********//
        
         
        text.text = str;
        inputField.text = "";
        Debug.Log("セーブ成功しました");
    }
}
