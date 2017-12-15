using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsTexts : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject MapFrame;
    public GameObject SituationButton;
    public GameObject BGMButton;
    public GameObject SEButton;
    public GameObject ReturnButton;
    /*グローバル変数*/
    private Scrollbar BGMScrollbar;
    private Scrollbar SEScrollbar;
    private int UIcount;
    private Text Setting01;
    private Image BGMScrollbar03Handle;
    private Image SEScrollbar04Handle;

    private bool BGMFlag = false;
    private bool SEFlag = false;
    private float BGMNumber = 0;
    private float SENumber = 0;
    //初期値 = 0 保存した値があった時のみ読み込む
    private int SaveCount = 0;

    void Start ()
    {
        BGMScrollbar = GameObject.Find("BGMScrollbar03").GetComponent<Scrollbar>();
        SEScrollbar = GameObject.Find("SEScrollbar04").GetComponent<Scrollbar>();
        Setting01 = GameObject.Find("TextSetting01").GetComponent<Text>();
        BGMScrollbar03Handle = GameObject.Find("BGMScrollbar03Handle").GetComponent<Image>();
        SEScrollbar04Handle = GameObject.Find("SEScrollbar04Handle").GetComponent<Image>();
        if (SaveData.HasKey("BGMSetting") == true && SaveData.HasKey("SESetting") == true)
        {
            /*データ存在時*/
            BGMScrollbar.value = SaveData.GetFloat("BGMSetting");
            SEScrollbar.value = SaveData.GetFloat("SESetting");
        }
        else
        {
            Debug.Log("ない");
        }
    }
	void Update ()
    {
        switch (UIcount)
        {
            /*ケース1を飛ばしてケース3が実行されるため、ケース2を挟んだ*/
            /*ケース1 メニューボタンロックし、UI表示カウントアップ*/
            case 1:
                SettingsTrue();
                FindObjectOfType<MenuManager>().GetMainControlFlag(true);
                eventSystem.SetSelectedGameObject(BGMButton);
                UIcount++;
                break;
            case 2:
                break;
            /*ケース2 UI非表示 メニューボタンロック解除 メニュー戻れないようにする解除*/
            case 3:
                SettingsFalse();
                FindObjectOfType<MenuManager>().GetMainControlFlag(false);
                eventSystem.SetSelectedGameObject(SituationButton);
                UIcount = 0;
                SaveData.SetFloat("SESetting", SENumber);
                SaveData.SetFloat("BGMSetting", BGMNumber);
                break;
        }

        if (BGMFlag == true && SEFlag == false)
        {
            Debug.Log("BGM:" + Mathf.CeilToInt(BGMScrollbar.value * 100) + " E決定");
            eventSystem.sendNavigationEvents = false;
            /*右が押されたら＋方向へ変動*/
            if (Input.GetKey(KeyCode.RightArrow))
            {
                BGMScrollbar.value += 0.05f;
            }
            /*左が押されたら-方向へ変動*/
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                BGMScrollbar.value -= 0.05f;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                eventSystem.sendNavigationEvents = true;
                BGMNumber = BGMScrollbar.value;
                BGMFlag = false;
            }
        }
        if (SEFlag == true && BGMFlag == false)
        {
            Debug.Log("SE:" + Mathf.CeilToInt(SEScrollbar.value * 100) + " E決定");
            eventSystem.sendNavigationEvents = false;
            /*右が押されたら＋方向へ変動*/
            if (Input.GetKey(KeyCode.RightArrow))
            {
                SEScrollbar.value += 0.05f;
            }
            /*左が押されたら-方向へ変動*/
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                SEScrollbar.value -= 0.05f;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                eventSystem.sendNavigationEvents = true;
                SENumber = SEScrollbar.value;
                
                SEFlag = false;
            }
        }
    }

    private void SettingsTrue()
    {
        /*メニュー表示*/
        MapFrame.SetActive(true);
        BGMButton.GetComponent<Text>().enabled = true;
        SEButton.GetComponent<Text>().enabled = true;
        ReturnButton.GetComponent<Text>().enabled = true;
        Setting01.enabled = true;
        BGMScrollbar.GetComponent<Image>().enabled = true;
        SEScrollbar.GetComponent<Image>().enabled = true;
        BGMScrollbar03Handle.enabled = true;
        SEScrollbar04Handle.enabled= true;
    }
    private void SettingsFalse()
    {
        /*メニュー非表示*/
        MapFrame.SetActive(false);
        BGMButton.GetComponent<Text>().enabled = false;
        SEButton.GetComponent<Text>().enabled = false;
        ReturnButton.GetComponent<Text>().enabled = false;
        Setting01.enabled = false;
        BGMScrollbar.GetComponent<Image>().enabled = false;
        SEScrollbar.GetComponent<Image>().enabled = false;
        BGMScrollbar03Handle.enabled = false;
        SEScrollbar04Handle.enabled = false;
    }
    public void SettingsCount()
    {
        UIcount++;
    }

    public void BGMVolumeSettings()
    {
        BGMFlag = true;
        SEFlag = false;
        Debug.Log("BGM設定ボタン押されました Eで戻れます");
    }
    public void SEVolumeSettings()
    {
        SEFlag = true;
        BGMFlag = false;
        Debug.Log("SE設定ボタン押されました　Eで戻れます");
    }

    public void GetBGMVolumeValue(float bgmvalue)
    {
        bgmvalue = BGMScrollbar.value;
    }
    public void GetSEVolumeValue(float sevalue)
    {
        sevalue = BGMScrollbar.value;
    }
}
