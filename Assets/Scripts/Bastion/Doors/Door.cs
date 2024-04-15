using UnityEngine;

public abstract class Door : MonoBehaviour
{
    [HideInInspector] public bool IsOpen = false;

    public abstract void ChangeDoorToOpen();
    public abstract void PlayOpenDoorAnimation();
    public void Open()
    {
        IsOpen = true;
        PlayOpenDoorAnimation();
    }
}