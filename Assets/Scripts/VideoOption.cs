using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoOptions : MonoBehaviour
{
    public Dropdown resolutionDropdown;

    void Start()
    {
        // 초기 해상도 설정
        SetResolution(800, 600, false);
        InitResolutionDropdown();
    }

    void InitResolutionDropdown()
    {
        resolutionDropdown.options.Clear();
        resolutionDropdown.AddOptions(new List<string> { "1920x1080", "1280x720", "800x600" });
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChange);
    }

    public void OnResolutionChange(int index)
    {
        Debug.Log("실행");
        switch (index)
        {
            case 0:
                SetResolution(1920, 1080, false);
                break;
            case 1:
                SetResolution(1280, 720, false);
                break;
            case 2:
                SetResolution(800, 600, false);
                break;
        }
    }

    void SetResolution(int width, int height, bool fullscreen)
    {
        Screen.SetResolution(width, height, fullscreen);
    }

    void Update()
    {
        Camera.main.aspect = (float)Screen.width / Screen.height;
    }
}
