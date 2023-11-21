using System.Collections;
using TMPro;
using UnityEngine;

public class GameOverTextEffect : MonoBehaviour
{
    public float letterDelay = 0.1f;
    private TextMeshProUGUI gameOverText;
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject quitGameButton;
    [SerializeField] private GameObject saveGameButton;

    // Reference to the TransitionOne script
    [SerializeField] private TransitionOne transitionOne;

    void Start()
    {
        startGameButton.SetActive(false);
        quitGameButton.SetActive(false);
        saveGameButton.SetActive(false);
        gameOverText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        string originalText = gameOverText.text;
        gameOverText.text = "";

#if DEBUG
        yield return null;
#else
    for (int i = 0; i < originalText.Length; i++)
    {
        gameOverText.text += originalText[i];
        yield return new WaitForSeconds(letterDelay);
    }  
#endif

        // Show buttons
        startGameButton.SetActive(true);
        quitGameButton.SetActive(true);
        saveGameButton.SetActive(true);

        // Call OnTextAnimationComplete directly
        transitionOne.OnTextAnimationComplete();
    }
}