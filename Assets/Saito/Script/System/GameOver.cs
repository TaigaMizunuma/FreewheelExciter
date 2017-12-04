using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    SceneChange sceneChange;

	void Start () {
        sceneChange = this.GetComponent<SceneChange>();	
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sceneChange.Change();
        }
	}
}
