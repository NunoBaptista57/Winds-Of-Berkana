using System;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    public GameObject Body;
    public LocomotionState State;

    public Vector2 BaseAngle = Vector2.right;
    public Vector2 InputDirection = Vector2.zero;
    public Vector3 BaseVelocity = Vector3.zero;

    [SerializeField] private float _deadzone = 0.9f;
    [SerializeField] private float _walkDeadzone = 0.5f;

    private Rigidbody _rigidbody;

    public void ChangeState(LocomotionState locomotionState)
    {
        State = locomotionState;
    }

    public void Jump(float jumpForce)
    {
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y + jumpForce, _rigidbody.velocity.z);
    }

    public void StopJump(float force)
    {
        float fallSpeed = _rigidbody.velocity.y;
        if (fallSpeed < BaseVelocity.y)
        {
            return;
        }

        fallSpeed -= force;
        if (fallSpeed < BaseVelocity.y)
        {
            fallSpeed = BaseVelocity.y;
        }
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, fallSpeed, _rigidbody.velocity.z);
    }

    private void Fall()
    {
        float fallSpeed = _rigidbody.velocity.y;

        if (fallSpeed > State.MaxFallSpeed)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, fallSpeed, _rigidbody.velocity.z);
        }
    }

    private void Move()
    {
        Vector2 newDirection = GetNewDirection(InputDirection);
        Rotate(newDirection);
        ChangeVelocity(newDirection);
    }

    private void Decelerate()
    {
        Vector2 currentVelocity = new(_rigidbody.velocity.x - BaseVelocity.x, _rigidbody.velocity.z - BaseVelocity.y);
        if (currentVelocity == Vector2.zero)
        {
            return;
        }

        Vector2 newVelocity = currentVelocity - currentVelocity.normalized * State.Deceleration;

        if (currentVelocity.normalized != newVelocity.normalized)
        {
            newVelocity = Vector2.zero;
        }

        _rigidbody.velocity = new(newVelocity.x + BaseVelocity.x, _rigidbody.velocity.y, newVelocity.y + BaseVelocity.y);
    }

    // TODO: Study a new way of doing this
    private Vector2 GetNewDirection(Vector2 direction)
    {
        // Camera
        float angle = Vector2.SignedAngle(Vector2.right, BaseAngle);
        float directionAngle = Vector2.SignedAngle(Vector2.right, direction);
        float targetAngle = (angle + directionAngle + 90) % 360;
        if (targetAngle < 0)
        {
            targetAngle += 360;
        }

        float newRotation = -(Body.transform.rotation.eulerAngles.y - 90) % 360;
        if (newRotation < 0)
        {
            newRotation += 360;
        }

        if (targetAngle > newRotation)
        {
            if (targetAngle - newRotation > 180)
            {
                newRotation -= State.RotationSpeed;
                if (newRotation < 0 && newRotation + 360 < targetAngle)
                {
                    newRotation = targetAngle;
                }
            }
            else
            {
                newRotation += State.RotationSpeed;
                if (newRotation > targetAngle)
                {
                    newRotation = targetAngle;
                }
            }
        }
        else if (targetAngle < newRotation)
        {
            if (newRotation - targetAngle > 180)
            {
                newRotation += State.RotationSpeed;
                if (newRotation > 360 && newRotation - 360 > targetAngle)
                {
                    newRotation = targetAngle;
                }
            }
            else
            {
                newRotation -= State.RotationSpeed;
                if (newRotation < targetAngle)
                {
                    newRotation = targetAngle;
                }
            }
        }

        newRotation *= Mathf.PI / 180;

        Vector2 newDirection = new(Mathf.Cos(newRotation), Mathf.Sin(newRotation));
        return newDirection;
    }

    private void ChangeVelocity(Vector2 direction)
    {
        float inputMagnitude = InputDirection.magnitude;

        Vector2 currentVelocity = new(_rigidbody.velocity.x, _rigidbody.velocity.z);
        currentVelocity -= new Vector2(BaseVelocity.x, BaseVelocity.z);
        Vector2 newVelocity = currentVelocity;

        float maxSpeed = State.MaxSpeed;
        float acceleration = State.Acceleration;
        Debug.Log(InputDirection + "," + inputMagnitude);
        // Walking
        if (inputMagnitude <= _walkDeadzone)
        {
            maxSpeed /= 2;
            acceleration /= 2;
        }

        if (currentVelocity.magnitude > maxSpeed)
        {
            newVelocity -= currentVelocity.normalized * State.Deceleration;
            newVelocity += direction.normalized * acceleration;
        }
        else
        {
            newVelocity += direction.normalized * acceleration;

            if (newVelocity.magnitude > maxSpeed)
            {
                newVelocity = newVelocity.normalized * maxSpeed;
            }
        }

        newVelocity += new Vector2(BaseVelocity.x, BaseVelocity.z);
        _rigidbody.velocity = new(newVelocity.x, _rigidbody.velocity.y, newVelocity.y);
    }

    private void Rotate(Vector2 direction)
    {
        Body.transform.rotation = Quaternion.Euler(0, -(Vector2.SignedAngle(Vector2.right, direction) - 90), 0);
    }

    private void Start()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();
        State = GetComponent<RunningState>();
        if (State == null)
        {
            Debug.Log("RunningState component not added to object.");
        }
    }

    private void FixedUpdate()
    {
        // Stopping
        if (InputDirection.magnitude <= (1 - _deadzone))
        {
            Decelerate();
        }
        else
        {
            Move();
        }
        Fall();
    }

    private void OnCollisionEnter(Collision other)
    {
        ChangeState(GetComponent<RunningState>());
    }
}