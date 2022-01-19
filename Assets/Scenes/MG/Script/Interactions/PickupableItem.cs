using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : InteractableItem
{

    public override void OnInteractionBegin(Transform playerTransform)
    {
        Debug.Log("Picking Up " + this.name);
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.transform.position = playerTransform.position;
        this.transform.parent = playerTransform.transform;
    }

    public override void OnInteractionEnd()
    {
        Debug.Log("Stop Picking Up " + this.name);
        this.transform.parent = null;
        this.GetComponent<Collider>().enabled = true;
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().isKinematic = false;
    }
}
