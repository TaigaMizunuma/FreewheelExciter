using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScnarioTEst : MonoBehaviour {

    [SerializeField]
    bool textTest;

	void Start () {
		
	}
	
	void Update () {
	
        if(textTest == true)
        {
            FindObjectOfType<StoryCSVReader>().battleScenarioSwitch = true;
            textTest = false;
        }
        	
	}
}
