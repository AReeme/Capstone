using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LootType
{
    Sword,
    Axe,
    Bow
}

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    public Sprite lootSprite;
    public string lootName;
    public int dropChance;
    public LootType lootType;

    public Loot(string lootName, int dropChance, LootType lootType)
    {
        this.lootName = lootName;
        this.dropChance = dropChance;
        this.lootType = lootType;
    }
}
