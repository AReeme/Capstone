using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GiveValues : MonoBehaviour
{
    public int level = 1;
    public float health = 100;
    public float MAX_HELATH = 100;
    public float xp = 0;
    public float requiredXp = 83;
    public bool dash = false;
    public bool regen = false;
    public bool healthUp = false;
    public bool speedUp = false;
    public bool damageUp = false;
    public int enemiesKilled = 0;
    public float damageTaken = 0;
    public float timeSurvived = 0;

    public static GiveValues instance;

    private void Awake()
    {
        // Check if an instance already exists
        if (instance != null)
        {
            // Destroy the new instance since there can only be one persistent instance
            Destroy(gameObject);
            return;
        }

        // Make this object persistent across scene changes
        DontDestroyOnLoad(gameObject);

        // Set the instance to this object
        instance = this;
    }

    public void GetValues()
    {
        // Load player data if available
        Dictionary<string, object> loadedData = SaveSystem.LoadPlayer();
        if (loadedData != null)
        {
            // Debug logs for loaded data
            Debug.Log("Loaded player data:");
            foreach (var kvp in loadedData)
            {
                Debug.Log($"{kvp.Key}: {kvp.Value}");
            }

            // Set values from loaded data
            level = (int)loadedData["level"];
            health = (float)loadedData["health"];
            MAX_HELATH = (float)loadedData["maxhealth"];
            xp = (float)loadedData["xp"];
            requiredXp = (float)loadedData["requiredXp"];
            dash = (bool)loadedData["hasDashAbility"];
            regen = (bool)loadedData["hasRegenAbility"];
            healthUp = (bool)loadedData["hasHealthUpAbility"];
            speedUp = (bool)loadedData["hasSpeedUpAbility"];
            damageUp = (bool)loadedData["hasDamageUpAbility"];
            enemiesKilled = (int)loadedData["enemiesKilled"];
            damageTaken = (float)loadedData["damageTaken"];
            timeSurvived = (float)loadedData["timeSurvived"];

            // Use SceneManager to change the scene
            SceneManager.LoadScene((string)loadedData["currentScene"]);
        }

        //// Debug logs for final values
        //Debug.Log("Final values after initialization:");
        //Debug.Log($"Level: {level}");
        //Debug.Log($"Health: {health}");
        //Debug.Log($"XP: {xp}");
        //Debug.Log($"Dash: {dash}");
        //Debug.Log($"Regen: {regen}");
        //Debug.Log($"HealthUp: {healthUp}");
        //Debug.Log($"SpeedUp: {speedUp}");
        //Debug.Log($"DamageUp: {damageUp}");
    }
}