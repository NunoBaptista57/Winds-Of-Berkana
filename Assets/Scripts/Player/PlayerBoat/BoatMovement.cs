using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatMovement : MonoBehaviour
{
    new Rigidbody rigidbody;
    [Header("Wind/Sails")]
    [Min(0), SerializeField] float WindForce;
    [Min(0), SerializeField] float MinSailMultiplier = 0.0f;

    [Min(0), SerializeField] float SailEffectStrength = 1;
    [Min(0), SerializeField] float ReelSpeed = 1;
    [Min(0)] public float MaxVelocity;
    [ReadOnlyInspector] public float CurrentMaxVelocity;
    [Min(0), SerializeField] float VelocityLimitingStrength = 1;
    [Min(0), SerializeField] float TurningTorque;
    [Min(0), SerializeField] float BoostMaxSpeedMultiplier = 1.1f;
    [Min(0), SerializeField] float BoostAccelerationMultiplier = 1.1f;

    [Min(0), SerializeField] float MaxBoostCapacity = 1f;
    [Min(0), SerializeField] float BoostUsagePerSecond = 0.3f;
    [Min(0), SerializeField] float BoostRecoveredPerSecond = 0.2f;
    [ReadOnlyInspector, SerializeField] float CurrentBoost = 0;
    [ReadOnlyInspector, SerializeField] bool Boosting = false;
    [ReadOnlyInspector, SerializeField] bool BoostHitBottom = false;
    

    struct PlayerInput
    {
        public float Turn;
        public float Pitch;
        public float Reel;
        public bool Boost;
    }
    [ReadOnlyInspector, SerializeField] float currentSail;
    [ReadOnlyInspector, SerializeField] float currentSailMultiplier;

    [Header("Angle Limiting")]
    [SerializeField, Range(0, 90)] float MinVerticalAngle;
    [SerializeField, Range(0, 90)] float MaxVerticalAngle;
    [SerializeField, Min(0)] float LimitingTorqueMultiplier = 1;
    [SerializeField, Min(0)] float LimitingOffsetExponent = 1;


    PlayerInput input;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        currentSail = 1;
        currentSailMultiplier = 1;
        CurrentMaxVelocity = MaxVelocity;

        CurrentBoost = MaxBoostCapacity;
        BoostHitBottom = false;
    }

    void Update()
    {
        currentSail = Mathf.Clamp(currentSail + input.Reel * Time.deltaTime, 0, 1);
        currentSailMultiplier = Mathf.Lerp(
            currentSailMultiplier,
            Mathf.Lerp(MinSailMultiplier, 1, currentSail),
            SailEffectStrength * Time.deltaTime);

        if (input.Boost && !BoostHitBottom)
        {
            Boosting = true;
            CurrentBoost -= BoostUsagePerSecond * Time.deltaTime;
            if (CurrentBoost <= 0)
            {
                CurrentBoost = 0;
                Boosting = false;
                BoostHitBottom = true;
            }
        } 
        else if (CurrentBoost > MaxBoostCapacity)
        {
            CurrentBoost = MaxBoostCapacity;
            BoostHitBottom = false;
        } 
        else if (CurrentBoost < MaxBoostCapacity)
        {
            CurrentBoost += BoostRecoveredPerSecond * Time.deltaTime;
            if (CurrentBoost > MaxBoostCapacity)
            {
                CurrentBoost = MaxBoostCapacity;
                BoostHitBottom = false;
            }
        }
    }

    void FixedUpdate()
    {
        rigidbody.velocity = Vector3.Project(rigidbody.velocity, transform.forward);
        rigidbody.AddForce(WindForce * (Boosting ? BoostAccelerationMultiplier : 1) * transform.forward, ForceMode.Acceleration);

        if (rigidbody.velocity.magnitude > currentSailMultiplier * CurrentMaxVelocity * (Boosting ? BoostMaxSpeedMultiplier : 1))
        {
            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, Mathf.Lerp(rigidbody.velocity.magnitude, currentSailMultiplier * CurrentMaxVelocity * (Boosting ? BoostMaxSpeedMultiplier : 1), VelocityLimitingStrength * Time.fixedDeltaTime));
        }
        rigidbody.AddTorque(TurningTorque * input.Turn * Vector3.up, ForceMode.Acceleration);
        rigidbody.AddTorque(TurningTorque * input.Pitch * Vector3.Cross(transform.forward, Vector3.up).normalized, ForceMode.Acceleration);

        float angle = Vector3.SignedAngle(transform.forward, transform.forward.HorizontalProjection(), transform.right);
        if (angle > MaxVerticalAngle)
            rigidbody.AddTorque(LimitingTorqueMultiplier * Mathf.Pow(angle - MaxVerticalAngle, LimitingOffsetExponent) * transform.right, ForceMode.Acceleration);
        if (-angle > MinVerticalAngle)
            rigidbody.AddTorque(LimitingTorqueMultiplier * Mathf.Pow(-angle - MinVerticalAngle, LimitingOffsetExponent) * -transform.right, ForceMode.Acceleration);

        var rot = rigidbody.rotation.eulerAngles;
        rot.z = 0;
        rigidbody.rotation = Quaternion.Euler(rot);

    }

    void OnTurn(InputValue value)
    {
        input.Turn = value.Get<float>();
    }

    void OnReel(InputValue value)
    {
        input.Reel = value.Get<float>();
    }

    void OnPitch(InputValue value)
    {
        input.Pitch = value.Get<float>();
    }

    void OnBoost(InputValue value)
    {
        input.Boost = Convert.ToBoolean(value.Get());
    }
}
