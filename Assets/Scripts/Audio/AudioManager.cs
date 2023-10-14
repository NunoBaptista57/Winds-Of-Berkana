using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    AudioSource _audioSource;

    public void Start()
    {
        _audioSource = this.GetComponentInChildren<AudioSource>();
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

    public void PlayAudioClip(AudioClip audio, bool loop=false)
    {
        _audioSource.clip = audio;
        _audioSource.loop = loop;
        _audioSource.Play();
    }




}
