using UnityEngine.UI;
using UnityEngine;

public class DamageGage : MonoBehaviour
{
    Slider damageGage;
    Slider Gage;//親のゲージ

    void Awake()
    {
        damageGage = GetComponent<Slider>();
        Gage = transform.parent.transform.GetComponent<Slider>();
    }
    void Start()
    {
        damageGage.maxValue = Gage.maxValue;
        damageGage.value = Gage.value;
    }

    void Update()
    {
        if(Gage.value < damageGage.value)
        {
            damageGage.value-=0.5f;
        }
        else
        {
        }
    }

    public void setdamageGage(int maxvalue, int value)
    {
        damageGage.maxValue = maxvalue;
        damageGage.value = value;
    }
}
