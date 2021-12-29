using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class InputManager : MonoBehaviour
{
    PlayerActions playerControls;
    PlayerLocomotion playerLocomotion;

    public Vector2 movementInput;
    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;
    public GrapplingHook gun;

    private AnimatorManager animator;

    public bool jumpInput;
    public bool glideInput;
    public bool shootInput;
    public bool aimInput;

    public Cinemachine.CinemachineVirtualCameraBase baseCamera;
    public Cinemachine.CinemachineVirtualCameraBase aimCamera;

    private void OnEnable()
    {
        animator = this.GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        if (playerControls == null)
        {
            Debug.Log("Awaken");
            playerControls = new PlayerActions();

            playerControls.Character.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Character.Jump.performed += i => jumpInput = true;
            playerControls.Character.Glide.performed += i => HandleGlidingInput();
            playerControls.Character.Fire.performed += i => HandleShooting();
            playerControls.Character.Aim.started += i => aimInput = true;
            playerControls.Character.Aim.canceled += i => aimInput = false;
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
        HandleGlidingInput();
        HandleAiming();
    }


    public void HandleMovementInput()
    {
        verticalInput = movementInput.x;
        horizontalInput = movementInput.y;

        moveAmount = Mathf.Clamp01(Math.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        animator.UpdateAnimatorValues(horizontalInput, verticalInput);

    }


    private void HandleJumpingInput()
    {
        if (jumpInput == true)
        {
            jumpInput = false;
            playerLocomotion.HandleJumping();
        }
    }

    private void HandleGlidingInput()
    {
        if (jumpInput == false)
            return;

        if (playerLocomotion.isJumping && glideInput == false)
        {
            glideInput = true;
            playerLocomotion.HandleGlide();

        }
        else if (glideInput == true)
        {
            glideInput = false;
            playerLocomotion.HandleGlide();
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
            
        }

        else if (aimInput == false)
        {
            aimCamera.Priority = 5;
        }


       
    }
}
