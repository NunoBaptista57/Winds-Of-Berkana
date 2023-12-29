using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public List<Key> Keys = new();
    private BastionManager _bastionManager;

    public bool[] SaveKeys()
    {
        bool[] keys = new bool[Keys.Count];

        for (int i = 0; i < Keys.Count; i++)
        {
            keys[i] = Keys[i].Collected;
        }

        return keys;
    }

    public void LoadKeys(bool[] keys)
    {
        if (keys.Length != Keys.Count)
        {
            throw new UnityException("Error loading keys.");
        }

        for (int i = 0; i < keys.Length; i++)
        {
            Keys[i].Collected = true;
            Keys[i].gameObject.SetActive(false);
        }
    }

    public void CollectKey(string key)
    {
        _bastionManager.CollectKey(key);
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

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Key key))
            {
                Keys.Add(key);
            }
        }
    }
}
