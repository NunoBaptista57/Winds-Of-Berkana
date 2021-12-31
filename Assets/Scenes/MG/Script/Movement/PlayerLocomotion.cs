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
    public bool isJumping = false;
    public bool isGliding = false;

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5f;
    public float sprintingSpeed = 7f;
    public float playerRotationSpeed = 15f;
    
    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    //public float glideVelocity;
    //private float currentFallingVelocity;
    public float raycastOriginOffSet = 0.5f;

    [Header("JumpSpeed")]
    public float jumpHeight = 3.0f;
    public float jumpCoeficient = 0.1f;
    public float jumpControlCoeficient = 20;
    public float gravityValue = -9.81f;

    [Header("Glide")]
    public float glideAcceleration = 3.0f;
    public float glideCoeficient = 30;


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
      //  currentFallingVelocity = fallingVelocity;
    }

    public void HandleAllMovement()
    {
        // HandleGlide();
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

        if (inputManager.runningInput)
        {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else 
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
        Vector3 targetPosition;
        raycastOrigin.y = raycastOrigin.y + raycastOriginOffSet;
        targetPosition = transform.position;
        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }


            //  inAirTimer += + Time.deltaTime;
            // rb.AddForce(transform.forward * leapingVelocity);
            //rb.AddForce(-Vector3.up * currentFallingVelocity * inAirTimer);
            
            // Controlling direction of movement
            moveDirection = cam.transform.forward * inputManager.horizontalInput; //Movement Input
            moveDirection = moveDirection + cam.transform.right * inputManager.verticalInput;
            moveDirection.Normalize();

            if (isGliding)
            {
                rb.AddForce(moveDirection.x * glideCoeficient, glideAcceleration, moveDirection.z * glideCoeficient, ForceMode.Acceleration);

            }
            else
            {
                rb.AddForce(moveDirection.x * jumpControlCoeficient, 0.0f, moveDirection.z * jumpControlCoeficient, ForceMode.Acceleration);
            }

            //Rotation while falling
            HandleRotation();


        }


        if (Physics.SphereCast(raycastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            if(!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);
                
               
            }

            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;


            inAirTimer = 0;
            
            isGrounded = true;
            isGliding = false;

        }
        else
        {
            isGrounded = false;
        }

        if(isGrounded && !isJumping)
        {
            if (playerManager.isInteracting || inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
                animatorManager.animator.SetBool("isJumping", false);
                animatorManager.animator.SetBool("Gliding", false);
            }
        }
    }


    public void HandleJumping()
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", true);
            animatorManager.animator.SetBool("Gliding", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityValue * jumpHeight);
           // float jumpingVelocity = jumpHeight;

            //  Vector3 playerVelocity = moveDirection;
            //  playerVelocity.y = jumpingVelocity;
            // rb.velocity = playerVelocity;
            //currentFallingVelocity = fallingVelocity;

            //Easiest way to do the jump
            rb.AddForce(moveDirection.x * jumpCoeficient, jumpingVelocity, moveDirection.z * jumpCoeficient, ForceMode.Impulse);
        }
    }

    public void ActivateGlide()
    {
        if (!isGrounded)
        {

                Debug.Log("Activate Glide");
                animatorManager.animator.SetBool("Gliding", true);
                animatorManager.PlayTargetAnimation("Glide", true);
                
                // We are no longer using this....so we need to think of another way of simulating a glide
                //currentFallingVelocity = glideVelocity;
                isGliding = true;
        }
       
    }

   public void DeactivateGlide()
     {
        Debug.Log("Deactivate Glide");
        animatorManager.animator.SetBool("Gliding", false);
      //  currentFallingVelocity = fallingVelocity;
        isGliding = false;
    }

 }
