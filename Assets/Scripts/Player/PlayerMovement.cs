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
    private bool dashAbilityActivated = false;
    public bool hasSpeedAbility;
    private bool speedAbilityActivated = false;

    [Header("Character Settings")]
    public float moveSpeed = 5f;
    Vector2 movement;
    Vector2 moveDirection = Vector2.zero;
    public Rigidbody2D rb;
    public Animator animator;
    public Tilemap tilemap;
    private TileInteraction tileInteraction;
    public int AbilitiesSelected;

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1;
    [SerializeField] float dashCooldown = 1;
    bool isDashing;
    bool canDash = true;
    public TrailRenderer dashTrail;

    private PlayerAttack playerAttack;
    private AbilityManager abilityManager;

    private void Start()
    {
        hasDashAbility = (bool)GiveValues.instance?.dash;
        hasSpeedAbility = (bool)GiveValues.instance?.speedUp;
        canDash = true;
        tileInteraction = GetComponent<TileInteraction>();
        playerAttack = GetComponent<PlayerAttack>();
        abilityManager = FindObjectOfType<AbilityManager>();
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

        hasSword = playerAttack.hasSword;
        hasAxe = playerAttack.hasAxe;
        hasBow = playerAttack.hasBow;

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetAxisRaw("Horizontal") != 0 ||
            Input.GetAxisRaw("Vertical") != 0)
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

        moveDirection = movement.normalized * moveSpeed;

        if (Input.GetKeyDown(KeyCode.E) && hasDashAbility && canDash) 
        {
            StartCoroutine(Dash());
        }

        if(!hasDashAbility)
        {
            dashTrail.enabled = false;
        }

        if(hasDashAbility)
        {
            dashTrail.enabled = true;
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (hasSpeedAbility && !speedAbilityActivated)
        {
            ActivateSpeedAbility();
        }

        if (!hasSpeedAbility && speedAbilityActivated)
        {
            DeactivateSpeedAbility();
        }

        if (hasDashAbility && !dashAbilityActivated)
        {
            ActivateDashAbility();
        }

        if (!hasDashAbility && dashAbilityActivated)
        {
            DeactivateDashAbility();
        }

        Vector2 newPosition = rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime;

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

        // Enable the trail when dashing starts
        if (dashTrail != null)
            dashTrail.emitting = true;

        rb.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
        //rb.velocity = dash * dashSpeed;
        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        // Disable the trail when dashing is finished
        if (dashTrail != null)
            dashTrail.emitting = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void ActivateSpeedAbility()
    {
        hasSpeedAbility = true;
        moveSpeed += 5;
        speedAbilityActivated = true;

        if (abilityManager != null && abilityManager.speedUpIcon != null) abilityManager.speedUpIcon.enabled = true;
    }

    public void DeactivateSpeedAbility()
    {
        hasSpeedAbility = true;
        moveSpeed -= 5;
        speedAbilityActivated = false;

        if (abilityManager != null && abilityManager.speedUpIcon != null) abilityManager.speedUpIcon.enabled = false;
    }

    public void ActivateDashAbility()
    {
        hasDashAbility = true;
        dashAbilityActivated = true;

        if (abilityManager != null && abilityManager.dashIcon != null) abilityManager.dashIcon.enabled = true;
    }

    public void DeactivateDashAbility()
    {
        hasDashAbility = false;
        dashAbilityActivated = false;

        if (abilityManager != null && abilityManager.dashIcon != null) abilityManager.dashIcon.enabled = false;
    }
}