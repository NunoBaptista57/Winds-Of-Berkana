using System.Collections;
using System.Collections.Generic;
using AmplifyShaderEditor;
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

    [Header("References")] // Might refactor to not be needed
    public Transform playerPos;

    private bool starting = true;
    
    private Spline currentSpline;

    private Rigidbody rb;


    public void HitJunction(Spline path)
    {
        currentSpline = path;
    }

    IEnumerator ActivateForDuration()
    {
        starting = true;
        yield return new WaitForSeconds(startDuration);
        starting = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentSpline = path.Splines[0];

        StartCoroutine(ActivateForDuration());
    }

    private void FixedUpdate()
    {
        Vector3 toPlayer = playerPos.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);
        bool hasPlayer = angleToPlayer > angleThreshold && toPlayer.magnitude <= maxDistance;
            
        if (hasPlayer || !needsPlayerProximity || starting)
        {
            var native = new NativeSpline(currentSpline);
            float distance = SplineUtility.GetNearestPoint(native, transform.localPosition, out float3 nearest,out float t);
            
            Vector3 forward = Vector3.Normalize(native.EvaluateTangent(t));

            if (Vector3.Angle(forward, -transform.up) > 30f){forward = -transform.up;} // Prevent problems with crossing splines

            Vector3 up = native.EvaluateUpVector(t);
            
            var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(new Vector3(0,0,1), new Vector3(0,1,0)));
        
            transform.rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;

            transform.rotation *= Quaternion.Euler(offsetRotationX, 0, 0);

            transform.position = transform.position + forward * speed * Time.deltaTime; // Might change to physics
        }
    }
}
