using System.Collections;
using System.Collections.Generic;
using AmplifyShaderEditor;
using Unity.Mathematics;
using UnityEngine;

using UnityEngine.Splines;

public class pathFollow : MonoBehaviour
{
    [SerializeField] private SplineContainer path;

    private Spline currentSpline;

    private Rigidbody rb;

    public float offsetY = 10f;
    public float offsetRotationX = 10f;

    public float speed = 3f;

    public void HitJunction(Spline path)
    {
        currentSpline = path;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentSpline = path.Splines[0];
    }

    private void FixedUpdate()
    {
        var native = new NativeSpline(currentSpline);
        float distance = SplineUtility.GetNearestPoint(native, transform.localPosition, out float3 nearest,out float t);
        
        Vector3 forward = Vector3.Normalize(native.EvaluateTangent(t));

        if (Vector3.Angle(forward, -transform.up) > 30f){forward = -transform.up;} // Prevent problems with crossing splines

        Vector3 up = native.EvaluateUpVector(t);
        
        var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(new Vector3(0,0,1), new Vector3(0,1,0)));
     
        transform.rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;

        transform.rotation *= Quaternion.Euler(offsetRotationX, 0, 0);

        transform.position = transform.position + forward * speed * Time.deltaTime;
    }
}
