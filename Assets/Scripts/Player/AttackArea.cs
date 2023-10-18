using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int damage = 3;
    [SerializeField]
    private float knockbackForce = 10.0f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Health>() != null)
        {
            Health health = collider.GetComponent<Health>();
            health.Damage(damage);

            Vector2 direction = (collider.transform.position - transform.position).normalized;

            Rigidbody2D targetRigidbody = collider.GetComponent<Rigidbody2D>();
            Debug.Log(targetRigidbody.ToString());
            if (targetRigidbody != null)
            {
                targetRigidbody.AddForce(-direction * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}