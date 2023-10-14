using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private EventSender _eventSender;

    private void OnTriggerEnter(Collider other)
    {
        _eventSender.InvokeReachedCheckpointEvent(transform);
    }
}