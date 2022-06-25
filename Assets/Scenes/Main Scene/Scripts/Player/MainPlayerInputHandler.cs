using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerInputHandler : MonoBehaviour
{
    PlayerActions playerControls;
    MainPlayerLocomotion playerLocomotion;

    [Header("Debug Purposes Only")]
    public Vector2 movementInput;
    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public Interact pickup;

    private MainPlayerAnimationManager animator;
    private Camera _mainCamera;
    private VitralPuzzleManager _puzzle;

    public bool jumpInput;
    public bool glideInput;
    public bool visionInput;
    public bool pickupInput = false;
    public bool runningInput = false;
    public bool flashInput = false;
    public bool SolvingPuzzle = false;

    public Light flashlight;
    public int respawnTimer;

    private MainGameManager manager;


    void Start()
    {

        manager = MainGameManager.Instance;

        MainGameManager.OnGameStateChanged += GameManagerOnGameStateChanged;

    }

    // Its good practice to unsubscribe from events
    void OnDestroy()
    {
        MainGameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void OnEnable()
    {
        animator = this.GetComponent<MainPlayerAnimationManager>();
        playerLocomotion = GetComponent<MainPlayerLocomotion>();
        if (playerControls == null)
        {
            playerControls = new PlayerActions();

            playerControls.Character.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Character.Jump.performed += i => jumpInput = true;
            playerControls.Character.Glide.performed += i => HandleGliding();
            playerControls.Character.Run.performed += i => runningInput = !runningInput;
            playerControls.Character.Flashlight.performed += i => HandleFlashlight();
            playerControls.Character.Reset.performed += i => manager.UpdateGameState(GameState.Remake);
            //playerControls.Character.Pickup.performed += i => pickup.HandleInteraction();
            playerControls.Character.Vision.performed += i => HandleVision();
            playerControls.Character.Interact.performed += i => HandleInteract();
            playerControls.Character.Pause.performed += i => manager.UpdateGameState(GameState.Paused);
            playerControls.Enable();
        }

        //stributes camera to variable
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        if (manager.State != GameState.Paused)
        {
            HandleMovementInput();
            HandleJumpingInput();
        }
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        if (state == GameState.Play)
        {
            Reset();
        }
    }

    public void Reset()
    {
        // Reset all movement 
    }


    public void HandleMovementInput()
    {
        if (MainGameManager.Instance.State == GameState.Play)
        {
            verticalInput = movementInput.x;
            horizontalInput = movementInput.y;
            moveAmount = Mathf.Clamp01(Math.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            animator.UpdateAnimatorValues(horizontalInput, verticalInput);
        }

    }

    public void HandleFlashlight()
    {
        if (!flashInput)
        {
            flashlight.enabled = true;
            flashInput = true;
        }
        else
        {
            flashlight.enabled = false;
            flashInput = false;
        }
    }


    private void HandleJumpingInput()
    {
        if (jumpInput == true)
        {
            jumpInput = false;
            playerLocomotion.HandleJumping();
        }
    }

    private void HandleGliding()
    {

        if (!playerLocomotion.isGliding)
        {
            playerLocomotion.ActivateGlide();

        }
        else if (playerLocomotion.isGliding)
        {
            playerLocomotion.DeactivateGlide();
        }

    }


    // These methods should probably be somewhere else
    private void HandleVision()
    {
        if (!SolvingPuzzle)
        {
            _mainCamera.enabled = !_mainCamera.enabled;
            _puzzle.SolvingPuzzle();
        }
        else
        {
            _puzzle.SelectPiece();
        }

    }

    private void HandleInteract()
    {
        //SolvingPuzzle = !SolvingPuzzle;
    }
}
