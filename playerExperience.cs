using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    public Text experienceText;
    public PlayerStats playerStats;  // Reference to PlayerStats

    private void Start()
    {
        if (playerStats == null)
        {
            Debug.LogError("PlayerExperience: PlayerStats not assigned!");
            return;
        }

        playerStats.OnStatsChanged += UpdateUI;
        UpdateUI();
    }

    private void Update()
    {
        // Press Z to simulate collecting souls
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AddSouls(50);
        }
    }

    public void AddSouls(int amount)
    {
        if (playerStats != null)
        {
            playerStats.souls += amount;
            playerStats.NotifyStatsChanged();  // Trigger UI update
        }
    }

    private void UpdateUI()
    {
        if (experienceText != null && playerStats != null)
        {
            experienceText.text = $"Souls: {playerStats.souls}";
        }
    }
}
