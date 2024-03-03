using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public List<Door> Doors = new();

    public bool[] SaveDoors()
    {
        bool[] doors = new bool[Doors.Count];

        for (int i = 0; i < Doors.Count; i++)
        {
            doors[i] = Doors[i].IsOpen;
        }
        return doors;
    }

    public void LoadDoors(bool[] doors)
    {
        for (int i = 0; i < Doors.Count; i++)
        {
            if (doors[i])
            {
                Doors[i].IsOpen = true;
                Doors[i].ChangeDoorLook();
            }
        }
    }

    public void OpenDoor(string doorName)
    {
        foreach (Door door in Doors)
        {
            if (door.gameObject.name == doorName)
            {
                door.Open();
            }
        }
    }

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Door door))
            {
                Doors.Add(door);
            }
        }
    }
}
