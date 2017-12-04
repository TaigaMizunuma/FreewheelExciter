using UnityEngine;
using UnityEngine.UI;

public class ExpGage : MonoBehaviour
{
    Slider exp_gage;
    int exp;//上昇前EXP
    int add_exp;//上昇後EXP
    float speed = 1.0f;//ゲージが上昇するスピード

    void Start()
    {
        exp_gage = GetComponent<Slider>();
        Enabled(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (exp_gage.value < add_exp) 
        {
            exp_gage.value += speed;
        }
        else
        {

        }
    }

    /// <summary>
    /// EXPをゲージに渡す
    /// </summary>
    /// <param name="exp1">上昇前EXP</param>
    /// <param name="exp2">上昇後EXP</param>
    public void SetExpGage(int exp1,int exp2)
    {
        Enabled(true);
        exp_gage.value = exp1;
        add_exp = exp2;
    }


    public void Enabled(bool value)
    {
        foreach (Transform child in transform)
        {
            child.transform.gameObject.SetActive(value);
        }
    }
}
