using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public GameObject fullDashIconPrefab;
    public GameObject emptyDashIconPrefab;
    public Transform dashIconContainer;
    public float dashCooldown = 2f;

    private List<Image> dashIcons = new List<Image>();
    private List<bool> isDashReady = new List<bool>();

    void Start()
    {
        GenerateDashIcons();
        playerStats.OnStatsChanged += RegenerateDashIcons;
    }

    void GenerateDashIcons()
    {
        foreach (Transform child in dashIconContainer)
            Destroy(child.gameObject);
        dashIcons.Clear();
        isDashReady.Clear();

        int initialDashes = Mathf.Min(playerStats.endurance.level / 3, 10);

        for (int i = 0; i < initialDashes; i++)
        {
            GameObject icon = Instantiate(fullDashIconPrefab, dashIconContainer);
            dashIcons.Add(icon.GetComponent<Image>());
            isDashReady.Add(true);
        }
    }

    void RegenerateDashIcons()
    {
        GenerateDashIcons();
    }

    public bool TryUseDash()
    {
        for (int i = 0; i < isDashReady.Count; i++)
        {
            if (isDashReady[i])
            {
                UseDash(i);
                return true;
            }
        }

        Debug.Log("Brak dostêpnych dashy!");
        return false;
    }

    void UseDash(int index)
    {
        isDashReady[index] = false;
        dashIcons[index].sprite = emptyDashIconPrefab.GetComponent<Image>().sprite;
        StartCoroutine(RestoreDash(index));
    }

    IEnumerator RestoreDash(int index)
    {
        yield return new WaitForSeconds(dashCooldown);
        isDashReady[index] = true;
        dashIcons[index].sprite = fullDashIconPrefab.GetComponent<Image>().sprite;
    }

    public bool TryAddExtraDash()
    {
        // Zwiêksz Endurance o 3 = +1 dash
        playerStats.endurance.level += 3;
        playerStats.NotifyStatsChanged(); // Uaktualnij UI
        return true;
    }
}
