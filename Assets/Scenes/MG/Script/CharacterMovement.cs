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
    private Animator animator;

    private Vector3 playerVelocity;

    public bool groundedPlayer;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public Transform lookTarget;

    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;

    [SerializeField, Tooltip("Mmmmm")]
    private float smoothInputSpeed = .2f;

    [SerializeField, Tooltip("Rotation Power")]
    public float rotationPower = 1f;



    void Start()
    {
        animator = this.GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        lookAction = playerInput.actions["Look"];
        fireAction = playerInput.actions["Fire"];
    }

    // Update is called once per frame
    void Update()
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
        if(move.sqrMagnitude > 0.1)
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
        Debug.Log("Look: " + _look);
        lookTarget.transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);
    

}

    public void OnClick(InputAction.CallbackContext context)
    {
        Debug.Log("Left click on mouse work");
    }
}
