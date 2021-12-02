using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class WindCurrent : MonoBehaviour
{
    public Vector3[] points = new Vector3[3];
    public float radius = 1f;

    [Range(1, 20)]
    public int steps = 5;

    public Vector3 GetCurvePoint(float t) 
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
		return oneMinusT * oneMinusT * points[0] + 2f * oneMinusT * t * points[1] + t * t * points[2];
    }

    public Vector3 GetCurvePointDerivative(float t) 
    {
        return 2f * (1f - t) * (points[1] - points[0]) + 2f * t * (points[2] - points[1]);
    }

    private void Start()
    {
        for (int i = 0; i <= steps; i++) {
            GameObject colliderObject = new GameObject("WindCurrentCollider");
            colliderObject.transform.position = transform.position + GetCurvePoint(i / (float) steps);
            
            SphereCollider sphereCollider = colliderObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = radius;

            WindCurrentCollider script = colliderObject.AddComponent<WindCurrentCollider>();
            script.direction = GetCurvePointDerivative(i / (float) steps);

        }
    }


    private void OnDrawGizmos() 
    {
        Transform handleTransform = transform;

        Handles.color = Color.white;
        Vector3 lineStart = handleTransform.position + GetCurvePoint(0f);
        for (int i = 0; i <= steps; i++) 
        {
            Vector3 lineEnd = handleTransform.position + GetCurvePoint(i/ (float) steps);
            Gizmos.DrawLine(lineStart, lineEnd);
            lineStart = lineEnd;

            Handles.DrawWireDisc(handleTransform.position + GetCurvePoint(i / (float) steps), GetCurvePointDerivative(i / (float) steps), radius);
        }
    }

    private void OnValidate()
    {
        if (points.Length != 3)
            Array.Resize(ref points, 3);
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        BoatEntity bm = other.GetComponent<BoatEntity>();
        if (bm != null)
        {
            Debug.Log("TEST");
        }
    }
    */
}
