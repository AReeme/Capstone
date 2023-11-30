using System.Collections;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    [Header("Stats")]
    public float health = 200;
    public int damage = 30;
    public int moveSpeed = 3;
    public int chaseRange = 10;
    public int attackRange = 3;

    [Header("Inspector Items")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public Animator animator;
    [SerializeField] public Transform player;

    [Header("Scene Objects")]
    [SerializeField] public GameObject sceneTransition;
    [SerializeField] public AudioSource victoryTheme;
    [SerializeField] public AudioSource bossMusic;

    [Header("Attack Settings")]
    [SerializeField] public GameObject attackArea;
    public float attackDuration = 1f;

    private enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Death
    }
    private EnemyState currentState;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        currentState = EnemyState.Idle;
        bossMusic.Play();
        attackArea.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchToDeathState();
        }

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            switch (currentState)
            {
                case EnemyState.Idle:
                    if (health <= 0)
                    {
                        SwitchToDeathState();
                    }
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
                    if (health <= 0)
                    {
                        SwitchToDeathState();
                    }
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
                    if (health <= 0)
                    {
                        SwitchToDeathState();
                    }
                    if (distanceToPlayer > attackRange)
                    {
                        SwitchToChaseState();
                    }
                    Attack();
                    break;
                case EnemyState.Death:
                    Die();
                    break;
            }
        }
    }

    void Chase()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        rb.velocity = directionToPlayer * moveSpeed;

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

        animator.SetFloat("Horizontal", rb.velocity.x);
        animator.SetFloat("Vertical", rb.velocity.y);
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetFloat("Last_Horizontal", rb.velocity.x);
        animator.SetFloat("Last_Vertical", rb.velocity.y);
    }

    void Attack()
    {
        if (!animator.GetBool("isAttack"))
        {
            animator.SetBool("isAttack", true);
            StartCoroutine(EnableAttackAreaDuringAttack());
        }

        animator.SetFloat("Horizontal", rb.velocity.x);
        animator.SetFloat("Vertical", rb.velocity.y);
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetFloat("Last_Horizontal", rb.velocity.x);
        animator.SetFloat("Last_Vertical", rb.velocity.y);
    }

    void Death()
    {
        rb.velocity = Vector3.zero;
        animator.SetTrigger("Death");

        animator.SetFloat("Horizontal", rb.velocity.x);
        animator.SetFloat("Vertical", rb.velocity.y);
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetFloat("Last_Horizontal", rb.velocity.x);
        animator.SetFloat("Last_Vertical", rb.velocity.y);

    }

    void SwitchToChaseState()
    {
        animator.SetBool("isAttack", false);
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

    void SwitchToDeathState()
    {
        currentState = EnemyState.Death;
    }

    public void Damage(float amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative damage.");
        }

        health -= amount;
        StartCoroutine(VisualIndicator(Color.black));

        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator EnableAttackAreaDuringAttack()
    {
        attackArea.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        attackArea.SetActive(false);
        animator.SetBool("isAttack", false);
    }

    private IEnumerator VisualIndicator(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void Die()
    {
        Death();
        Destroy(gameObject);
        sceneTransition.SetActive(true);
        bossMusic.Stop();
        victoryTheme.Play();
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(3f);
    }
}
