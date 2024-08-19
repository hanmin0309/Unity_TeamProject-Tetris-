using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //��𿡼��� ���� ���� �ֵ��� ���� �޸𸮿� ��������
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
        //����� �÷��̾� �ʱ�ȭ
        GameObject bgmObjcet = new GameObject("BgmPlayer");
        bgmObjcet.transform.parent = transform;
        bgmplayer = bgmObjcet.AddComponent<AudioSource>();
        //bgmplayer.playOnAwake = false; >> ����Ż�� �̺κ��� ĳ���� ���������� ����� ������ ���� �׷��� false;
        bgmplayer.playOnAwake = true;
        bgmplayer.loop = true;
        bgmplayer.volume = bgmVolume;
        bgmplayer.clip = bgmClip;


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
}
