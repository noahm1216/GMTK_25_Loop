using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    private static readonly int State = Animator.StringToHash("state");
    public Animator samuraiAnimator;


    private void OnMouseEnter()
    {
        if (samuraiAnimator != null)
        {
            samuraiAnimator.SetInteger(State, 1);
        }
    }

    private void OnMouseExit()
    {
        if (samuraiAnimator != null)
        {
            samuraiAnimator.SetInteger(State, 0);
        }
    }

    private void OnMouseDown()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
