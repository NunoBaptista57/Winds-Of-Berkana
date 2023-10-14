using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BastionLandingZone : MonoBehaviour
{

    public bool inLandingZone = false;
    public Canvas promptCanvas;
    public string LevelToLoad;

    public void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<BoatMovement>().onInteraction += OnInteraction;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !inLandingZone)
        {
            promptCanvas.gameObject.SetActive(true);
            inLandingZone = true;

        }

    }


    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && inLandingZone)
        {
            promptCanvas.gameObject.SetActive(false);
            inLandingZone = false;

        }
    }



void OnInteraction()
    {
        if(inLandingZone)
            SceneManager.LoadScene(LevelToLoad);
    }
}
