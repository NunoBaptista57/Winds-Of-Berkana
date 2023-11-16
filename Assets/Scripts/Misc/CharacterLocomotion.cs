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
    private ILocomotionState _locomotionState;
    private CharacterController _characterController;

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
        Debug.Log(_locomotionState);
    }

    public float DecelerateJump(float force, float fallSpeed)
    {
        if (fallSpeed < BaseVelocity.y)
        {
            return fallSpeed;
        }

        fallSpeed -= force;
        if (fallSpeed < BaseVelocity.y)
        {
            fallSpeed = BaseVelocity.y;
        }
        return fallSpeed;
    }

    public Vector3 GetNewHorizontalVelocity(float acceleration, float maxSpeed, float deceleration)
    {
        Vector2 newVelocity = new(_characterController.velocity.x, _characterController.velocity.z);
        newVelocity -= new Vector2(BaseVelocity.x, BaseVelocity.z);
        Vector2 forward = new(Body.forward.x, Body.forward.z);

        if (Input == Vector2.zero && newVelocity != Vector2.zero)
        {
            Vector2 friction = newVelocity - deceleration * Time.deltaTime * newVelocity.normalized;

            if (friction.normalized == newVelocity.normalized)
            {
                newVelocity = friction;
            }
        }

        if (newVelocity.magnitude > maxSpeed)
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
        float fallSpeed = _characterController.velocity.y - BaseVelocity.y;
        fallSpeed -= acceleration * Time.deltaTime;

        if (-fallSpeed > maxSpeed)
        {
            fallSpeed += deceleration * Time.deltaTime;
            if (-fallSpeed < maxSpeed)
            {
                fallSpeed = -maxSpeed;
            }
        }
        return fallSpeed + BaseVelocity.y;
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
        if (_characterController.isGrounded)
        {
            _locomotionState.Ground();
        }
        else
        {
            _locomotionState.Fall();
        }
    }

    private void Start()
    {
        ChangeState<RunningState>();
        _characterController = GetComponentInParent<CharacterController>();
    }
}