using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonShakeEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float shakeAmount = 1.0f;
    public float shakeDuration = 1.0f;

    private Vector3 originalPosition;
    private bool isMouseOver = false;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (isMouseOver)
        {
            // If the mouse is over the button, continue the shake effect
            StartCoroutine(Shake());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Mouse hover: start the shake effect
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Mouse exit: stop the shake effect
        isMouseOver = false;
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