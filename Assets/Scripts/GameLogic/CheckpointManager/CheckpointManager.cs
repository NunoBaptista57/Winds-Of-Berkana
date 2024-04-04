using System;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Checkpoint CurrentCheckpoint;

    public void ChangeCheckpoint(Checkpoint checkpoint)
    {
        Debug.Log("Change Checkpoint");
        CurrentCheckpoint = checkpoint;
        // TODO AutoSave maybe...
    }
}