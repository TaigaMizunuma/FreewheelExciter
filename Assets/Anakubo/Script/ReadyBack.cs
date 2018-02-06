using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyBack : MonoBehaviour {
    public Sprite[] back_img;
    private int story_num;

	// Use this for initialization
	void Start () {
        story_num = GameObject.Find("TextManager").GetComponent<StoryCSVReader>().GetStoryNumber();
        gameObject.GetComponent<Image>().sprite = back_img[story_num-1];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
