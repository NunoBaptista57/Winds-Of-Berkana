using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    InputManager inputManager;
    AnimatorManager animatorManager;

    private Vector3 moveDirection;

    private Animator animator;
    private Vector3 playerVelocity;

    private Rigidbody rb;

    [Header("Movement Flags")]
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5f;
    public float sprintingSpeed = 7f;
    public float playerRotationSpeed = 15f;
    
    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float raycastOriginOffSet = 0.5f;

    [Header("JumpSpeed")]
    public float jumpHeight = 3.0f;
    public float gravityValue = -9.81f;


    public LayerMask groundLayer;
    public Transform lookTarget;

    private Camera cam;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    public void HandleAllMovement()
    {

        HandleFallingandLanding(); 
        if (playerManager.isInteracting)
            return;

        if (isJumping)
            return;


        HandleMovement();

     
        HandleRotation();
    }


    private void HandleMovement()
    {
       
        moveDirection = cam.transform.forward * inputManager.horizontalInput; //Movement Input
        moveDirection = moveDirection + cam.transform.right * inputManager.verticalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        // If we arew running select the sprintingsapeed
        // If we are running select the running speed
        //If we are walking select the running speed;
        if(inputManager.moveAmount >= 0.5f)
        {
            moveDirection = moveDirection * runningSpeed;
        }
        else
        {
            moveDirection = moveDirection * walkingSpeed;
        }

        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;


    }

    private void HandleRotation()
    {
        //Handle Rotation
     
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cam.transform.forward * inputManager.horizontalInput;
        targetDirection = targetDirection + cam.transform.right * inputManager.verticalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, playerRotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }


    private void HandleFallingandLanding()
    {
        RaycastHit hit;

        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y = raycastOrigin.y + raycastOriginOffSet;

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }


        if (Physics.SphereCast(raycastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            if(!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);
            }
            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }


    public void HandleJumping()
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityValue * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            rb.velocity = playerVelocity;


        }
    }
}
