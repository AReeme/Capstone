using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    
    [Header("Health Settings")]
    public float health;
    private float lerpTimer;
    public float MAX_HEATH = 100f;

    [Header("HealthBar Animations")]
    public float chipSpeed = 2f; 
    public HealthManager healthManager;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI healthText;

    [Header("Health Abilites")]
    public bool hasRegenAbility;
    public bool hasHealthUpAbility;
    private bool healthAbilityActivated;
    float healCooldown = 10f;
    bool canHeal = true;

    private void Start()
    {
        canHeal = true;
        health = MAX_HEATH;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, MAX_HEATH);
        UpdateHealthUI();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(Random.Range(5, 10));
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Heal(Random.Range(5, 10));
        }

        if (hasRegenAbility && canHeal)
        {
            StartCoroutine(Regen());
        }
    }

    private void FixedUpdate()
    {
        if (hasHealthUpAbility && !healthAbilityActivated)
        {
            ActivateHealthUpAbility();
        }

        if (!hasHealthUpAbility && healthAbilityActivated)
        {
            DeactivateHealthUpAbility();
        }
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / MAX_HEATH;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
        healthText.text = Mathf.Round(health) + "/" + Mathf.Round(MAX_HEATH);
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

    public void Damage(int amount)
    {

        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative damage.");
        }

        healthManager.TakeDamage(amount);
        lerpTimer = 0f;
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
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("Death");
        Invoke("SwitchScene", 1.1f);
        
    }

    public void IncreaseHealth(int level)
    {
        MAX_HEATH += (health * 0.01f) * ((100 - level) * 0.1f);
        health = MAX_HEATH;
    }

    private IEnumerator Regen()
    {
        Heal(10);
        canHeal = false;
        yield return new WaitForSeconds(healCooldown);
        canHeal = true;
    }

    private void SwitchScene()
    {
        SceneManager.LoadScene("Game Over");
        Camera.main.transform.parent = null;
        Destroy(gameObject);
    }

    private void ActivateHealthUpAbility()
    {
        if (!healthAbilityActivated)
        {
            health += 50;
            MAX_HEATH = health;
            healthAbilityActivated = true;
        }
    }

    private void DeactivateHealthUpAbility()
    {
        if (healthAbilityActivated)
        {
            health -= 50;
            MAX_HEATH = health;
            healthAbilityActivated = false;
        }
    }
}
