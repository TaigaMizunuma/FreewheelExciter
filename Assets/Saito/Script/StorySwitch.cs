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
        s_flag.StoryTurn();
    }

    void StoryCheck()
    {
        //こっから長いです...

        //チュートリアル
        if(s_flag.scenarioNum == 0)
        {

        }


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
                        s_reader.battleEvent = true;
                        s_reader.battleScenarioSwitch = true;
                        s_flag.battleStoryCount[0] = 1;
                    }
                }
            }
            if(s_flag.battleStoryCount[1] == 0)
            {
                if (s_flag.s_turnCount >= 4)
                {
                    s_flag.i_storyNum = 1;
                    s_flag.StoryNumCheck();
                    s_reader.battleScenarioSwitch = true;
                    s_flag.battleStoryCount[1] = 1;
                }
            }
            if(s_flag.battleStoryCount[2] == 0)
            {
                if (s_flag.g_enemyObj != null && s_flag.g_enemyObj.GetComponent<Character>()._totalhp == 0)
                {
                    s_flag.i_storyNum = 2;
                    s_flag.StoryNumCheck();
                    s_reader.battleScenarioSwitch = true;
                    s_flag.battleStoryCount[2] = 1;
                }
            }
        }

        //STAGE2
        if(s_flag.scenarioNum == 2)
        {

        }

        //STAGE3
        if(s_flag.scenarioNum == 3)
        {

        }

        //STAGE4
        if(s_flag.scenarioNum == 4)
        {

        }

        //STAGE5
        if(s_flag.scenarioNum == 5)
        {

        }
    }

}
