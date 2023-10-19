using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour, IManager
{
    public List<IKey> Keys = new();
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
    }

    public void UpdateKeys()
    {
        int nKeys = 0;

        foreach (IKey key in Keys)
        {
            if (key.IsCollected())
            {
                nKeys++;
            }
        }

        CollectedKeys = nKeys;
        ServiceLocator.instance.GetService<SphereColor>().UpdateKeys();
    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            IKey key = child.gameObject.GetComponent<IKey>();
            Keys.Add(key);
            key.SetKeyManager(this);
        }
    }
}
