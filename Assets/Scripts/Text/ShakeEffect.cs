using System.Collections;
using TMPro;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    public float shakeAmount = 1.0f;
    public float shakeDuration = 1.0f;

    private TMP_Text textComponent;
    private Vector3 originalPosition;

    private void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        originalPosition = transform.position;
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            transform.position = originalPosition + Random.insideUnitSphere * shakeAmount;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset to the original position
        transform.position = originalPosition;
    }
}