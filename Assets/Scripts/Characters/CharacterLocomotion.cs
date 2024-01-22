using System;
using AmplifyShaderEditor;
using tripolygon.UModelerLite;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class CharacterLocomotion : MonoBehaviour
{
    public float BaseRotation = 0;
    public PlayerAnimation PlayerAnimation;
    public Vector3 BaseVelocity = Vector3.zero;
    public Transform Body;
    public Vector2 Input;
    public Vector3 NewVelocity;
    [HideInInspector] public CharacterController Controller;
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
    public void Tunnel(){
        _locomotionState.Tunnel();
    }

    public void ChangeState<T>() where T : MonoBehaviour, ILocomotionState
    {
        _locomotionState = GetComponent<T>();
        _locomotionState.StartState();
    }

    public Vector3 GetNewHorizontalVelocity(float acceleration, float maxSpeed, float deceleration)
    {
        Vector2 newVelocity = new(Controller.velocity.x, Controller.velocity.z);
        newVelocity -= new Vector2(BaseVelocity.x, BaseVelocity.z);
        Vector2 forward = new(Body.forward.x, Body.forward.z);

        if (Input == Vector2.zero && newVelocity != Vector2.zero)
        {
            Vector2 friction = newVelocity - deceleration * Time.deltaTime * newVelocity.normalized;

            if (friction.normalized == newVelocity.normalized)
            {
                newVelocity = friction;
            }
            else
            {
                newVelocity = Vector2.zero;
            }
        }

        if (newVelocity.magnitude > maxSpeed)
        {
            newVelocity -= deceleration / 2 * Time.deltaTime * newVelocity.normalized;
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
        float fallSpeed = Controller.velocity.y - BaseVelocity.y;
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
        newAngle = Mathf.Round(Mathf.MoveTowardsAngle(newAngle, targetAngle, rotationSpeed));
        Body.transform.localRotation = Quaternion.Euler(Body.transform.rotation.x, newAngle, Body.transform.rotation.z);
    }

    private void Update()
    {
        _locomotionState.Move();
        if (Controller.isGrounded)
        {
            Vector3 spherePosition = transform.position + Controller.center + Vector3.up * (-Controller.height * 0.5F + Controller.radius + 0.02f);
            if (Physics.SphereCast(spherePosition, Controller.radius, Vector3.down, out RaycastHit _, 3))
            {
                _locomotionState.Ground();
            }
        }

        else if(_locomotionState != null && !(_locomotionState is WindTunnel))
        {
            BaseVelocity = Vector3.zero;
            _locomotionState.Fall();
        }

        Controller.Move(NewVelocity);
        NewVelocity = Vector3.zero;
    }



    private void Start()
    {
        NewVelocity = Vector3.zero;
        ChangeState<RunningState>();
    }

    private void Awake()
    {
        Controller = GetComponentInParent<CharacterController>();
    }
}