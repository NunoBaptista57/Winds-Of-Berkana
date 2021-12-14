using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;

    private Vector3 moveDirection;

    private Animator animator;
    private Vector3 playerVelocity;

    private Rigidbody rb;
    public bool groundedPlayer;
    public float playerSpeed = 2.0f;
    public float playerRotationSpeed = 15f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public Transform lookTarget;

    private Vector2 currentInputVector;

    [SerializeField, Tooltip("Mmmmm")]
    private float smoothInputSpeed = .2f;

    private Camera cam;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }


    private void HandleMovement()
    {
       
        moveDirection = cam.transform.forward * inputManager.horizontalInput; //Movement Input
        moveDirection = moveDirection + cam.transform.right * inputManager.verticalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * playerSpeed;

        Vector3 movementVelocity = moveDirection;

        rb.velocity = movementVelocity;


    }

    private void HandleRotation()
    {
        //Handle Rotation
     
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cam.transform.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cam.transform.right * inputManager.horizontalInput;
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
}
