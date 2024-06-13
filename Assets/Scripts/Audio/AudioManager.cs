using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;

    public AudioSource _musicSource, _sfxSource;

    public AudioEnvironment _environment;

    public BastionEnvironment _bastionEnvironment;
    public void Start()
    {
        _environment = _bastionEnvironment;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name, bool loop = false)
    {

        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null) Debug.Log("Sound Not Found");
        else
        {
            _musicSource.clip = s.clip;
            _musicSource.loop = loop;
            _musicSource.Play();
        }
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }
    public void StopSFX()
    {
        float maxSuddenStopDuration = 1.0f;

        if (_sfxSource.clip != null)
            if (_sfxSource.clip.length > maxSuddenStopDuration)
            {
                _sfxSource.Stop();
            }
        _sfxSource.loop = false;
    }
    public void WalkSound()
    {
        _environment.WalkSound(_sfxSource);
    }
    public void LandingSound()
    {
        _environment.LandingSound(_sfxSource);

    }
    public void GlidingSound()
    {
        _environment.GlidingSound(_sfxSource);

    }

    public void RunningSound()
    {
        _environment.RunningSound(_sfxSource);
    }

    /*public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null) Debug.Log("Sound Not Found");
        else
        {
            _sfxSource.clip = s.clip;
            _sfxSource.Play();
        }
    }*/

    public void ToggleMusic()
    {
        _musicSource.mute = !_musicSource.mute;
    }

    public void ToggleSFX()
    {
        _sfxSource.mute = !_sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        _musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        _sfxSource.volume = volume;
    }
}
