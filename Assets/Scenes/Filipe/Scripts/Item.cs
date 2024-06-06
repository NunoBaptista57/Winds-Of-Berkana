using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : MonoBehaviour, ICollectable
{
    public static event Action OnItemCollected;
    public void Collect()
    {
        //Debug.Log("You collected the item");
        Destroy(gameObject);
        OnItemCollected?.Invoke();
    }

}
