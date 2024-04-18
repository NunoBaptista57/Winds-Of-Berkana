using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    public Sound[] musicSounds,sfxSounds;

    public AudioSource _musicSource, _sfxSource;

    public void Start()
    {
        //_audioSource = this.GetComponentInChildren<AudioSource>();
    }

    private void Awake() {
        if(Instance == null)
        {
            Instance = this;
            //Debug.LogError("Audio Manager is null");
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }        
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

    public void StopMusic(){
        _musicSource.Stop();
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
