using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float arrowDamage;

    public void SetArrowDamage(float damage)
    {
        arrowDamage = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is an enemy
        if (other.CompareTag("Enemy"))
        {
            // Call a method on the enemy or do something else
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.Damage(arrowDamage);
            }

            // Destroy the arrow
            Destroy(gameObject);
        }
    }
}
