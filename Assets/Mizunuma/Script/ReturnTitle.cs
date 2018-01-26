using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnTitle : MonoBehaviour
{
    public void ReturnTitleButton()
    {
        FindObjectOfType<Fade>().SetScene("Title");
        FindObjectOfType<Fade>().SetOutFade(true);
        FindObjectOfType<Fade>().SetSceneChangeSwitch(true);
    }
}
