using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenLogic : MonoBehaviour
{
    // Method to handle title screen logic based on loaded data
    public static void HandleTitleScreen()
    {
        // Load player data
        //Dictionary<string, object> loadedData = SaveSystem.LoadPlayer();
        GiveValues.instance?.GetValues();

        //if (loadedData != null)
        //{
        //    // You can now use the loaded data to perform logic on the title screen
        //    Debug.Log("Title Screen Logic based on loaded data:");
        //    Debug.Log("Level: " + loadedData["level"]);
        //    Debug.Log("Health: " + loadedData["health"]);
        //    Debug.Log("XP: " + loadedData["xp"]);
        //
        //    // Example: If the player has a certain level or condition, show a special message or option on the title screen
        //    if ((int)loadedData["level"] >= 10)
        //    {
        //        Debug.Log("Special message or option for players with level 10 or higher");
        //    }
        //}
        //else
        //{
        //    // If no data is found, you can handle this case as needed
        //    Debug.Log("No saved data found. Handle this case on the title screen.");
        //}
    }
}