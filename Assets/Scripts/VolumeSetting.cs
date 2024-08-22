using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Audio;
using UnityEngine.UI;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        SetMusicVolume();
        SetSFXVolume();
        SetMasterVolume();
    }

    public void SetMasterVolume()
    {
        float volume = MasterSlider.value;
        myMixer.SetFloat("MASTER", Mathf.Log10(volume) * 20);
    }
    public void SetMusicVolume()
    {
        float volume = BGMSlider.value;
        myMixer.SetFloat("BGM", Mathf.Log10(volume)*20);
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
