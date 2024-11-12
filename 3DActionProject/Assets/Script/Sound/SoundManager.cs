using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _Effect_Sound;
    [SerializeField] private AudioClip _BackgroundMusic;

    private static SoundManager _instance;

    private AudioSource Audio;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject projectile = (GameObject)Instantiate((GameObject)Resources.Load("Manager/SoundManager"));
                _instance = projectile.GetComponent<SoundManager>();
            }

            return _instance;
        }
    }

    private bool _musicOnOff;

    public bool MusicOnOff
    {
        get
        {
            return _musicOnOff;
        }

        set
        {
            _musicOnOff = value;
            if (_musicOnOff)
            {
                Play_BackgroundMusic();
            }
            else
            {
                Audio.Stop();
            }
        }
    }

    private bool _effectOnOff;

    public bool EffectOnOff
    {
        get => _effectOnOff;

        set => _effectOnOff = value;
    }

    private void Awake()
    {
        Audio = GetComponent<AudioSource>();
        _effectOnOff = true;
        _musicOnOff = true;
    }

    public void Play_BossAttackSound()
    {
        if (_effectOnOff)
        {
            Audio.PlayOneShot(_Effect_Sound[0]); // 3번째 사운드 재생
        }
    }

    public void Play_PlayerAttackSound()
    {
        if (_effectOnOff)
        {
            Audio.PlayOneShot(_Effect_Sound[1]);
        }
    }

    public void Play_PlayerDoungonAttackSound()
    {
        if (_effectOnOff)
        {
            Audio.PlayOneShot(_Effect_Sound[3]);
        }
    }

    public void Play_CoinSound()
    {
        if (_effectOnOff)
        {
            Audio.PlayOneShot(_Effect_Sound[2]);
        }
    }


    public void Play_BackgroundMusic()
    {
        if (_musicOnOff)
        {
            Audio.clip = _BackgroundMusic;
            Audio.loop = true;

            Audio.Play();
        }
        else
        {
            Audio.Stop();
        }
    }

}
