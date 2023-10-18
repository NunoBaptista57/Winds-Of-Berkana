using System;
using UnityEngine;

public class TestLever : MonoBehaviour, ILever
{
    public int ID = 0;
    private bool _isActivated = false;
    private bool _toActivate = false;

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

        leverManager.AddLever(this);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<MainPlayerInputHandler>().Interact += Activate;
    }

    private void Activate()
    {
        _toActivate = true;
        ServiceLocator.instance.GetService<LeverManager>().UpdateLevers();
    }
}
