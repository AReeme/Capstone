using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    private Animator playerAnimator;
    private AttackArea attackArea;
    private PlayerMovement playerMovement;
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();
    private LootType lootTypeToApply;

    // Add a cooldown time for weapon switching
    private bool canSwitchWeapon = true;
    public float weaponSwitchCooldown = 1.0f;
    private bool hasSpawnedLoot = false;

    private void Start()
    {
        playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        //// Get the AttackArea component from the player's children
        //attackArea = GameObject.FindWithTag("Player").GetComponentInChildren<AttackArea>();

        //// Check if attackArea is null
        //if (attackArea == null)
        //{
        //    Debug.LogError("AttackArea component not found.");
        //}
        //else
        //{
        //    Debug.Log("AttackArea component found.");
        //}
    }

    Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();
        foreach (Loot item in lootList)
        {
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }
        if (possibleItems.Count > 0)
        {
            Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }

        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        if (canSwitchWeapon && !hasSpawnedLoot)
        {
            Loot droppedItem = GetDroppedItem();
            if (droppedItem != null)
            {
                //GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
                //lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;

                //BoxCollider2D prefabCollider = droppedItemPrefab.GetComponent<BoxCollider2D>();
                //BoxCollider2D lootCollider = lootGameObject.GetComponent<BoxCollider2D>();

                //lootCollider.size = prefabCollider.size;
                //lootCollider.offset = prefabCollider.offset;

                // Update player's bools based on the loot obtained
                lootTypeToApply = droppedItem.lootType;

                UpdatePlayerBools(lootTypeToApply);

                // Set the flag to true, indicating that loot has been spawned
                hasSpawnedLoot = true;
            }
        }
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
                if (playerMovement.moveSpeed < 7.5f)
                {
                    playerMovement.moveSpeed = 7.5f;
                }
                break;
            case LootType.Axe:
                playerAnimator.SetBool("isBowAttack", false);
                playerAnimator.SetBool("isSwordAttack", false);
                playerAnimator.SetBool("HasAxe", true);
                //attackArea.WeaponDamage(20);
                playerMovement.moveSpeed -= 5;
                break;
            case LootType.Bow:
                playerAnimator.SetBool("isSwordAttack", false);
                playerAnimator.SetBool("isAxeAttack", false);
                playerAnimator.SetBool("HasBow", true);
                //attackArea.WeaponDamage(5);
                if (playerMovement.moveSpeed < 7.5f)
                {
                    playerMovement.moveSpeed = 7.5f;
                }
                break;
        }
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerAnimator.SetBool("isSwordAttack", false);
    //        playerAnimator.SetBool("isAxeAttack", false);
    //        playerAnimator.SetBool("isBowAttack", false);
    //        playerAnimator.SetBool("isAttack", false);

    //        Debug.Log("Collision with player detected");

    //        UpdatePlayerBools(lootTypeToApply);

    //        // Destroy the LootBag GameObject
    //        Destroy(gameObject);
    //    }
    //}
}