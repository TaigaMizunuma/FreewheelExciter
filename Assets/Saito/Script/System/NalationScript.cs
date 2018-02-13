using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NalationScript : MonoBehaviour {

    //章ナンバー
    int s_Number;
    //章タイトル
    string s_Title;
    //読み込んでるナレーション部分の番号
    int nalationNum;

    [Space(10)]

    //表示するテキスト類
    string nalation;

    //章ナンバーテキスト
    [SerializeField]
    Text s_NumberText;
    //章タイトルテキスト
    [SerializeField]
    Text s_TitleText;
    //ナレーションのテキスト
    [SerializeField]
    Text nalationText;

    //テキストをオブジェクト扱いにして動かす
    [SerializeField]
    GameObject nalationObj;

    //章タイトルのフラグ
    [SerializeField]
    bool nalTitleFlag;
    //ナレーション読了フラグ
    [SerializeField]
    bool nalationFlag;
    //ナレーション処理完全終了フラグ
    bool nalationEndFlag;

    //ナレーション背景
    [SerializeField]
    Image nalBackImage;

    //ナレーションの流れる方向
    Vector2 nalationVector;

    //ナレーションの流れる速度
    [SerializeField]
    float scrollSpeed;

    //アルファ値
    float alfa = 1;
    //背景アルファ値
    float bgAlfa = 1;

    //消えていく速度
    float n_fadeSpeed;
    //消えるまでの時間
    [SerializeField]
    float n_fadeTime;

    //章タイトルが消えるまでの時間
    [SerializeField]
    float n_titleTime;

    //フェードスクリプト
    Fade s_fade;

    //ナレーションの有無
    [SerializeField]
    bool nalationOnOFF;

    //ナレーション拝啓
    GameObject nalationBack;

    void Start () {

        NaltionSwitch();

        nalationBack = GameObject.Find("NalationBack");

        if(nalationOnOFF == true)
        {
            nalationBack.SetActive(false);
            nalTitleFlag = true;
            nalationFlag = true;    
            nalationEndFlag = true;
        }

        s_fade = GetComponent<Fade>();

        nalationNum = FindObjectOfType<StoryCSVReader>().GetNalationNum();

        Nalation nalationExcel = Resources.Load("Data/Nalation") as Nalation;

        nalation = nalationExcel.param[nalationNum].Nalation;

        nalationText.text = nalation;

        s_Number = FindObjectOfType<StoryCSVReader>().GetStoryNumber();
        s_Title = FindObjectOfType<StoryCSVReader>().GetStoryTitle();
        nalationVector = nalationObj.transform.position;

        FadeTime();
	}
	
	void Update () {

        n_titleTime -= 1f;
        if(n_titleTime <= 0)
        {
            n_titleTime = 0;
            nalTitleFlag = true;
        }

        if (nalTitleFlag == true )
        {
            s_TitleText.GetComponent<Text>().color = new Color(1, 1, 1, alfa);
            s_NumberText.GetComponent<Text>().color = new Color(1, 1, 1, alfa);
            alfa -= n_fadeSpeed;
            if (alfa <= 0)
            {
                alfa = 0;
                nalationObj.SetActive(true);
                NalationTextMove();
            }
            if(nalationObj.transform.localPosition.y == 500.0f)
            {
                nalationFlag = true;
            }
        }
        if(nalationFlag == true && nalTitleFlag == true)
        {
            nalationObj.SetActive(false);
            nalBackImage.GetComponent<Image>().color = new Color(0, 0, 0, bgAlfa);
            bgAlfa -= n_fadeSpeed;
            nalationFlag = true;
            if (bgAlfa <= 0)
            {
                bgAlfa = 0;
                nalationEndFlag = true;
            }
        }
        NalationDraw();
        NalationCommand();
	}

    void NalationDraw()
    {
        s_NumberText.text = "第" + s_Number.ToString() + "章";
        s_TitleText.text = s_Title;
    }

    void NalationTextMove()
    {
        nalationObj.transform.position = new Vector2(nalationVector.x, nalationVector.y);
        nalationVector.y += scrollSpeed;
    }

    void NalationCommand()
    {
        if(s_fade.isFadeIn == false) 
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                if(nalTitleFlag == false)
                {
                    nalTitleFlag = true;
                }
                else if(nalTitleFlag == true && nalationFlag == false)
                {
                    nalationFlag = true;
                }
            }
        }
    }

    public bool GetNalationEndFlag()
    {
        return nalationEndFlag;
    }

    void FadeTime()
    {
        n_fadeSpeed = Time.deltaTime / (n_fadeTime);
    }

    void NaltionSwitch()
    {
        int nalswitch = FindObjectOfType<StoryCSVReader>().GetNalOnOFF();

        if(nalswitch == 1)
        {
            nalationOnOFF = false;
        }
        else
        {
            nalationOnOFF = true;
        }
    }
}
