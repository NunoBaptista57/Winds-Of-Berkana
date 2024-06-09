using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BastionEnvironment : MonoBehaviour, AudioEnvironment
{
    [SerializeField] private string walkingSound;
    [SerializeField] private string landingSound;
    [SerializeField] private string glidingSound;

    [SerializeField] private string runningSound;

    public Sound[] sfxSounds;


    public void WalkSound(AudioSource audio)
    {
        Debug.Log("playing walking sound");
        Sound s = Array.Find(sfxSounds, x => x.name == walkingSound);
        if (s == null) Debug.Log("Sound Not Found");
        else
        {
            audio.clip = s.clip;
            audio.loop = true;
            audio.Play();
        }
    }
    public void LandingSound(AudioSource audio)
    {
        Debug.Log("playeing landing sound");
        Sound s = Array.Find(sfxSounds, x => x.name == landingSound);
        if (s == null) Debug.Log("Sound Not Found");
        else
        {
            audio.clip = s.clip;
            audio.Play();
        }
    }
    public void GlidingSound(AudioSource audio)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == glidingSound);
        if (s == null) Debug.Log("Sound Not Found");
        else
        {
            audio.clip = s.clip;
            audio.loop = true;
            audio.Play();
        }
    }

    public void RunningSound(AudioSource audio)
    {
        Debug.Log("playing running sound");
        Sound s = Array.Find(sfxSounds, x => x.name == runningSound);
        if (s == null) Debug.Log("Sound Not Found");
        else
        {
            audio.clip = s.clip;
            audio.loop = true;
            audio.Play();
        }
    }
}