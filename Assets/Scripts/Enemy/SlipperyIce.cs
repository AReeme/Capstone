using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyIce : MonoBehaviour
{
    public bool isPlayerOnTile = false;
    public float iceFriction = 0.2f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerOnTile = true;
            ChangePlayerFriction(iceFriction);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerOnTile = false;
            ChangePlayerFriction(0f);
        }
    }

    private void ChangePlayerFriction(float friction)
    {
        Rigidbody2D rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        rb.sharedMaterial.friction = friction;
    }
}
