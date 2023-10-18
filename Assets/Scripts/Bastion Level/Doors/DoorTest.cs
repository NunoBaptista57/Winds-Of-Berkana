using System;
using UnityEngine;

public class DoorTest : MonoBehaviour, IDoor
{
    private bool _isOpen = false;

    public void Open()
    {
        Debug.Log("Door Open");
        _isOpen = true;
    }

    public bool IsOpen()
    {
        return _isOpen;
    }

    private void Activate(LevelState levelState)
    {
        if (levelState == LevelState.WindTunnel)
        {
            Open();
        }
    }

    private void Start()
    {
        ServiceLocator.instance.GetService<Bastion1Manager>().OnLevelStateChanged += Activate;
    }
}