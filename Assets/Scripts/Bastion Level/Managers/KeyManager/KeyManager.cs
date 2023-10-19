using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public List<IKey> Keys = new();
    public int CollectedKeys = 0;

    public void UpdateKeys()
    {
        int nKeys = 0;

        for (int i = 0; i < Keys.Count; i++)
        {
            if (Keys[i].IsCollected())
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
