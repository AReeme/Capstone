using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchManager : MonoBehaviour
{
    [SerializeField] GiveValues gv;

    public void SwitchToStoryScene()
    {
        gv.level = 1;
        gv.health = 100;
        gv.MAX_HELATH = 100;
        gv.xp = 0;
        gv.requiredXp = 83;
        gv.dash = false;
        gv.regen = false;
        gv.healthUp = false;
        gv.speedUp = false;
        gv.damageUp = false;
        gv.enemiesKilled = 0;
        gv.damageTaken = 0;
        gv.timeSurvived = 0;
        SceneManager.LoadScene("Story");
    }

    public void SwitchToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        try
        {
            Application.Quit();
        }
        catch (Exception e)
        {
            Debug.LogError("Error quitting game: " + e.Message);
        }
    }

    public void SwitchToMainMenu()
    {
        SceneManager.LoadScene("Title Screen");
    }

    public void SwitchToFirstLevel()
    {
        SceneManager.LoadScene("Game Level 1");
    }

    public void SwitchToSecondLevel()
    {
        SceneManager.LoadScene("Game Level 2");
    }

    public void SwitchToThirdLevel()
    {
        SceneManager.LoadScene("Game Level 3");
    }

    public void SwitchToFinalLevel()
    {
        SceneManager.LoadScene("Game Level 4");
    }
}
