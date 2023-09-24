using System;
using UnityEngine;

public class DoorTest : MonoBehaviour, IDoor
{
    public void Open()
    {

    }

    public bool CanOpen()
    {
        return false;
    }

    public bool IsOpen()
    {
        return false;
    }
}