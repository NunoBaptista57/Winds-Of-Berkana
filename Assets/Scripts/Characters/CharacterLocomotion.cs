using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    public Transform Body;
    public Vector2 Input = Vector2.zero;
    public Transform BasePosition;
    public Vector3 PushVelocity { get; private set; } = Vector3.zero;
    public Vector3 BaseVelocity { get; private set; } = Vector3.zero;
    public Vector3 InputVelocity { get; private set; } = Vector3.zero;
    public Vector3 Gravity { get; private set; } = Vector3.zero;
    private CharacterManager _characterManager;
    public CharacterController _controller;
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

    public void ChangePushVelocity(Vector3 pushVelocity)
    {
        if (InputVelocity == Vector3.zero)
        {
            PushVelocity = pushVelocity;
        }
    }

    public void ChangeBaseVelocity(Vector3 baseVelocity)
    {
        BaseVelocity = baseVelocity;
    }

    public void ChangeInputVelocity(Vector2 input, float acceleration, float maxSpeed, float deceleration)
    {
        // Breaking
        if (input == Vector2.zero && InputVelocity != Vector3.zero)
        {
            Vector3 afterFriction = InputVelocity - deceleration * Time.deltaTime * InputVelocity.normalized;

            if (afterFriction.normalized == InputVelocity.normalized)
            {
                InputVelocity = afterFriction;
            }
            else
            {
                InputVelocity = Vector3.zero;
            }
        }

        if (InputVelocity.magnitude > maxSpeed)
        {
            InputVelocity -= deceleration / 2 * Time.deltaTime * InputVelocity.normalized;
        }
        else if (input != Vector2.zero)
        {
            InputVelocity += acceleration * Time.deltaTime * transform.forward;
            if (InputVelocity.magnitude > maxSpeed)
            {
                InputVelocity = InputVelocity.normalized * maxSpeed;
            }
        }
    }

    public void Rotate(Vector2 input, float rotationSpeed, bool canDo180)
    {
        if (input == Vector2.zero)
        {
            return;
        }
        Vector3 newInput = BasePosition.forward * input.y
                        + BasePosition.right * input.x;
        newInput.y = 0;

        Vector2 targetVector = new(newInput.x, newInput.z);
        float targetAngle = Vector2.SignedAngle(targetVector, Vector2.up);

        float newAngle = transform.eulerAngles.y;

        if (InputVelocity == Vector3.zero && canDo180)
        {
            newAngle = targetAngle;
        }
        else if (Mathf.Abs(Vector2.SignedAngle(targetVector, new Vector2(InputVelocity.x, InputVelocity.z))) > 90f)
        {
            newAngle = targetAngle;
            _locomotionState.Break();
        }
        else
        {
            newAngle = Mathf.Round(Mathf.MoveTowardsAngle(newAngle, targetAngle, rotationSpeed * Time.deltaTime));
        }
        transform.rotation = Quaternion.Euler(transform.rotation.x, newAngle, transform.rotation.z);

        // Rotate body
        if (Body.transform.eulerAngles.y != transform.eulerAngles.y)
        {
            float bodyAngle = Body.transform.eulerAngles.y;
            newAngle = Mathf.Round(Mathf.MoveTowardsAngle(bodyAngle, transform.eulerAngles.y, rotationSpeed * Time.deltaTime));
            Body.transform.rotation = Quaternion.Euler(Body.transform.rotation.x, newAngle, Body.transform.rotation.z);
        }
    }

    public void ChangeGravity(float acceleration)
    {
        float fallSpeed = Gravity.y;
        fallSpeed -= acceleration * Time.deltaTime;
        Gravity = new(0f, fallSpeed, 0f);
    }

    public void ChangeImediateGravity(float fallSpeed)
    {
        Gravity = new(0f, -fallSpeed, 0f);
    }

    public void ChangeGravity(float acceleration, float maxSpeed, float deceleration)
    {
        float fallSpeed = Gravity.y;
        if (fallSpeed > -maxSpeed)
        {
            fallSpeed -= acceleration * Time.deltaTime;
            if (fallSpeed < -maxSpeed)
            {
                fallSpeed = -maxSpeed;
            }
        }
        else if (fallSpeed < -maxSpeed)
        {
            fallSpeed += deceleration * Time.deltaTime;
            if (fallSpeed > -maxSpeed)
            {
                fallSpeed = -maxSpeed;
            }
        }
        Gravity = new(0f, fallSpeed, 0f);
    }

    public void AddJumpForce(float force)
    {
        Gravity = new(0f, force, 0f);
    }

    public void StopJumpForce(float force)
    {
        if (Gravity.y > 0f)
        {
            float stopForce = Gravity.y;
            stopForce -= force;
            if (stopForce < 0f)
            {
                stopForce = 0f;
            }

            Gravity = new Vector3(0f, stopForce, 0f);
        }
    }

    private void Update()
    {
        _locomotionState.Move(Input);
        if (Physics.SphereCast(transform.position + transform.up * _controller.radius, _controller.radius, transform.up * -1, out RaycastHit hit, 0.1f) && !hit.collider.isTrigger)
        {
            _locomotionState.Ground();
            if (hit.collider.gameObject.TryGetComponent(out MovingPlatform movingPlatform))
            {
                transform.parent.SetParent(movingPlatform.transform);
            }
        }
        else if (_locomotionState != null && _locomotionState is not WindTunnel)
        {
            _locomotionState.Fall();
            transform.parent.SetParent(null);
            ChangeBaseVelocity(Vector3.zero);
        }
        Debug.Log(Gravity);
        _controller.Move(PushVelocity + BaseVelocity + (InputVelocity + Gravity) * Time.deltaTime);
    }

    private void Start()
    {
        ChangeState<RunningState>();
    }

    private void Awake()
    {
        _characterManager = GetComponentInParent<CharacterManager>();
        _controller = GetComponentInParent<CharacterController>();
    }
}