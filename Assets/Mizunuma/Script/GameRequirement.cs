using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRequirement : MonoBehaviour
{
    /*セーブデータ格納変数*/
    private int SaveDataTurn = 0;
    private float SaveDataTime = 0;
    private int SaveOne = 0;
    void Start()
    {
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    Debug.Log("セーブデータクリア");
        //    SaveData.Clear();
        //}
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    Debug.Log("セーブしますた");
        //  
        //    GameSituationDataSave();
        //}
    }
    /// <summary>
    /// ゲームクリアしたときに呼ばれるメソッド
    /// </summary>
    public void GameClear()
    {
        /*クリア時セーブ*/
        GameSituationDataSave();
        /*次の章へ現在はタイトルにしている。*/
        FindObjectOfType<Fade>().SetScene("Story");
        /*フェード実行*/
        FadeInitialize();
    }
    /// <summary>
    /// ゲームオーバーしたときに呼ばれるメソッド
    /// </summary>
    public void GameOver()
    {
        /*タイトルへ戻る*/
        FindObjectOfType<Fade>().SetScene("GameOver");
        /*フェード実行*/
        FadeInitialize();
    }
    /// <summary>
    /// フェードアウトを実行する処理
    /// </summary>
    public void FadeInitialize()
    {
        FindObjectOfType<Fade>().SetOutFade(true);
        /*シナリオを全部読むためのフラグ*/
        FindObjectOfType<Fade>().SetSceneChangeSwitch(true);
    }
    /// <summary>
    /// 累計ターンのセーブや累計タイムのためのセーブ
    /// セーブデータ用の処理に使う　毎回書き換える
    /// </summary>
    private void GameSituationDataSave()
    {
        GameDataFolderSave();
    }

    private void GameDataFolderSave()
    {
        if (SaveOne == 0)
        {
            /*ターンセーブ*/
            SaveDataTurn = FindObjectOfType<SituationTexts>().GetTurn();
            SaveData.SetInt("GameDataTurn", SaveDataTurn);
            /*タイムセーブ*/
            SaveDataTime = FindObjectOfType<SituationTexts>().GetTime();
            SaveData.SetFloat("GameDataTime", SaveDataTime);
            /*章番号セーブ*/
            SaveData.SetInt("GameDataNumber", FindObjectOfType<StoryCSVReader>().GetStoryNumber());
            /*章タイトルセーブ*/
            SaveData.SetString("GameDataTitle", FindObjectOfType<StoryCSVReader>().GetStoryTitle());
            //////////////////////////////////////////////////
            /*キャラクターデータセーブ*/
            var charas = GameObject.FindGameObjectsWithTag("Player");
            for (var i = 0; i < charas.Length; i++)
            {
                charas[i].GetComponent<StatusSave>().CharactorDataSave(charas[i].GetComponent<Character>()._name);
            }
            //////////////////////////////////////////////////
            /*クリア時にセーブ*/
            SaveData.Save();
            /*セーブ用キー作成*/
            SaveData.SetString("GameSaveCheck", "SaveCheck");
           
            /*セーブカウント*/
            SaveOne = 1;
        }
    }
}
