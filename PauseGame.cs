using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject keyboardControlsPanel;
    public GameObject padControlsPanel;

    [Header("Volume UI")]
    public Slider musicSlider;
    public Text musicVolumeText;
    public Slider vfxSlider;
    public Text vfxVolumeText;

    private bool isPaused = false;

    void Start()
    {
        // Load and apply saved volume values
        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 50f);
        float savedVFX = PlayerPrefs.GetFloat("VFXVolume", 50f);

        musicSlider.minValue = 0;
        musicSlider.maxValue = 100;
        musicSlider.value = savedMusic;
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        vfxSlider.minValue = 0;
        vfxSlider.maxValue = 100;
        vfxSlider.value = savedVFX;
        vfxSlider.onValueChanged.AddListener(OnVFXVolumeChanged);

        UpdateVolumeTexts(savedMusic, savedVFX);

        pauseUI?.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGameMethod();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseUI?.SetActive(false);
        pausePanel?.SetActive(false);
        settingsPanel?.SetActive(false);
        keyboardControlsPanel?.SetActive(false);
        padControlsPanel?.SetActive(false);
        isPaused = false;
    }

    private void PauseGameMethod()
    {
        Time.timeScale = 0f;
        pauseUI?.SetActive(true);
        pausePanel?.SetActive(true);
        settingsPanel?.SetActive(false);
        keyboardControlsPanel?.SetActive(false);
        padControlsPanel?.SetActive(false);
        isPaused = true;
    }

    public void OpenSettingsFromPause()
    {
        pausePanel?.SetActive(false);
        settingsPanel?.SetActive(true);
        keyboardControlsPanel?.SetActive(false);
        padControlsPanel?.SetActive(false);
    }

    public void BackFromSettingsToPause()
    {
        settingsPanel?.SetActive(false);
        pausePanel?.SetActive(true);
    }

    public void OpenKeyboardControls()
    {
        settingsPanel?.SetActive(false);
        keyboardControlsPanel?.SetActive(true);
    }

    public void OpenPadControls()
    {
        settingsPanel?.SetActive(false);
        padControlsPanel?.SetActive(true);
    }

    public void BackFromKeyboardControls()
    {
        keyboardControlsPanel?.SetActive(false);
        settingsPanel?.SetActive(true);
    }

    public void BackFromPadControls()
    {
        padControlsPanel?.SetActive(false);
        settingsPanel?.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void OnMusicVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        // Apply value to your actual music audio mixer here
        UpdateVolumeTexts(value, vfxSlider.value);
    }

    public void OnVFXVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("VFXVolume", value);
        // Apply value to your actual VFX audio mixer here
        UpdateVolumeTexts(musicSlider.value, value);
    }

    private void UpdateVolumeTexts(float music, float vfx)
    {
        if (musicVolumeText != null)
            musicVolumeText.text = "Music " + Mathf.RoundToInt(music) + "%";
        if (vfxVolumeText != null)
            vfxVolumeText.text = "VFX " + Mathf.RoundToInt(vfx) + "%";
    }
}
