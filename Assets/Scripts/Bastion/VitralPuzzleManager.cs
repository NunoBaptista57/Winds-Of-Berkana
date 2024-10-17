using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;


public class VitralPuzzleManager : MonoBehaviour
{

    private PlayerActions _playerActions;
    [HideInInspector] public bool IsActivated = false;

    public bool IsComplete = false;
    [SerializeField] GameObject[] puzzlePieces;
    [SerializeField] GameObject puzzle;
    private int _piecesCollected = 0;
    private int _pieceSelected = 0;

    private bool isNear = false;

    private bool isSolving = false;

    [Header("Canvas Objects")]
    public GameObject startInteractionText;
    public GameObject directionText;
    public GameObject puzzlePiecesText;
    [Header("CompleteImage")]
    public GameObject completedPanel;

    private SphereColor sphereController;

    private bool overworldCamera = true;
    [SerializeField]
    private CinemachineVirtualCamera vitralCam;


    [SerializeField]
    private CinemachineFreeLook playerCam;

    private float _rotationPiece = 0f;


    void Start()
    {
        sphereController = GameObject.Find("PuzzleSphere").GetComponent<SphereColor>();
    }


  public void Activate(InputAction.CallbackContext context)
    {
        if (!isNear || IsActivated)
        {
            return;
        }
        IsActivated = true;
        Debug.Log("Vitral Minigame started");
        SolvingPuzzle();
    }


private void Update(){
    if (isSolving && Mathf.Abs(_rotationPiece) > 0.1f)
    {
        RotatePiece(_rotationPiece * 2f);  // Adjust rotation sensitivity if needed
    }
}

public void OnMovement(InputAction.CallbackContext context){
    if (!isSolving) return;

    Vector2 movementInput = context.ReadValue<Vector2>();
    float horizontal = movementInput.x;  
    float vertical = movementInput.y;    

    if(context.performed){
            _rotationPiece = horizontal;
    }

    if (Mathf.Abs(vertical) > 0.1f)
    {
        SelectPiece(); 
    }    
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
        if (isSolving)
        {
            puzzlePieces[_pieceSelected].gameObject.transform.Rotate(0, 0, rotation);
            CheckPosition();
        }
    }
    // If players Press E they start trying to solve the Vitral
    public void SolvingPuzzle()
    {
        //directionText.SetActive(true);
        isSolving = true;
        SwitchPriority();
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
            //startInteractionText.gameObject.SetActive(true);
            //puzzlePiecesText.SetActive(true);
            //puzzlePiecesText.GetComponent<UnityEngine.UI.Text>().text = "Collected " + _piecesCollected + "/3 Puzzle Pieces";
            isNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //startInteractionText.gameObject.SetActive(false);
            //puzzlePiecesText.SetActive(false);
            //directionText.SetActive(false);
            isNear = false;
        }
    }
    // The objective of this function is to Check if the panels are in the correct Position
    public void CheckPosition()
    {
        Debug.Log("checking position");
        foreach (var p in puzzlePieces)
        {
            Debug.Log(p.transform.rotation.z);
            if (p.transform.rotation.z < -0.01 || p.transform.rotation.z > 0.01)
                return;
        }
        // Completed the panel
        CompletedVitral();
    }
    // Show the final panel, animations and walls going up should be called here
    public void CompletedVitral()
    {
        completedPanel.SetActive(true);
        IsComplete = true;
        puzzlePieces[0].SetActive(false);
        puzzlePieces[1].SetActive(false);
        puzzlePieces[2].SetActive(false);
        SwitchPriority();
        isSolving = false;
    }


    private void SwitchPriority()   
    {
         if (overworldCamera && IsComplete == false)
         {
             vitralCam.Priority = playerCam.Priority;
             playerCam.Priority = 0;
             overworldCamera = !overworldCamera;

         }

         else if (overworldCamera == false && IsComplete)
         {
             playerCam.Priority = vitralCam.Priority;
             vitralCam.Priority = 0;
             overworldCamera = !overworldCamera;

         }
    }

     private void OnEnable()
    {
        _playerActions = new();
        _playerActions.Character.Interact.started += Activate;
        _playerActions.Character.Move.performed += OnMovement;
        _playerActions.Enable();
    }

    private void OnDisable()
    {
        _playerActions.Character.Interact.started -= Activate;
        _playerActions.Character.Move.performed -= OnMovement;
        _playerActions.Disable();
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