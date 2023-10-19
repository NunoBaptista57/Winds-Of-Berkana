using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class DoorManager : MonoBehaviour, IManager
{
    public List<IDoor> Doors = new();

    public SaveFile Save(SaveFile saveFile)
    {
        bool[] doors = new bool[Doors.Count];

        for (int i = 0; i < Doors.Count; i++)
        {
            doors[i] = Doors[i].IsOpen();
        }

        saveFile.Doors = doors;

        return saveFile;
    }

    public void Load(SaveFile saveFile)
    {
        for (int i = 0; i < Doors.Count; i++)
        {
            if (saveFile.Doors[i])
            {
                Doors[i].Open();
            }
        }
    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            IDoor door = child.gameObject.GetComponent<IDoor>();
            Doors.Add(door);
            door.SetDoorManager(this);
        }
    }
}
