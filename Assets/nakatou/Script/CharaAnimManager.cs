using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAnimManager : MonoBehaviour
{
    Animator _animator;
   
    // Use this for initialization
    void Start()
    {
        _animator = GetComponent(typeof(Animator)) as Animator;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RunningStart()
    {
        _animator.CrossFade("Run", 0.0f);
    }

    public void RunnigEnd()
    {
        _animator.CrossFade("NoneDamy", 0.0f);
    }
}
