using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private TransitionTwo transitionTwo;
    [SerializeField] private TransitionThree transitionThree;

    private bool skipTextAnimation = false;

    void Start()
    {
        startGameButton.SetActive(false);
        quitGameButton.SetActive(false);
        saveGameButton.SetActive(false);
        gameOverText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(ShowText());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            letterDelay /= 2;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            letterDelay *= 2;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            skipTextAnimation = true;
        }
    }

    IEnumerator ShowText()
    {
        string originalText = gameOverText.text;
        gameOverText.text = "";

        for (int i = 0; i < originalText.Length; i++)
        {
            if (skipTextAnimation)
            {
                gameOverText.text = originalText;
                break;
            }

            gameOverText.text += originalText[i];
            yield return new WaitForSeconds(letterDelay);
        }  

        // Show buttons
        startGameButton.SetActive(true);
        quitGameButton.SetActive(true);
        saveGameButton.SetActive(true);

        // Call OnTextAnimationComplete directly
        if (transitionOne != null) 
        {
            transitionOne.OnTextAnimationComplete();
        }

        if (transitionTwo != null)
        {
            transitionTwo.OnTextAnimationComplete();
        }

        if (transitionThree != null)
        {
            transitionThree.OnTextAnimationComplete();
        }
    }
}