using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [System.Serializable]
    public class Stat
    {
        public string statName;
        public int level = 1;
        public int baseCost = 100;

        public int GetLevelUpCost()
        {
            return baseCost + (level - 1) * 50;
        }
    }

    public Stat strength = new Stat { statName = "Strength" };
    public Stat dexterity = new Stat { statName = "Dexterity" };
    public Stat endurance = new Stat { statName = "Endurance" };
    public Stat vitality = new Stat { statName = "Vitality" };
    public Stat intelligence = new Stat { statName = "Intelligence" };

    public delegate void StatsChanged();
    public event StatsChanged OnStatsChanged;

    public int totalLevel = 1;
    public int souls = 1000;

   
    

    private void Awake()
    {
        // Singleton pattern to persist between scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool TryLevelUp(Stat stat)
    {
        int cost = stat.GetLevelUpCost();
        if (souls >= cost)
        {
            souls -= cost;
            stat.level++;
            totalLevel++;
            OnStatsChanged?.Invoke();
            return true;
        }
        return false;
    }

    public void NotifyStatsChanged()
    {
        if (OnStatsChanged != null)
            OnStatsChanged.Invoke();
    }
}
