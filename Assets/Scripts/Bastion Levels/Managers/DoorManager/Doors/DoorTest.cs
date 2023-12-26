using System;
using UnityEngine;

public class DoorTest : MonoBehaviour, IDoor
{
    private bool _isOpen = false;
    private DoorManager _doorManager;

    public void SetDoorManager(DoorManager doorManager)
    {
        _doorManager = doorManager;
    }

    public void Open()
    {
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
        ServiceLocator.Instance.GetService<Bastion1Manager>().OnLevelStateChanged += Activate;
    }

    private void OnDisable()
    {
        ServiceLocator.Instance.GetService<Bastion1Manager>().OnLevelStateChanged -= Activate;
    }
}