using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour
{
    public bool hasSword;
    public bool hasAxe;
    public bool hasBow;

    [Header("Attack Area Settings")]
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
    bool isShooting;

    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
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

            if(timer >= timeToAttack)
            {
                if (hasSword)
                {
                    timer = 0;
                    attacking = false;
                    attackArea.SetActive(attacking);
                    animator.SetBool("isSwordAttack", false);
                    animator.SetBool("isAttack", false);
                } else if (hasAxe)
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
                } else
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
        if (hasSword) 
        {
            attacking = true;
            attackArea.SetActive(attacking);
            animator.SetBool("isSwordAttack", true);
        }
        else if (hasAxe)
        {
            attacking = true;
            attackArea.SetActive(attacking);
            animator.SetBool("isAxeAttack", true);
        } else if (hasBow && canShoot)
        {
            StartCoroutine(ShootCooldown());
            animator.SetBool("isBowAttack", true);
        }
        else
        {
            attacking = true;
            attackArea.SetActive(attacking);
            animator.SetBool("isAttack", true);
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

        if (horizontal > 0)
        {
            arrowRenderer.flipX = true;
        }
        if (vertical > 0)
        {
            arrow.transform.Rotate(Vector3.forward * -90);
        }
        else if (vertical < 0)
        {
            arrow.transform.Rotate(Vector3.forward * 90);
        }

        Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
        arrowRb.velocity = new Vector2(shootDirection.x * arrowSpeed, shootDirection.y * arrowSpeed);

        Destroy(arrow, 2f);
    }

    private IEnumerator ShootCooldown()
    {
        attacking = true;
        canShoot = false;
        isShooting = true;
        yield return new WaitForSeconds(0.1f);
        ShootArrow();
        yield return new WaitForSeconds(0.4f);
        isShooting = false;
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }
}
