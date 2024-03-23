using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class SanctumEntrance : MonoBehaviour
{
    public int KeysToOpen = 0;
    [SerializeField] private List<GameObject> _altars = new();
    [HideInInspector] public int PlacedKeys = 0;
    private BastionManager _bastionManager;
    private bool _playerIsNear = false;
    private PlayerActions _playerActions;

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
        for (int i = PlacedKeys; i < collectedKeys; i++)
        {
            PlaceKey(_altars[i]);
        }
        if (PlacedKeys == KeysToOpen)
        {
            OpenSanctum();
        }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (context.started && _playerIsNear)
        {
            PlaceKeys();
        }
    }

    private void PlaceKey(GameObject altar)
    {
        PlacedKeys++;
        Debug.Log("Key placed");
    }

    private void OnTriggerEnter(Collider other)
    {
        _playerIsNear = other.gameObject.CompareTag("Player");
        if (_bastionManager.GetCollectedKeys() > PlacedKeys)
        {
            Debug.Log("Press E to place key");
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
        if (_bastionManager == null && transform.parent.TryGetComponent(out BastionManager bastionManager))
        {
            _bastionManager = bastionManager;
        }
        else if (_bastionManager == null)
        {
            Debug.Log("KeyManager: BastionManager not found. Using ServiceLocator...");
            _bastionManager = ServiceLocator.Instance.GetService<BastionManager>();
        }
    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        _playerActions = new();
        _playerActions.Character.Interact.started += Interact;
        _playerActions.Enable();
    }

    private void OnDisable()
    {
        _playerActions.Character.Interact.started -= Interact;
        _playerActions.Disable();
    }
}