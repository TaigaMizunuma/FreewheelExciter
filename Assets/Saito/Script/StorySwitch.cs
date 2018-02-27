//会話の条件判断
//できればこんなものは作りとうなかった...が時間がないので...
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySwitch : MonoBehaviour
{

    StoryFlag s_flag;

    StoryCSVReader s_reader;

    [HideInInspector]
    public bool tutorialFlagOne = false;
    [HideInInspector]
    public bool tutorialFlagTwo = false;
    [HideInInspector]
    public bool tutorialFlagThree = false;
    [HideInInspector]
    public bool tutorialFlagFour = false;
    [HideInInspector]
    public bool tutorialFlagFive = false;
    [HideInInspector]
    public bool tutorialFlagSix = false;

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

        //チュートリアル(内容は該当関数内を参照)
        if(s_flag.scenarioNum == 0)
        {
            TutorialStory();
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

                        StoryTransition(1, 1, true, true);
                    }
                    else
                    {
                        StoryTransition(2, 1, true, true);
                    }
                }
            }
            if(s_flag.battleStoryCount[2] == 0)
            {
                if (s_flag.g_enemyObj != null && s_flag.g_enemyObj.GetComponent<Character>()._name == "フライヤー" && s_flag.g_enemyObj.GetComponent<Character>()._totalhp == 0)
                {
                    StoryTransition(3, 2, false, true);
                }
            }
        } 

        //STAGE4
        if(s_flag.scenarioNum == 4)
        {
            if(s_flag.battleStoryCount[0] == 0)
            {
                if (s_flag.s_bossName == "フウスイ")
                {
                    if(s_flag.s_playerName == "フィーナ")
                    {
                        StoryTransition(0, 0, true, true);
                    }
                    else if (s_flag.s_playerName == "ヴィック")
                    {
                        StoryTransition(1, 0, true, true);
                    }
                    else
                    {
                        StoryTransition(2, 0, true, true);
                    }
                }
            }
            if (s_flag.battleStoryCount[1] == 0)
            {
                if (s_flag.g_enemyObj != null && s_flag.g_enemyObj.GetComponent<Character>()._name == "フウスイ" && s_flag.g_enemyObj.GetComponent<Character>()._totalhp == 0)
                {
                    StoryTransition(3, 1, false, true);
                }
            }
        }

        //STAGE5
        if (s_flag.scenarioNum == 5)
        {
            if(s_flag.battleStoryCount[0] == 0)
            {
                if (s_flag.s_bossName == "レア" && s_flag.s_playerName == "ヒュー")
                {
                    StoryTransition(0, 0, true, true);
                }
            }
            if(s_flag.battleStoryCount[1] == 0)
            {
                if (s_flag.s_bossName == "フウスイ")
                {
                    if (s_flag.s_playerName == "レア")
                    {
                        StoryTransition(1, 1, true, true);
                    }
                    else
                    {
                        StoryTransition(2, 1, true, true);
                    }
                }
            }
            if (s_flag.battleStoryCount[2] == 0)
            {
                if (s_flag.g_enemyObj != null && s_flag.g_enemyObj.GetComponent<Character>()._name == "ガードン" && s_flag.g_enemyObj.GetComponent<Character>()._totalhp == 0)
                {
                    StoryTransition(3, 2, false, true);
                }
            }
        }
    }

    //ストーリーを読み込ませる処理
    void StoryTransition(int num, int count_num ,bool b_event,bool b_switch)
    {
        s_flag.i_storyNum = num;
        s_flag.StoryNumCheck();
        s_reader.battleEvent = b_event;
        s_reader.battleScenarioSwitch = b_switch;
        s_flag.battleStoryCount[count_num] = 1;
    }

    //チュートリアル用の処理
    void TutorialStory()
    {
        //チュートリアル用ローカル変数
        //名前は許して...

        //最初
        if(s_flag.battleStoryCount[2] == 0)
        {
            if (tutorialFlagOne == false)
            {
                StoryTransition(2, 2, true, true);
                //ケンにrayboxが重なったらフラグ1をtrueに
            }
        }
        //○を押す
        if (s_flag.battleStoryCount[3] == 0)
        {
            if(tutorialFlagOne == true)
            {
                StoryTransition(3, 3, true, true);
                //○を押したらフラグ2をtrueに
            }
        }
        //黒いところまで移動
        if(s_flag.battleStoryCount[4] == 0)
        {
            if(tutorialFlagTwo == true)
            {
                StoryTransition(4, 4, true, true);
                //黒い所にケンが到達したらフラグ3をtrueに
            }
        }

        //ウィンドウ説明
        if(s_flag.battleStoryCount[5] == 0)
        {
            if(tutorialFlagThree == true)
            {
                StoryTransition(5, 5, true, true);
                //待機を押したらフラグ4をtrueに
            }
        }
        //攻撃の仕方
        if (s_flag.battleStoryCount[6] == 0)
        {
            if (tutorialFlagFour == true)
            {
                StoryTransition(6, 6, true, true);
                //攻撃して、敵を一体倒したらフラグ5をtrueに
            }

        }
        //ヒューで撃破を試みる
        if(s_flag.battleStoryCount[7] == 0)
        {
            if(tutorialFlagFive == true)
            {
                StoryTransition(7, 7, true, true);
                //キース以外を倒したらフラグ6をtrueに
            }
        }

        //無事に両方で倒した時
        if(s_flag.battleStoryCount[8] == 0)
        {
            if(tutorialFlagSix == true)
            {
                StoryTransition(8, 8, true, true);
            }
        }

        if (s_flag.battleStoryCount[0] == 0)
        {
            if (s_flag.s_bossName == "キース")
            {
                StoryTransition(0, 0, true, true);
            }
        }
        if (s_flag.battleStoryCount[1] == 0)
        {
            if (s_flag.g_enemyObj != null && s_flag.g_enemyObj.GetComponent<Character>()._totalhp == 0)
            {
                StoryTransition(1, 1, false, true);
            }
        }
    }
}
