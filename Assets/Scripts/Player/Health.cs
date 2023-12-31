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
    public float health = 100;
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

    private AbilityManager abilityManager;
    public AudioSource damageSound;
    public AudioSource deathSound;

    public float damageTaken;

    private void Start()
    {
        health = (float)GiveValues.instance?.health;
        MAX_HEATH = (float)GiveValues.instance?.MAX_HELATH;
        hasRegenAbility = (bool)GiveValues.instance?.regen;
        hasHealthUpAbility = (bool)GiveValues.instance?.healthUp;
        damageTaken= (float)GiveValues.instance?.damageTaken;
        canHeal = true;
        //health = MAX_HEATH;
        abilityManager = FindObjectOfType<AbilityManager>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, MAX_HEATH);
        UpdateHealthUI();
        if (Input.GetKeyDown(KeyCode.C))
        {
            Damage(Random.Range(5, 10));
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Heal(Random.Range(5, 10));
        }

        if (hasRegenAbility && canHeal)
        {
            if (abilityManager != null && abilityManager.regenIcon != null) abilityManager.regenIcon.enabled = true;
            StartCoroutine(Regen());
        }

        if(!hasRegenAbility)
        {
            if (abilityManager != null && abilityManager.regenIcon != null) abilityManager.regenIcon.enabled = false;
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

        if (hasRegenAbility && canHeal)
        {
            ActivateRegenAbility();
        }

        if (!hasRegenAbility)
        {
            if (abilityManager != null && abilityManager.regenIcon != null) abilityManager.regenIcon.enabled = false;
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
        damageSound.Play();
        StartCoroutine(VisualIndicator(Color.red));

        if (health <= 0)
        {
            Die();
        }

        damageTaken = amount + damageTaken;
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
        deathSound.Play();
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

    public void ActivateRegenAbility()
    {
        hasRegenAbility = true;
        if (abilityManager != null && abilityManager.regenIcon != null) abilityManager.regenIcon.enabled = true;
        StartCoroutine(Regen());
    }

    public void ActivateHealthUpAbility()
    {
        hasHealthUpAbility = true;
        if (!healthAbilityActivated)
        {
            health += 50;
            MAX_HEATH = health;
            healthAbilityActivated = true;

            if (abilityManager != null && abilityManager.healthUpIcon != null) abilityManager.healthUpIcon.enabled = true;
        }
    }

    private void DeactivateHealthUpAbility()
    {
        if (healthAbilityActivated)
        {
            health -= 50;
            MAX_HEATH = health;
            healthAbilityActivated = false;

            if (abilityManager != null && abilityManager.healthUpIcon != null) abilityManager.healthUpIcon.enabled = false;
        }
    }
}
