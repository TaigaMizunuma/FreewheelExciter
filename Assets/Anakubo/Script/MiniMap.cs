using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour {
    public Sprite[] mini_map;
    private int story_num;
    // Use this for initialization
    void Start () {
        story_num = GameObject.Find("TextManager").GetComponent<StoryCSVReader>().GetStoryNumber();
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(mini_map[story_num - 1].textureRect.width, mini_map[story_num - 1].textureRect.height);
        gameObject.GetComponent<Image>().sprite = mini_map[story_num - 1];
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
