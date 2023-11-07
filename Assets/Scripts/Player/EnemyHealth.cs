using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private Transform player;
    private LevelSystem playerLevel;
    public EnemyController eController;

    [SerializeField]
    public float health = 100;
    private float MAX_HEATH = 100;

    public void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerLevel = player.GetComponent<LevelSystem>();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    Damage(10);
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    Heal(10);
        //}
    }

    private IEnumerator VisualIndicator(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void SetHealth(int maxHealth, int health)
    {
        this.MAX_HEATH = maxHealth;
        this.health = health;
    }

    public void Damage(float amount)
    {

        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative damage.");
        }

        this.health -= amount;
        StartCoroutine(VisualIndicator(Color.red));

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing.");
        }

        bool wouldBeOverMaxHealth = health + amount > MAX_HEATH;
        StartCoroutine(VisualIndicator(Color.green));

        if (wouldBeOverMaxHealth)
        {
            this.health = MAX_HEATH;
        } else
        {
            this.health += amount;
        }
    }

    private void Die()
    {
        playerLevel.GainExperienceScalable(eController.enemyXP, eController.enemyLevel);
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        Destroy(gameObject);
    }
}
