using TMPro;
using UnityEngine;

public class TransitionThree : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelComplete;
    [SerializeField] SavingManager sm;

    // Called when the text animation is complete
    public void OnTextAnimationComplete()
    {
        // Retrieve the latest level value from SavingManager
        int latestLevel = sm.level;
        int enemiesKilled = sm.enemiesKilled;
        float damageTaken = sm.damageTaken;
        float timeSurvived = sm.timeSurvived;

        // Update the text with the latest level value
        levelComplete.text = "Level Complete!\n\n" +
            "Stats:\r\nTotal Time Survived: " + Mathf.RoundToInt(timeSurvived) + " Seconds" +
            "\r\nCurrent Level: " + latestLevel + 
            "\r\nEnemies Killed: " + enemiesKilled + 
            "\r\nDamage Taken: " + damageTaken + 
            "\r\n\r\nContinue?\r\n\r\nTip: Good Luck";
    }
}