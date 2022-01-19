using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    Transform currentObject;
    Transform camera;

    public float pickupRange = 1.0f;

    void Awake()
    {
        camera = Camera.main.GetComponent<Transform>();
    }

    public void StartPickup(GameObject obj)
    {
        Debug.Log("Picking up");
        this.currentObject = obj.GetComponent<Transform>();
        currentObject.GetComponent<Rigidbody>().useGravity = false;
        currentObject.GetComponent<Collider>().enabled = false;
        currentObject.GetComponent<Rigidbody>().isKinematic = true;
        currentObject.transform.position = this.transform.position;
        currentObject.transform.parent = this.transform;

    }

    public void StopPick()
    {
        Debug.Log("Stop Picking Up");
        currentObject.transform.parent = null;
        currentObject.GetComponent<Collider>().enabled = true;
        currentObject.GetComponent<Rigidbody>().useGravity = true;
        currentObject.GetComponent<Rigidbody>().isKinematic = false;
        currentObject = null;
    }


    public void PickUpObject()
    {
        if (currentObject != null)
            StopPick();
        else
        {
            RaycastHit hit;
            // Debug.Log("Starting Grapple");
            if (Physics.Raycast(camera.position, camera.forward, out hit, pickupRange))
            {
                if (hit.transform.tag == "Interactable")
                    StartPickup(hit.transform.gameObject);
            }
        }
    }


}
