using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class KeyManager : MonoBehaviour
{
    public List<Key> Keys = new();
    public UnityEvent<Key> CollectedKeyEvent;

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

    public void CollectKey(Key key)
    {
        CollectedKeyEvent.Invoke(key);
    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Key key))
            {
                Keys.Add(key);
            }
        }
    }
}