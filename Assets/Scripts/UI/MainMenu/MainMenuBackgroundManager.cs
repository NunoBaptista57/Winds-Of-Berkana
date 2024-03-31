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
            float offset = p.image.uvRect.x + Time.deltaTime * p.speed;

            // Loop the offset back to 0 when it reaches 1 to create a looping effect
            if (offset > 1f)
            {
                offset -= 1f;
            }

            p.image.uvRect = new Rect(offset, 0f, 1f, 1f);
        }
       
    }
}
