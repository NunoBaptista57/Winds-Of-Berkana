using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    // Start is called before the first frame update

    private CharacterController controller;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction fireAction;
    private InputAction jumpAction;

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
    private Vector2 smoothInputVelocity;

    [SerializeField, Tooltip("Mmmmm")]
    private float smoothInputSpeed = .2f;

    [SerializeField, Tooltip("Rotation Power")]
    public float rotationPower = 1f;

    private Camera cam;


    void Start()
    {
        animator = this.GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
        rb = this.GetComponent<Rigidbody>();
        cam = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        lookAction = playerInput.actions["Look"];
        fireAction = playerInput.actions["Fire"];
    }

    // Update is called once per frame
    void Movement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothInputSpeed);
        Vector3 move = new Vector3(currentInputVector.x, 0, currentInputVector.y);

        controller.Move(move * Time.deltaTime * playerSpeed);



      //  Debug.Log(move.sqrMagnitude);
        if (move.sqrMagnitude > 0.1)
        {
            if (move.sqrMagnitude > 0.6)
                animator.SetBool("Running", true);
            else
            {
                animator.SetBool("Walking", true);
                animator.SetBool("Running", false);
            }
        }
        else
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
        }
        

        if (jumpAction.triggered && groundedPlayer)
        {
            //anim.Play(jumpanimationId);
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);


        Vector2 _look = lookAction.ReadValue<Vector2>();
       // Vector3 direction = this.rb.velocity;
       // Debug.Log("Look: " + _look);
        lookTarget.transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);
    

}

    public void OnClick(InputAction.CallbackContext context)
    {
        Debug.Log("Left click on mouse work");
    }


    public void HandleMovement()
    {
       var input = moveAction.ReadValue<Vector2>();
        moveDirection = cam.transform.forward * input.y; //Movement Input
        moveDirection = moveDirection + cam.transform.right * input.x;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * playerSpeed;

        Vector3 movementVelocity = moveDirection;

        rb.velocity = movementVelocity;

      //  var input = moveAction.ReadValue<Vector2>();
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cam.transform.forward * input.y;
        targetDirection = targetDirection + cam.transform.right * input.x;
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

    public void HandleRotation()
    {
        //Handle Rotation
        var input = moveAction.ReadValue<Vector2>();
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cam.transform.forward * input.y;
        targetDirection = targetDirection + cam.transform.right * input.x;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, playerRotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }
}
