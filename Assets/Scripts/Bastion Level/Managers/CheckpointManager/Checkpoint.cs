using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform RespawnPosition;

    private void OnTriggerEnter()
    {
        ServiceLocator.instance.GetService<CheckpointManager>().CurrentCheckpoint = RespawnPosition;
        Debug.Log("entrou");
    }
}