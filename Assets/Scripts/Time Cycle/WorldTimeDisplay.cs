using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class WorldTimeDisplay : MonoBehaviour
{
    [SerializeField]
    private WorldTime worldTime;
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        worldTime.worldTimeChanged += OnWorldTimeChanged;
    }

    private void OnDestroy()
    {
        worldTime.worldTimeChanged -= OnWorldTimeChanged;
    }

    private void OnWorldTimeChanged(object sender, TimeSpan newTime)
    {
        text.SetText(newTime.ToString(@"hh\:mm"));
    }
}
