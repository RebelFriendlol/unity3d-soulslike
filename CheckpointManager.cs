using UnityEngine;
using UnityEngine.UI;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    public Checkpoint currentCheckpoint;

    [Header("UI")]
    public Text checkpointPromptText;
    public GameObject checkpointWindow;  // Okno do wyœwietlania opcji

    [Header("Spawn Points")]
    public Transform defaultSpawnPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Ustawienie okna na nieaktywne na pocz¹tku gry
        if (checkpointWindow != null)
        {
            checkpointWindow.SetActive(false);
        }
    }

    public void SetCurrentCheckpoint(Checkpoint checkpoint)
    {
        if (currentCheckpoint != null)
            currentCheckpoint.SetAsCurrent(false);

        currentCheckpoint = checkpoint;
        currentCheckpoint.SetAsCurrent(true);
    }

    public Transform GetCurrentSpawnPoint()
    {
        if (currentCheckpoint != null)
        {
            return currentCheckpoint.GetSpawnPoint();  // aktywny checkpoint
        }
        else if (defaultSpawnPoint != null)
        {
            return defaultSpawnPoint;  // jeœli brak aktywnego, wróæ do defaulta
        }
        else
        {
            Debug.LogWarning("Brak punktu respawnu!");
            return null;
        }
    }

    public bool HasActivatedCheckpoint()
    {
        return currentCheckpoint != null;
    }

    public void RespawnPlayer(Transform player)
    {
        Transform spawnPoint = GetCurrentSpawnPoint();

        if (spawnPoint != null)
        {
            player.position = spawnPoint.position;
        }
        else
        {
            Debug.LogWarning("Nie mo¿na zrespawnowaæ gracza — brak punktu!");
        }
    }

    public void ShowPrompt(string message)
    {
        if (checkpointPromptText != null)
        {
            checkpointPromptText.text = message;
            checkpointPromptText.enabled = true;
        }
    }

    public void HidePrompt()
    {
        if (checkpointPromptText != null)
        {
            checkpointPromptText.enabled = false;
        }
    }

    // Metody do wyœwietlania i ukrywania okna
    public void ShowCheckpointWindow()
    {
        if (checkpointWindow != null)
        {
            checkpointWindow.SetActive(true);
            PlayerController.Instance.SetMovementAllowed(false);  // Zablokuj ruch
        }
    }

    public void HideCheckpointWindow()
    {
        if (checkpointWindow != null)
        {
            checkpointWindow.SetActive(false);
            PlayerController.Instance.SetMovementAllowed(true);  // Odblokuj ruch
        }
    }
}
