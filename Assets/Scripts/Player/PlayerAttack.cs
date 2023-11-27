using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool hasSword;
    public bool hasAxe;
    public bool hasBow;

    [Header("Audio")]
    public AudioSource swordSwing;
    public AudioSource axeSwing;
    public AudioSource bowAttack;
    public AudioSource punchAttack;

    [Header("AttackArea Settings")]
    private GameObject attackArea = default;
    public Arrow arrowScript;
    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0f;
    public Animator animator;

    [Header("Bow Settings")]
    [SerializeField] Transform hand;
    public GameObject arrowPrefab;
    public float arrowSpeed = 10f;
    bool canShoot = true;
    //bool isShooting;

    [Header("Weapon Durability")]
    public int swordDurability = 15;
    public int axeDurability = 5;
    public int bowDurability = 10;

    private int currentSwordDurability;
    private int currentAxeDurability;
    private int currentBowDurability;

    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        currentSwordDurability = swordDurability;
        currentAxeDurability = axeDurability;
        currentBowDurability = bowDurability;
    }

    void Update()
    {
        hasSword = animator.GetBool("HasSword");
        hasAxe = animator.GetBool("HasAxe");
        hasBow = animator.GetBool("HasBow");

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAttack)
            {
                if (hasSword)
                {
                    timer = 0;
                    attacking = false;
                    attackArea.SetActive(attacking);
                    animator.SetBool("isSwordAttack", false);
                    animator.SetBool("isAttack", false);
                }
                else if (hasAxe)
                {
                    timer = 0;
                    attacking = false;
                    attackArea.SetActive(attacking);
                    animator.SetBool("isAxeAttack", false);
                    animator.SetBool("isAttack", false);
                }
                else if (hasBow)
                {
                    timer = 0;
                    attacking = false;
                    animator.SetBool("isBowAttack", false);
                    animator.SetBool("isAttack", false);
                }
                else
                {
                    timer = 0;
                    attacking = false;
                    attackArea.SetActive(attacking);
                    animator.SetBool("isAttack", false);
                }
            }
        }
    }

    private void Attack()
    {
        if (hasSword && currentSwordDurability > 0)
        {
            // Sword attack logic
            Debug.Log("Sword Durability: " + currentSwordDurability);

            if (currentSwordDurability == 0)
            {
                SwitchToRegularAttack();
            }
            else
            {
                attacking = true;
                attackArea.SetActive(attacking);
                animator.SetBool("isSwordAttack", true);
                swordSwing.Play();
                DeductSwordDurability();
            }
        }
        else if (hasAxe && currentAxeDurability > 0)
        {
            // Axe attack logic
            Debug.Log("Axe Durability: " + currentAxeDurability);

            if (currentAxeDurability == 0)
            {
                SwitchToRegularAttack();
            }
            else
            {
                attacking = true;
                attackArea.SetActive(attacking);
                animator.SetBool("isAxeAttack", true);
                axeSwing.Play();
                DeductAxeDurability();
            }
        }
        else if (hasBow && canShoot && currentBowDurability > 0)
        {
            // Bow attack logic
            StartCoroutine(ShootCooldown());
            Debug.Log("Bow Durability: " + currentBowDurability);

            if (currentBowDurability == 0)
            {
                SwitchToRegularAttack();
            }
            else
            {
                animator.SetBool("isBowAttack", true);
            }
        }
        else
        {
            // Regular attack logic
            attacking = true;
            attackArea.SetActive(attacking);
            animator.SetBool("isAttack", true);
            punchAttack.Play();
        }
    }

    void ShootArrow()
    {
        Vector2 shootDirection;

        // Check the player's last facing direction
        float horizontal = animator.GetFloat("Last_Bow_Horizontal");
        float vertical = animator.GetFloat("Last_Bow_Vertical");
        GameObject arrow = Instantiate(arrowPrefab, hand.position, Quaternion.identity);
        SpriteRenderer arrowRenderer = arrow.GetComponent<SpriteRenderer>();

        // If the player is not facing a specific direction, default to shooting upwards
        if (horizontal == 0 && vertical == 0)
        {
            shootDirection = Vector2.down;
            arrow.transform.Rotate(Vector3.forward * 90);
        }
        else
        {
            shootDirection = new Vector2(horizontal, vertical).normalized;
        }

        if (horizontal > 0 && vertical > 0)
        {
            arrowRenderer.flipX = true;
            arrow.transform.Rotate(Vector3.forward * 45);
        }
        else if (horizontal < 0 && vertical > 0)
        {
            arrow.transform.Rotate(Vector3.forward * -45);
        }
        else if (horizontal > 0 && vertical < 0)
        {
            arrowRenderer.flipX = true;
            arrow.transform.Rotate(Vector3.forward * -45);
        }
        else if (horizontal < 0 && vertical < 0)
        {
            arrow.transform.Rotate(Vector3.forward * 45);
        }
        else if (horizontal > 0)
        {
            arrowRenderer.flipX = true;
        }
        else if (vertical > 0)
        {
            arrow.transform.Rotate(Vector3.forward * -90);
        }
        else if (vertical < 0)
        {
            arrow.transform.Rotate(Vector3.forward * 90);
        }

        Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
        arrowRb.velocity = new Vector2(shootDirection.x * arrowSpeed, shootDirection.y * arrowSpeed);

        bowAttack.Play();

        Destroy(arrow, 2f);

        DeductBowDurability();
    }

    private IEnumerator ShootCooldown()
    {
        attacking = true;
        canShoot = false;
        //isShooting = true;
        yield return new WaitForSeconds(0.1f);
        ShootArrow();
        yield return new WaitForSeconds(0.4f);
        //isShooting = false;
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }

    public void DeductSwordDurability()
    {
        currentSwordDurability--;

        // Add logic for handling zero durability and switching to regular attack
        if (currentSwordDurability == 0)
        {
            SwitchToRegularAttack();
        }
    }

    public void DeductAxeDurability()
    {
        currentAxeDurability--;

        // Add logic for handling zero durability and switching to regular attack
        if (currentAxeDurability == 0)
        {
            SwitchToRegularAttack();
        }
    }

    public void DeductBowDurability()
    {
        currentBowDurability--;

        // Add logic for handling zero durability and switching to regular attack
        if (currentBowDurability == 0)
        {
            SwitchToRegularAttack();
        }
    }

    private void SwitchToRegularAttack()
    {
        // Switch to regular attack logic
        Debug.Log("Switching to regular attack");

        // Reset durability for the current weapon
        if (hasSword)
        {
            currentSwordDurability = swordDurability;
            animator.SetBool("HasAxe", false);
            animator.SetBool("HasSword", false);
            animator.SetBool("HasBow", false);
            animator.SetBool("isSwordAttack", false);
            animator.SetBool("isAxeAttack", false);
            animator.SetBool("isBowAttack", false);
            swordDurability = 30;
        }
        else if (hasAxe)
        {
            currentAxeDurability = axeDurability;
            animator.SetBool("HasAxe", false);
            animator.SetBool("HasSword", false);
            animator.SetBool("HasBow", false);
            animator.SetBool("isSwordAttack", false);
            animator.SetBool("isAxeAttack", false);
            animator.SetBool("isBowAttack", false);
            axeDurability = 10;
        }
        else if (hasBow)
        {
            currentBowDurability = bowDurability;
            animator.SetBool("HasAxe", false);
            animator.SetBool("HasSword", false);
            animator.SetBool("HasBow", false);
            animator.SetBool("isSwordAttack", false);
            animator.SetBool("isAxeAttack", false);
            animator.SetBool("isBowAttack", false);
            bowDurability = 20;
        }

        animator.SetBool("isSwordAttack", false);
        animator.SetBool("isAxeAttack", false);
        animator.SetBool("isBowAttack", false);
        animator.SetBool("isAttack", false);
    }
}