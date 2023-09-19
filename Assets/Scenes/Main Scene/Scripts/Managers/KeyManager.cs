using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UIElements;

public class KeyManager
{
    [SerializeField] private List<IKey> _keys;
    [SerializeField] private List<IDoor> _doors;

    
}

// public class KeyManager : MonoBehaviour
// {
//     [SerializeField] private GameObject[] _keys;
//     private bool[] _piecesCollected;
//     private int _npiecesCollected;
//     [SerializeField] Sphere_Color _sphere;
//     [SerializeField] GameObject _door;

//     public event Action CollectedAllKeys;
//     public event Action<int> CollectedNKey;

//     private bool _allPiecesCollected;
//     private bool _doorUnlocked;

//     [Serializable]
//     public struct LevelChanges
//     {
//         public GameObject movingPiece;
//         public int pickedUpPieces;
//     }

//     [SerializeField] public LevelChanges[] levelChanges;

//     void Awake()
//     {
//         _piecesCollected = new bool[_keys.Length];

//         foreach(GameObject go in _keys)
//         {
//             _sphere.puzzle_piece.Add(go);
//             go.GetComponent<Key>().Collect += KeyCollected;
//         }
//     }


//     // Increase the Number of Puzzle Pieces Collected
//     public void KeyCollected(int key)
//     {
//         // Key number != index in the list
//         var index = key - 1;
//         _piecesCollected[index] = true;
//         _npiecesCollected += 1;

//         if(_npiecesCollected == _keys.Length)
//         {
//             _allPiecesCollected = true;
//             // When all pieces are collected send an event to whoever is listening
//             CollectedAllKeys?.Invoke();
//         }

//         //  When a piece is collected send an event to whoever is listening
//         CollectedNKey?.Invoke(key);
//         _sphere.RemoveKey(_keys[index]);
//         _sphere.GetClosestKey();
//         HandleLevelChange(key);
//     }


//     public void HandleLevelChange(int pickedPieceNumber)
//     {
//         var relevantObjects = levelChanges.ToList().FindAll(x => x.pickedUpPieces == pickedPieceNumber);
       
//         // Enact Changes in the Level
//         foreach(var r in relevantObjects)
//         {
//             // Ideally the have an Animator that plays on Trigger
//             r.movingPiece.GetComponent<Animator>().SetTrigger("Move");
//         }

//     }

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
