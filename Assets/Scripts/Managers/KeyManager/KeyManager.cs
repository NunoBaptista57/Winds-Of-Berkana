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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyManager : MonoBehaviour
{

    private List<IDoor> _allDoors;
    private List<IKey> _allKeys;
    private bool[] _collectedKeys;
    private bool[] _openDoors;

    private void Awake()
    {
        _allDoors = new();
        _allKeys = new();

        foreach (IDoor door in transform.Find("Doors").GetComponentsInChildren<IDoor>())
        {
            _allDoors.Add(door);
        }

        foreach (IKey key in transform.Find("Keys").GetComponentsInChildren<IKey>())
        {
            _allKeys.Add(key);
        }

        _collectedKeys = new bool[_allKeys.Count];
        _openDoors = new bool[_allDoors.Count];

    }

    public void UpdateValues()
    {
        for (int i = 0; i < _allKeys.Count; i++)
        {
            if (_allKeys[i].IsCollected())
            {
                _collectedKeys[i] = true;
            }
        }

        for (int i = 0; i < _allDoors.Count; i++)
        {
            IDoor door = _allDoors[i];

            if (door.CanOpen() && !door.IsOpen())
            {
                door.Open();
                _openDoors[i] = true;
            }
        }
    }
}
