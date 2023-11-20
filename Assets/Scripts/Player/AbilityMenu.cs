using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityMenu : MonoBehaviour
{
    public GameObject abilityMenu;
    public LevelSystem levelSystem;
    public PlayerMovement playerMovement;
    public Health playerHealth;
    public AttackArea playerAttack;

    public TextMeshProUGUI abilityInfoText;
    public Button[] abilityButtons; // Assign these buttons in the Unity Inspector

    private bool isMenuActive = false;

    private void Start()
    {
        abilityMenu.SetActive(false);
        UpdateAbilityButtonsInteractivity();
    }

    private void Update()
    {
        // Check if the level is a multiple of 5, the menu is not already active, and the player has not selected 5 abilities
        if (levelSystem.level % 5 == 0 && !isMenuActive && playerMovement.AbilitiesSelected < 5)
        {
            ShowAbilityMenu();
        }
    }

    public void ShowAbilityMenu()
    {
        Time.timeScale = 0f; // Pause the game
        abilityMenu.SetActive(true);
        isMenuActive = true;
    }

    private void HideAbilityMenu()
    {
        Time.timeScale = 1f; // Resume the game
        abilityMenu.SetActive(false);
        isMenuActive = false;
    }

    public void ChooseAbility(int abilityIndex)
    {
        // Check if the player already has the ability and if the maximum number of abilities has not been reached
        if (!CheckPlayerHasAbility(abilityIndex) && playerMovement.AbilitiesSelected < 5)
        {
            // Apply the chosen ability based on the index
            switch (abilityIndex)
            {
                case 0:
                    if (!playerMovement.hasDashAbility)
                    {
                        playerMovement.ActivateDashAbility();
                        playerMovement.AbilitiesSelected++;
                        HideAbilityMenu();
                    }
                    break;
                case 1:
                    if (!playerHealth.hasRegenAbility)
                    {
                        playerHealth.ActivateRegenAbility();
                        playerMovement.AbilitiesSelected++;
                        HideAbilityMenu();
                    }
                    break;
                case 2:
                    if (!playerHealth.hasHealthUpAbility)
                    {
                        playerHealth.ActivateHealthUpAbility();
                        playerMovement.AbilitiesSelected++;
                        HideAbilityMenu();
                    }
                    break;
                case 3:
                    if (!playerMovement.hasSpeedAbility)
                    {
                        playerMovement.ActivateSpeedAbility();
                        playerMovement.AbilitiesSelected++;
                        HideAbilityMenu();
                    }
                    break;
                case 4:
                    if (!playerAttack.hasDamageUpAbility)
                    {
                        playerAttack.ActivateDamageUpAbility();
                        playerMovement.AbilitiesSelected++;
                        HideAbilityMenu();
                    }
                    break;

                default:
                    break;
            }

            // Update the interactability of buttons after choosing an ability
            UpdateAbilityButtonsInteractivity();
        }
    }

    private bool CheckPlayerHasAbility(int abilityIndex)
    {
        // Implement logic to check if the player has the corresponding ability
        switch (abilityIndex)
        {
            case 0:
                return playerMovement.hasDashAbility;
            case 1:
                return playerHealth.hasRegenAbility;
            case 2:
                return playerHealth.hasHealthUpAbility;
            case 3:
                return playerMovement.hasSpeedAbility;
            case 4:
                return playerAttack.hasDamageUpAbility;

            default:
                return false;
        }
    }

    public void OnPointerEnterAbilityButton(int abilityIndex)
    {
        string title = "";
        string description = "";

        // Determine the title and description based on the abilityIndex
        switch (abilityIndex)
        {
            case 0:
                title = "Dash Ability";
                description = "Allows you to dash in all eight directions. Press E to Dash.";
                break;
            case 1:
                title = "Regen Ability";
                description = "Allows you to regenerate 10 HP every 10 seconds.";
                break;
            case 2:
                title = "Health Up Ability";
                description = "Increases Health By 50.";
                break;
            case 3:
                title = "Speed Up Ability";
                description = "Increases Your Movement Speed.";
                break;
            case 4:
                title = "Damage Up Ability";
                description = "Increases Your Damage.";
                break;

            default:
                break;
        }

        // Update the ability information text
        UpdateAbilityInfoText(title, description);
    }

    public void OnPointerExitAbilityButton()
    {
        // Clear the ability information text when the mouse exits the button
        UpdateAbilityInfoText("", "");
    }

    private void UpdateAbilityInfoText(string title, string description)
    {
        if (abilityInfoText != null)
        {
            abilityInfoText.text = $"{title}\n{description}";
        }
    }

    private void UpdateAbilityButtonsInteractivity()
    {
        // Assuming abilityButtons array is properly set up in the Inspector
        for (int i = 0; i < abilityButtons.Length; i++)
        {
            bool playerHasAbility = CheckPlayerHasAbility(i);

            // Set the interactability of the button based on whether the player has the ability or not
            abilityButtons[i].interactable = !playerHasAbility;
        }
    }
}