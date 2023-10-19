using System;
using UnityEngine;

public class CheckpointManager : MonoBehaviour, IManager
{
    public Vector3 CurrentCheckpoint;

    public SaveFile Save(SaveFile saveFile)
    {
        saveFile.Checkpoint = CurrentCheckpoint;
        return saveFile;
    }

    public void Load(SaveFile saveFile)
    {
        CurrentCheckpoint = saveFile.Checkpoint;
    }
}