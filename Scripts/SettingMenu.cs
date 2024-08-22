using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public GameObject panel;
    private bool isOptionOpen = false;
    public bool GameIsPaused = false;
    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOptionOpen)
            {
                // 옵션 창이 열리면
                AudioManager.instance.EffectBgm(true);
                AudioManager.instance.LowerBgmVolume();
                isOptionOpen = true;
                StopGame();
            }
            else
            {
                // 옵션 창이 닫히면
                AudioManager.instance.EffectBgm(false);
                AudioManager.instance.RestoreBgmVolume();
                isOptionOpen = false;
                ResumeGame();
            }
        }
    }

    void StopGame()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;  // 게임을 일시정지
    }

    public void ResumeGame()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;  // 게임을 재개
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
