using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class WorldLight : MonoBehaviour
{
    private Light2D _light;

    [SerializeField]
    private WorldTime worldTime;
    [SerializeField]
    private Gradient gradient;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
        worldTime.worldTimeChanged += OnWorldTimeChanged;
    }

    private void OnDestroy()
    {
        worldTime.worldTimeChanged -= OnWorldTimeChanged;
    }

    private void OnWorldTimeChanged(object sender, TimeSpan newTime)
    {
        _light.color = gradient.Evaluate(PercentOfDay(newTime));
    }

    private float PercentOfDay(TimeSpan timeSpan)
    {
        return (float) timeSpan.TotalMinutes % WorldTimeConstants.mintuesInDay / WorldTimeConstants.mintuesInDay;
    }
}
