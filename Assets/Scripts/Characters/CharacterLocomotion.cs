using System;
using System.Data.Common;
using System.Linq;
using Cinemachine.Utility;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

public class CharacterLocomotion : MonoBehaviour
{
    public Transform Body;
    public Vector2 Input = Vector2.zero;
    public Transform BasePosition;
    public Vector3 BaseVelocity { get; private set; } = Vector3.zero;
    public Vector3 InputVelocity { get; private set; } = Vector3.zero;
    public Vector3 FallVelocity { get; private set; } = Vector3.zero;
    public CharacterController _controller;
    private CharacterManager _characterManager;
    private ILocomotionState _locomotionState;
   // [SerializeField] private TMP_Text _debugText;
    [SerializeField] private float _slideSpeed;
    private Vector3 _hitPosition = new(0, 0, 0);
    private GameObject _obstacle;

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
        if (accessAudioManager() != null)
        {
            accessAudioManager().StopSFX();
        }
        _locomotionState.StartState();
    }

    public void ChangeState<T>(GameObject obstacle) where T : MonoBehaviour, ILocomotionState
    {
        _locomotionState = GetComponent<T>();
        if (accessAudioManager() != null)
        {
            accessAudioManager().StopSFX();
        }
        _locomotionState.StartState(obstacle);
    }

    public void ChangeAnimationState(CharacterAnimation.AnimationState animationState)
    {
        _characterManager.ChangeAnimation(animationState);
    }

    public void ChangeBaseVelocity(Vector3 baseVelocity)
    {
        BaseVelocity = baseVelocity;
    }

    public void Interact(bool active)
    {
        _locomotionState.Interact(active);
    }

    public void ChangeInputVelocity(Vector2 input, float acceleration, float maxSpeed, float deceleration, bool absolute)
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
            // Slide through walls
            if (_hitPosition != Vector3.zero)
            {
                Vector3 velocityProjection = Vector3.Project(InputVelocity, _hitPosition);

                if (Math.Abs(Vector3.SignedAngle(InputVelocity.HorizontalProjection(), _hitPosition, Vector3.up)) > 45f && _obstacle.CompareTag("Pushable"))
                {
                    Debug.Log("Entrou");
                    Body.rotation = Quaternion.LookRotation(-_hitPosition);
                    _locomotionState.Push(_obstacle);
                }
                else if (Math.Abs(Vector3.SignedAngle(InputVelocity.HorizontalProjection(), _hitPosition, Vector3.up)) > 90f)
                {
                    float angleBetween = Vector3.Angle(InputVelocity, _hitPosition);
                    float angleFactor = Mathf.InverseLerp(180f, 0f, angleBetween);
                    float dynamicMaxSpeed = maxSpeed * angleFactor;
                    InputVelocity -= velocityProjection;
                    InputVelocity = Vector3.ClampMagnitude(InputVelocity + acceleration * Time.deltaTime * transform.forward, dynamicMaxSpeed);
                    _hitPosition = Vector3.zero;
                    return;
                }
                _hitPosition = Vector3.zero;
            }

            if (absolute)
            {
                Vector3 direction3D = new(input.x, 0, input.y);
                InputVelocity = Vector3.ClampMagnitude(InputVelocity + acceleration * Time.deltaTime * direction3D, maxSpeed);
            }
            else
            {
                InputVelocity = Vector3.ClampMagnitude(InputVelocity + acceleration * Time.deltaTime * transform.forward, maxSpeed);
            }
        }
    }

    public void ChangeImediateInputVelocity(Vector3 input)
    {
        InputVelocity = input;
    }


    public Vector2 CalculateVector(Vector2 input)
    {
        Vector3 newInput = BasePosition.forward * input.y
                        + BasePosition.right * input.x;
        newInput.y = 0;

        return new(newInput.x, newInput.z);   
    }

    public void RotateBody(Vector2 input, float rotationSpeed, bool canDo180)
    {
        if (input == Vector2.zero)
        {
            return;
        }

        Vector2 targetVector = CalculateVector(input);

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


    public void ChangeImediateFallVelocity(float fallSpeed)
    {
        FallVelocity = new(0f, -fallSpeed, 0f);
    }

    public void ChangeFallVelocity(float acceleration)
    {
        float fallSpeed = FallVelocity.y;
        fallSpeed -= acceleration * Time.deltaTime;
        FallVelocity = new(0f, fallSpeed, 0f);
    }

    public void ChangeFallVelocity(float acceleration, float maxSpeed, float deceleration)
    {
        float fallSpeed = FallVelocity.y;
        if (fallSpeed > -maxSpeed)
        {
            fallSpeed = Math.Clamp(fallSpeed - acceleration * Time.deltaTime, -maxSpeed, float.MaxValue);
        }
        else if (fallSpeed < -maxSpeed)
        {
            fallSpeed = Math.Clamp(fallSpeed + deceleration * Time.deltaTime, float.MinValue, -maxSpeed);
        }
        FallVelocity = new(0f, fallSpeed, 0f);
    }

    public void AddJumpForce(float force)
    {
        FallVelocity = new(0f, force, 0f);
    }

    public void StopJumpForce(float force)
    {
        if (FallVelocity.y > 0f)
        {
            float stopForce = FallVelocity.y;
            stopForce = Math.Clamp(stopForce - force, 0f, float.MaxValue);
            FallVelocity = new Vector3(0f, stopForce, 0f);
        }
    }

    public AudioManager accessAudioManager()
    {
        return _characterManager.audioManager;
    }
    // Todo: review angle
    public void OnCollision(ControllerColliderHit hit)
    {
        float height = hit.point.y - transform.position.y;
        float angle = Vector3.Angle(hit.normal, Vector3.up);


        if (height > _controller.stepOffset)
        {
            _obstacle = hit.gameObject;
            _hitPosition = hit.normal.HorizontalProjection().normalized;
        }
        else if (angle > _controller.slopeLimit)
        {
            if (Physics.Raycast(transform.position + Vector3.up * _controller.stepOffset - hit.normal.HorizontalProjection().normalized * _controller.stepOffset,
             -Vector3.up, out RaycastHit groundHit, _controller.stepOffset, ~LayerMask.GetMask("Player"), QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(groundHit.normal, Vector3.up);

                if (groundAngle > _controller.stepOffset)
                {
                    _hitPosition = hit.normal.HorizontalProjection().normalized;
                }
            }
        }
    }


    private void Update()
    {
        _locomotionState.Move(Input);

        Vector3 horizontalVelocity = InputVelocity;

        float angle = 0;

        RaycastHit[] results = new RaycastHit[10];

        if (Physics.SphereCastNonAlloc(transform.position + _controller.height / 2 * Vector3.up, _controller.radius, transform.up * -1,
         results, _controller.height / 2 - _controller.radius, ~LayerMask.GetMask("Player"), QueryTriggerInteraction.Ignore) > 0)
        {
            RaycastHit lessSteepHit = results[0];

            foreach (RaycastHit hit in results)
            {
                if (!hit.transform)
                {
                    continue;
                }

                if (hit.collider.gameObject.TryGetComponent(out MovingPlatform movingPlatform))
                {
                    transform.parent.SetParent(movingPlatform.transform);
                }

                float newAngle = Vector3.Angle(hit.normal, Vector3.up);

                if (newAngle < Vector3.Angle(lessSteepHit.normal, Vector3.up))
                {
                    lessSteepHit = hit;
                }
            }

            angle = Vector3.Angle(lessSteepHit.normal, Vector3.up);
            // Slide
            if (angle > _controller.slopeLimit)
            {           
                horizontalVelocity += (90f - angle) / (90f - _controller.slopeLimit) * FallVelocity.magnitude * lessSteepHit.normal.HorizontalProjection();
                _locomotionState.Slide();
            }
            else
            {
                // Uphill
                if (Vector3.SignedAngle(InputVelocity.HorizontalProjection(), lessSteepHit.normal.HorizontalProjection(), lessSteepHit.normal.HorizontalProjection()) > 90f)
                {
                    horizontalVelocity = Vector3.ClampMagnitude(InputVelocity, InputVelocity.magnitude * (_controller.slopeLimit * 1.5f - angle) / _controller.slopeLimit);

                }
                _locomotionState.Ground();
            }
        }
        else if (_locomotionState != null)
        {
            _locomotionState.Fall();
            transform.parent.SetParent(null);
            ChangeBaseVelocity(Vector3.zero);
        }

        Vector3 velocity = (BaseVelocity + horizontalVelocity + FallVelocity) * Time.deltaTime;

        _controller.Move(velocity);

        LocomotionDebug(angle, horizontalVelocity);

    }

    private void LocomotionDebug(float slope, Vector3 horizontalVelocity)
    {
        string text = "Global Velocity: " + (BaseVelocity + horizontalVelocity + FallVelocity);
        text += "\nVelocity: " + (InputVelocity + FallVelocity);
        text += "\nInput Velocity: " + InputVelocity;
        text += "\nHorizontal Velocity: " + horizontalVelocity;
        text += "\nFall Velocity: " + FallVelocity;
        text += "\nBase Velocity: " + BaseVelocity;
        text += "\nSlope: " + slope;
        text += "\n" + _locomotionState;

       // _debugText.SetText(text);
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