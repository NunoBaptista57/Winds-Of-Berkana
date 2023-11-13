using System;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public int ID = 0;
    [HideInInspector] public LeverManager LeverManager;
    [HideInInspector] public bool IsActivated = false;
    [HideInInspector] public bool DoorOpened = false;
    private bool _playerIsNear = false;

    private void Start()
    {
        LeverManager leverManager = ServiceLocator.instance.GetService<LeverManager>();

        if (ID != 0)
        {
            ID = leverManager.Levers.Count;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        // TODO FIX
        // player.GetComponent<MainPlayerInputHandler>().Interact += Activate;
    }

    private void Activate()
    {
        if (!_playerIsNear || IsActivated)
        {
            return;
        }
        IsActivated = true;
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