using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BoatEnvironment : MonoBehaviour, AudioEnvironment
{
    [SerializeField] private string slowSpeed;
    [SerializeField] private string fastSpeed;
    [SerializeField] private string glidingSound;

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

    public void SlowSound(AudioSource audio)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == slowSpeed);
        if (s == null) Debug.Log("Sound Not Found");
        else {
            if(lastPlayed == null || !lastPlayed.Equals(slowSpeed)){
                
                    Debug.Log("Enetered sound change");
                    audio.clip = s.clip;
                    audio.loop = true;
                    audio.Play();
                    lastPlayed = s.name;
                }
        }
    }
    public void FastSound(AudioSource audio)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == fastSpeed);
        if (s == null) Debug.Log("Sound Not Found");
        else {
            if(lastPlayed == null || !lastPlayed.Equals(fastSpeed)){
                
                    Debug.Log("Enetered sound change");
                    audio.clip = s.clip;
                    audio.loop = true;
                    audio.Play();
                    lastPlayed = s.name;
                }
        }
    }
    public void GlidingSound(AudioSource audio)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == glidingSound);
        if (s == null) Debug.Log("Sound Not Found");
        else {
              if(lastPlayed == null || !lastPlayed.Equals(glidingSound)){
                
                    Debug.Log("Enetered sound change");
                    audio.clip = s.clip;
                    audio.loop = true;
                    audio.Play();
                    lastPlayed = s.name;
                }
            }
    }
    

    public void WalkSound(AudioSource audio){}
    public void LandingSound(AudioSource audio){}

    public void RunningSound(AudioSource audio){}
}