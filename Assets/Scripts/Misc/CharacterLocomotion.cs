using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.TextCore.Text;

public class CharacterLocomotion : MonoBehaviour
{
    // public GameObject Body;
    // public LocomotionState State;

    // public Vector2 BaseAngle;
    // public Vector2 InputDirection = Vector2.zero;
    // public Vector3 BaseVelocity = Vector3.zero;

    // [SerializeField] private float _deadzone = 0.9f;
    // [SerializeField] private float _walkDeadzone = 0.5f;

    // private CharacterController _characterController;

    // public void ChangeState(LocomotionState locomotionState)
    // {
    //     State = locomotionState;
    // }

    // public void Jump(float jumpForce)
    // {
    //     _characterController.Move(new Vector3(_characterController.velocity.x, _characterController.velocity.y + jumpForce, _characterController.velocity.z));
    // }

    // public void StopJump(float force)
    // {
    //     float fallSpeed = _characterController.velocity.y;
    //     if (fallSpeed < BaseVelocity.y)
    //     {
    //         return;
    //     }

    //     fallSpeed -= force;
    //     if (fallSpeed < BaseVelocity.y)
    //     {
    //         fallSpeed = BaseVelocity.y;
    //     }
    //     _characterController.Move(new Vector3(_characterController.velocity.x, fallSpeed, _characterController.velocity.z));
    // }

    // private Vector3 Fall(Vector3 velocity)
    // {
    //     float fallSpeed = velocity.y;
    //     fallSpeed -= State.FallSpeed;

    //     if (-fallSpeed > State.MaxFallSpeed)
    //     {
    //         fallSpeed = -State.MaxFallSpeed;
    //     }
    //     return new Vector3(velocity.x, fallSpeed, velocity.z);
    // }

    // private Vector3 Move(Vector3 velocity)
    // {
    //     Vector2 newDirection = GetNewDirection(InputDirection);
    //     Rotate(newDirection);
    //     return GetHorizontalVelocity(newDirection, velocity);
    // }

    public float BaseRotation = 0;
    public Vector3 BaseVelocity = Vector3.zero;
    public Transform Body;
    public Vector2 Input;
    private CharacterController _characterController;
    private ILocomotionState _locomotionState;

    public void StartJump()
    {
        _locomotionState.StartJump();
    }

    public void StopJump()
    {
        _locomotionState.StopJump();
    }

    public void Run()
    {
        _locomotionState.Run();
    }

    public void ChangeState<T>() where T : MonoBehaviour, ILocomotionState
    {
        _locomotionState = GetComponent<T>();
        _locomotionState.SetCharacterLocomotion(this);
    }

    public Vector3 GetNewHorizontalVelocity(float acceleration, float maxSpeed, float deceleration)
    {
        Vector2 newVelocity = new(_characterController.velocity.x, _characterController.velocity.z);
        newVelocity -= new Vector2(BaseVelocity.x, BaseVelocity.z);
        Vector2 forward = new(Body.forward.x, Body.forward.z);

        if (Input == Vector2.zero && newVelocity != Vector2.zero)
        {
            newVelocity -= deceleration * Time.deltaTime * newVelocity.normalized;

            if (newVelocity.normalized != newVelocity.normalized)
            {
                newVelocity = Vector2.zero;
            }
        }
        else if (newVelocity.magnitude > maxSpeed)
        {
            newVelocity -= deceleration * Time.deltaTime * newVelocity.normalized;
        }
        else if (Input != Vector2.zero)
        {
            newVelocity += acceleration * Time.deltaTime * forward;
            if (newVelocity.magnitude > maxSpeed)
            {
                newVelocity = newVelocity.normalized * maxSpeed;
            }
        }

        newVelocity += new Vector2(BaseVelocity.x, BaseVelocity.z);
        return new Vector3(newVelocity.x, 0, newVelocity.y);
    }

    public float GetNewVerticalSpeed(float acceleration, float maxSpeed, float deceleration)
    {
        return 0;
    }

    public void Rotate(float rotationSpeed)
    {
        if (Input == Vector2.zero)
        {
            return;
        }

        float newAngle = Body.transform.eulerAngles.y - BaseRotation;
        transform.parent.rotation = Quaternion.Euler(transform.parent.rotation.x, BaseRotation, transform.parent.rotation.z);
        float targetAngle = Vector2.SignedAngle(Input, Vector2.up);
        newAngle = Mathf.MoveTowardsAngle(newAngle, targetAngle, rotationSpeed);
        Body.transform.localRotation = Quaternion.Euler(Body.transform.rotation.x, newAngle, Body.transform.rotation.z);
    }

    private void Update()
    {
        _characterController.Move(_locomotionState.Move() * Time.deltaTime);
    }

    private void Start()
    {
        ChangeState<RunningState>();
        _characterController = GetComponentInParent<CharacterController>();
    }
}