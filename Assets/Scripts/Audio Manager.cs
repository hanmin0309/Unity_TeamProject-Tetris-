using System.Net.NetworkInformation;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header ("----------------Audio Source--------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;


    [Header("---------------Audio Clip--------------")]
    public AudioClip BGM;
    public AudioClip sword;
    public AudioClip range;
    public AudioClip defeat;
    public AudioClip upGrade;
    public AudioClip victory;
    public AudioClip lose;
    public AudioClip levelUp;
    public AudioClip Dead;

    private void Start()
    {
        musicSource.clip = BGM;
        musicSource.Play(); 
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}