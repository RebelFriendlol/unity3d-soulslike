using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject exitConfirmationPanel;
    public GameObject menuPanel;
    public GameObject controlsPanel;

    [Header("Volume")]
    public Slider volumeSlider;
    public Text volumeText;

    private void Start()
    {
        // Panel init
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (exitConfirmationPanel != null) exitConfirmationPanel.SetActive(false);
        if (controlsPanel != null) controlsPanel.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(true);

        // Volume
        float savedVolume = PlayerPrefs.GetFloat("Volume", 50f);
        AudioListener.volume = savedVolume / 100f;
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0;
            volumeSlider.maxValue = 100;
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
        UpdateVolumeText(savedVolume);
    }

    public void StartGame() => SceneManager.LoadSceneAsync(1);

    public void Settings()
    {
        settingsPanel?.SetActive(true);
        menuPanel?.SetActive(false);
        exitConfirmationPanel?.SetActive(false);
    }

    public void ReturnToSettings()
    {
        controlsPanel?.SetActive(false);
        settingsPanel?.SetActive(true);
    }

    public void BackFromSettings()
    {
        settingsPanel?.SetActive(false);
        menuPanel?.SetActive(true);
    }

    public void OpenControls()
    {
        controlsPanel?.SetActive(true);
        settingsPanel?.SetActive(false);
        exitConfirmationPanel?.SetActive(false);
        menuPanel?.SetActive(false);
    }

    public void BackFromControls()
    {
        controlsPanel?.SetActive(false);
        settingsPanel?.SetActive(true);
    }

    public void ExitFromGame()
    {
        exitConfirmationPanel?.SetActive(true);
        menuPanel?.SetActive(false);
        settingsPanel?.SetActive(false);
        controlsPanel?.SetActive(false);
    }

    public void ConfirmExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void CancelExit()
    {
        exitConfirmationPanel?.SetActive(false);
        menuPanel?.SetActive(true);
    }

    public void BackToMenu()
    {
        settingsPanel?.SetActive(false);
        exitConfirmationPanel?.SetActive(false);
        controlsPanel?.SetActive(false);
        menuPanel?.SetActive(true);
    }

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value / 100f;
        PlayerPrefs.SetFloat("Volume", value);
        UpdateVolumeText(value);
    }

    private void UpdateVolumeText(float value)
    {
        if (volumeText != null)
            volumeText.text = "Volume " + Mathf.RoundToInt(value) + "%";
    }
}
