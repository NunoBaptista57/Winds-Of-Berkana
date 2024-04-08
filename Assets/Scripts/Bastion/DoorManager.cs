using System.Collections.Generic;
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
                Doors[i].ChangeDoorToOpen();
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
