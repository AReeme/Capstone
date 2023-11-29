using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackArea : MonoBehaviour
{
    public bool hasSword;
    public bool hasAxe;
    public bool hasBow;
    public float damage = 3;
    [SerializeField]
    private Rigidbody2D rb2d;
    public Animator animator;
    public Arrow arrowScript;
    [SerializeField]
    private float strength = 16, delay = 1.0f;
    public UnityEvent OnBegin, OnDone;

    [Header("Damage Abilities")]
    public bool hasDamageUpAbility;
    private bool damageUpAbilityActivated = false;
    private PlayerAttack playerAttack;

    private AbilityManager abilityManager;

    public void Start()
    {
        hasDamageUpAbility = (bool)GiveValues.instance?.damageUp;

        abilityManager = FindObjectOfType<AbilityManager>();

        hasSword = animator.GetBool("HasSword");
        hasAxe = animator.GetBool("HasAxe");
        hasBow = animator.GetBool("HasBow");

        // Find and store a reference to PlayerAttack script
        playerAttack = GetComponentInParent<PlayerAttack>();

        if (hasSword)
        {
            WeaponDamage(10);
        }

        if (hasAxe)
        {
            WeaponDamage(20);
        }

        if (hasBow)
        {
            WeaponDamage(5);
            arrowScript.SetArrowDamage(damage);
        }
    }

    private void FixedUpdate()
    {
        if (hasDamageUpAbility && !damageUpAbilityActivated)
        {
            ActivateDamageUpAbility();
        }

        if (!hasDamageUpAbility && damageUpAbilityActivated)
        {
            DeactivateDamageUpAbility();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<EnemyHealth>() != null)
        {
            EnemyHealth health = collider.GetComponent<EnemyHealth>();
            health.Damage(damage);

            // Apply knockback to the enemy
            ApplyKnockback(collider.gameObject);

            // Deduct durability when hitting an enemy
            if (hasSword)
            {
                playerAttack.DeductSwordDurability();
            }
            else if (hasAxe)
            {
                playerAttack.DeductAxeDurability();
            }
            else if (hasBow)
            {
                playerAttack.DeductBowDurability();
            }
        }
    }

    private void ApplyKnockback(GameObject enemy)
    {
        if (!enemy.CompareTag("Dragon"))
        {
            Rigidbody2D enemyRigidbody = enemy.GetComponent<Rigidbody2D>();
            if (enemyRigidbody != null)
            {
                Vector2 direction = (enemy.transform.position - transform.position).normalized;
                enemy.GetComponent<EnemyController>().Stun(delay);
                enemyRigidbody.AddForce(direction * strength, ForceMode2D.Impulse);
                //StartCoroutine(Reset(enemyRigidbody));
            }
        }
    }

    private IEnumerator Reset(Rigidbody2D enemyRigidbody)
    {
        yield return new WaitForSeconds(delay);

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

    public void IncreaseDamage(int level)
    {
        damage += (damage * 0.01f) * ((100 - level) * 0.5f);
    }

    public void WeaponDamage(int weaponDamage)
    {
        damage += weaponDamage;
    }

    public void ActivateDamageUpAbility()
    {
        hasDamageUpAbility = true;
        damageUpAbilityActivated = true;
        damage += 5;

        if (abilityManager != null && abilityManager.damageUpIcon != null) abilityManager.damageUpIcon.enabled = true;
    }

    private void DeactivateDamageUpAbility()
    {
        hasDamageUpAbility = false;
        damageUpAbilityActivated = false;
        damage -= 5;

        if (abilityManager != null && abilityManager.damageUpIcon != null) abilityManager.damageUpIcon.enabled = false;
    }
}