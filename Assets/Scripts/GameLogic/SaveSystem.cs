using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private List<GameObject> _toSave = new();
    private readonly List<ISavable> _savables = new();
    private const string saveName = "Save01.json";

    public void Save()
    {
        SaveFile saveFile = new();

        foreach (ISavable savable in _savables)
        {
            saveFile = savable.Save(saveFile);
        }

        string json = JsonUtility.ToJson(saveFile);
        Debug.Log(json);
        File.WriteAllText(Application.persistentDataPath + saveName, json);
    }

    public void Load()
    {

        string loadSaveFile = File.ReadAllText(Application.persistentDataPath + saveName);
        Debug.Log(loadSaveFile);
        SaveFile saveFile = JsonUtility.FromJson<SaveFile>(loadSaveFile);

        foreach (ISavable savable in _savables)
        {
            savable.Load(saveFile);
        }
    }

    private void Start()
    {
        foreach (GameObject _gameObject in _toSave)
        {
            if (!TryGetComponent<ISavable>(out ISavable savable))
            {
                Debug.Log(_gameObject.name + " has no ISavable interface.");
            }
            _savables.Add(savable);
        }
    }
}

[Serializable]
public struct SaveFile
{
    public bool[] Levers;
    public bool[] Doors;
    public int PlacedKeys;
    public int CollectedKeys;
    public bool VitralIsComplete;
    public Vector3 Checkpoint;
}