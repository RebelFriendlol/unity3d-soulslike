using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTeleportZone : MonoBehaviour
{
    public string sceneToLoad = "NazwaTwojejSceny"; // Ustaw w Inspectorze
    public KeyCode interactKey = KeyCode.E;
    public Text interactionText; // Przypisz UI tekst z Canvasu

    private bool playerInside = false;

    private void Start()
    {
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(interactKey))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            if (interactionText != null)
            {
                interactionText.text = "Naciœnij E, aby wejœæ";
                interactionText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (interactionText != null)
                interactionText.gameObject.SetActive(false);
        }
    }
}
