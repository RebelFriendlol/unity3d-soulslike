using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Klawisz R zosta� naci�ni�ty");
            ResetToSpawnPoint();  // Respawn zawsze � albo do checkpointa, albo do defaulta
        }
    }

    private void ResetToSpawnPoint()
    {
        if (CheckpointManager.Instance == null)
        {
            Debug.LogWarning("Brak odniesienia do CheckpointManager!");
            return;
        }

        Transform spawnPoint = CheckpointManager.Instance.GetCurrentSpawnPoint();

        if (spawnPoint != null)
        {
            // Najpierw spr�buj wy��czy� CharacterController, je�li istnieje
            CharacterController controller = GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                transform.position = spawnPoint.position;
                controller.enabled = true;
                Debug.Log("Gracz zresetowany z CharacterController do: " + spawnPoint.position);
                return;
            }

            // Je�li mamy Rigidbody, u�yj jego metod
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.position = spawnPoint.position;
                Debug.Log("Gracz zresetowany z Rigidbody do: " + spawnPoint.position);
                return;
            }

            // Je�li nic z powy�szych � ustaw zwyk�y transform
            transform.position = spawnPoint.position;
            Debug.Log("Gracz zresetowany przez transform do: " + spawnPoint.position);
        }
        else
        {
            Debug.LogWarning("Brak dost�pnego punktu respawnu!");
        }
    }
}
