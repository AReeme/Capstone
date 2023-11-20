using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    [Header("Damage Up Ability")]
    public bool hasDamageUpAbility;
    public Image damageUpIcon;

    [Header("Health Up Ability")]
    public bool hasHealthUpAbility;
    public Image healthUpIcon;

    [Header("Regen Ability")]
    public bool hasRegenAbility;
    public Image regenIcon;

    [Header("Dash Ability")]
    public bool hasDashAbility;
    public Image dashIcon;

    [Header("Speed Up Ability")]
    public bool hasSpeedUpAbility;
    public Image speedUpIcon;

    public static AbilityManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

}
