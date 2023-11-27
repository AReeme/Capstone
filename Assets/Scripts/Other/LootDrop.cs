using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class LootDrop : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    private LootType lootType;
    private Animator playerAnimator;

    public void SetDropType(LootType type)
    {
        lootType = type;
    }

    private void Start()
    {
        playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rb.gravityScale = 0;
        col.isTrigger = true;
    }

    void UpdatePlayerBools(LootType lootType)
    {
        switch (lootType)
        {
            case LootType.Sword:
                playerAnimator.SetBool("isBowAttack", false);
                playerAnimator.SetBool("isAxeAttack", false);
                playerAnimator.SetBool("HasSword", true);
                //attackArea.WeaponDamage(10);
                break;
            case LootType.Axe:
                playerAnimator.SetBool("isBowAttack", false);
                playerAnimator.SetBool("isSwordAttack", false);
                playerAnimator.SetBool("HasAxe", true);
                //attackArea.WeaponDamage(20);
                break;
            case LootType.Bow:
                playerAnimator.SetBool("isSwordAttack", false);
                playerAnimator.SetBool("isAxeAttack", false);
                playerAnimator.SetBool("HasBow", true);
                //attackArea.WeaponDamage(5);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerAnimator.SetBool("isSwordAttack", false);
            playerAnimator.SetBool("isAxeAttack", false);
            playerAnimator.SetBool("isBowAttack", false);
            playerAnimator.SetBool("isAttack", false);

            UpdatePlayerBools(lootType);

            Destroy(gameObject);
        }
    }
}
