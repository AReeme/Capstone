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
        if (!hasSpawnedLoot)
        {
            Loot droppedItem = GetDroppedItem();
            if (droppedItem != null)
            {
                lootTypeToApply = droppedItem.lootType;
                GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
                lootGameObject.GetComponent<LootDrop>().SetDropType(lootTypeToApply);
                lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;

                BoxCollider2D prefabCollider = droppedItemPrefab.GetComponent<BoxCollider2D>();
                BoxCollider2D lootCollider = lootGameObject.GetComponent<BoxCollider2D>();

                lootCollider.size = prefabCollider.size;
                lootCollider.offset = prefabCollider.offset;

                // Set the flag to true, indicating that loot has been spawned
                hasSpawnedLoot = true;
            }
        }
    }
}