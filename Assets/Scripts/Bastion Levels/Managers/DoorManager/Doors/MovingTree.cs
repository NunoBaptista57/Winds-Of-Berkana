using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class MovingTree : MonoBehaviour, IDoor
{
    private bool _isOpen = false;
    private DoorManager _doorManager;

    public void SetDoorManager(DoorManager doorManager)
    {
        _doorManager = doorManager;
    }

    public void Open()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Move");
        _isOpen = true;
    }

    public bool IsOpen()
    {
        return _isOpen;
    }

    private void Start()
    {
        ServiceLocator.Instance.GetService<Bastion1Manager>().OnLevelStateChanged += CheckState;
    }

    private void OnDestroy()
    {
        ServiceLocator.Instance.GetService<Bastion1Manager>().OnLevelStateChanged -= CheckState;
    }

    private void CheckState(LevelState levelState)
    {
        if (!_isOpen && levelState == LevelState.BastionState_Puzzle1)
        {
            Open();
        }
    }
}