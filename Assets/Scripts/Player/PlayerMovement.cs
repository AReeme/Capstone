using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetAxisRaw("Horizontal") == 1 ||
           Input.GetAxisRaw("Horizontal") == -1 ||
           Input.GetAxisRaw("Vertical") == 1 ||
           Input.GetAxisRaw("Vertical") == -1)
        {
            animator.SetFloat("Last_Horizontal",
                              Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("Last_Vertical",
                              Input.GetAxisRaw("Vertical"));
        }

        if (Input.GetKeyDown(KeyCode.Space)) animator.SetBool("isAttack", true);
    }

    void FixedUpdate()
    {
        if (animator.GetBool("isAttack") == true)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.MovePosition(rb.position + movement * moveSpeed *
                            Time.fixedDeltaTime);
        }
    }

    void StopAttack()
    {
        if (animator.GetBool("isAttack"))
            animator.SetBool("isAttack", false);
    }
}
