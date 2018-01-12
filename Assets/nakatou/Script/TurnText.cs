using UnityEngine.UI;
using UnityEngine;

public class TurnText : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// ターン経過のアニメーションが終わった時に呼ぶ
    /// </summary>
    public void AnimEnd()
    {
        GetComponent<Animator>().SetBool("Init", false);
        GetComponent<Text>().text = "";
    }
}
