using UnityEngine;

public abstract class Door : MonoBehaviour
{
    [HideInInspector] public bool IsOpen = false;
    public abstract void Open();
    public abstract void ChangeDoorLook();
}