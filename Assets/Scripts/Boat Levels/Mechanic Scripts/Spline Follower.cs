using System.Collections;
using Unity.Mathematics;
using UnityEngine;

using UnityEngine.Splines;

public class pathFollow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool needsPlayerProximity = true;
    [SerializeField] private SplineContainer path;
    public float speed = 3f;
    public float offsetRotationX = 10f; // Visual only Rotation fix
    public float maxDistance;
    public float angleThreshold;
    [Tooltip("The duration of time the entity should move at the start regardless of the player's position")]
    public float startDuration = 0f;
    [Tooltip("The ideal distance to maintain from the player")]
    public float distanceToMaintain = 10f;

    [Header("References")]
    public BoatMovement playerBoatMovement;

    private bool starting = true;
    private Spline currentSpline;
    private Rigidbody rb;
    private Transform playerPos;
    private int i = 0;


    public void HitJunction(Spline path)
    {
        currentSpline = path;
    }

    public void restart(){
        StartCoroutine(ActivateForDuration());
    }

    IEnumerator ActivateForDuration()
    {
        starting = true;
        i++;
        yield return new WaitForSeconds(startDuration);
        i--;
        if (i == 0)
        {
            starting = false;
        }
    }

    private void Start()
    {
        playerPos = playerBoatMovement.transform;

        rb = GetComponent<Rigidbody>();

        currentSpline = path.Splines[0];

        StartCoroutine(ActivateForDuration());
    }

    private void Update()
    {
        var native = new NativeSpline(currentSpline);
        float distance = SplineUtility.GetNearestPoint(native, transform.localPosition, out float3 nearest, out float t);

        Vector3 forward = Vector3.Normalize(native.EvaluateTangent(t));

        Vector3 toPlayer = playerPos.position - transform.position;
        float angleToPlayer = Vector3.Angle(forward, toPlayer);
        bool hasPlayer = angleToPlayer > angleThreshold && toPlayer.magnitude <= maxDistance;

        if (hasPlayer || !needsPlayerProximity || starting)
        {
            if (Vector3.Angle(forward, -transform.up) > 30f) { forward = -transform.up; } // Prevent problems with crossing splines

            Vector3 up = native.EvaluateUpVector(t);

            var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(new Vector3(0, 0, 1), new Vector3(0, 1, 0)));

            transform.rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;

            transform.rotation *= Quaternion.Euler(offsetRotationX, 0, 0);

            float adaptiveSpeed = speed;
            if (hasPlayer)
            {
                float distanceToPlayer = toPlayer.magnitude;
                float desiredDistance = distanceToPlayer - distanceToMaintain;
                adaptiveSpeed = Mathf.Clamp(playerBoatMovement.currentSpeed * (distanceToMaintain / distanceToPlayer), 0f, 2*speed);
            }
            transform.position += forward * adaptiveSpeed * Time.deltaTime;
        }
    }
}