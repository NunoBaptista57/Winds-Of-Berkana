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

    private string lastPlayed;

    public float time_until_play;
    private float counter_to_use;


    public void Start()
    {
        counter_to_use = time_until_play;
    }
    public void Update()
    {
        counter_to_use = counter_to_use - 1 * Time.deltaTime;
    }

    public void WalkSound(AudioSource audio)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == walkingSound);
        if (s == null) Debug.Log("Sound Not Found");
        else
        {
            audio.clip = s.clip;
            audio.loop = true;
            audio.Play();
            lastPlayed = s.name;
        }
    }
    public void LandingSound(AudioSource audio)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == landingSound);
        if (s == null) Debug.Log("Sound Not Found");
        else
        {
            audio.clip = s.clip;
            audio.Play();
            lastPlayed = s.name;
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
            lastPlayed = s.name;
        }
    }

    public void RunningSound(AudioSource audio)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == runningSound);
        if (s == null) Debug.Log("Sound Not Found");
        else
        {
            if (counter_to_use < 0)
            {
                if (!(audio.isPlaying && lastPlayed.Equals(landingSound)))
                {
                    audio.clip = s.clip;
                    audio.Play();
                    counter_to_use = time_until_play;
                    lastPlayed = s.name;
                }
            }
        }
    }
}