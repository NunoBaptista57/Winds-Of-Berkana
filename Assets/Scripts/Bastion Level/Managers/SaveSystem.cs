using System;
using System.Collections.Generic;
using UnityEngine;

// TODO
public static class SaveSystem
{
    public static void Save(SaveFile saveFile)
    {
        Debug.Log("Save");
    }

    public static SaveFile Load()
    {
        SaveFile saveFile = new();
        Debug.Log("Load");
        return saveFile;
    }
}

[Serializable]
public struct SaveFile
{
    public bool[] Levers;
    public bool[] Doors;
    public int PlacedKeys;
    public int CollectedKeys;
    public Vector3 Checkpoint;
    public LevelState LevelState;
}