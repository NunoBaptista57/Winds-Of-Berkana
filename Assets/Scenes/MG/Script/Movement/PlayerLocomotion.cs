using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    InputManager inputManager;
    AnimatorManager animatorManager;

    private Vector3 moveDirection;

    public Rigidbody playerRigidBody;
    public GameObject gliderObject;

    [Header("Movement Settings")]
    public bool doubleJumpAbility;
    public bool glideAbility;
    public bool sprintAbility;

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
    public float raycastOriginOffSet = 0.5f;

    [Header("JumpSpeed")]
    public float jumpHeight = 3.0f;
    public float jumpCoeficient = 0.1f;
    public float jumpControlCoeficient = 20;
    public float gravityValue = -9.81f;

    [Header("Dodge")]
    public float dodgeDistance = 3.0f;

    [Header("Glide Inverse Gravity Acceleration")]
    public float glideAcceleration = 3.0f;
    [Header("Glide MovDirection Influence")]
    public float glideControlCoeficient = 30;

    [Header("Climb")]
    public float wallRaycastDistance = 3.0f;
    public bool isClimbing = false;
    public LayerMask climbLayer;
    public LayerMask groundLayer;
    public Transform lookTarget;

    private Camera cam;
    private bool doublejumped;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        playerRigidBody = GetComponent<Rigidbody>();
        cam = Camera.main;
        //  currentFallingVelocity = fallingVelocity;
    }

    public void HandleAllMovement()
    {

        if (ServiceLocator.instance.GetService<LevelManager>().State == GameState.Paused || ServiceLocator.instance.GetService<LevelManager>().State == GameState.Death)
            return;

        HandleFallingandLanding();

        if (playerManager.isInteracting)
            return;

        HandleRotation();

 
        HandleMovement();

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
            if (sprintAbility)
                moveDirection = moveDirection * sprintingSpeed;
            else moveDirection = moveDirection * runningSpeed;
        }
        else
        if (inputManager.moveAmount >= 0.5f)
        {
            moveDirection = moveDirection * runningSpeed;
        }
        else
        {
            moveDirection = moveDirection * walkingSpeed;
        }

        Vector3 movementVelocity = moveDirection;
        playerRigidBody.velocity = movementVelocity;


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


          //  animatorManager.animator.SetBool("isUsingRootMotion", false);

            if (isClimbing)
            {

                Debug.Log("IsClimbing" + isClimbing); 
                moveDirection = new Vector3(0.0f, 2.0f, 0.0f) * inputManager.horizontalInput;
                Vector3 movementVelocity = moveDirection;
                playerRigidBody.velocity = movementVelocity;

            }
            else
            {


                //  inAirTimer += + Time.deltaTime;
                // rb.AddForce(transform.forward * leapingVelocity);
                //rb.AddForce(-Vector3.up * currentFallingVelocity * inAirTimer);


                // Controlling direction of movement
                moveDirection = cam.transform.forward * inputManager.horizontalInput; //Movement Input
                moveDirection = moveDirection + cam.transform.right * inputManager.verticalInput;
                //moveDirection = moveDirection * walkingSpeed;
               // moveDirection.Normalize();

                if (isGliding)
                {
                    var x = moveDirection.x * glideControlCoeficient;
                    var y = glideAcceleration;
                    var z = moveDirection.z * glideControlCoeficient;

                    Debug.Log("Gliding  x:" + x + " y:" + y + " z:" + z);

                    playerRigidBody.AddForce(x, y, z, ForceMode.Acceleration);

                }
                else
                {

                    var x = moveDirection.x * jumpControlCoeficient;
                    var y = 0.0f;
                    var z = moveDirection.z * jumpControlCoeficient;

                    Debug.Log("Jumping  x:" + x + " y:" + y + " z:" + z);

                    playerRigidBody.AddForce(x, y, z, ForceMode.Acceleration);

                  
                }



                //Rotation while falling
                HandleRotation();

            }
        }


        if (Physics.SphereCast(raycastOrigin, 0.5f, -Vector3.up, out hit, groundLayer))
        {
            if (!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);
            }

            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;


            inAirTimer = 0;

            isGrounded = true;
            isGliding = false;
            if (doubleJumpAbility)
                doublejumped = false;

        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && !isJumping)
        {
             //What was this for?
            if (playerManager.isInteracting || inputManager.moveAmount > 0)
            {
                Debug.Log("is Interacting");
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


    public void Jump()
    {
        Debug.Log("Jumped");
        animatorManager.animator.SetBool("isJumping", true);
        animatorManager.PlayTargetAnimation("Jump", true);
        animatorManager.animator.SetBool("Gliding", false);

        float jumpingVelocity = Mathf.Sqrt(-2 * gravityValue * jumpHeight);
        playerRigidBody.velocity = Vector3.zero;
        playerRigidBody.AddForce(moveDirection.x * jumpCoeficient, jumpingVelocity, moveDirection.z * jumpCoeficient, ForceMode.Impulse);
    }

    public void HandleDodge()
    {
        if (playerManager.isInteracting)
            return;

        animatorManager.PlayTargetAnimation("Dodge", true, true);
        // TOGGLE INVULNERABILITY FOR NO HP DMG DURING ANIMATION


    }


    public void HandleJumping()
    {
        if (isGrounded)
        {
            Jump();
        }

        else if (!doublejumped && doubleJumpAbility)
        {
            Jump();
            doublejumped = true;
        }
    }

    public void ActivateGlide()
    {
        if (!isGrounded && glideAbility)
        {

            Debug.Log("Activate Glide");
            animatorManager.animator.SetBool("Gliding", true);
            animatorManager.PlayTargetAnimation("Glide", true);
            gliderObject?.SetActive(true);
            // We are no longer using this....so we need to think of another way of simulating a glide
            //currentFallingVelocity = glideVelocity;
            isGliding = true;
        }

    }

    public void DeactivateGlide()
    {
        if (glideAbility)
        {
            Debug.Log("Deactivate Glide");
            animatorManager.animator.SetBool("Gliding", false);
            gliderObject?.SetActive(false);
            //  currentFallingVelocity = fallingVelocity;
            isGliding = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
    
        if (collision.gameObject.layer == LayerMask.NameToLayer("Climbable") && !isClimbing)
        {

            Debug.Log("Climb!");
            isClimbing = true;
        }

     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Death" && ServiceLocator.instance.GetService<LevelManager>().State != GameState.Death)
        {
            Debug.Log("Player has fallen to its Death");
            ServiceLocator.instance.GetService<LevelManager>().UpdateGameState(GameState.Death);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Climbable"))
        {
            isClimbing = false;

        }
    }
}
