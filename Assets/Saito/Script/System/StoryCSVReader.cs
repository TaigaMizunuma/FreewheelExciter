using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class StoryCSVReader : MonoBehaviour
{
    //次にロードするCSVの名前
    //勝手に入ります
    public static string nextStory;

    //章タイトル
    public string title;

    //今表示してるID
    int storyID;
    //開始部分の番号
    [SerializeField]
    int readStartNumber;
    //終了部分の番号
    [SerializeField]
    int readEndNumber;

    //次の始点と終点(勝手に入ります)
    public static int nextreadStartNumber;
    public static int nextreadEndNumber;

    [Space(8)]

    //何処からストーリーデータをロードするか
    [SerializeField]
    string dataLoadName;

    //今表示しているテキスト
    string storySheetText;

    //今話しているキャラ
    string storyCharacterName;

    //テキストの終了判定
    int endFlag;

    [Space(8)]

    //表示用のテキスト
    [SerializeField]
    Text storyText;

    //名前のテキスト
    [SerializeField]
    Text nameText;

    //キャラクターグラフィックの番号
    int LOneCharacterImageNum;
    int LTwoCharacterImageNum;
    int ROneCharacterImageNum;
    int RTwoCharacterImageNum;

    //キャラクターのグラの表示設定
    //ON...表示 OFF...非表示 SHADE...うす暗く
    string LOneBlackOut;
    string LTwoBlackOut;
    string ROneBlackOut;
    string RTwoBlackOut;


    //キャラクターのグラフィック
    [SerializeField]
    Image LOneCharacterWindowImage;
    [SerializeField]
    Image LTwoCharacterWindowImage;
    [SerializeField]
    Image ROneCharacterWindowImage;
    [SerializeField]
    Image RTwoCharacterWindowImage;

    //章番号
    public static int storyNumber;

    int sceneMode;

    //CSVファイル(勝手に入ります)
    TextAsset storyCSVFile;

    //CSVデータ(勝手に入ります)
    List<string[]> storyCSVDatas = new List<string[]>();

    //
    int storyCSVHeight = 0;

    int i;//縦
    int j;//横

    [Space(8)]

    //フェードアウト/インの切り替えスクリプト
    [SerializeField]
    Fade fade;

    //名前の表示枠
    [SerializeField]
    GameObject nameImage;

    [SerializeField]
    Vector2 LNameDisplayVector;
    [SerializeField]
    Vector2 RNameDisplayVector;

    CharacterImageManager c_ImgManager;

    float red, green, blue, alfa;

    ScenePattern scenePattern;

    enum ScenePattern
    {
        Message,
        Battle,
    }

    void Awake()
    {
        c_ImgManager = GetComponent<CharacterImageManager>();

        storyCSVFile = Resources.Load("Data/" + dataLoadName) as TextAsset;
        StringReader reader = new StringReader(storyCSVFile.text);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            storyCSVDatas.Add(line.Split(','));
            storyCSVHeight++;
        }
    }

    void Start()
    {
        for (i = 0; i < storyCSVDatas.Count; i++)
        {
            for (j = 0; j < storyCSVDatas[i].Length; j++)
            {
                storyNumber = int.Parse(storyCSVDatas[1][0]);
                title = storyCSVDatas[storyID + 1][17];
                storyID = readStartNumber;
                storyCharacterName = storyCSVDatas[storyID + 1][2];
                storySheetText = storyCSVDatas[storyID + 1][3];
                storyText.text = storySheetText;
                if (storySheetText != null)
                {
                    nameText.text = storyCharacterName;
                }
                else
                {
                    nameText.text = "";
                }
                LOneCharacterImageNum = int.Parse(storyCSVDatas[storyID + 1][4]);
                ROneCharacterImageNum = int.Parse(storyCSVDatas[storyID + 1][5]);
                LTwoCharacterImageNum = int.Parse(storyCSVDatas[storyID + 1][6]);
                RTwoCharacterImageNum = int.Parse(storyCSVDatas[storyID + 1][7]);

                LOneCharacterWindowImage.sprite = c_ImgManager.characterImage[LOneCharacterImageNum];
                LTwoCharacterWindowImage.sprite = c_ImgManager.characterImage[LTwoCharacterImageNum];
                ROneCharacterWindowImage.sprite = c_ImgManager.characterImage[ROneCharacterImageNum];
                RTwoCharacterWindowImage.sprite = c_ImgManager.characterImage[RTwoCharacterImageNum];

                LOneBlackOut = storyCSVDatas[storyID + 1][8];
                ROneBlackOut = storyCSVDatas[storyID + 1][9];
                LTwoBlackOut = storyCSVDatas[storyID + 1][10];
                RTwoBlackOut = storyCSVDatas[storyID + 1][11];

                endFlag = int.Parse(storyCSVDatas[storyID + 1][12]);
                nextStory = storyCSVDatas[storyID + 1][13];
                nextreadStartNumber = int.Parse(storyCSVDatas[storyID + 1][14]);
                nextreadEndNumber = int.Parse(storyCSVDatas[storyID + 1][15]);
                sceneMode = int.Parse(storyCSVDatas[storyID + 1][16]);
            }
        }
        red = 1f; green = 1f; blue = 1f;
        BlackOut();
    }

    void Update()
    {
        TextDisplay();
    }

    void TextDisplay()
    {
        if (endFlag == 0)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                storyID += 1;
                endFlag = int.Parse(storyCSVDatas[storyID + 1][12]);
                storyCharacterName = storyCSVDatas[storyID + 1][2];
                storySheetText = storyCSVDatas[storyID + 1][3];
                storyText.text = storySheetText;
                nameText.text = storyCharacterName;
                CharacterImageDisplay();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (scenePattern == ScenePattern.Message)
                {
                    dataLoadName = nextStory;
                    readStartNumber = nextreadStartNumber;
                    readEndNumber = nextreadEndNumber;
                    fade.isFadeOut = true;
                    fade.sceneChangeSwitch = true;
                }
                if (scenePattern == ScenePattern.Battle)
                {
                    dataLoadName = nextStory;
                    readStartNumber = nextreadStartNumber;
                    readEndNumber = nextreadEndNumber;
                }
            }
        }
        else if (endFlag == 1)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                if(scenePattern == ScenePattern.Message)
                {
                    dataLoadName = nextStory;
                    readStartNumber = nextreadStartNumber;
                    readEndNumber = nextreadEndNumber;
                    fade.isFadeOut = true;
                    fade.sceneChangeSwitch = true;
                }
                if(scenePattern == ScenePattern.Battle)
                {
                    dataLoadName = nextStory;
                    readStartNumber = nextreadStartNumber;
                    readEndNumber = nextreadEndNumber;
                }
            }
        }
    }
    public void CharacterImageDisplay()
    {

        LOneCharacterImageNum = int.Parse(storyCSVDatas[storyID + 1][4]);
        ROneCharacterImageNum = int.Parse(storyCSVDatas[storyID + 1][5]);
        LTwoCharacterImageNum = int.Parse(storyCSVDatas[storyID + 1][6]);
        RTwoCharacterImageNum = int.Parse(storyCSVDatas[storyID + 1][7]);

        LOneCharacterWindowImage.sprite = c_ImgManager.characterImage[LOneCharacterImageNum];
        LTwoCharacterWindowImage.sprite = c_ImgManager.characterImage[LTwoCharacterImageNum];
        ROneCharacterWindowImage.sprite = c_ImgManager.characterImage[ROneCharacterImageNum];
        RTwoCharacterWindowImage.sprite = c_ImgManager.characterImage[RTwoCharacterImageNum];

        LOneBlackOut = storyCSVDatas[storyID + 1][8];
        ROneBlackOut = storyCSVDatas[storyID + 1][9];
        LTwoBlackOut = storyCSVDatas[storyID + 1][10];
        RTwoBlackOut = storyCSVDatas[storyID + 1][11];

        BlackOut();
    }

    ///顔グラの表示設定
    void BlackOut()
    {
        switch (LOneBlackOut)
        {
            case "ON":
                LOneCharacterWindowImage.color = new Color(red, green, blue, 1);
                nameImage.transform.position = LNameDisplayVector;
                break;
            case "OFF":
                LOneCharacterWindowImage.color = new Color(red, green, blue, 0);
                break;
            case "SHADE":
                LOneCharacterWindowImage.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
                break;
        }
        switch (LTwoBlackOut)
        {
            case "ON":
                LTwoCharacterWindowImage.color = new Color(red, green, blue, 1);
                nameImage.transform.position = LNameDisplayVector;
                break;
            case "OFF":
                LTwoCharacterWindowImage.color = new Color(red, green, blue, 0);
                break;
            case "SHADE":
                LTwoCharacterWindowImage.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
                break;
        }
        switch (ROneBlackOut)
        {
            case "ON":
                ROneCharacterWindowImage.color = new Color(red, green, blue, 1);
                nameImage.transform.position = RNameDisplayVector;
                break;
            case "OFF":
                ROneCharacterWindowImage.color = new Color(red, green, blue, 0);
                break;
            case "SHADE":
                ROneCharacterWindowImage.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
                break;
        }
        switch (RTwoBlackOut)
        {
            case "ON":
                RTwoCharacterWindowImage.color = new Color(red, green, blue, 1);
                nameImage.transform.position = RNameDisplayVector;
                break;
            case "OFF":
                RTwoCharacterWindowImage.color = new Color(red, green, blue, 0);
                break;
            case "SHADE":
                RTwoCharacterWindowImage.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
                break;
        }
    }


    public static int GetStoryNumber()
    {
        return storyNumber;
    }
}
