using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using AmplifyShaderEditor;

public class BoatMovement : MonoBehaviour
{
    new Rigidbody rigidbody;

    [Header("Boat Mode")]
    [SerializeField] bool boat_mode;

    [Header("Control Settings")]
    public bool canMove = true;

    [Header("Wind")]
    [Min(0), SerializeField] float WindForce;
    [Min(0)] public float MaxVelocity;
    [ReadOnlyInspector] public float CurrentMaxVelocity;
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


    PlayerInput input;
    public Action onInteraction;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        CurrentMaxVelocity = MaxVelocity;
    }

    public void AllowPlayerControl(bool setTo) { canMove = setTo; }

    void FixedUpdate()
    {
        if (!canMove) { return; }

        if (Input.GetKey(KeyCode.P))
        {
            if (CurrentMaxVelocity < MaxVelocity * 2)
            {
                CurrentMaxVelocity += .1f;
            }
        }
        if (Input.GetKey(KeyCode.O))
        {
            if (CurrentMaxVelocity > 0)//MaxVelocity/2)
            {
                CurrentMaxVelocity -= .1f;
            }
        }


        rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, Vector3.Project(rigidbody.velocity, transform.forward), ForwardStabilization);
        rigidbody.AddForce(WindForce * transform.forward, ForceMode.Acceleration);

        if (rigidbody.velocity.magnitude > CurrentMaxVelocity)
        {
            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, Mathf.Lerp(rigidbody.velocity.magnitude, CurrentMaxVelocity, VelocityLimitingStrength * Time.fixedDeltaTime));
        }
        rigidbody.AddTorque(TurningTorque * input.Turn * Vector3.up, ForceMode.Acceleration);
        //rigidbody.AddTorque(TurningTorque * input.Pitch * Vector3.Cross(transform.forward, Vector3.up).normalized, ForceMode.Acceleration);

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
        // Debug.Log("Landing");
        input.Turn = value.Get<float>();
    }

    void OnPitch(InputValue value)
    {
        if (boat_mode)
        {
            return;
        }
        //Debug.Log("Lwasdanding");
        input.Pitch = value.Get<float>();
    }

    void OnRelease()
    {
        Debug.Log("Interactin");
        onInteraction.Invoke();
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
            CurrentMaxVelocity += MaxVelocity / 2;
            Debug.Log("Entrou");
        }
    }
}
