﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    SceneChange sceneChange;
    private int gameOverTime;
    
    Fade fade;

    void Start () {
        sceneChange = this.GetComponent<SceneChange>();
        fade = GetComponent<Fade>();
    }
	
	void Update () {
        //どっかキー押されるか5秒経過でタイトルに戻る
        gameOverTime++;
        if (Input.anyKeyDown || gameOverTime > 300) 
        {
            FindObjectOfType<Fade>().SetOutFade(true);
            FindObjectOfType<Fade>().SetSceneChangeSwitch(true);
            FindObjectOfType<Fade>().SetScene("Title");
        }
    }
}
