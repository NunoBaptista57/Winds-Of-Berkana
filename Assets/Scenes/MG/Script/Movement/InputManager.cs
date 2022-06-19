using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

class InputManager : MonoBehaviour
{
    PlayerActions playerControls;
    PlayerLocomotion playerLocomotion;

    [Header("Debug Purposes Only")]
    public Vector2 movementInput;
    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public GrapplingHook gun;
    public Interact pickup;

    private AnimatorManager animator;
    private Camera _mainCamera;
    private VitralPuzzleManager _puzzle;

    public bool jumpInput;
    public bool dodgeInput;
    public bool glideInput;
    public bool shootInput;
    public bool aimInput;
    public bool visionInput;
    public bool pickupInput = false;
    public bool runningInput = false;
    public bool flashInput = false;
    public bool SolvingPuzzle = false;

    public Light flashlight;
    public int respawnTimer;

    public Cinemachine.CinemachineVirtualCameraBase baseCamera;
    public Cinemachine.CinemachineVirtualCameraBase aimCamera;
    public Cinemachine.CinemachineVirtualCameraBase deathCamera;

    private Canvas aimCanvas;
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
        animator = this.GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        aimCanvas = aimCamera.GetComponentInChildren<Canvas>();
       

        aimCanvas.enabled = false;
        if (playerControls == null)
        {
            playerControls = new PlayerActions();

            playerControls.Character.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Character.Jump.performed += i => jumpInput = true;
            playerControls.Character.Dodge.performed += i => dodgeInput = true;
            playerControls.Character.Glide.performed += i => HandleGliding();
          //  playerControls.Character.Fire.performed += i => HandleShooting();
          //  playerControls.Character.Aim.started += i => aimInput = true;
          //  playerControls.Character.Aim.canceled += i => aimInput = false;
            playerControls.Character.Run.performed += i => runningInput = !runningInput;
            playerControls.Character.Flashlight.performed += i => HandleFlashlight();
            playerControls.Character.Reset.performed += i => manager.UpdateGameState(GameState.Remake);
            playerControls.Character.Pickup.performed += i => pickup.HandleInteraction();
            playerControls.Character.Vision.performed += i => HandleVision();
            playerControls.Character.Interact.performed += i => HandleInteract();

            playerControls.Character.Pause.performed += i => manager.UpdateGameState(GameState.Paused);
        }

        playerControls.Enable();

        //stributes camera to variable
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        _puzzle = VitralPuzzleManager.Instance;
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
            HandleAiming();
            HandleDodgeInput();
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
        if (!SolvingPuzzle)
        {
            verticalInput = movementInput.x;
            horizontalInput = movementInput.y;
            moveAmount = Mathf.Clamp01(Math.Abs(horizontalInput) + Mathf.Abs(verticalInput));

            animator.UpdateAnimatorValues(horizontalInput, verticalInput);
        }
        else
        {
            //horizontalInput = movementInput.x;
          //  _puzzle.RotatePiece(movementInput.x);
        }
        

    }

    public void HandleFlashlight()
    {
        Debug.Log("Handling Flashlight" + flashInput);


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

    private void HandleDodgeInput()
    {
        if (dodgeInput == true)
        {
            dodgeInput = false;
            playerLocomotion.HandleDodge();
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

    private void HandleShooting()
    {
        if (shootInput == true)
        {
            shootInput = false;
            gun.StopGrapple();
        }

        else if (shootInput == false)
        {
            shootInput = true;
            gun.StartGrapple();
            Time.timeScale = 1f;
        }
    }


    private void HandleAiming()
    {

        if (aimInput == true)
        {
            aimCamera.Priority = 15;
            aimCanvas.enabled = true;
            Time.timeScale = 0.5f;

        }

        else if (aimInput == false)
        {
            aimCanvas.enabled = false;
            aimCamera.Priority = 5;
            Time.timeScale = 1f;
        }
    }

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
        SolvingPuzzle = !SolvingPuzzle;
    }
}
