using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    private Animator player;
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Animator>();
    }

    Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();
        foreach(Loot item in lootList) 
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
            GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;

            float dropForce = 10f;
            Vector2 dropDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            lootGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);

            BoxCollider2D prefabCollider = droppedItemPrefab.GetComponent<BoxCollider2D>();
            BoxCollider2D lootCollider = lootGameObject.GetComponent<BoxCollider2D>();

            if (prefabCollider != null && lootCollider != null)
            {
                lootCollider.size = prefabCollider.size;
                lootCollider.offset = prefabCollider.offset;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collision with player detected");

            // Call InstantiateLoot and pass the position of the current LootBag
            InstantiateLoot(transform.position);
        }
    }
}
