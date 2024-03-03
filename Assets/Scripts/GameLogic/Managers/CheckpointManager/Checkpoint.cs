using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform RespawnPosition;
    private CheckpointManager _checkpointManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _checkpointManager.ChangeCheckpoint(this);
        }
    }

    private void Awake()
    {
        _checkpointManager = GetComponentInParent<CheckpointManager>();
    }
}