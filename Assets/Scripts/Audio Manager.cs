using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static AudioManager;

public class AudioManager : MonoBehaviour
{
    //��𿡼��� ���� ���� �ֵ��� ���� �޸𸮿� ��������
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    private float originalBgmVolume;  // ���� BGM ���� ����
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
        //����� �÷��̾� �ʱ�ȭ
        GameObject bgmObjcet = new GameObject("BgmPlayer");
        bgmObjcet.transform.parent = transform;
        bgmplayer = bgmObjcet.AddComponent<AudioSource>();
        //bgmplayer.playOnAwake = false; >> ����Ż�� �̺κ��� ĳ���� ���������� ����� ������ ���� �׷��� false;
        bgmplayer.playOnAwake = true;
        bgmplayer.loop = true;
        bgmplayer.volume = bgmVolume;
        bgmplayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        originalBgmVolume = bgmVolume;  // �ʱ� BGM ���� ����

        //ȿ���� �÷��̾� �ʱ�ȭ
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
        bgmplayer.volume = bgmVolume * 0.2f;  // BGM ������ 20%�� ����
    }

    public void RestoreBgmVolume()
    {
        bgmplayer.volume = originalBgmVolume;  // BGM ������ ������� ����
    }
}
