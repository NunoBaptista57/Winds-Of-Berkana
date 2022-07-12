using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class MainVitralManager : MonoBehaviour
{

    [SerializeField] GameObject[] puzzlePieces;
    private int _pieceSelected = 0;

    private bool isNear = false;
    private bool isInteracting = false;

    [SerializeField]
    private CinemachineFreeLook playerCam;
    
    [SerializeField]
    private CinemachineVirtualCamera vitralCam;
    
    [Header("Canvas Objects")]
    public Text infoText;

    [Header("CompleteImage")]
    public GameObject completedPanel;

    private MainPlayerInputHandler player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MainPlayerInputHandler>();
        player.Interact += HandlePlayerInteraction;
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
        infoText.text = "Solving Puzzle";
    }

    private void FixedUpdate()
    {
        if (!isNear) return;

        if (isInteracting)
            RotatePiece(player.movementInput.x);

        if (player.movementInput.y > 0)
            SelectPiece();
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
            infoText.gameObject.SetActive(true);
            isNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            infoText.gameObject.SetActive(false);
            isNear = false;
            isInteracting = false;
        }
    }


    // The objective of this function is to Check if the panels are in the correct Position
    public void CheckPosition()
    {
            foreach (var p in puzzlePieces)
            {
                if (p.transform.rotation.z <= -5 && p.transform.rotation.z >= 5)
                    return;

            }
            // Completed the panel
            CompletedVitral();
        

    }

    // Show the final panel, animations and walls going up should be called here
    public void CompletedVitral()
    {
        SwitchPriority();
        completedPanel.SetActive(true);
        infoText.text = "Congratulations \n You have finished this Demo";
    }


    public void HandlePlayerInteraction()
    {
        if (isNear)
        {
            SwitchPriority();
            isInteracting = true;
        }
    }

    private void SwitchPriority()
    {
        if (isInteracting)
        {
            playerCam.Priority = 11;
            vitralCam.Priority = 10;
        }

        else
        {
            playerCam.Priority = 10;
            vitralCam.Priority = 11;
        }
    }
}
