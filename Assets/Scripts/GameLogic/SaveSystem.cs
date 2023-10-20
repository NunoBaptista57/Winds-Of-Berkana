using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// TODO
public static class SaveSystem
{

    private const string saveName = "Save01.json";

    public static void Save(SaveFile saveFile)
    {
        string json = JsonUtility.ToJson(saveFile);
        Debug.Log(json);
        File.WriteAllText(Application.persistentDataPath + saveName, json);
    }

    public static SaveFile Load()
    {
        SaveFile saveFile;

        string loadSaveFile = File.ReadAllText(Application.persistentDataPath + saveName);
        Debug.Log(loadSaveFile);
        saveFile = JsonUtility.FromJson<SaveFile>(loadSaveFile);

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