using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("Weapon and Ability Checks")]
    public bool hasSword;
    public bool hasAxe;
    public bool hasBow;
    public bool hasDashAbility;

    [Header("Character Settings")]
    public float moveSpeed = 5f;
    Vector2 movement;
    Vector2 moveDirection;
    public Rigidbody2D rb;
    public Animator animator;
    public Tilemap tilemap;
    private TileInteraction tileInteraction;

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1;
    [SerializeField] float dashCooldown = 1;
    bool isDashing;
    bool canDash = true;


    private void Start()
    {
        canDash = true;
        tileInteraction = GetComponent<TileInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDashing)
        {
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        hasSword = animator.GetBool("HasSword");
        hasAxe = animator.GetBool("HasAxe");
        hasBow = animator.GetBool("HasBow");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetAxisRaw("Horizontal") == 1 ||
            Input.GetAxisRaw("Horizontal") == -1 ||
            Input.GetAxisRaw("Vertical") == 1 ||
            Input.GetAxisRaw("Vertical") == -1)
        {
            animator.SetFloat("Last_Horizontal", Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("Last_Vertical", Input.GetAxisRaw("Vertical"));
            if(hasSword)
            {
                animator.SetFloat("Last_Sword_Horizontal", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("Last_Sword_Vertical", Input.GetAxisRaw("Vertical"));
            } else if (hasAxe)
            {
                animator.SetFloat("Last_Axe_Horizontal", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("Last_Axe_Vertical", Input.GetAxisRaw("Vertical"));

                moveSpeed = 4.5f;
            } else if (hasBow)
            {
                animator.SetFloat("Last_Bow_Horizontal", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("Last_Bow_Vertical", Input.GetAxisRaw("Vertical"));
            }
        }

        moveDirection = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);

        if (Input.GetKeyDown(KeyCode.E) && hasDashAbility && canDash) 
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        // Check if the next position is not a wall tile using the TileInteraction script.
        if (!tileInteraction.IsNextPositionWall(newPosition))
        {
            rb.MovePosition(newPosition);
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}