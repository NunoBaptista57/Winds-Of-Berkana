using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform RespawnPosition;
    public Collider Collider;

    private void OnTriggerEnter()
    {
        ServiceLocator.instance.GetService<CheckpointManager>().CurrentCheckpoint = RespawnPosition;
    }
}