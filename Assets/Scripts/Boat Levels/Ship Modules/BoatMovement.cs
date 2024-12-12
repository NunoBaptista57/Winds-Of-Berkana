using System.Collections;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatMovement : MonoBehaviour
{
    new Rigidbody rigidbody;

    [Header("Flight Settings")]
    [SerializeField] public bool flightMode = true;
    [ReadOnlyInspector] public float speedModifier = 1f; 

    [Header("Control Settings")]
    public bool canMove = true;
    [Min(0)] public float MaxVelocity;
    [ReadOnlyInspector] public float currentSpeed;
    [Min(0), SerializeField] float acceleration = 0.1f;


    [Header("Wind")]
    [ReadOnlyInspector] public Vector3 windVector;
    [ReadOnlyInspector] public Vector3 dodgeVector;
    [ReadOnlyInspector] public bool canLeftDodge;
    [Min(0), SerializeField] float WindForce;
    [Min(0), SerializeField] float VelocityLimitingStrength = 1;
    [Min(0), SerializeField] float TurningTorque;

    [Header("Angle Limiting")]
    [SerializeField, Range(0, 90)] public float MinVerticalAngle;
    [SerializeField, Range(0, 90)] public float MaxVerticalAngle;
    [SerializeField, Min(0)] float LimitingTorqueMultiplier = 1;
    [SerializeField, Min(0)] float LimitingOffsetExponent = 1;

    [Header("Stabilization")]
    [SerializeField, Range(0, 1)] float ForwardStabilization;

    public struct PlayerInput
    {
        public float Turn;
        public float Pitch;
        public float Reel;
        public float Slow;
        public float SpeedUp;
    }

    public PlayerInput input;
    public Action onInteraction;
    public AudioManager audioManager;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        currentSpeed = 0;
        windVector = new Vector3(0f,0f,0f);
        dodgeVector = new Vector3(0f,0f,10f);
        canLeftDodge = true;

        if (!flightMode)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
        }

        audioManager = AudioManager.Instance;

        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager not found in the scene.");
        }
        else
        {
            Debug.Log("AudioManager found.");
        }
    }

    public void AllowPlayerControl(bool setTo) { canMove = setTo; }

    void FixedUpdate()
    {
        if (!canMove) { rigidbody.velocity = Vector3.zero; return; }

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
    
        rigidbody.AddForce(windVector* 3, ForceMode.Impulse);
        rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, Vector3.Project(rigidbody.velocity, transform.forward), ForwardStabilization);
        rigidbody.AddForce(WindForce * transform.forward, ForceMode.Acceleration);

        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, Mathf.Lerp(rigidbody.velocity.magnitude, currentSpeed * speedModifier, VelocityLimitingStrength * Time.fixedDeltaTime));
        
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
        //Debug.Log("speedModifier:" + speedModifier + "   speed:" + Math.Round(rigidbody.velocity.magnitude, 3) + "   Max speed:" + Math.Round(MaxVelocity*speedModifier, 3));
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

    public void setFlyingMode(){
        flightMode = true;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
    }

    public void setSailingMode(){
        flightMode = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
    }


    ///// Input Functions /////


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

    void OnSlow(InputValue value)
    {
        if (!flightMode)
        {
            Debug.Log("SlowSound");
            audioManager.SlowSound();
        }
        else{
            audioManager.GlidingSound();
        }
        input.Slow = value.Get<float>();
    }

    void OnSpeedUp(InputValue value)
    {
        if (!flightMode)
        {
            Debug.Log("FastSound");
            audioManager.FastSound();
        }
        else
        {
            audioManager.GlidingSound();
        }        
        input.SpeedUp = value.Get<float>();
    }


    ///// Mechanic Functions /////


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ring"))
        {
            currentSpeed += MaxVelocity / 2;
            Debug.Log("Entrou");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("WindTunnel"))
        {
            //remove wind vector
            windVector = new Vector3(0f,0f,0f);
            Debug.Log("is out of wind");
            
            //Debug.Log(collision.gameObject.GetComponent<windVector>());
        }
    }

    //Collision with item is detected
    private void OnEnable()
    {
        Item.OnItemCollected += ItemSpeedBoost;
    }

    private void OnDisable()
    {
        Item.OnItemCollected -= ItemSpeedBoost;
    }

    private void ItemSpeedBoost()
    {   
        //Debug.Log("speed up");
        StartCoroutine (SpeedBoost());
        acceleration = acceleration * 2;
        MaxVelocity = MaxVelocity * 4;
    }

    private IEnumerator SpeedBoost()
    {
        yield return new WaitForSeconds(5);
        acceleration = acceleration / 2;
        MaxVelocity = MaxVelocity / 4;
        if (currentSpeed > MaxVelocity)
        {
            currentSpeed = MaxVelocity;
        }
        //Debug.Log("speed down");
    }

    private IEnumerator LeftDodge()
    {
        yield return new WaitForSeconds(5);
        canLeftDodge = true;
        //Debug.Log("speed down");
    }
}
