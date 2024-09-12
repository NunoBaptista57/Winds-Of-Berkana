// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using Cinemachine;
// using TMPro;

// public class MainVitralManager : MonoBehaviour
// {

//     [SerializeField] GameObject[] puzzlePieces;
//     private int _pieceSelected = 0;

//     private bool isNear = false;
//     private bool isInteracting = false;


//     [SerializeField]
//     private CinemachineFreeLook playerCam;

//     [SerializeField]
//     private CinemachineVirtualCamera vitralCam;

//     private bool overworldCamera = true;
//     private bool panelIsComplete = false;

//     [Header("Canvas Objects")]
//     public TextMeshProUGUI infoText;

//     [Header("CompleteImage")]
//     public GameObject completedPanel;

//     private MainPlayerInputHandler player;

//     private float delaySelectingTime;
//     public float timeHolder;

//     private void Start()
//     {
//     }

//     // Rotating the Vitral using movement controls
//     private void RotatePiece(float rotation)
//     {
//         if (isInteracting)
//         {
//             puzzlePieces[_pieceSelected].transform.Rotate(0, 0, rotation);
//             CheckPosition();
//         }
//     }

//     // If players Press E they start trying to solve the Vitral
//     private void SolvingPuzzle()
//     {
//         infoText.text = "Solving Puzzle";
//         SwitchPriority();

//     }

//     private void FixedUpdate()
//     {
//         if (delaySelectingTime > 0)
//         {
//             delaySelectingTime -= Time.deltaTime;
//         }

//         if (!isNear) return;

//         if (isInteracting)
//         {
//             // Way too sensitive
//             if (player.movementInput.x > 0.5f || player.movementInput.x < -0.5f)
//             {
//                 RotatePiece(player.movementInput.x);
//             }

//             if ((player.movementInput.y > 0.5f || player.movementInput.y < -0.5f) && delaySelectingTime <= 0)
//             {
//                 delaySelectingTime = timeHolder;
//                 SelectPiece();
//             }
//         }
//     }


//     // Select which Piece of the Vitral is being controlled
//     private void SelectPiece()
//     {
//         if (_pieceSelected == puzzlePieces.Length - 1)
//         {
//             puzzlePieces[_pieceSelected].gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 127, 0.7f);
//             _pieceSelected = 0;
//         }
//         else
//         {
//             puzzlePieces[_pieceSelected].gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 127, 0.7f);
//             _pieceSelected += 1;
//         }

//         puzzlePieces[_pieceSelected].gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 127, 255);
//     }



//     //  Triger Enter and Exit to check if Player is near the Vitral
//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.gameObject.CompareTag("Player"))
//         {
//             infoText.gameObject.SetActive(true);
//             isNear = true;
//         }
//     }

//     private void OnTriggerExit(Collider other)
//     {
//         if (other.gameObject.CompareTag("Player"))
//         {
//             infoText.gameObject.SetActive(false);
//             isNear = false;
//             isInteracting = false;
//         }
//     }


//     // The objective of this function is to Check if the panels are in the correct Position
//     private void CheckPosition()
//     {
//         foreach (var p in puzzlePieces)
//         {
//             float currentRot = p.transform.rotation.eulerAngles.z;
//             Debug.Log(currentRot);
//             while (currentRot < -360f || currentRot > 360f)
//             {
//                 if (currentRot < -360f)
//                     currentRot += 360f;
//                 else if (currentRot > 360f)
//                     currentRot -= 360f;
//             }

//             Debug.Log(currentRot);
//             if (currentRot <= -5f || currentRot >= 5f)
//             {
//                 Debug.Log("esta a entrar");
//                 return;
//             }

//         }
//         // Completed the panel
//         CompletedVitral();


//     }

//     // Show the final panel, animations and walls going up should be called here
//     private void CompletedVitral()
//     {
//         completedPanel.SetActive(true);
//         puzzlePieces[0].SetActive(false);
//         puzzlePieces[1].SetActive(false);
//         puzzlePieces[2].SetActive(false);
//         panelIsComplete = true;
//         SwitchPriority();
//         player.SolvingPuzzle = false;
//         infoText.text = "Congratulations \n You have finished this Demo";
//     }


//     private void HandlePlayerInteraction()
//     {
//         if (isNear)
//         {
//             isInteracting = true;
//             player.SolvingPuzzle = true;
//             SolvingPuzzle();
//         }
//     }

//     private void SwitchPriority()
//     {
//         if (overworldCamera && panelIsComplete == false)
//         {
//             vitralCam.Priority = playerCam.Priority;
//             playerCam.Priority = 0;
//             overworldCamera = !overworldCamera;

//         }

//         else if (overworldCamera == false && panelIsComplete)
//         {
//             playerCam.Priority = vitralCam.Priority;
//             vitralCam.Priority = 0;
//             overworldCamera = !overworldCamera;

//         }
//     }
// }
