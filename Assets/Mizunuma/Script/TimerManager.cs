using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerManager : MonoBehaviour
{
    /*タイマーの中身*/
    public float starttimer;
    /*タイム止める為のフラグ*/
    /*true = 動く false = 動かない*/
    public bool timerstop = false;
    public bool controlflag = false;
    /*リセットするときの変数*/
    private float resettimer = 0.0f;
    void Update()
    {
        if (timerstop == true)
        {
            starttimer -= Time.deltaTime;
        }
        GetComponent<Text>().text =
            (string.Format("{1:00}:{2:00}",
            Mathf.Floor(starttimer / 3600f),
            Mathf.Floor(starttimer / 60f),
            Mathf.Floor(starttimer % 60f),
            starttimer % 1 * 99));

        if (controlflag == false)
        {
            
            if (starttimer <= 1)
            {
                timerstop = false;
                //FindObjectOfType<GameControlManager>().GameOver();
                controlflag = true;
            }
        }
    }
    public void TimerStart()
    {
        if (timerstop == false)
        {
            timerstop = true;
        }
        else if (timerstop == true)
        {
            timerstop = false;
        }
    }
    public void TimeReset()
    {
        starttimer = resettimer;
    }

    public void TimeStop()
    {
        timerstop = false;
    }

}
