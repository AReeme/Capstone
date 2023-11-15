using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTime : MonoBehaviour
{
    public event EventHandler<TimeSpan> worldTimeChanged;

    [SerializeField]
    private float dayLength;
    private TimeSpan currentTime;
    private float minuteLength => dayLength / WorldTimeConstants.mintuesInDay;

    private void Start()
    {
        currentTime = new TimeSpan(6, 0, 0);
        StartCoroutine(AddMinute());
    }

    private IEnumerator AddMinute()
    {
        currentTime += TimeSpan.FromMinutes(1);
        worldTimeChanged?.Invoke(this, currentTime);
        yield return new WaitForSeconds(minuteLength);
        StartCoroutine(AddMinute());
    }
}
