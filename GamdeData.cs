[System.Serializable]
public class StatData
{
    public string statName;
    public int level;
    public int baseCost;
}

[System.Serializable]
public class GameData
{
    public int playerHealth;
    public float[] playerPosition;

    public int totalLevel;
    public int souls;

    public StatData strength;
    public StatData dexterity;
    public StatData endurance;
    public StatData vitality;
    public StatData intelligence;

    public string currentCheckpointName;
    public string currentSceneName; // <- nowo dodane
}
