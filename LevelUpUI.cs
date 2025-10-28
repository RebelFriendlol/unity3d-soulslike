using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    [Header("Player Info")]
    public Text playerSoulsText;

    [Header("UI Panels")]
    public GameObject levelUpPanel;

    [Header("Stat Upgrade Buttons")]
    public Button strengthButton;
    public Button enduranceButton;
    public Button vitalityButton;

    [Header("Cost Texts")]
    public Text strengthCostText;
    public Text enduranceCostText;
    public Text vitalityCostText;

    [Header("Other")]
    public Button closeButton;
    public PlayerStats playerStats;

    void Start()
    {
        levelUpPanel.SetActive(false);

        if (strengthButton != null) strengthButton.onClick.AddListener(() => TryLevelUpStat("Strength"));
        if (enduranceButton != null) enduranceButton.onClick.AddListener(() => TryLevelUpStat("Endurance"));
        if (vitalityButton != null) vitalityButton.onClick.AddListener(() => TryLevelUpStat("Vitality"));
        if (closeButton != null) closeButton.onClick.AddListener(HideLevelUpPanel);

        if (playerStats == null)
        {
            Debug.LogError("LevelUpUI: playerStats NIE przypisany w Inspectorze!");
        }
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (playerStats == null) return;

        strengthCostText.text = $"Upgrade: {playerStats.strength.level}   Cost: {playerStats.strength.GetLevelUpCost()} souls";
        enduranceCostText.text = $"Upgrade: {playerStats.endurance.level}   Cost: {playerStats.endurance.GetLevelUpCost()} souls";
        vitalityCostText.text = $"Upgrade: {playerStats.vitality.level}   Cost: {playerStats.vitality.GetLevelUpCost()} souls";

        playerSoulsText.text = "Souls: " + playerStats.souls;
    }

    public void ShowLevelUpPanel()
    {
        levelUpPanel.SetActive(true);
        UpdateUI();
    }

    public void HideLevelUpPanel()
    {
        levelUpPanel.SetActive(false);
    }

    void TryLevelUpStat(string statName)
    {
        if (playerStats == null)
        {
            Debug.LogError("LevelUpUI: playerStats NIE przypisany – nie mo¿na levelowaæ!");
            return;
        }

        PlayerStats.Stat statToLevel = null;

        switch (statName)
        {
            case "Strength": statToLevel = playerStats.strength; break;
            case "Endurance": statToLevel = playerStats.endurance; break;
            case "Vitality": statToLevel = playerStats.vitality; break;
            default:
                Debug.LogError("Nieznana statystyka: " + statName);
                return;
        }

        if (statToLevel != null)
        {
            bool success = playerStats.TryLevelUp(statToLevel);
            if (success)
            {
                Debug.Log($"Levelled up {statToLevel.statName} to level {statToLevel.level}");
                UpdateUI();
            }
            else
            {
                Debug.Log($"Not enough souls to level up {statToLevel.statName}");
            }
        }
    }
}
