using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public List<IDoor> Doors = new();

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
