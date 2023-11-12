using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool hasSword;
    private GameObject attackArea = default;
    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0f;
    public Animator animator;

    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        hasSword = animator.GetBool("HasSword");

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (attacking)
        {
            timer += Time.deltaTime;

            if(timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
                if (hasSword)
                {
                    animator.SetBool("isSwordAttack", false);
                    animator.SetBool("isAttack", false);
                }
                else
                {
                    animator.SetBool("isAttack", false);
                    animator.SetBool("isSwordAttack", false);
                }
            }
        }
    }

    private void Attack()
    {
        attacking = true;
        attackArea.SetActive(attacking);
        if (hasSword) 
        {
            animator.SetBool("isSwordAttack", true);
        }
        else
        {
            animator.SetBool("isAttack", true);
        }
    }
}
