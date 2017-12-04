using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterGraphic : MonoBehaviour
{
    //今表示してるID
    public int storyID;

    //開始部分の番号(外部から変更可能)
    [HideInInspector]
    public int readStartNumber;

    //今話しているキャラ
    string storyCharacterName;

    //キャラクターグラフィックの番号
    [SerializeField]
    int LOneCharacterImageNum;
    [SerializeField]
    int LTwoCharacterImageNum;
    [SerializeField]
    int ROneCharacterImageNum;
    [SerializeField]
    int RTwoCharacterImageNum;

    //キャラクターのグラの表示設定
    //ON...表示 OFF...非表示 SHADE...うす暗く
    [SerializeField]
    string LOneBlackOut;
    [SerializeField]
    string LTwoBlackOut;
    [SerializeField]
    string ROneBlackOut;
    [SerializeField]
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

    [Space(10)]

    //名前の表示枠
    [SerializeField]
    GameObject nameDisplay;

    CharacterImageManager c_ImgManager;

    public List<string[]> cg_storyCSVDatas;

    float red, green, blue, alfa;

    [Space(10)]

    [SerializeField]
    Vector2 LNameDisplayVector;
    [SerializeField]
    Vector2 RNameDisplayVector;

    int c_i;
    int c_j;

    void Start()
    {
        c_ImgManager = this.gameObject.GetComponent<CharacterImageManager>();


        LOneBlackOut = cg_storyCSVDatas[storyID + 1][9];
        LOneBlackOut = cg_storyCSVDatas[storyID + 1][10];
        LOneBlackOut = cg_storyCSVDatas[storyID + 1][11];
        LOneBlackOut = cg_storyCSVDatas[storyID + 1][12];

        red = 1f; green = 1f; blue = 1f;
    }

    /// <summary>
    /// 顔グラの表示処理
    /// </summary>
    public void CharacterImageDisplay()
    {

        LOneCharacterImageNum = int.Parse(cg_storyCSVDatas[storyID + 1][5]);
        ROneCharacterImageNum = int.Parse(cg_storyCSVDatas[storyID + 1][6]);
        LTwoCharacterImageNum = int.Parse(cg_storyCSVDatas[storyID + 1][7]);
        RTwoCharacterImageNum = int.Parse(cg_storyCSVDatas[storyID + 1][8]);

        LOneCharacterWindowImage.sprite = c_ImgManager.characterImage[LOneCharacterImageNum];
        LTwoCharacterWindowImage.sprite = c_ImgManager.characterImage[LTwoCharacterImageNum];
        ROneCharacterWindowImage.sprite = c_ImgManager.characterImage[ROneCharacterImageNum];
        RTwoCharacterWindowImage.sprite = c_ImgManager.characterImage[RTwoCharacterImageNum];

        LOneBlackOut = cg_storyCSVDatas[storyID + 1][9];
        LOneBlackOut = cg_storyCSVDatas[storyID + 1][10];
        LOneBlackOut = cg_storyCSVDatas[storyID + 1][11];
        LOneBlackOut = cg_storyCSVDatas[storyID + 1][12];

        BlackOut();
    }

    ///顔グラの表示設定
    void BlackOut()
    {
        switch (LOneBlackOut)
        {
            case "ON":
                LOneCharacterWindowImage.color = new Color(red, green, blue, 1);
                nameDisplay.transform.position = LNameDisplayVector;
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
                nameDisplay.transform.position = LNameDisplayVector;
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
                nameDisplay.transform.position = RNameDisplayVector;
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
                nameDisplay.transform.position = RNameDisplayVector;
                break;
            case "OFF":
                RTwoCharacterWindowImage.color = new Color(red, green, blue, 0);
                break;
            case "SHADE":
                RTwoCharacterWindowImage.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
                break;
        }
    }
}
