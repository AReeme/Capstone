using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StopwatchTimer : MonoBehaviour
{
    public TMP_Text timerText;
    private float elapsedTime;
    private bool isRunning = false;

    void Start()
    {
        elapsedTime = 0f;
        UpdateTimerText();
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        timerText.text = FormatTime(elapsedTime);
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
