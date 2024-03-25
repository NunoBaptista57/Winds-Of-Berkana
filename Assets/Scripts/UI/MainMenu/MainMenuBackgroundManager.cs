using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBackgroundManager : MonoBehaviour
{
   [Serializable]
    public struct ParallaxComponent
    {
       public RawImage image;
       public float speed;
    }

    public ParallaxComponent[] parallaxComponents;

    private void Update()
    {
        foreach(var p in parallaxComponents)
        {
            float offSet = Time.deltaTime * p.speed;

            p.image.uvRect = new Rect(offSet, 0.0f, 1, 1);
        }
       
    }
}
