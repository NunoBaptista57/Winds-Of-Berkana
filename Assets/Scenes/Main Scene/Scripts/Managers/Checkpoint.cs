using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private EventManager _eventManager;

    private void OnTriggerEnter(Collider other)
    {
        _eventManager.InvokeReachedCheckpointEvent(this);
    }
}