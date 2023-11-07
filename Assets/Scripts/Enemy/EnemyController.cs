using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{
    private Transform player;
    private LevelSystem playerLevel;

    [SerializeField]
    [Range(1, 15)]
    private float moveSpeed = 5;
    [SerializeField]
    private int damage = 5;

    public Rigidbody2D rb;
    public Animator animator;

    public int enemyLevel;
    public float enemyXP;
    public float XPMultiplier;

    private Vector3 wanderDirection;
    private float wanderInterval = 2.0f;
    private float wanderTimer;

    private float chaseRange = 10f;

    private enum EnemyState
    {
        Wander,
        Chase
    };
    private EnemyState currentState;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerLevel = player.GetComponent<LevelSystem>();
        enemyLevel = Random.Range(1, playerLevel.level + 2);
        damage *= enemyLevel;

        // Have all enemies spawned in the wander state
        currentState = EnemyState.Wander;
        SetRandomWanderDirection();

        // Enemy is a Higher Level than the Player
        enemyXP = Mathf.Round(enemyLevel * 6 * XPMultiplier);
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            switch (currentState)
            {
                case EnemyState.Wander:
                    if (distanceToPlayer <= chaseRange) 
                    {
                        SwitchToChaseState();
                    }
                    Wander();
                    break;

                case EnemyState.Chase:
                    if (distanceToPlayer >= chaseRange)
                    {
                        SwitchToWanderState();
                    }
                    Chase();
                    break;
            }
        }
    }

    void Wander()
    {
        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0)
        {
            SetRandomWanderDirection();
            wanderTimer = wanderInterval;
        }

        rb.velocity = wanderDirection * moveSpeed;

        // Set animator parameters for animation (same for Wander and Chase).
        animator.SetFloat("Horizontal", rb.velocity.x);
        animator.SetFloat("Vertical", rb.velocity.y);
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetFloat("Last_Horizontal", rb.velocity.x);
        animator.SetFloat("Last_Vertical", rb.velocity.y);
    }

    void SetRandomWanderDirection()
    {
        wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
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

    void SwitchToChaseState()
    {
        currentState = EnemyState.Chase;
    }

    void SwitchToWanderState()
    {
        currentState = EnemyState.Wander;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (collider.GetComponent<Health>() != null) 
            {
                collider.GetComponent<Health>().Damage(damage);
                this.GetComponent<EnemyHealth>().Damage(10000); 
            }
        }
    }
}