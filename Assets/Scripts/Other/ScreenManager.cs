using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator.SetTrigger("GameStart");   
    }
}
