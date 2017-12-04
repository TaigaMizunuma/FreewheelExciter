using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryReader : MonoBehaviour
{

    //今表示してるID
    [SerializeField]
    int storyID;
    //開始部分の番号(外部から変更可能)
    public int readStartNumber;
    //終了部分の番号(外部から変更可能)
    public int readEndNumber;

    [Space(8)]

    //何処からストーリーデータをロードするか
    [SerializeField]
    string dataLoadName;

    //今表示しているテキスト
    string storySheetText;

    //今話しているキャラ
    string storyCharacterName;

    [Space(8)]

    //表示用のテキスト
    [SerializeField]
    Text storyText;

    //名前のテキスト
    [SerializeField]
    Text nameText;

    //章番号
    public int storyNumber;

    [SerializeField]
    Entity_Story1 storySheet;

    CharacterGraphic c_graphic;

    [Space(8)]

    //フェードアウト/インの切り替えスクリプト
    [SerializeField]
    Fade fade;

    void Awake()
    {
        storySheet = Resources.Load("Data/" + dataLoadName) as Entity_Story1;
        storyID = storySheet.param[readStartNumber].ID;
        storyCharacterName = storySheet.param[storyID].Name;
        storySheetText = storySheet.param[storyID].Story;
        storyNumber = storySheet.param[storyID].StoryNumber;

        c_graphic = GetComponent<CharacterGraphic>();

        c_graphic.storyID = storyID;
        c_graphic.readStartNumber = readStartNumber;
    }

    void Start()
    {
    }

    void Update()
    {
        TextDisplay();
    }

    //テキストの処理
    void TextDisplay()
    {
        if (storyID < readEndNumber)
        {
            if (fade.isFadeIn == false)
            {
                if (Input.GetKeyDown(KeyCode.U))
                {
                    storyID += 1;
                    c_graphic.storyID = storyID;
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    fade.isFadeOut = true;
                }
            }
            c_graphic.CharacterImageDisplay();
            storySheetText = storySheet.param[storyID].Story;
            storyNumber = storySheet.param[storyID].StoryNumber;
            storyCharacterName = storySheet.param[storyID].Name;
            if (storyCharacterName != "")
            {
                nameText.text = storyCharacterName;
            }
            else
            {
                nameText.text = "";
            }
            storyText.text = storySheetText;
        }
        else if (storyID == readEndNumber)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                fade.isFadeOut = true;
            }
        }
    }

    public int GetStoryNumber()
    {
        return storyNumber;

    }
}
