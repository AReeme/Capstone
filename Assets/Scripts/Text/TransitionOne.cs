using TMPro;
using UnityEngine;

public class TransitionOne : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelComplete;
    [SerializeField] SavingManager sm;

    // Called when the text animation is complete
    public void OnTextAnimationComplete()
    {
        // Retrieve the latest level value from SavingManager
        int latestLevel = sm.level;
        Debug.Log(latestLevel);

        // Update the text with the latest level value
        levelComplete.text = "Level Complete!\n\n" +
            "Stats:\r\nTotal Time Survived: \r\nCurrent Level: " + latestLevel + "\r\nEnemies Killed: \r\nDamage Taken: \r\n\r\nContinue?\r\n\r\nTip: Acid Does 10 Damage, Be Wary";
    }
}