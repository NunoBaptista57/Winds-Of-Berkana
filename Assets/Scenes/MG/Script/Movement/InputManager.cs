using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

    class InputManager : MonoBehaviour
    {
    PlayerActions playerControls;

    public Vector2 movementInput;
    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    private AnimatorManager animator;

    private void OnEnable()
    {
        animator = this.GetComponent<AnimatorManager>();
        if(playerControls == null)
        {
            Debug.Log("Awaken");
            playerControls = new PlayerActions();

            playerControls.Character.Move.performed += i => movementInput = i.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    public void HandleAllInputs()
    {
        Debug.Log("Handling input");
        HandleMovementInput();
    }


    public void HandleMovementInput()
    {
        verticalInput = movementInput.x;
        horizontalInput = movementInput.y;

        moveAmount = Mathf.Clamp01(Math.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        animator.UpdateAnimatorValues(horizontalInput, verticalInput);

    }


}
