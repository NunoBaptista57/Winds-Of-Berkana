using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    
    public Sound[] musicSounds,sfxSounds;

    public AudioSource _musicSource, _sfxSource;

    public void Start()
    {
        //_audioSource = this.GetComponentInChildren<AudioSource>();
    }

    public static AudioManager Instance {
        get { if(_instance == null)
            {
                Debug.LogError("Audio Manager is null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void PlayMusic(string name, bool loop=false)
    {
        //_audioSource.clip = audio;
        //_audioSource.loop = loop;
       // _audioSource.Play();
       Sound s = Array.Find(musicSounds, x => x.name == name);
       if (s == null) Debug.Log("Sound Not Found");
       else{
            _musicSource.clip = s.clip;
            _musicSource.loop = loop;
            _musicSource.Play();
       }
    }

    public void PlaySFX(string name)
    {
       Sound s = Array.Find(sfxSounds, x => x.name == name);
       if (s == null) Debug.Log("Sound Not Found");
       else{
            _sfxSource.clip = s.clip;
            _sfxSource.Play();
       }
    }

    public void ToggleMusic(){
        _musicSource.mute = !_musicSource.mute;
    }

    public void ToggleSFX(){
        _sfxSource.mute = !_sfxSource.mute;
    }

    public void MusicVolume(float volume){
        _musicSource.volume = volume;
    }

    public void SFXVolume(float volume){
        _sfxSource.volume = volume;
    }
}
