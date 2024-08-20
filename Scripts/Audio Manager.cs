using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //어디에서나 쉽게 쓸수 있도록 정적 메모리에 담을거임
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmplayer;

    [Header("#SFX")]
    public AudioClip[] sfxClip;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxplayers;
    int channelIndex;


    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        //배경음 플레이어 초기화
        GameObject bgmObjcet = new GameObject("BgmPlayer");
        bgmObjcet.transform.parent = transform;
        bgmplayer = bgmObjcet.AddComponent<AudioSource>();
        //bgmplayer.playOnAwake = false; >> 골드메탈은 이부분을 캐릭터 선택했을때 브금이 나오게 구성 그러니 false;
        bgmplayer.playOnAwake = true;
        bgmplayer.loop = true;
        bgmplayer.volume = bgmVolume;
        bgmplayer.clip = bgmClip;


        //효과음 플레이어 초기화
        GameObject sfxObjcet = new GameObject("SfxPlayer");
        sfxObjcet.transform.parent = transform;
        sfxplayers = new AudioSource[channels];

        for (int index = 0; index < sfxplayers.Length; index++)
        {
            sfxplayers[index] = sfxObjcet.AddComponent<AudioSource>();
            sfxplayers[index].playOnAwake = false;
            sfxplayers[index].volume = sfxVolume;
        }
    }
}
