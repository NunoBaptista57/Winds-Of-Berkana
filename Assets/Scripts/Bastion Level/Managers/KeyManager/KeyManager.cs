using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public List<IKey> AllKeys = new();
    public int CollectedKeys = 0;

    public void UpdateKeys()
    {
        int nKeys = 0;

        for (int i = 0; i < AllKeys.Count; i++)
        {
            if (AllKeys[i].IsCollected())
            {
                nKeys++;
            }
        }

        CollectedKeys = nKeys;
        ServiceLocator.instance.GetService<SphereColor>().UpdateKeys();
        ServiceLocator.instance.GetService<Bastion1Manager>().PickUpKey(nKeys);
    }

    public void AddKey(IKey key)
    {
        AllKeys.Add(key);
    }
}
