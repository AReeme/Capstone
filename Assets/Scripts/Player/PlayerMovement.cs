using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public bool hasSword;
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    Vector2 movement;
    public Tilemap tilemap;
    public StopwatchTimer stopwatchTimer;

    private TileInteraction tileInteraction;

    private void Start()
    {
        tileInteraction = GetComponent<TileInteraction>();

        // Find the StopwatchTimer script in the scene (or on the same GameObject).
        stopwatchTimer = FindObjectOfType<StopwatchTimer>();

        // Start the timer automatically when the game starts.
        if (stopwatchTimer != null)
        {
            stopwatchTimer.StartTimer();
        }
        else
        {
            Debug.LogError("StopwatchTimer script not found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        hasSword = animator.GetBool("HasSword");

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
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        // Check if the next position is not a wall tile using the TileInteraction script.
        if (!tileInteraction.IsNextPositionWall(newPosition))
        {
            rb.MovePosition(newPosition);
        }
    }
}