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
                        StoryTransition(0, 0, true, true);
                    }
                }
            }
            if (s_flag.battleStoryCount[1] == 0)
            {
                if (s_flag.s_turnCount >= 4)
                {
                    StoryTransition(1, 1, false, true);
                }
            }
            if (s_flag.battleStoryCount[2] == 0)
            {
                if (s_flag.g_enemyObj != null && s_flag.g_enemyObj.GetComponent<Character>()._totalhp == 0)
                {
                    StoryTransition(2, 2, false, true);
                }
            }
        }

        //STAGE2
        if (s_flag.scenarioNum == 2)
        {
            if (s_flag.battleStoryCount[0] == 0)
            {
                if (s_flag.s_bossName == "イゴール" && s_flag.s_playerName == "ヒュー")
                {
                    StoryTransition(0, 0, true, true);
                }
            }
            if (s_flag.battleStoryCount[1] == 0)
            {
                if (s_flag.s_bossName == "サイモン" && s_flag.s_playerName == "イゴール")
                {
                    StoryTransition(1, 1, true, true);
                }
            }
            if (s_flag.battleStoryCount[2] == 0)
            {
                if (s_flag.s_bossName == "シングス")
                {
                    if (s_flag.s_playerName == "イゴール")
                    {
                        StoryTransition(2, 2, true, true);
                    }
                    else if (s_flag.s_playerName == "サイモン")
                    {
                        StoryTransition(3, 2, true, true);
                    }
                    else
                    {
                        StoryTransition(5, 2, true, true);
                    }

                }
            }
            if (s_flag.battleStoryCount[3] == 0)
            {
                if (s_flag.g_enemyObj != null && s_flag.g_enemyObj.GetComponent<Character>()._name == "シングス" && s_flag.g_enemyObj.GetComponent<Character>()._totalhp == 0) 
                {
                    StoryTransition(4, 3, false, true);
                }
            }
        }

        //STAGE3
        if(s_flag.scenarioNum == 3)
        {
            if(s_flag.battleStoryCount[0] == 0)
            {
                if (s_flag.s_bossName == "ジャイロ" && s_flag.s_playerName == "ジェーコブ")
                {
                    StoryTransition(0, 0, true, true);
                }
            }
            if (s_flag.battleStoryCount[1] == 0)
            {
                if (s_flag.s_bossName == "フライヤー")
                {
                    if(s_flag.s_playerName == "ジャイロ")
                    {

                        StoryTransition(1, 1, false, true);
                    }
                    else
                    {
                        StoryTransition(2, 1, false, true);
                    }
                }
            }
            if(s_flag.battleStoryCount[2] == 0)
            {

            }
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

    void StoryTransition(int num, int count_num ,bool b_event,bool b_switch)
    {
        s_flag.i_storyNum = num;
        s_flag.StoryNumCheck();
        s_reader.battleEvent = b_event;
        s_reader.battleScenarioSwitch = b_switch;
        s_flag.battleStoryCount[count_num] = 1;
    }

}
