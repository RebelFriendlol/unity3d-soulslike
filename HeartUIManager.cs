using UnityEngine;
using System.Collections.Generic;

public class HeartUIManager : MonoBehaviour
{
    public GameObject heartPrefab; // Prefab serca (Sprite/Image)
    public Transform heartContainer; // Obiekt UI (Empty z Horizontal Layout Group)
    public PlayerStats playerStats;

    private List<GameObject> heartIcons = new List<GameObject>();

    void Start()
    {
        playerStats.OnStatsChanged += UpdateHearts;
        UpdateHearts();
    }

    public void UpdateHearts()
    {
        foreach (var heart in heartIcons)
        {
            Destroy(heart);
        }
        heartIcons.Clear();

        int vitalityLevel = playerStats.vitality.level;
        int heartCount = Mathf.Min(vitalityLevel / 3, 10);

        for (int i = 0; i < heartCount; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartContainer);
            heartIcons.Add(newHeart);
        }
    }
}
