using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class StoryCSVReader : MonoBehaviour
{
    //次にロードするCSVの名前
    static string nextStory;

    //章タイトル
    static string s_title;

    //今表示してるID
    [SerializeField]
    int storyID;

    //章番号
    static int storyNumber;

    //バトル中かストーリー画面かの判定(バトル中に他シーンに飛ばないようにするため)
    [SerializeField]
    int sceneMode;

    //次の始点と終点(勝手に入ります)
    int nextreadStartNumber;
    int nextreadEndNumber;

    //次にロードするシーンの名前
    [SerializeField]
    string nextLoadScene;

    //今表示しているテキスト
    string storySheetText;

    //今話しているキャラ
    string storyCharacterName;

    //キャラクターグラフィックの番号
    int LOneCharacterImageNum;
    int LTwoCharacterImageNum;
    int ROneCharacterImageNum;
    int RTwoCharacterImageNum;

    //背景グラフィックの番号
    int backGroundImageNum;

    //キャラクターのグラの表示設定
    //ON...表示 OFF...非表示 SHADE...うす暗く
    string LOneBlackOut;
    string LTwoBlackOut;
    string ROneBlackOut;
    string RTwoBlackOut;

    //クリア条件
    string clearCondition;

    //テキストの終了判定
    [SerializeField]
    int endFlag;

    //何処からストーリーデータをロードするか
    [SerializeField]
    string dataLoadName;

    //開始部分の番号
    [SerializeField]
    int readStartNumber;

    //終了部分の番号
    [SerializeField]
    int readEndNumber;

    [Space(12)]

    //表示用のテキスト
    [SerializeField]
    Text storyText;

    //名前のテキスト
    [SerializeField]
    Text nameText;

    //キャラクターのグラフィック
    [SerializeField]
    Image LOneCharacterWindowImage;
    [SerializeField]
    Image LTwoCharacterWindowImage;
    [SerializeField]
    Image ROneCharacterWindowImage;
    [SerializeField]
    Image RTwoCharacterWindowImage;

    //背景グラフィック
    [SerializeField]
    Image backGroundImage;

    //名前の表示枠
    [SerializeField]
    GameObject nameImage;

    //名前の表示枠の位置
    [SerializeField]
    Vector2 LNameDisplayVector;
    [SerializeField]
    Vector2 RNameDisplayVector;

    int nameImageLR;

    //CSVファイル(勝手に入ります)
    TextAsset storyCSVFile;

    //CSVデータ(勝手に入ります)
    List<string[]> storyCSVDatas = new List<string[]>();
    int storyCSVHeight = 0;

    int column;//縦
    int row;//横

    [Space(12)]

    //フェードアウト/インの切り替えスクリプト
    [SerializeField]
    Fade fade;

    //顔グラの登録用クラス
    CharacterImageManager c_ImgManager;

    //色(いじらないで！)
    float red, green, blue, alfa;

    //シーンごとの処理分けステート
    [SerializeField]
    ScenePattern scenePattern;

    //ナレーションとストーリー部分の切り替え
    NalationOrStory n_sState;

    //メッセージウィンドウ
    [SerializeField]
    GameObject messageWindow;

    //ナレーション番号
    int st_nalationNum;

    //ナレーションの有無
    int st_nalOnOff;

    //再開時ストーリーのどこからロードされるか
    int s_loadNumber;

    //戦闘中の会話フラグ(時止め用)
    bool battleStoryFlag;

    static bool loadGameFlag;

    //会話用スイッチ
    public bool battleScenarioSwitch;


    enum ScenePattern
    {
        Message,
        Battle,
    }

    public enum NalationOrStory
    {
        Nalation,
        Story,
    }

    void Awake()
    {
        if (loadGameFlag == false)
        {
            LoadNumber();
        }
        loadGameFlag = true;

        //次に読み込むストーリーが確定してたら
        if(nextStory != null)
        {
            dataLoadName = nextStory;
        }

        c_ImgManager = GetComponent<CharacterImageManager>();

        //CSVデータ読み込み
        storyCSVFile = Resources.Load("CSV/StorySentenceData/" + dataLoadName) as TextAsset;
        StringReader reader = new StringReader(storyCSVFile.text);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            storyCSVDatas.Add(line.Split(','));
            storyCSVHeight++;
        }
        //読み込んだデータの処理
        for (column = 0; column < storyCSVDatas.Count; column++)
        {
            for (row = 0; row < storyCSVDatas[column].Length; row++)
            {

                storyNumber = int.Parse(storyCSVDatas[1][0]);
                sceneMode = int.Parse(storyCSVDatas[1][16]);

                if (sceneMode == 0)
                {
                    scenePattern = ScenePattern.Message;
                }
                else
                {
                    scenePattern = ScenePattern.Battle;
                }
                st_nalOnOff = int.Parse(storyCSVDatas[1][22]);
                s_title = storyCSVDatas[storyID + 1][17];
                storyID = readStartNumber;
                if(st_nalOnOff != 0)
                {
                    st_nalationNum = int.Parse(storyCSVDatas[1][21]);
                }
                clearCondition = storyCSVDatas[storyID + 1][19];
                storyCharacterName = storyCSVDatas[storyID + 1][2];
                storySheetText = storyCSVDatas[storyID + 1][3];
                storyText.text = storySheetText;
                if (storySheetText != null)
                {
                    nameText.text = storyCharacterName;
                }
                else {
                    nameImage.SetActive(false);
                    nameText.text = "";
                }

                if(backGroundImage != null)
                {
                    backGroundImageNum = int.Parse(storyCSVDatas[1][20]);
                    if (backGroundImageNum == 7)
                    {
                        return;
                    }
                    else
                    {
                        backGroundImage.sprite = c_ImgManager.backGround[backGroundImageNum];
                    }
                }

                LOneCharacterImageNum = int.Parse(storyCSVDatas[storyID + 1][4]);
                LTwoCharacterImageNum = int.Parse(storyCSVDatas[storyID + 1][5]);
                ROneCharacterImageNum = int.Parse(storyCSVDatas[storyID + 1][6]);
                RTwoCharacterImageNum = int.Parse(storyCSVDatas[storyID + 1][7]);

                LOneCharacterWindowImage.sprite = c_ImgManager.characterImage[LOneCharacterImageNum];
                LTwoCharacterWindowImage.sprite = c_ImgManager.characterImage[LTwoCharacterImageNum];
                ROneCharacterWindowImage.sprite = c_ImgManager.characterImage[ROneCharacterImageNum];
                RTwoCharacterWindowImage.sprite = c_ImgManager.characterImage[RTwoCharacterImageNum];

                LOneBlackOut = storyCSVDatas[storyID + 1][8];
                LTwoBlackOut = storyCSVDatas[storyID + 1][9];
                ROneBlackOut = storyCSVDatas[storyID + 1][10];
                RTwoBlackOut = storyCSVDatas[storyID + 1][11];

                endFlag = int.Parse(storyCSVDatas[storyID + 1][12]);

                FindObjectOfType<Fade>().SetScene(storyCSVDatas[storyID + 1][18]);
            }
        }

    }

    void Start()
    {
        red = 1f; green = 1f; blue = 1f;
        BlackOut();
    }

    void Update()
    {
        TextDisplay();
    }

    /// <summary>
    /// ストーリー全般の処理
    /// </summary>
    void TextDisplay()
    {

        if (fade.isFadeIn == false)
        {
                if (scenePattern == ScenePattern.Message)
                {
                    MessageStory();
                }
                if (scenePattern == ScenePattern.Battle)
                {
                    BattleStory();
                }
        }
    }

    void MessageStory()
    {
        bool n_eFlag = FindObjectOfType<NalationScript>().GetNalationEndFlag();

        if (n_eFlag == true)
        {
            n_sState = NalationOrStory.Story;
        }
        else
        {
            n_sState = NalationOrStory.Nalation;
        }

        if (n_sState == NalationOrStory.Story)
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
                    if (storyCharacterName != "")
                    {
                        nameText.text = storyCharacterName;
                        nameImage.SetActive(true);
                    }
                    else {
                        nameText.text = "";
                        nameImage.SetActive(false);
                    }
                    CharacterImageDisplay();
                }

                if (Input.GetKeyDown(KeyCode.O))
                {
                    if (scenePattern == ScenePattern.Message)
                    {
                        nextStory = storyCSVDatas[1][13];

                        dataLoadName = nextStory;
                        nextLoadScene = FindObjectOfType<Fade>().GetScene();
                        FindObjectOfType<Fade>().SetOutFade(true);
                        FindObjectOfType<Fade>().SetSceneChangeSwitch(true);
                    }
                }
            }
            if (endFlag == 1)
            {
                if (Input.GetKeyDown(KeyCode.U))
                {
                    if (scenePattern == ScenePattern.Message)
                    {
                        nextStory = storyCSVDatas[1][13];
                        dataLoadName = nextStory;
                        readStartNumber = 0;
                        readEndNumber = 0;
                        nextLoadScene = FindObjectOfType<Fade>().GetScene();
                        FindObjectOfType<Fade>().SetOutFade(true);
                        FindObjectOfType<Fade>().SetSceneChangeSwitch(true);
                    }
                }
            }
        }
    }

    /// <summary>
    /// バトル中のメッセージウィンドウ制御
    /// </summary>
    void BattleStory()
    {
        if (battleScenarioSwitch == true)
        {
            messageWindow.SetActive(true);
            {
                if (Input.GetKeyDown(KeyCode.U))
                {
                        storyID += 1;
                        storyCharacterName = storyCSVDatas[storyID + 1][2];
                        storySheetText = storyCSVDatas[storyID + 1][3];
                        storyText.text = storySheetText;
                        if (storyCharacterName != "")
                        {
                            nameText.text = storyCharacterName;
                            nameImage.SetActive(true);
                        }
                        else {
                            nameText.text = "";
                            nameImage.SetActive(false);
                        }
                        CharacterImageDisplay();

                    if (storyID > readEndNumber)
                    {
                        if (Input.GetKeyDown(KeyCode.U))
                        {
                            readStartNumber = nextreadStartNumber;
                            readEndNumber = nextreadEndNumber;
                            messageWindow.SetActive(false);
                            battleScenarioSwitch = false;
                            FindObjectOfType<StoryFlag>().scenarioNum += 1; 
                        }
                    }
                }
            }
        }

        if (!GameObject.FindGameObjectWithTag("Enemy"))
        {
            {
                nextStory = storyCSVDatas[1][13];
                storyNumber = int.Parse(storyCSVDatas[2][0]);
                s_title = storyCSVDatas[2][17];
                dataLoadName = nextStory;
                readStartNumber = nextreadStartNumber;
                readEndNumber = nextreadEndNumber;
            }
        }
        else if (!GameObject.FindGameObjectWithTag("Player"))
        {
            nextStory = "Story1Before";
            dataLoadName = "Story1Before";
            readStartNumber = nextreadStartNumber;
            readEndNumber = nextreadEndNumber;
        }
    }

    /// <summary>
    /// 顔グラの登録、更新
    /// </summary>
    void CharacterImageDisplay()
    {
        LOneCharacterImageNum = int.Parse(storyCSVDatas[storyID + 1][4]);
        LTwoCharacterImageNum = int.Parse(storyCSVDatas[storyID + 1][5]);
        ROneCharacterImageNum = int.Parse(storyCSVDatas[storyID + 1][6]);
        RTwoCharacterImageNum = int.Parse(storyCSVDatas[storyID + 1][7]);

        LOneCharacterWindowImage.sprite = c_ImgManager.characterImage[LOneCharacterImageNum];
        LTwoCharacterWindowImage.sprite = c_ImgManager.characterImage[LTwoCharacterImageNum];
        ROneCharacterWindowImage.sprite = c_ImgManager.characterImage[ROneCharacterImageNum];
        RTwoCharacterWindowImage.sprite = c_ImgManager.characterImage[RTwoCharacterImageNum];

        LOneBlackOut = storyCSVDatas[storyID + 1][8];
        LTwoBlackOut = storyCSVDatas[storyID + 1][9];
        ROneBlackOut = storyCSVDatas[storyID + 1][10];
        RTwoBlackOut = storyCSVDatas[storyID + 1][11];

        BlackOut();
    }

    /// <summary>
    /// 顔グラの表示の設定
    /// </summary>
    void BlackOut()
    {
        if(nameImageLR == 0)
        {
            nameImage.transform.position = LNameDisplayVector;
        }
        else if (nameImageLR == 1)
        {
            nameImage.transform.position = RNameDisplayVector;
        }
        else { return; }

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

    public int GetStoryID()
    {
        return storyID;
    }

    /// <summary>
    /// 章番号
    /// </summary>
    /// <returns></returns>
    public int GetStoryNumber()
    {
        return storyNumber;
    }

    /// <summary>
    /// 章タイトル
    /// </summary>
    /// <returns></returns>
    public string GetStoryTitle()
    {
        return s_title;
    }

    public int GetNalationNum()
    {
        return st_nalationNum;
    }

    /// <summary>
    /// クリア条件
    /// </summary>
    /// <returns></returns>
    public string GetClearCondition()
    {
        return clearCondition;
    }

    public int GetNalOnOFF()
    {
        return st_nalOnOff;
    }

    /// <summary>
    /// この数値を参照してどのストーリーを呼ぶか決める
    /// </summary>
    /// <param name="loadNum"></param>
    public void SetLoadStoryNumber(int loadNum)
    {
        s_loadNumber = loadNum;
    }

    public void SetReadStartNum(int startNum)
    {
        nextreadStartNumber = startNum;
    }

    public void SetReadEndNum(int endNum)
    {
        nextreadEndNumber = endNum;
    }

    void LoadNumber()
    {
        switch (s_loadNumber)
        {
            case 1:
                dataLoadName = "Story2Before";
                break;
            case 2:
                dataLoadName = "Story3Before";
                break;
            case 3:
                dataLoadName = "Story4Before";
                break;
            case 4:
                dataLoadName = "Story5Before";
                break;
            case 5:
                dataLoadName = "Ending";
                break;
        }
    }
}
