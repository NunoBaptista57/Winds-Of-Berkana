using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VitralPuzzleManager : MonoBehaviour
{
    public bool IsComplete = false;
    [SerializeField] GameObject[] puzzlePieces;
    [SerializeField] GameObject puzzle;
    private int _piecesCollected = 0;

    private int _pieceSelected = 0;

    private bool isNear = false;

    [Header("Canvas Objects")]
    public GameObject startInteractionText;
    public GameObject directionText;
    public GameObject puzzlePiecesText;

    [Header("CompleteImage")]
    public GameObject completedPanel;

    private SphereColor sphereController;


    void Start()
    {
        sphereController = GameObject.Find("PuzzleSphere").GetComponent<SphereColor>();
    }


    // Increase the Number of Puzzle Pieces Collected
    public void PuzzleCollected()
    {
        puzzlePieces[_piecesCollected].gameObject.SetActive(true);
        _piecesCollected += 1;
        sphereController.GetClosestKey();

    }

    // Rotating the Vitral using movement controls
    public void RotatePiece(float rotation)
    {
        if (isNear)
        {
            puzzlePieces[_pieceSelected].gameObject.transform.Rotate(0, 0, rotation);
            CheckPosition();
        }
    }

    // If players Press E they start trying to solve the Vitral
    public void SolvingPuzzle()
    {
        directionText.SetActive(true);
    }


    // Select which Piece of the Vitral is being controlled
    public void SelectPiece()
    {
        if (_pieceSelected == puzzlePieces.Length - 1)
        {
            puzzlePieces[_pieceSelected].gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 127, 0.7f);
            _pieceSelected = 0;
        }
        else
        {
            puzzlePieces[_pieceSelected].gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 127, 0.7f);
            _pieceSelected += 1;
        }

        puzzlePieces[_pieceSelected].gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 127, 255);
    }



    //  Triger Enter and Exit to check if Player is near the Vitral
    private void OnTriggerEnter(Collider other)
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
    }


    // The objective of this function is to Check if the panels are in the correct Position
    public void CheckPosition()
    {
        if (_piecesCollected == 3)
        {
            foreach (var p in puzzlePieces)
            {
                if (p.transform.rotation.z <= -5 && p.transform.rotation.z >= 5)
                    return;

            }
            // Completed the panel
            CompletedVitral();
        }

    }

    // Show the final panel, animations and walls going up should be called here
    public void CompletedVitral()
    {
        completedPanel.SetActive(true);
        IsComplete = true;
    }


    #region Singleton

    private static VitralPuzzleManager _instance;
    public static VitralPuzzleManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<VitralPuzzleManager>();
            return _instance;
        }
    }

    #endregion
}
