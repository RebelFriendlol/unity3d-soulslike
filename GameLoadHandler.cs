using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoadHandler : MonoBehaviour
{
    public static GameData loadedData;

    public void LoadGameFromMenu()
    {
        GameData data = SaveSystem.Load();
        if (data != null)
        {
            loadedData = data;
            SceneManager.LoadScene(data.currentSceneName);
        }
        else
        {
            Debug.LogWarning("Brak zapisu do wczytania.");
        }
    }
}
