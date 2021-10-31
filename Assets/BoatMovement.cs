using System.Collections;
using System.Collections.Generic;
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
    [Min(0), SerializeField] float MaxVelocity;
    [Min(0), SerializeField] float VelocityLimitingStrength = 1;
    [Min(0), SerializeField] float TurningTorque;

    struct PlayerInput
    {
        public float Turn;
        public float Pitch;
        public float Reel;
    }
    [ReadOnlyInspector, SerializeField] float currentSail;
    [ReadOnlyInspector, SerializeField] float currentSailMultiplier;

    PlayerInput input;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        currentSail = 1;
        currentSailMultiplier = 1;
    }

    void Update()
    {
        currentSail = Mathf.Clamp(currentSail + input.Reel * Time.deltaTime, 0, 1);
        currentSailMultiplier = Mathf.Lerp(
            currentSailMultiplier,
            Mathf.Lerp(MinSailMultiplier, 1, currentSail),
            SailEffectStrength * Time.deltaTime);
    }

    void FixedUpdate()
    {
        rigidbody.velocity = Vector3.Project(rigidbody.velocity, transform.forward);
        rigidbody.AddForce(WindForce * transform.forward, ForceMode.Acceleration);

        if (rigidbody.velocity.magnitude > currentSailMultiplier * MaxVelocity)
        {
            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, Mathf.Lerp(rigidbody.velocity.magnitude, currentSailMultiplier * MaxVelocity, VelocityLimitingStrength * Time.fixedDeltaTime));
        }
        rigidbody.AddTorque(TurningTorque * input.Turn * Vector3.up, ForceMode.Acceleration);
        rigidbody.AddTorque(TurningTorque * input.Pitch * Vector3.Cross(transform.forward, Vector3.up).normalized, ForceMode.Acceleration);

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
}
