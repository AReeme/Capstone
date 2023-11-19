using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverTextEffect : MonoBehaviour
{
    public float letterDelay = 0.1f;
    private TextMeshProUGUI gameOverText;
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject quitGameButton;

    void Start()
    {
        startGameButton.SetActive(false);
        quitGameButton.SetActive(false);
        gameOverText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        string originalText = gameOverText.text;
        gameOverText.text = "";

        for (int i = 0; i < originalText.Length; i++)
        {
            gameOverText.text += originalText[i];
            yield return new WaitForSeconds(letterDelay);
        }

        startGameButton.SetActive(true);
        quitGameButton.SetActive(true);
    }
}
