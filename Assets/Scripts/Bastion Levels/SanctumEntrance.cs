using System.Collections.Generic;
using UnityEngine;

public class SanctumEntrance : MonoBehaviour, ISavable
{
    public int NKeysToOpen = 0;
    [HideInInspector] public int PlacedKeys = 0;

    private List<GameObject> _altars = new();
    private bool _playerIsNear = false;

    public SaveFile Save(SaveFile saveFile)
    {
        saveFile.PlacedKeys = PlacedKeys;
        return saveFile;
    }

    public void Load(SaveFile saveFile)
    {
        PlacedKeys = saveFile.PlacedKeys;
    }

    public void UpdateKeys()
    {
        if (!_playerIsNear)
        {
            return;
        }

        if (ServiceLocator.Instance.GetService<KeyManager>().CollectedKeys <= 0)
        {
            return;
        }

        PlaceKeys();
    }

    public void PlaceKeys()
    {
        for (int i = PlacedKeys; i < ServiceLocator.Instance.GetService<KeyManager>().CollectedKeys; i++)
        {
            ServiceLocator.Instance.GetService<Bastion1Manager>().PickUpKey(i + 1);
            PlaceKey(_altars[i]);
            PlacedKeys++;
        }
    }
    private void Start()
    {
        if (NKeysToOpen == 0)
        {
            NKeysToOpen = transform.childCount;
        }

        foreach (Transform child in transform)
        {
            _altars.Add(child.gameObject);
        }

        // TODO FIX
        // ServiceLocator.instance.GetService<MainPlayerInputHandler>().Interact += UpdateKeys;
    }

    private void PlaceKey(GameObject altar)
    {
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
}