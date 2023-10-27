using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackArea : MonoBehaviour
{
    private int damage = 3;
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private float strength = 16, delay = 0.15f;
    public UnityEvent OnBegin, OnDone;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<EnemyHealth>() != null)
        {
            EnemyHealth health = collider.GetComponent<EnemyHealth>();
            health.Damage(damage);

            // Apply knockback to the enemy
            ApplyKnockback(collider.gameObject);
        }
    }

    private void ApplyKnockback(GameObject enemy)
    {
        // Check if the enemy has a Rigidbody2D component
        Rigidbody2D enemyRigidbody = enemy.GetComponent<Rigidbody2D>();
        if (enemyRigidbody != null)
        {
            // Calculate the direction from this object to the enemy
            Vector2 direction = (enemy.transform.position - transform.position).normalized;

            // Apply an impulse force to the enemy's Rigidbody2D
            enemyRigidbody.AddForce(direction * strength, ForceMode2D.Impulse);

            // Start the coroutine to reset the velocity
            StartCoroutine(Reset(enemyRigidbody));
        }
    }

    private IEnumerator Reset(Rigidbody2D enemyRigidbody)
    {
        yield return new WaitForSeconds(delay);

        // Check if the enemyRigidbody is still valid before accessing it
        if (enemyRigidbody != null)
        {
            enemyRigidbody.velocity = Vector2.zero;
        }

        OnDone?.Invoke();
    }

    public void PlayFeedback(GameObject sender)
    {
        StopAllCoroutines();
        OnBegin?.Invoke();
        ApplyKnockback(sender);
    }
}