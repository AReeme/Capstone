using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public Health health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        health.health -= damage;
        healthBar.fillAmount = health.health / 100f;
    }

    public void Heal(int healingAmount)
    {
        health.health += healingAmount;
        health.health = Mathf.Clamp(health.health, 0, 100);

        healthBar.fillAmount = health.health / 100f;
    }
}
