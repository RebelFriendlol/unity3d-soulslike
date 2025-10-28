using UnityEngine;
using UnityEngine.UI;

public class InteractionTrigger : MonoBehaviour
{
    [Tooltip("Tekst wyœwietlany po naciœniêciu E.")]
    public string interactionText = "Domyœlna wiadomoœæ.";

    [Tooltip("UI Text do komunikatu 'Kliknij E...'")]
    public Text promptText;

    [Tooltip("UI Text do g³ównego tekstu po interakcji")]
    public Text messageText;

    private bool playerInRange = false;
    private bool hasInteracted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            hasInteracted = false;

            if (promptText != null)
                promptText.text = "Kliknij E, aby zobaczyæ";

            if (messageText != null)
                messageText.text = "";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            hasInteracted = false;

            if (promptText != null)
                promptText.text = "";

            if (messageText != null)
                messageText.text = "";
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !hasInteracted)
        {
            hasInteracted = true;

            if (promptText != null)
                promptText.text = "";

            if (messageText != null)
                messageText.text = interactionText;
        }
    }
}
