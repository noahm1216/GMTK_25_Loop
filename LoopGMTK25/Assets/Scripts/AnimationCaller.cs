using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCaller : MonoBehaviour
{

    public Animator animCon;
    private string nextAnimation;


    public void SetString(string _animation)
    {
        nextAnimation = _animation;
    }

    public void SetBool(bool _true)
    {
        if (animCon) animCon.SetBool(nextAnimation, _true);
    }

    public void SetInteger(int _num)
    {
        if (animCon) animCon.SetInteger(nextAnimation, _num);
    }

    public void SetTrigger(string _animation)
    {
        if (animCon) animCon.SetTrigger(_animation);
    }

    public void ResetTrigger(string _animation)
    {
        if (animCon) animCon.ResetTrigger(_animation);
    }
}
