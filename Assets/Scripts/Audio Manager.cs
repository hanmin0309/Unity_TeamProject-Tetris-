using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static AudioManager;

public class AudioManager : MonoBehaviour
{
    //어디에서나 쉽게 쓸수 있도록 정적 메모리에 담을거임
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    private float originalBgmVolume;  // 원래 BGM 볼륨 저장
    AudioSource bgmplayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxplayers;
    int channelIndex;

    public enum Sfx { Dead, Sword, Range, RangeHit, Defeat, Upgrade, Victorty, Lose, LevelUP  }


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
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        originalBgmVolume = bgmVolume;  // 초기 BGM 볼륨 저장

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

    public void PlayBgm(bool isplay)
    {
        if (isplay)
        {
            bgmplayer.Play();

        }
        else
            bgmplayer.Stop();
    }

    public void EffectBgm(bool isplay)
    {
        bgmEffect.enabled = isplay;
    }


    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxplayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxplayers.Length;

            if (sfxplayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxplayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxplayers[loopIndex].Play();
            break;

        }
       
    }

    public void LowerBgmVolume()
    {
        bgmplayer.volume = bgmVolume * 0.2f;  // BGM 볼륨을 20%로 줄임
    }

    public void RestoreBgmVolume()
    {
        bgmplayer.volume = originalBgmVolume;  // BGM 볼륨을 원래대로 복원
    }
}
