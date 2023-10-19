using System.Collections.Generic;
using UnityEngine;

public class SanctumEntrance : MonoBehaviour
{
    public int NKeysToOpen = 0;
    [HideInInspector] public int PlacedKeys = 0;
    [HideInInspector] public bool _open = false;

    private List<GameObject> _altars = new();

    public void UpdateKeys()
    {
        KeyManager keyManager = ServiceLocator.instance.GetService<KeyManager>();

        if (keyManager.CollectedKeys <= 0)
        {
            return;
        }

        for (int i = PlacedKeys; i < keyManager.CollectedKeys; i++)
        {

            PlaceKey(_altars[i]);
            PlacedKeys++;
        }

        if (PlacedKeys == NKeysToOpen)
        {
            OpenSanctum();
        }
    }

    public void OpenSanctum()
    {
        Debug.Log("Open Sanctum");
        _open = true;
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

        ServiceLocator.instance.GetService<MainPlayerInputHandler>().Interact += UpdateKeys;
    }

    private void PlaceKey(GameObject altar)
    {
        Debug.Log("Placed");
    }
}