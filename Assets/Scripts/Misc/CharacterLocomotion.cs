using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.TextCore.Text;

public class CharacterLocomotion : MonoBehaviour
{
    public float BaseRotation = 0;
    public Vector3 BaseVelocity = Vector3.zero;
    public Transform Body;
    [HideInInspector] public CharacterController CharacterController;
    public Vector2 Input;
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

    public void Walk(bool walk)
    {
        _locomotionState.Walk(walk);
    }

    public void ChangeState<T>() where T : MonoBehaviour, ILocomotionState
    {
        _locomotionState = GetComponent<T>();
        _locomotionState.StartState();
        Debug.Log(_locomotionState);
    }

    public Vector3 GetNewHorizontalVelocity(float acceleration, float maxSpeed, float deceleration)
    {
        Vector2 newVelocity = new(CharacterController.velocity.x, CharacterController.velocity.z);
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
        float fallSpeed = CharacterController.velocity.y - BaseVelocity.y;
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
        _locomotionState.Move();
        if (CharacterController.isGrounded)
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
        CharacterController = GetComponentInParent<CharacterController>();
    }
}