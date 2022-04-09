using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public GameObject currentObject;
    public GameObject nearbyObject;

    public GameObject currentZone;

    void Awake()
    {
    }

    public void StartObjectInteraction(GameObject obj)
    {
        currentObject = obj;
        currentObject.GetComponent<InteractableItem>().OnInteractionBegin(this.transform);
        
    }

    public void StartZoneInteraction()
    {
        currentZone.GetComponent<InteractableZone>().OnInteractionBegin(currentObject);
           
    }

    public void EndObjectInteraction()
    {
        currentObject.GetComponent<InteractableItem>().OnInteractionEnd();
        this.currentObject = null;
    }


    public void HandleInteraction()
    {
        Debug.Log("Handle Interaction");
        if(currentObject != null && currentZone != null)
        {
            StartZoneInteraction();
            EndObjectInteraction();


        }

        else if(currentZone == null)
        {
            if (currentObject != null)
                EndObjectInteraction();
            else
            {
                if (nearbyObject != null)
                {
                    StartObjectInteraction(nearbyObject.gameObject);
                }
            }
        }


       
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Interactable Item" || other.gameObject.tag == "Puzzle_Piece")
        {
            nearbyObject = other.gameObject;
        }
        else if (other.gameObject.tag == "Interactable Zone")
        {
            currentZone = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Interactable Item" || other.gameObject.tag == "Puzzle_Piece")
        {
            nearbyObject = null;
        }
        else if (other.gameObject.tag == "Interactable Zone")
        {
            currentZone = null;
        }
    }


}
