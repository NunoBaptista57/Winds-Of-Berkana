using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KeyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _keys;
    private bool[] _piecesCollected;
    private int _npiecesCollecetd;
    [SerializeField] Sphere_Color _sphere;
    [SerializeField] GameObject _door;

    public event Action KeysCollected;

    private bool _allPiecesCollected;
    private bool _doorUnlocked;

    /*[Header("Canvas Objects")]
    public GameObject startInteractionText;
    public GameObject directionText;
    public GameObject puzzlePiecesText;

    [Header("CompleteImage")]
    public GameObject completedPanel;*/

    //private Sphere_Color sphereController;


    void Awake()
    {
        //sphereController = GameObject.Find("PuzzleSphere").GetComponent<Sphere_Color>();
        _piecesCollected = new bool[_keys.Length];

        foreach(GameObject go in _keys)
        {
            _sphere.puzzle_piece.Add(go);
            go.GetComponent<Key>().Collect += KeyCollected;
        }
    }


    // Increase the Number of Puzzle Pieces Collected
    public void KeyCollected(int key)
    {
        /*_keys[peca].gameObject.SetActive(true);
        _piecesCollected += 1;
        sphereController.NextSphere();*/

        _piecesCollected[key] = true;
        _npiecesCollecetd += 1;

        if(_npiecesCollecetd == _keys.Length)
        {
            _allPiecesCollected = true;
            KeysCollected?.Invoke();
        }

        _sphere.RemoveKey(_keys[key]);
        _sphere.GetClosestKey();
    }



    //  Triger Enter and Exit to check if Player is near the Vitral
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            startInteractionText.gameObject.SetActive(true);
            puzzlePiecesText.SetActive(true);
            puzzlePiecesText.GetComponent<UnityEngine.UI.Text>().text = "Collected " + _piecesCollected + "/3 Puzzle Pieces";
            isNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            startInteractionText.gameObject.SetActive(false);
            puzzlePiecesText.SetActive(false);
            directionText.SetActive(false);
            isNear = false;
        }
    }*/


    // The objective of this function is to Check if the panels are in the correct Position
    public void CheckPosition()
    {
        /*if (_piecesCollected == 3)
        {
            foreach (var p in _keys)
            {
                if (p.transform.rotation.z <= -5 && p.transform.rotation.z >= 5)
                    return;

            }
            // Completed the panel
            CompletedVitral();
        }*/

    }

    // Show the final panel, animations and walls going up should be called here
    public void CompletedVitral()
    {
        //completedPanel.SetActive(true);
    }


    #region Singleton

    private static KeyManager _instance;
    public static KeyManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<KeyManager>();
            return _instance;
        }
    }

    #endregion
}
