using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 仮のタイトルクラス
/// </summary>
public class Title : MonoBehaviour
{
    public Text text;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(DelayMethod.DelayMethodCall(0.5f, () =>
        {
            test();
        }));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(1);
        }
    }

    /// <summary>
    /// 点滅するよ!!
    /// </summary>
    void test()
    {
        text.enabled = !text.enabled;
        StartCoroutine(DelayMethod.DelayMethodCall(0.5f, () =>
        {
            test();
        }));
    }
}
