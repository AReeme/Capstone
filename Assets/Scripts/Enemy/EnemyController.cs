using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    [SerializeField]
    [Range(1, 15)]
    private int moveSpeed = 5;
    private int attackRange = 1;
    [SerializeField]
    [Range(5, 20)]
    public int damage = 5;
    public Rigidbody2D rb;
    public Animator animator;
    Vector2 movement;

    public bool isAttacking = false;

    void Update()
    {
        // Calculate the movement direction
        float horizontalInput = player.position.x - transform.position.x;
        float verticalInput = player.position.y - transform.position.y;
        movement = new Vector2(horizontalInput, verticalInput).normalized;

        // Set animator parameters for animation
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.magnitude);

        //if (!isAttacking)
        //{
        //    if (IsPlayerInAttackRange())
        //    {
        //        isAttacking = true;
        //        AttackPlayer();
        //    }
        //    else
        //    {
                rb.velocity = movement.normalized * moveSpeed; 
        //    }
        //}
        //else
        //{
            
        //}
    }

    //bool IsPlayerInAttackRange()
    //{
    //    return Vector3.Distance(transform.position, player.position) <= attackRange;
    //}

    //void AttackPlayer()
    //{
    //    if (IsPlayerInAttackRange())
    //    {
    //        movement = Vector2.zero;
    //        animator.SetBool("isAttack", true);
    //        Health playerHealth = player.GetComponent<Health>();
    //        if (playerHealth != null)
    //        {
    //            playerHealth.TakeDamage(damage);
    //        }
    //    }
    //}

    //void StopAttack()
    //{
    //    if (animator.GetBool("isAttack"))
    //        animator.SetBool("isAttack", false);
    //}
}