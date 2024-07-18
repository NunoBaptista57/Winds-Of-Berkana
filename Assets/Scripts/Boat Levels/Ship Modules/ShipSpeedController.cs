using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipSpeedController : MonoBehaviour
{
    private BoatMovement _BoatMovement;
    private bool flightMode = false;
    private float MaxVerticalAngle;
    private float MinVerticalAngle;
    private float speedModifier = 1f;

    [Header("Tilt Boost Settings")]
    [SerializeField, Min(0)] float maxDownBoost = 1.5f;
    [SerializeField, Min(0)] float maxUpSlow = 0.5f;
    [SerializeField, Min(0)] float accelerationRate = 2.5f;
    [SerializeField, Min(0)] float decelerationRate = 2.5f;

    void Awake()
    {
        // Get Required Values for calculations
        _BoatMovement = gameObject.GetComponent<BoatMovement>();

        flightMode = _BoatMovement.flightMode;
        MaxVerticalAngle = _BoatMovement.MaxVerticalAngle;
        MinVerticalAngle = _BoatMovement.MinVerticalAngle;
    }

    void FixedUpdate()
    {
        UpdateTiltModifier();
        ApplyModifiers();
    }

    private void ApplyModifiers()
    {
        _BoatMovement.speedModifier = speedModifier;
    }

    void UpdateTiltModifier()
    {
        if (flightMode)
        {
            float pitchAngle = Vector3.Angle(transform.forward, Vector3.up);

            if (pitchAngle < 90)
            {
                float t = Mathf.InverseLerp(90f, MaxVerticalAngle, pitchAngle);
                float targetModifier = Mathf.Lerp(speedModifier, maxUpSlow, t);
                speedModifier = Mathf.Lerp(speedModifier, Mathf.Lerp(1f, targetModifier, t), Time.fixedDeltaTime * decelerationRate);
            }
            else if (pitchAngle > 90)
            {
                float t = Mathf.InverseLerp(90f, MinVerticalAngle, pitchAngle);
                float targetModifier = Mathf.Lerp(maxDownBoost, speedModifier, t);
                speedModifier = Mathf.Lerp(speedModifier, Mathf.Lerp(targetModifier, 1f, t), Time.fixedDeltaTime * accelerationRate);
            }
        }
    }
}
