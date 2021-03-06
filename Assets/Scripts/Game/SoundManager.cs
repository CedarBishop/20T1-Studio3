﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicTracks { MainMenu, GameMusic, FinalRound, Win, Loss, Transistion}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    [HideInInspector] public AudioSource musicAudioSource;
    List<AudioSource> sfx = new List<AudioSource>();

    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip finalRoundMusic;
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip lossMusic;
    [SerializeField] private AudioClip transistionMusic;

    [SerializeField]
    Sound[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        musicAudioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        }
        
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.parent = transform;
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
            sfx.Add(_go.GetComponent<AudioSource>());
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 1.0f));
        }
        
        PlayMusic(MusicTracks.MainMenu);
    }

    public void PlaySFX(string soundName)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == soundName)
            {
                sounds[i].Play();
                return;
            }
        }
    }

    public void SetSFXParams (string soundName, float pitch, float reverb)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == soundName)
            {
                sounds[i].pitch = pitch;
                sounds[i].reverbZone = reverb;
                sounds[i].UpdateParams();
                return;
            }
        }
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        musicAudioSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
        foreach (Sound sound in sounds)
        {
            sound.source.volume = value * sound.volumeScaler;
        }
    }

    public void PlayMusic (MusicTracks musicTracks)
    {
        switch (musicTracks)
        {
            case MusicTracks.MainMenu:
                if (mainMenuMusic != null)
                {
                    musicAudioSource.loop = true;
                    musicAudioSource.clip = mainMenuMusic;
                    musicAudioSource.Play();
                }
                break;
            case MusicTracks.GameMusic:
                if (gameMusic != null)
                {
                    musicAudioSource.loop = false;
                    musicAudioSource.clip = gameMusic;
                    musicAudioSource.Play();
                }
                break;
            case MusicTracks.FinalRound:
                if (finalRoundMusic != null)
                {
                    musicAudioSource.loop = false;
                    musicAudioSource.clip = finalRoundMusic;
                    musicAudioSource.Play();
                }
                break;
            case MusicTracks.Win:
                if (winMusic != null)
                {
                    musicAudioSource.loop = true;
                    musicAudioSource.clip = winMusic;
                    musicAudioSource.Play();
                }
                break;
            case MusicTracks.Loss:
                if (lossMusic != null)
                {
                    musicAudioSource.loop = true;
                    musicAudioSource.clip = lossMusic;
                    musicAudioSource.Play();
                }
                break;
            case MusicTracks.Transistion:
                if (transistionMusic != null)
                {
                    musicAudioSource.loop = false;
                    musicAudioSource.clip = transistionMusic;
                    musicAudioSource.Play();
                }
                break;
            default:
                break;
        }
    }

    public void StopMusic()
    {
        if (musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
        }
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [HideInInspector]public AudioSource source;
    [Range(-3.0f, 3.0f)] public float pitch = 1;
    [Range(0.0f, 1.1f)] public float reverbZone;
    [Range(0.0f, 1.0f)] public float volumeScaler = 1.0f;


    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.pitch = pitch;
        source.volume = volumeScaler;
    }

    public void UpdateParams ()
    {
        source.pitch = pitch;
        source.reverbZoneMix = reverbZone;
    }



    public void Play()
    {
        if (clip == null || source == null)
        {
            return;
        }            
        source.Play();        
    }
}