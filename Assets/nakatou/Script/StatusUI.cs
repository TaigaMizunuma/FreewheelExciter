using UnityEngine;
using UnityEngine.UI;

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
    /// 戦闘時のUI表示
    /// </summary>
    /// <param name="player_name">プレーヤの名前</param>
    /// <param name="player_hp">プレーヤのHP</param>
    /// <param name="player_maxhp">プレーヤの最大HP</param>
    /// <param name="player_atk">プレーヤの攻撃</param>
    /// <param name="player_accuracy">プレーヤの命中</param>
    /// <param name="player_deadly">プレーヤの必殺</param>
    /// <param name="enemy_name">敵の名前</param>
    /// <param name="enemyr_hp">敵のHP</param>
    /// <param name="enemy_maxhp">敵の最大HP</param>
    /// <param name="enemy_atk">敵の攻撃</param>
    /// <param name="enemy_accuracy">敵の命中</param>
    /// <param name="enemy_deadly">敵の必殺</param>
    public void setBattleStatus(
        string player_name, int player_hp, int player_maxhp, int player_atk, float player_hit, float player_critical,
        string enemy_name, int enemy_hp, int enemy_maxhp, int enemy_atk, float enemy_hit, float enemy_critical)
    {
        unitName.text = player_name + " | " + enemy_name;
        hpGage.transform.gameObject.SetActive(false);
        HP.enabled = true;
        HP.text = "\n" +
            player_hp.ToString() + "/" + player_maxhp.ToString() +" | " + 
            enemy_hp + "/" + enemy_maxhp + "\n" +
            player_atk + "  |  " + enemy_atk + "\n" +
            player_hit + "%  |  " + enemy_hit + "% \n" +
            player_critical + "%  |  " + enemy_critical + "%";
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
        //player_hpGage.gameObject.SetActive(true);
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
        //enemy_hpGage.gameObject.SetActive(true);
        enemy_hpGage.maxValue = e_maxhp;
        enemy_hpGage.value = e_hp;
    }

    public void setPlayerDamage(int damage)
    {
        player_hpGage.value -= damage;
    }
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
