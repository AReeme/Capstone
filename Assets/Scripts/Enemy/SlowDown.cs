using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : MonoBehaviour
{
    public bool isPlayerOnTile = false;
    private float originalLinearDrag;
    private float slowLinearDrag = 2f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerOnTile = true;
            ChangeSlowfactor(slowLinearDrag);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerOnTile = false;
            ChangeSlowfactor(originalLinearDrag);
        }
    }

    private void ChangeSlowfactor(float linearDrag)
    {
        Rigidbody2D rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        if (!isPlayerOnTile)
        {
            originalLinearDrag = rb.drag;
        }

        rb.drag = linearDrag;
    }
}
