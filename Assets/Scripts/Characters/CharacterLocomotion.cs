using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    public Transform BasePosition;
    public Vector3 BaseVelocity = Vector3.zero;
    public Transform Body;
    public Vector2 Input;
    [HideInInspector] public Vector3 NewVelocity;
    private CharacterManager _characterManager;
    private CharacterController _controller;
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
    public void Tunnel()
    {
        _locomotionState.Tunnel();
    }

    public void ChangeState<T>() where T : MonoBehaviour, ILocomotionState
    {
        _locomotionState = GetComponent<T>();
        _locomotionState.StartState();
    }

    public void ChangeAnimationState(CharacterAnimation.AnimationState animationState)
    {
        _characterManager.ChangeAnimation(animationState);
    }

    public Vector3 GetNewHorizontalVelocity(float acceleration, float maxSpeed, float deceleration)
    {
        Vector2 newVelocity = new(_controller.velocity.x, _controller.velocity.z);
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
        float fallSpeed = _controller.velocity.y - BaseVelocity.y;
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

        float baseRotation = BasePosition.rotation.eulerAngles.y;
        float newAngle = Body.transform.eulerAngles.y - baseRotation;
        transform.parent.rotation = Quaternion.Euler(transform.parent.rotation.x, baseRotation, transform.parent.rotation.z);
        float targetAngle = Vector2.SignedAngle(Input, Vector2.up);
        newAngle = Mathf.Round(Mathf.MoveTowardsAngle(newAngle, targetAngle, rotationSpeed));
        Body.transform.localRotation = Quaternion.Euler(Body.transform.rotation.x, newAngle, Body.transform.rotation.z);
    }

    private void Update()
    {
        _locomotionState.Move();
        if (_controller.isGrounded)
        {
            _locomotionState.Ground();
        }

        else if (_locomotionState != null && _locomotionState is not WindTunnel)
        {
            BaseVelocity = Vector3.zero;
            _locomotionState.Fall();
        }

        _controller.Move(NewVelocity);
        NewVelocity = Vector3.zero;
    }



    private void Start()
    {
        NewVelocity = Vector3.zero;
        ChangeState<RunningState>();
    }

    private void Awake()
    {
        _characterManager = GetComponentInParent<CharacterManager>();
        _controller = GetComponentInParent<CharacterController>();
    }
}