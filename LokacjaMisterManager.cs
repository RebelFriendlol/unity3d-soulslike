using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LokacjaMisterManager : MonoBehaviour
{
    [Header("Fire Particle GameObjects (existing in scene)")]
    public GameObject[] floor1Particles;
    public GameObject[] floor2Particles;
    public GameObject[] floor3Particles;
    public GameObject[] floor4Particles;

    [Header("Fog Particle Systems to Fade")]
    public ParticleSystem[] particlesToFade;

    private GameObject[][] allFloorsParticles;
    private int currentActiveFloor = -1;

    [Header("Audio Settings")]
    public AudioSource backgroundMusic1;
    public AudioSource backgroundMusic2;
    private bool hasTriggeredAudio = false;

    [Header("Pause Menu UI")]
    public GameObject pausePanel;
    public GameObject settingsPanel;
    private GameObject controlsPanel;
    public GameObject pauseUI;
    public GameObject keyboardControlsPanel;
    public GameObject padControlsPanel;

    [Header("Volume UI")]
    public Slider musicSlider;
    public Text musicVolumeText;
    public Slider vfxSlider;
    public Text vfxVolumeText;

    private bool isPaused = false;

    private void Start()
    {
        allFloorsParticles = new GameObject[][]
        {
            floor1Particles,
            floor2Particles,
            floor3Particles,
            floor4Particles
        };

        if (currentActiveFloor >= 0 && currentActiveFloor < allFloorsParticles.Length)
        {
            var previousFloorParticles = allFloorsParticles[currentActiveFloor];
            if (previousFloorParticles != null)
            {
                foreach (var go in previousFloorParticles)
                {
                    if (go != null)
                        go.SetActive(false);
                }
            }
        }

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    #region Pause Menu

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseUI?.SetActive(false);
        pausePanel?.SetActive(false);
        settingsPanel?.SetActive(false);
        controlsPanel?.SetActive(false);
        keyboardControlsPanel?.SetActive(false);
        padControlsPanel?.SetActive(false);
        isPaused = false;
        ResumeMusic();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        pauseUI?.SetActive(true);
        pausePanel?.SetActive(true);
        settingsPanel?.SetActive(false);
        controlsPanel?.SetActive(false);
        keyboardControlsPanel?.SetActive(false);
        padControlsPanel?.SetActive(false);
        isPaused = true;
        PauseMusic();
    }

    public void OpenSettingsFromPause()
    {
        pausePanel?.SetActive(false);
        settingsPanel?.SetActive(true);
        controlsPanel?.SetActive(false);
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
        UpdateVolumeTexts(value, vfxSlider.value);
    }

    public void OnVFXVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("VFXVolume", value);
        UpdateVolumeTexts(musicSlider.value, value);
    }

    private void UpdateVolumeTexts(float music, float vfx)
    {
        if (musicVolumeText != null)
            musicVolumeText.text = "Music " + Mathf.RoundToInt(music) + "%";
        if (vfxVolumeText != null)
            vfxVolumeText.text = "VFX " + Mathf.RoundToInt(vfx) + "%";
    }

    private void PauseMusic()
    {
        if (backgroundMusic1 != null && backgroundMusic1.isPlaying)
            backgroundMusic1.Pause();
        if (backgroundMusic2 != null && backgroundMusic2.isPlaying)
            backgroundMusic2.Pause();
    }

    private void ResumeMusic()
    {
        if (backgroundMusic1 != null && !backgroundMusic1.isPlaying)
            backgroundMusic1.UnPause();
        if (backgroundMusic2 != null && !backgroundMusic2.isPlaying)
            backgroundMusic2.UnPause();
    }

    #endregion

    #region Floor Management

    public void PlayerEnteredFloor(int floorNumber)
    {
        int index = floorNumber - 1;

        if (currentActiveFloor != index)
        {
            if (currentActiveFloor >= 0 && currentActiveFloor < allFloorsParticles.Length)
            {
                foreach (var go in allFloorsParticles[currentActiveFloor])
                {
                    if (go != null && !go.CompareTag("FireParticle"))
                        go.SetActive(false);
                }
            }

            if (index >= 0 && index < allFloorsParticles.Length)
            {
                foreach (var go in allFloorsParticles[index])
                {
                    if (go != null)
                    {
                        go.SetActive(true);
                        foreach (var ps in go.GetComponentsInChildren<ParticleSystem>())
                            ps.Play();
                    }
                }
            }

            currentActiveFloor = index;
        }

        if (floorNumber == 4 && !hasTriggeredAudio)
        {
            hasTriggeredAudio = true;

            if (backgroundMusic2 != null && !backgroundMusic2.isPlaying)
                StartCoroutine(FadeInMusic(backgroundMusic2, 5f));
            if (backgroundMusic1 != null && backgroundMusic1.isPlaying)
                StartCoroutine(FadeOutMusic(backgroundMusic1, 1f));
        }

        if (floorNumber == 4)
            FadeOutParticles(particlesToFade);
    }

    private IEnumerator FadeInMusic(AudioSource musicSource, float duration)
    {
        musicSource.volume = 0f;
        musicSource.Play();
        float time = 0f;
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(0f, 1f, time / duration);
            yield return null;
        }
        musicSource.volume = 1f;
    }

    private IEnumerator FadeOutMusic(AudioSource musicSource, float duration)
    {
        float startVolume = musicSource.volume;
        float time = 0f;
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }
        musicSource.Stop();
        musicSource.volume = startVolume;
    }

    private void FadeOutParticles(ParticleSystem[] particles)
    {
        foreach (var ps in particles)
        {
            if (ps != null)
                StartCoroutine(FadeOutParticle(ps, 2f));
        }
    }

    private IEnumerator FadeOutParticle(ParticleSystem ps, float duration)
    {
        var main = ps.main;
        Color startColor = main.startColor.color;
        float time = 0f;
        while (time < duration)
        {
            float alpha = Mathf.Lerp(startColor.a, 0f, time / duration);

            var colorOverLifetime = ps.colorOverLifetime;
            if (!colorOverLifetime.enabled)
                colorOverLifetime.enabled = true;

            Gradient grad = new Gradient();
            grad.SetKeys(
                new GradientColorKey[] { new GradientColorKey(startColor, 0f) },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(alpha, 0f),
                    new GradientAlphaKey(0f, 1f)
                });

            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(grad);

            time += Time.deltaTime;
            yield return null;
        }
    }

    #endregion
}