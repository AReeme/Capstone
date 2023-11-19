using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchManager : MonoBehaviour
{
    public void SwitchToStoryScene()
    {
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
}
