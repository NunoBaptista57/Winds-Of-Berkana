using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour, ISavable
{
    public List<Key> Keys = new();
    public int CollectedKeys = 0;

    public SaveFile Save(SaveFile saveFile)
    {
        saveFile.CollectedKeys = CollectedKeys;
        return saveFile;
    }

    public void Load(SaveFile saveFile)
    {
        for (int i = 0; i < saveFile.CollectedKeys; i++)
        {
            Keys[i].Collect();
        }
        CollectedKeys = saveFile.CollectedKeys;
    }

    public void UpdateKeys()
    {
        int nKeys = 0;

        foreach (Key key in Keys)
        {
            if (key.Collected)
            {
                nKeys++;
            }
        }

        CollectedKeys = nKeys;
        ServiceLocator.Instance.GetService<SphereColor>().UpdateKeys();
    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            Key key = child.gameObject.GetComponent<Key>();
            Keys.Add(key);
            key.KeyManager = this;
        }
    }
}
