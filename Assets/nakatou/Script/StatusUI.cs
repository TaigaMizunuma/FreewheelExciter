using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// UIにステータスを表示させるためのクラス
/// </summary>
public class StatusUI : MonoBehaviour
{
    Text unitName;
    Text HP;
    Slider hpGage;

    Slider player_hpGage;
    Slider enemy_hpGage;

    void Awake()
    {
        unitName = transform.Find("NameText").GetComponent<Text>();
        HP = transform.Find("HPText").GetComponent<Text>();
        hpGage = transform.Find("HpGage").GetComponent<Slider>();
        player_hpGage = transform.Find("PlayerGage").GetComponent<Slider>();
        enemy_hpGage = transform.Find("EnemyGage").GetComponent<Slider>();
    }

    void Start()
    {
        player_hpGage.gameObject.SetActive(false);
        enemy_hpGage.gameObject.SetActive(false);
    }

    void Update()
    {
    }

    /// <summary>
    /// キャラステータスを表示
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="hp">現在のHP</param>
    /// <param name="maxhp">最大HP</param>
    public void setUnitStatus(string name, int hp, int maxhp)
    {
        hpGage.transform.gameObject.SetActive(true);
        HP.enabled = true;
        unitName.text = name;
        if (hp < 0) hp = 0;
        HP.text = hp.ToString()+ "/" + maxhp.ToString();
        hpGage.maxValue = maxhp;
        hpGage.value = hp;
    }

    /// <summary>
    /// 戦闘時のUI表示　改良版
    /// </summary>
    /// <param name="attack">攻撃側</param>
    /// <param name="defense">反撃側</param>
    public GameObject SetBattleStatus(GameObject attack, GameObject defense)
    {
        GameObject ui;
        if(!GameObject.Find("BattleStatusUI"))
        {
            //ステータス表示UI生成
            var obj= Resources.Load("BattleStatusUI");
            ui = Instantiate(obj, GameObject.Find("Canvas1").transform.FindChild("Frame")) as GameObject;
            ui.name = "BattleStatusUI";
        }
        else
        {
            ui = GameObject.Find("BattleStatusUI");
        }
        
        var atkText = GameObject.Find("AtkStatus").GetComponent<Text>();//攻撃側のステータス表示テキスト
        var defText = GameObject.Find("DefStatus").GetComponent<Text>();//反撃側のステータス表示テキスト

        var status1 = attack.GetComponent<Character>();
        var status2 = defense.GetComponent<Character>();

        List<string> chara1status = new List<string>();
        List<string> chara2status = new List<string>();

        int[] bs = FindObjectOfType<BattleManager>().GetBattleState(attack, defense);

        chara1status.Add(status1._name);
        chara1status.Add("HP:" + bs[0] + "/" + status1._totalMaxhp);
        chara1status.Add("攻撃力:" + bs[1]);
        //chara1status.Add("攻撃回数:" + status1._attack_count);
        chara1status.Add("命中率:" + bs[2]);
        //chara1status.Add("回避率:" + status1._avoidance);
        chara1status.Add("必殺率:" + bs[3]);

        chara2status.Add(status2._name);
        chara2status.Add("HP:" + bs[4] + "/" + status2._totalMaxhp);
        chara2status.Add("攻撃力:" + bs[5]);
        //chara2status.Add("攻撃回数:" + status2._attack_count);
        chara2status.Add("命中率:" + bs[6]);
        //chara2status.Add("回避率:" + status2._avoidance);
        chara2status.Add("必殺率:" + bs[7]);

        atkText.text = "";
        defText.text = "";
        foreach(var st in chara1status)
        {
            atkText.text += st + "\n";
        }

        foreach (var st in chara2status)
        {
            defText.text += st + "\n";
        }
        return ui as GameObject;
    }

    /// <summary>
    /// 床の効果とか表示
    /// </summary>
    /// <param name="mapname">現在は名前のみ</param>
    public void setMapStatus(string mapname)
    {
        unitName.text = mapname;
        hpGage.transform.gameObject.SetActive(false);
        HP.enabled = false;
    }

    /// <summary>
    /// 戦闘時に表示するplayerHPゲージ
    /// </summary>
    /// <param name="p_maxhp">プレーヤ最大HP</param>
    /// <param name="p_hp">プレーヤHP</param>
    public void setPlayerHpGage(int p_maxhp, int p_hp)
    {
        player_hpGage.maxValue = p_maxhp;
        player_hpGage.value = p_hp;
    }

    /// <summary>
    ///  戦闘時に表示するenemyHPゲージ
    /// </summary>
    /// <param name="e_maxhp">敵最大HP</param>
    /// <param name="e_hp">敵HP</param>
    public void setEnemyHpGage(int e_maxhp, int e_hp)
    {
        enemy_hpGage.maxValue = e_maxhp;
        enemy_hpGage.value = e_hp;
    }

    /// <summary>
    /// プレイヤーのHPゲージにダメージを反映
    /// </summary>
    /// <param name="damage"></param>
    public void setPlayerDamage(int damage)
    {
        player_hpGage.value -= damage;
    }

    /// <summary>
    /// エネミーのHPゲージにダメージを反映
    /// </summary>
    /// <param name="damage"></param>
    public void setEnemyDamage(int damage)
    {
        enemy_hpGage.value -= damage;
    }

    /// <summary>
    /// HPゲージの表示非表示
    /// </summary>
    /// <param name="value"></param>
    public void setactive(bool value)
    {
        player_hpGage.gameObject.SetActive(value);
        enemy_hpGage.gameObject.SetActive(value);
    }
}
