using System;
using UnityEngine;

public class TestLever : MonoBehaviour, ILever
{
    public int ID = 0;
    private bool _isActivated = false;
    private bool _toActivate = false;
    private bool _playerIsNear = false;

    public bool ToActivate()
    {
        return _toActivate;
    }

    public void SetActivated(bool activated)
    {
        _isActivated = activated;
    }

    public bool IsActivated()
    {
        return _isActivated;
    }

    public int GetID()
    {
        return ID;
    }

    private void Start()
    {
        LeverManager leverManager = ServiceLocator.instance.GetService<LeverManager>();

        if (ID != 0)
        {
            ID = leverManager.Levers.Count;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<MainPlayerInputHandler>().Interact += Activate;
    }

    private void Activate()
    {
        if (!_playerIsNear)
        {
            return;
        }
        _toActivate = true;
        ServiceLocator.instance.GetService<LeverManager>().UpdateLevers();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerIsNear = false;
        }
    }
}
