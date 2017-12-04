///他のステージに移動するためのスクリプトです
///これ単体では動かないので、ほかのシーンに移動したいタイミングでメソッドを渡してください
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {

    //移動するシーンの名前
    //ここにstringを渡せば干渉できます
    public string sceneName;

    public void Change()
    {
        SceneManager.LoadScene(sceneName);
    }
}
