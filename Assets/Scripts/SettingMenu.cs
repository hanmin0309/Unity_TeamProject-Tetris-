using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

    public GameObject panel;

    public bool GameIsPaused = false;

    //public GameObject pauseMenuUI;


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
            StopGame();
            
        }
    }
    void StopGame()
    {
        // 패널의 활성화 상태를 반전
        panel.SetActive(!panel.activeSelf);
        GameIsPaused = panel.activeSelf;

        Time.timeScale = 0f;

    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    
    public void ResumeGame()
    {
        panel.SetActive(!panel.activeSelf);
        Time.timeScale = 1f;
        GameIsPaused = false;
        
    }

    //public void pause()
    //{
    //    pauseMenuUI.SetActive(true);

    //}
}
