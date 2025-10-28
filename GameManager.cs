using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public PlayerStats stats;

    void Start()
    {
        if (GameLoadHandler.loadedData != null)
        {
            LoadFromData(GameLoadHandler.loadedData);
            GameLoadHandler.loadedData = null;
        }
    }

    public void SaveGame()
    {
        GameData data = new GameData();

        data.playerHealth = 100; // przyk³adowa wartoœæ
        data.playerPosition = new float[]
        {
            player.transform.position.x,
            player.transform.position.y,
            player.transform.position.z
        };

        data.totalLevel = stats.totalLevel;
        data.souls = stats.souls;

        data.strength = ToStatData(stats.strength);
        data.dexterity = ToStatData(stats.dexterity);
        data.endurance = ToStatData(stats.endurance);
        data.vitality = ToStatData(stats.vitality);
        data.intelligence = ToStatData(stats.intelligence);

        if (CheckpointManager.Instance != null && CheckpointManager.Instance.currentCheckpoint != null)
        {
            data.currentCheckpointName = CheckpointManager.Instance.currentCheckpoint.gameObject.name;
        }

        data.currentSceneName = SceneManager.GetActiveScene().name;

        SaveSystem.Save(data);
    }

    public void LoadFromData(GameData data)
    {
        player.transform.position = new Vector3(
            data.playerPosition[0],
            data.playerPosition[1],
            data.playerPosition[2]
        );

        stats.totalLevel = data.totalLevel;
        stats.souls = data.souls;

        FromStatData(stats.strength, data.strength);
        FromStatData(stats.dexterity, data.dexterity);
        FromStatData(stats.endurance, data.endurance);
        FromStatData(stats.vitality, data.vitality);
        FromStatData(stats.intelligence, data.intelligence);
        stats.NotifyStatsChanged();

        if (!string.IsNullOrEmpty(data.currentCheckpointName))
        {
            GameObject checkpointObj = GameObject.Find(data.currentCheckpointName);
            if (checkpointObj != null)
            {
                Checkpoint checkpoint = checkpointObj.GetComponent<Checkpoint>();
                if (checkpoint != null)
                {
                    CheckpointManager.Instance.SetCurrentCheckpoint(checkpoint);
                }
            }
        }
    }

    private StatData ToStatData(PlayerStats.Stat stat)
    {
        return new StatData
        {
            statName = stat.statName,
            level = stat.level,
            baseCost = stat.baseCost
        };
    }

    private void FromStatData(PlayerStats.Stat stat, StatData data)
    {
        stat.statName = data.statName;
        stat.level = data.level;
        stat.baseCost = data.baseCost;
    }
}
