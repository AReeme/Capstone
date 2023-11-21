using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavingManager : MonoBehaviour
{
    public int level = 1;
    public float health = 100;
    public float MAX_HELATH = 100;
    public float xp;
    public float requiredXp = 83;
    public bool hasDashAbility;
    public bool hasRegenAbility;
    public bool hasHealthUpAbility;
    public bool hasDamageUpAbility;
    public bool hasSpeedUpAbility;
    public string currentScene;

    GiveValues gv;

    private void Start()
    {
        gv = GiveValues.instance;
        level = gv.level;
        health = gv.health;
        MAX_HELATH = gv.MAX_HELATH;
        xp = gv.xp;
        requiredXp = gv.requiredXp;
        hasDashAbility = gv.dash;
        hasRegenAbility = gv.regen;
        hasHealthUpAbility = gv.healthUp;
        hasDamageUpAbility = gv.damageUp;
        hasSpeedUpAbility = gv.speedUp;
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void SavePlayer()
    {
        Dictionary<string, object> values = new Dictionary<string, object>
        {
            {"level", GiveValues.instance.level},
            {"health",GiveValues.instance.health},
            {"maxhealth", GiveValues.instance.MAX_HELATH},
            {"xp", GiveValues.instance.xp},
            {"requiredXp", GiveValues.instance.requiredXp},
            {"hasDashAbility", GiveValues.instance.dash},
            {"hasRegenAbility", GiveValues.instance.regen},
            {"hasHealthUpAbility", GiveValues.instance.healthUp},
            {"hasDamageUpAbility", GiveValues.instance.damageUp},
            {"hasSpeedUpAbility", GiveValues.instance.speedUp},
        };

        SaveSystem.SavePlayer(values, currentScene);
    }

    public void LoadPlayer()
    {
        Dictionary<string, object> loadedData = SaveSystem.LoadPlayer();

        if (loadedData != null)
        {
            // Assign loaded data to references
            //level = (int)loadedData["level"];
            //health = (float)loadedData["health"];
            //xp = (float)loadedData["xp"];
            //requiredXp = (float)loadedData["requiredXp"];
            //hasDashAbility = (bool)loadedData["hasDashAbility"];
            //hasRegenAbility = (bool)loadedData["hasRegenAbility"];
            //hasHealthUpAbility = (bool)loadedData["hasHealthUpAbility"];
            //hasSpeedUpAbility = (bool)loadedData["hasSpeedUpAbility"];
            //hasDamageUpAbility = (bool)loadedData["hasDamageUpAbility"];

            GiveValues.instance.level = (int)loadedData["level"];
            GiveValues.instance.health = (float)loadedData["health"];
            GiveValues.instance.MAX_HELATH = (float)loadedData["maxhealth"];
            GiveValues.instance.xp = (float)loadedData["xp"];
            GiveValues.instance.requiredXp = (float)loadedData["requiredXp"];
            GiveValues.instance.dash = (bool)loadedData["hasDashAbility"];
            GiveValues.instance.regen = (bool)loadedData["hasRegenAbility"];
            GiveValues.instance.healthUp = (bool)loadedData["hasHealthUpAbility"];
            GiveValues.instance.speedUp = (bool)loadedData["hasSpeedUpAbility"];
            GiveValues.instance.damageUp = (bool)loadedData["hasDamageUpAbility"];

            // Assign the current scene
            currentScene = (string)loadedData["currentScene"];

            // Use SceneManager to change the scene
            SceneManager.LoadScene(currentScene);

            // Debug logs
            Debug.Log("Loaded player data:");
            Debug.Log("Level: " + level);
            Debug.Log("Health: " + health);
            Debug.Log("XP: " + xp);
            Debug.Log("Current Scene: " + currentScene);
        }
    }
}