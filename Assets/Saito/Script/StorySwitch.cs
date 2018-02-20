//会話の条件判断
//できればこんなものは作りとうなかった...が時間がないので...
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySwitch : MonoBehaviour
{

    StoryFlag s_flag;

    StoryCSVReader s_reader;

    void Start()
    {
        s_flag = GetComponent<StoryFlag>();
        s_reader = GetComponent<StoryCSVReader>();
    }

    void Update()
    {
        StoryCheck();
    }

    void StoryCheck()
    {
        //こっから長いです...

        //STAGE1
        if (s_flag.scenarioNum == 1)
        {
            if (s_flag.battleStoryCount[0] == 0)
            {
                if (s_flag.s_bossName == "ローシーフ")
                {
                    {
                        s_flag.i_storyNum = 0;
                        s_flag.StoryNumCheck();
                        s_reader.battleScenarioSwitch = true;
                        s_flag.battleStoryCount[0] = 1;
                    }
                }
            }
        }
    }

}
