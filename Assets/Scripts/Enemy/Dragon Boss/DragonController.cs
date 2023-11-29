using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    [Header("Stats")]
    public float health = 200;
    public int damage = 30;
    public int moveSpeed = 5;
    public int chaseRange = 5;
    public int attackRange = 1;
    private bool isAlive;

    [Header("Inspector Items")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public Animator animator;
    [SerializeField] public Transform player;

    private enum EnemyState
    {
        Idle,
        Chase,
        Attack
    }
    private EnemyState currentState;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        isAlive = true;
        currentState = EnemyState.Idle;
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            switch (currentState)
            {
                case EnemyState.Idle:
                    if (distanceToPlayer <= attackRange)
                    {
                        SwitchToAttackState();
                    }
                    if (distanceToPlayer <= chaseRange)
                    {
                        SwitchToChaseState();
                    }
                    Idle();
                    break;
                case EnemyState.Chase:
                    if (distanceToPlayer <= attackRange) 
                    {
                        SwitchToAttackState();
                    }
                    if (distanceToPlayer > chaseRange)
                    {
                        SwitchToIdleState();
                    }
                    Chase();
                    break;
                case EnemyState.Attack:
                    if(distanceToPlayer > attackRange)
                    {
                        SwitchToChaseState();
                    }

                    break;
            }
        }
    }

    void Chase()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        rb.velocity = directionToPlayer * moveSpeed;

        // Set animator parameters for animation (same for Wander and Chase).
        animator.SetFloat("Horizontal", rb.velocity.x);
        animator.SetFloat("Vertical", rb.velocity.y);
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetFloat("Last_Horizontal", rb.velocity.x);
        animator.SetFloat("Last_Vertical", rb.velocity.y);
    }

    void Idle()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        rb.velocity = directionToPlayer * 0.01f;

        // Set animator parameters for animation (same for Wander and Chase).
        animator.SetFloat("Horizontal", rb.velocity.x);
        animator.SetFloat("Vertical", rb.velocity.y);
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetFloat("Last_Horizontal", rb.velocity.x);
        animator.SetFloat("Last_Vertical", rb.velocity.y);
    }

    void Attack()
    {

    }

    void SwitchToChaseState()
    {
        currentState = EnemyState.Chase;
    }

    void SwitchToIdleState()
    {
        currentState = EnemyState.Idle;
    }

    void SwitchToAttackState()
    {
        currentState = EnemyState.Attack;
    }

    private IEnumerator VisualIndicator(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
