using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    private void Start()
    {
        if (CheckpointManager.Instance != null)
        {
            Transform spawnPoint = CheckpointManager.Instance.GetCurrentSpawnPoint();

            if (spawnPoint != null)
            {
                transform.position = spawnPoint.position;
                Debug.Log("Gracz ustawiony na starcie.");
            }
            else
            {
                Debug.LogWarning("Brak punktu startowego!");
            }
        }
        else
        {
            Debug.LogWarning("Brak odniesienia do CheckpointManager!");
        }
    }
}
