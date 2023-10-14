// TODO: put this somewhere

//     // The objective of this function is to Check if the panels are in the correct Position
//     public void CheckPanelPosition()
//     {
//         /*if (_piecesCollected == 3)
//         {
//             foreach (var p in _keys)
//             {
//                 if (p.transform.rotation.z <= -5 && p.transform.rotation.z >= 5)
//                     return;

//             }
//             // Completed the panel
//             CompletedVitral();
//         }*/

//     }

//     // Show the final panel, animations and walls going up should be called here
//     public void CompletedVitral()
//     {
//         //completedPanel.SetActive(true);
//     }


//     #region Singleton

//     private static KeyManager _instance;
//     public static KeyManager Instance
//     {
//         get
//         {
//             if (_instance == null) _instance = FindObjectOfType<KeyManager>();
//             return _instance;
//         }
//     }

//     #endregion
// }
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering;

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
        ServiceLocator.instance.GetService<Bastion1LevelManager>().PickUpKey(n_keys);
    }

    public void AddKey(IKey key)
    {
        AllKeys.Add(key);
    }
}
