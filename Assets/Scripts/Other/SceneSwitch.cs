using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    int level;
    [SerializeField] LevelSystem levelD;
    float health;
    float maxhealh;
    [SerializeField] Health healthD;
    float xp;
    [SerializeField] LevelSystem xpD;
    float requiredXp;
    [SerializeField] LevelSystem requiredXpD;
    bool hasDashAbility;
    [SerializeField] PlayerMovement dashAbility;
    bool hasRegenAbility;
    [SerializeField] Health regenAbility;
    bool hasHealthUpAbility;
    [SerializeField] Health healthUpAbility;
    bool hasDamageUpAbility;
    [SerializeField] AttackArea damageUpAbility;
    bool hasSpeedUpAbility;
    [SerializeField] PlayerMovement speedAbility;

    private void Update()
    {
        level = levelD.level;
        health = healthD.health;
        maxhealh = healthD.MAX_HEATH;
        xp = xpD.currentXP;
        requiredXp = requiredXpD.requiredXP;
        hasDashAbility = dashAbility.hasDashAbility;
        hasRegenAbility = regenAbility.hasRegenAbility;
        hasHealthUpAbility = healthUpAbility.hasHealthUpAbility;
        hasDamageUpAbility = damageUpAbility.hasDamageUpAbility;
        hasSpeedUpAbility = speedAbility.hasSpeedAbility;
    }

    public string sceneToLoad;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int levelDataToKeep = level;
            GiveValues.instance.level = levelDataToKeep;
            float healthDataToKeep = health;
            GiveValues.instance.health = healthDataToKeep;
            float maxHelathDataToKeep = maxhealh;
            GiveValues.instance.MAX_HELATH = maxHelathDataToKeep;
            float xpDataToKeep = xp;
            GiveValues.instance.xp = xpDataToKeep;
            float requiredXpDataToKeep = requiredXp;
            GiveValues.instance.requiredXp = requiredXpDataToKeep;
            bool dashDataToKeep = hasDashAbility;
            GiveValues.instance.dash = dashDataToKeep;
            bool regenDataToKeep = hasRegenAbility;
            GiveValues.instance.regen = regenDataToKeep;
            bool damageDataToKeep = hasDamageUpAbility;
            GiveValues.instance.damageUp = damageDataToKeep;
            bool speedDataToKeep = hasSpeedUpAbility;
            GiveValues.instance.speedUp = speedDataToKeep;
            bool healthUpToKeep = hasHealthUpAbility;
            GiveValues.instance.healthUp = healthUpToKeep;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
