using System;
using AmplifyShaderEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class CharacterLocomotion : MonoBehaviour
{
    public LocomotionState State;
    public GameObject Body;
    public Vector2 BaseAngle = Vector2.right;
    public float Deadzone = 0.9f;
    public Vector2 BaseVelocity = Vector3.zero;
    public float WalkDeadzone = 0.5f;
    public Vector2 InputDirection = Vector2.zero;

    private Rigidbody _rigidbody;
    private bool _isMoving = false;

    public void Fall()
    {

    }

    public void Jump()
    {

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

    // TODO: Study a new way of doing thiss
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
        currentVelocity -= BaseVelocity;
        Vector2 newVelocity = currentVelocity;

        float maxSpeed = State.MaxSpeed;
        float acceleration = State.Acceleration;
        Debug.Log(InputDirection + "," + inputMagnitude);
        // Walking
        if (inputMagnitude <= WalkDeadzone)
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

        _rigidbody.velocity = new(newVelocity.x, _rigidbody.velocity.y, newVelocity.y);
    }

    // TODO: should be smoother
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
        if (InputDirection.magnitude <= (1 - Deadzone))
        {
            Decelerate();
        }
        else
        {
            Move();
        }
    }
}