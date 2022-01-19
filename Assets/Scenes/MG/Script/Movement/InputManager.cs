using System;
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

    public bool jumpInput;
    public bool glideInput;
    public bool shootInput;
    public bool aimInput;
    public bool pickupInput = false;
    public bool runningInput = false;
    public bool flashInput = false;

    public Light flashlight;

    public Cinemachine.CinemachineVirtualCameraBase baseCamera;
    public Cinemachine.CinemachineVirtualCameraBase aimCamera;

    private Canvas aimCanvas;

    private void OnEnable()
    {
        animator = this.GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        aimCanvas = aimCamera.GetComponentInChildren<Canvas>();
        aimCanvas.enabled = false;
        if (playerControls == null)
        {
            Debug.Log("Awaken");
            playerControls = new PlayerActions();

            playerControls.Character.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Character.Jump.performed += i => jumpInput = true;
            playerControls.Character.Glide.performed += i => HandleGliding();
            playerControls.Character.Fire.performed += i => HandleShooting();
            playerControls.Character.Aim.started += i => aimInput = true;
            playerControls.Character.Aim.canceled += i => aimInput = false;
            playerControls.Character.Run.performed += i => runningInput = !runningInput;
            playerControls.Character.Flashlight.performed += i => HandleFlashlight();
            playerControls.Character.Reset.performed += i => RestartScene();
            playerControls.Character.Pickup.performed += i => pickup.HandleInteraction();
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleJumpingInput();
        HandleAiming();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void HandleMovementInput()
    {
        verticalInput = movementInput.x;
        horizontalInput = movementInput.y;

        moveAmount = Mathf.Clamp01(Math.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        animator.UpdateAnimatorValues(horizontalInput, verticalInput);

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
        }
    }


    private void HandleAiming()
    {

        if (aimInput == true)
        {
            aimCamera.Priority = 15;
            aimCanvas.enabled = true;

        }

        else if (aimInput == false)
        {
            aimCanvas.enabled = false;
            aimCamera.Priority = 5;
        }


       
    }
}
