using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatMovement : MonoBehaviour
{
    new Rigidbody rigidbody;

    [Header("Flight Settings")]
    [SerializeField] bool flightMode = true;
    [SerializeField, Min(0)] float maxDownBoost = 1.5f; // Multiplier for maxSpeed when going down
    [SerializeField, Min(0)] float maxUpSlow = 0.5f;    // Multiplier for maxSpeed when going up
    float speedModifier = 1f; 

    [Header("Control Settings")]
    public bool canMove = true;
    [Min(0)] public float MaxVelocity;
    [ReadOnlyInspector] public float currentSpeed;
    [Min(0), SerializeField] float acceleration = 0.1f;


    [Header("Wind")]
    [Min(0), SerializeField] float WindForce;
    [Min(0), SerializeField] float VelocityLimitingStrength = 1;
    [Min(0), SerializeField] float TurningTorque;

    struct PlayerInput
    {
        public float Turn;
        public float Pitch;
        public float Reel;
        public float Slow;
        public float SpeedUp;
    }

    [Header("Angle Limiting")]
    [SerializeField, Range(0, 90)] float MinVerticalAngle;
    [SerializeField, Range(0, 90)] float MaxVerticalAngle;
    [SerializeField, Min(0)] float LimitingTorqueMultiplier = 1;
    [SerializeField, Min(0)] float LimitingOffsetExponent = 1;

    [Header("Stabilization")]
    [SerializeField, Range(0, 1)] float ForwardStabilization;

    [Header("Tilting")]
    [SerializeField] Transform visualModelTransform;
    [SerializeField, Range(0, 90)] float maxTiltingAngle = 10;


    PlayerInput input;
    public Action onInteraction;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        currentSpeed = 0;

        if (!flightMode)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public void AllowPlayerControl(bool setTo) { canMove = setTo; }

    void FixedUpdate()
    {
        if (!canMove) { rigidbody.velocity = Vector3.zero; return; }

        if (visualModelTransform != null)
        {
            Vector3 currentEulerAngles = visualModelTransform.rotation.eulerAngles;
            print(input.Turn);
            float tiltAngle = maxTiltingAngle * -input.Turn;
            currentEulerAngles.z = tiltAngle;
            Quaternion targetRotation = Quaternion.Euler(currentEulerAngles);
            visualModelTransform.rotation = Quaternion.Lerp(visualModelTransform.rotation, targetRotation, Time.fixedDeltaTime * 2.5f);
        }

        if (flightMode) // Dynamic speed limit going up or down
        {
            float pitchAngle = Vector3.Angle(transform.forward, Vector3.up);

            if (pitchAngle < 90)
            {
                float t = Mathf.InverseLerp(90f, MaxVerticalAngle, pitchAngle);
                float mappedValue = Mathf.Lerp(1f, maxUpSlow, t);
                speedModifier = Mathf.Lerp(speedModifier, mappedValue, Time.fixedDeltaTime*2.5f);
            }
            else if (pitchAngle > 90)
            {
                float t = Mathf.InverseLerp(90f, MinVerticalAngle, pitchAngle);
                float mappedValue = Mathf.Lerp(maxDownBoost, 1f, t);
                speedModifier = Mathf.Lerp(speedModifier, mappedValue, Time.fixedDeltaTime*2.5f);
            }
        }

        //print(currentSpeed * speedModifier);

        if (Input.GetKey(KeyCode.Space))
        {
            if (currentSpeed < MaxVelocity * 2)
            {
                currentSpeed += acceleration;
            }
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= acceleration;
            }
        }


        rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, Vector3.Project(rigidbody.velocity, transform.forward), ForwardStabilization);
        rigidbody.AddForce(WindForce * transform.forward, ForceMode.Acceleration);

        if (rigidbody.velocity.magnitude > currentSpeed * speedModifier)
        {
            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, Mathf.Lerp(rigidbody.velocity.magnitude, currentSpeed * speedModifier, VelocityLimitingStrength * Time.fixedDeltaTime));
        }

        rigidbody.AddTorque(TurningTorque * input.Turn * Vector3.up, ForceMode.Acceleration);

        if (flightMode)
        {
            rigidbody.AddTorque(TurningTorque * input.Pitch * Vector3.Cross(transform.forward, Vector3.up).normalized, ForceMode.Acceleration);
        }

        float angle = Vector3.SignedAngle(transform.forward, transform.forward.HorizontalProjection(), transform.right);
        if (angle > MaxVerticalAngle)
            rigidbody.AddTorque(LimitingTorqueMultiplier * Mathf.Pow(angle - MaxVerticalAngle, LimitingOffsetExponent) * transform.right, ForceMode.Acceleration);
        if (-angle > MinVerticalAngle)
            rigidbody.AddTorque(LimitingTorqueMultiplier * Mathf.Pow(-angle - MinVerticalAngle, LimitingOffsetExponent) * -transform.right, ForceMode.Acceleration);

        var rot = rigidbody.rotation.eulerAngles;
        rot.z = 0;
        rigidbody.rotation = Quaternion.Euler(rot);

    }

    public void setFlyingMode(){
        flightMode = true;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
    }

    public void setSailingMode(){
        flightMode = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
    }

    void OnTurn(InputValue value)
    {
        // Debug.Log("Landing");
        input.Turn = value.Get<float>();
    }

    void OnPitch(InputValue value)
    {
        if (!flightMode)
        {
            return;
        }
        //Debug.Log("Lwasdanding");
        input.Pitch = value.Get<float>();
    }

    void OnRelease()
    {
        Debug.Log("Interactin");
        //onInteraction.Invoke();
    }

    void OnSlow(InputValue value)
    {
        input.Slow = value.Get<float>();
    }

    void OnSpeedUp(InputValue value)
    {
        input.SpeedUp = value.Get<float>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ring"))
        {
            currentSpeed += MaxVelocity / 2;
            Debug.Log("Entrou");
        }
    }

    public void respawn() {
        StartCoroutine(DisableMovementforTime(2f));
    }

    IEnumerator DisableMovementforTime(float delayDuration)
    {
        canMove = false;
        RigidbodyConstraints constraints = rigidbody.constraints;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        yield return new WaitForSeconds(delayDuration);
        rigidbody.constraints = constraints;
        canMove = true;
    }
}
