using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private Transform player;
    private LevelSystem playerLevel;
    public EnemyController eController;
    private PlayerAttack playerAttack;

    [SerializeField]
    public float eHealth = 100;
    private float eMAX_HEATH = 100000;

    public void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerAttack = GameObject.FindWithTag("Player").GetComponent<PlayerAttack>();
        playerLevel = player.GetComponent<LevelSystem>();

        // Add a Rigidbody2D component to the enemy
        if (GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
        }
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
        eController = GetComponent<EnemyController>();
        float scalingFactor = 0.1f;

        eMAX_HEATH = maxHealth + (int)(maxHealth * eController.enemyLevel * scalingFactor);
        eHealth = health + (int)(health * eController.enemyLevel * scalingFactor);
    }

    public void Damage(float amount)
    {

        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative damage.");
        }

        eHealth -= amount;
        StartCoroutine(VisualIndicator(Color.red));

        if (eHealth <= 0)
        {
            Die();
            playerAttack.enemiesKilled++;
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing.");
        }

        bool wouldBeOverMaxHealth = eHealth + amount > eMAX_HEATH;
        StartCoroutine(VisualIndicator(Color.green));

        if (wouldBeOverMaxHealth)
        {
            eHealth = eMAX_HEATH;
        } else
        {
            eHealth += amount;
        }
    }

    private void Die()
    {
        playerLevel.GainExperienceScalable(eController.enemyXP, eController.enemyLevel);
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        Destroy(gameObject);
    }
}
