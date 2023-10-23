using System;
using UnityEngine;

public class CheckpointManager : MonoBehaviour, ISavable
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