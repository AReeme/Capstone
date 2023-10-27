using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public bool isPlayerOnTile = false;
    [SerializeField]
    public int damagePerSecond = 20;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerOnTile = true;
            InvokeRepeating("InflictDamage", 0f, 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerOnTile = false;
            CancelInvoke("InflictDamage");
        }
    }

    private void InflictDamage()
    {
        if (isPlayerOnTile) 
        {
            Health health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            health.Damage(damagePerSecond);
        }
    }
}
