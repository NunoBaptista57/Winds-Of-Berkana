using System;
using System.Collections.Generic;
using UnityEngine;

public class SanctumEntrance : MonoBehaviour
{
    public int KeysToOpen = 0;
    [HideInInspector] public int PlacedKeys = 0;
    private BastionManager _bastionManager;
    private List<GameObject> _altars = new();
    private bool _playerIsNear = false;

    // TODO: interaction

    public void LoadAltars(int placedKeys)
    {
        PlacedKeys = placedKeys;
    }

    public void OpenSanctum()
    {
        _bastionManager.OpenSanctum();
    }

    public void PlaceKeys()
    {
        int collectedKeys = _bastionManager.GetCollectedKeys();
        for (int i = 0; i < collectedKeys; i++)
        {
            PlaceKey(_altars[i]);
        }
        if (PlacedKeys == KeysToOpen)
        {
            OpenSanctum();
        }
    }

    private void PlaceKey(GameObject altar)
    {
        PlacedKeys++;
        Debug.Log("Placed key.");
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

    private void Awake()
    {
        if (_bastionManager != null && transform.parent.TryGetComponent(out BastionManager bastionManager))
        {
            _bastionManager = bastionManager;
        }
        else if (_bastionManager != null)
        {
            Debug.Log("KeyManager: BastionManager not found. Using ServiceLocator...");
            _bastionManager = ServiceLocator.Instance.GetService<BastionManager>();
        }
    }
}