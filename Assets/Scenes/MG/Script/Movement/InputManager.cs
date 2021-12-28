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

    private AnimatorManager animator;

    public bool jumpInput;
    public bool glideInput;

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
            playerControls.Character.Glide.performed += i => glideInput = true;
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
            
        if (glideInput == true)
        {
            Debug.Log("Gliding");
            glideInput = false;
            playerLocomotion.HandleGlide();
        }
    }
}
