using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public List<IKey> AllKeys = new();

    public void UpdateKeys()
    {
        int n_keys = 0;

        for (int i = 0; i < AllKeys.Count; i++)
        {
            if (AllKeys[i].IsCollected())
            {
                n_keys++;
            }
        }

        ServiceLocator.instance.GetService<SphereColor>().UpdateKeys();
        ServiceLocator.instance.GetService<Bastion1Manager>().PickUpKey(n_keys);
    }

    public void AddKey(IKey key)
    {
        AllKeys.Add(key);
    }
}
