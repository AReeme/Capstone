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
        }
     }
}
