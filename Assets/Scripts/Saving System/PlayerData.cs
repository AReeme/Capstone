using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public float health;
    public float xp;
    public float requiredXp;
    public bool hasDashAbility;
    public bool hasRegenAbility;
    public bool hasHealthUpAbility;
    public bool hasDamageUpAbility;
    public bool hasSpeedUpAbility;
    public string currentScene;

    public PlayerData(Dictionary<string, object> values)
    {
        if (values.ContainsKey("level"))
        {
            level = (int)values["level"];
            xp = (float)values["xp"];
            requiredXp = (float)values["requiredXp"];
        }

        if (values.ContainsKey("health"))
        {
            health = (float)values["health"];
            hasRegenAbility = (bool)values["hasRegenAbility"];
            hasHealthUpAbility = (bool)values["hasHealthUpAbility"];
        }

        if (values.ContainsKey("hasDashAbility"))
        {
            hasSpeedUpAbility = (bool)values["hasSpeedUpAbility"];
            hasDashAbility = (bool)values["hasDashAbility"];
        }

        if (values.ContainsKey("hasDamageUpAbility"))
        {
            hasDamageUpAbility = (bool)values["hasDamageUpAbility"];
        }

        currentScene = (string)values["currentScene"];
    }
}