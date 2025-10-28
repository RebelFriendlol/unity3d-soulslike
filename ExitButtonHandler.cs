using UnityEngine;
using UnityEngine.UI;  // Dodajemy przestrzeñ nazw do pracy z UI

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Button exitButton;  // Przycisk wyjœcia w UI

    private void Start()
    {
        if (exitButton != null)
        {
            // Dodajemy nas³uchiwanie na klikniêcie przycisku
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
    }

    // Funkcja wywo³ywana po klikniêciu przycisku
    private void OnExitButtonClicked()
    {
        CheckpointManager.Instance.HideCheckpointWindow();  // Ukryj okno
    }
}
