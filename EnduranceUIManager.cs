using UnityEngine;
using System.Collections.Generic;

public class EnduranceUIManager : MonoBehaviour
{
    public GameObject enduranceIconPrefab; // Prefab ikonki (np. tarcza, zbroja itp.)
    public Transform enduranceContainer;   // Obiekt UI (Empty z Horizontal Layout Group)
    public PlayerStats playerStats;

    private List<GameObject> enduranceIcons = new List<GameObject>();

    void Start()
    {
        playerStats.OnStatsChanged += UpdateIcons;
        UpdateIcons();
    }

    public void UpdateIcons()
    {
        foreach (var icon in enduranceIcons)
        {
            Destroy(icon);
        }
        enduranceIcons.Clear();

        int endLevel = playerStats.endurance.level;
        int iconCount = Mathf.Min(endLevel / 3, 10);

        for (int i = 0; i < iconCount; i++)
        {
            GameObject newIcon = Instantiate(enduranceIconPrefab, enduranceContainer);
            enduranceIcons.Add(newIcon);
        }
    }
}
