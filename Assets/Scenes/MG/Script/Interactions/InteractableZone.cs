using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableZone : MonoBehaviour
{

    public virtual bool OnInteractionBegin(GameObject currentObject)
    {
        return false;
    }

    public virtual void OnInteractionEnd()
    {

    }
}
