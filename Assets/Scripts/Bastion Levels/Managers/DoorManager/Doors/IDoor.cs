using System;
using UnityEngine;

public interface IDoor
{
    public void Open();
    public bool IsOpen();
    public void SetDoorManager(DoorManager doorManager);
}