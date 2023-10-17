using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private GameObject attackArea;
    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0f;
    public Animator animator;
    public float attackRange = 3f;
    public Transform playerPrefab;

    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerPrefab.position);

        if (distanceToPlayer <= attackRange && !attacking)
        {
            // Calculate the direction to the player
            Vector3 directionToPlayer = (playerPrefab.position - transform.position).normalized;

            Attack();
        }

        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
                animator.SetBool("isAttack", false);
            }
        }
    }

    private void Attack()
    {
        attacking = true;
        attackArea.SetActive(attacking);
        animator.SetBool("isAttack", true);
    }
}