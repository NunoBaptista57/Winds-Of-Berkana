using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface AudioEnvironment
{
    public void WalkSound(AudioSource audio);
    public void LandingSound(AudioSource audio);
    public void GlidingSound(AudioSource audio);

    public void RunningSound(AudioSource audio);
}
