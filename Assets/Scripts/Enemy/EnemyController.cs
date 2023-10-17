using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    [SerializeField]
    [Range(1, 15)]
    private float moveSpeed = 5;
    public Rigidbody2D rb;
    public Animator animator;
    private float lastHorizontal = 0f;
    private float lastVertical = 0f;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate the direction to move towards the player
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            // Calculate the movement direction
            float horizontalInput = directionToPlayer.x;
            float verticalInput = directionToPlayer.y;

            // Set animator parameters for animation
            animator.SetFloat("Horizontal", horizontalInput);
            animator.SetFloat("Vertical", verticalInput);
            animator.SetFloat("Speed", directionToPlayer.magnitude);

            // Move the enemy towards the player
            rb.velocity = directionToPlayer * moveSpeed;

            // Update the last horizontal and vertical directions
            lastHorizontal = directionToPlayer.x;
            lastVertical = directionToPlayer.y;

            // Pass the last horizontal and vertical directions to the animator
            animator.SetFloat("Last_Horizontal", lastHorizontal);
            animator.SetFloat("Last_Vertical", lastVertical);
        }
    }
}