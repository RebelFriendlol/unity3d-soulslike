using UnityEngine;
using UnityEngine.InputSystem;

public class Checkpoint : MonoBehaviour
{
    public bool wasActivated = false;
    public bool isCurrent = false;

    private bool playerInRange = false;
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        controls.Player.Interact.performed -= OnInteract;
        controls.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (playerInRange)
        {
            ActivateCheckpoint();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!wasActivated)
                CheckpointManager.Instance.ShowPrompt("Naciœnij E / Y, aby aktywowaæ ognisko.");
            else
                CheckpointManager.Instance.ShowPrompt("Naciœnij E / Y, aby odpocz¹æ.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            CheckpointManager.Instance.HidePrompt();
        }
    }

    private void ActivateCheckpoint()
    {
        if (!wasActivated)
        {
            wasActivated = true;
            Debug.Log($"{gameObject.name} activated for the first time.");
        }

        CheckpointManager.Instance.SetCurrentCheckpoint(this);

        if (CheckpointManager.Instance.checkpointWindow.activeSelf)
        {
            CheckpointManager.Instance.HideCheckpointWindow();
        }
        else
        {
            CheckpointManager.Instance.ShowCheckpointWindow();
        }
    }

    public void SetAsCurrent(bool value)
    {
        isCurrent = value;

        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = isCurrent ? Color.red : Color.gray;
        }
    }

    public Transform GetSpawnPoint()
    {
        return transform;
    }
}
