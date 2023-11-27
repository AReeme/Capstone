using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSystem : MonoBehaviour
{
    public AttackArea attackArea;
    public AbilityMenu abilityMenu;

    public int level;
    public float currentXP;
    public float requiredXP;

    private float lerpTimer;
    private float delayTimer;

    [Header("UI")]
    public Image frontXPBar;
    public Image backXPBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;
    [Header("Multipliers")]
    [Range(1f, 300f)]
    public float additionMultiplier = 300;
    [Range(2f, 4f)]
    public float powerMultiplier = 2;
    [Range(7f, 14f)]
    public float divisionMultiplier = 7;

    void Start()
    {
        level = (int)GiveValues.instance?.level;
        currentXP = (float)GiveValues.instance?.xp;
        requiredXP = (float)GiveValues.instance?.requiredXp;
        frontXPBar.fillAmount = currentXP / requiredXP;
        backXPBar.fillAmount = currentXP / requiredXP;

        requiredXP = CalculateRequiredXP();

        levelText.text = "Level " + level;
    }

    void Update()
    {
        UpdateXPUI();
        if(Input.GetKeyDown(KeyCode.Equals))
        {
            GainExperienceFlatRate(120);
        }

        if (currentXP > requiredXP)
        {
            LevelUp();
        }
    }

    public void UpdateXPUI()
    {
        float xpFraction = currentXP / requiredXP;
        float FXP = frontXPBar.fillAmount;

        if (FXP < xpFraction)
        {
            delayTimer += Time.deltaTime;
            backXPBar.fillAmount = xpFraction;
            if (delayTimer > 3)
            {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / 4;
                frontXPBar.fillAmount = Mathf.Lerp(FXP, backXPBar.fillAmount, percentComplete);
            }
        }
        xpText.text = currentXP + "/" + requiredXP;
    }

    public void GainExperienceFlatRate(float xpGained)
    {
        currentXP += xpGained;
        lerpTimer = 0;
        delayTimer = 0;
    }

    public void GainExperienceScalable(float xpGained, int passedLevel)
    {
        if(passedLevel < level)
        {
            float multiplier = 1 + (level - passedLevel) * 0.01f;
            currentXP += xpGained * multiplier;
        }
        else
        {
            currentXP += xpGained;
        }
        lerpTimer = 0;
        delayTimer = 0;
    }

    public void LevelUp()
    {
        level++;
        frontXPBar.fillAmount = 0;
        backXPBar.fillAmount = 0;
        currentXP = Mathf.RoundToInt(currentXP - requiredXP);
        GetComponent<Health>().IncreaseHealth(level);
        attackArea.GetComponent<AttackArea>().IncreaseDamage(level);
        requiredXP = CalculateRequiredXP();
        levelText.text = "Level " + level;

        if (level % 5 == 0) 
        {
            abilityMenu.ShowAbilityMenu();
        }

        if (level >= 25)
        {
            abilityMenu.enabled = false;
        }
    }

    private int CalculateRequiredXP()
    {
        int solveForRequiredXP = 0;
        for (int levelCycle = 1; levelCycle <= level; levelCycle++) 
        {
            solveForRequiredXP += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredXP / 4;
    }
}
