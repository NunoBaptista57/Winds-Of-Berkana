using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class WindCurrent : MonoBehaviour
{
    public Vector3[] points = new Vector3[3];

    public float radius = 1f;

    [Range(1, 20)]
    public int steps = 5;

    public Quaternion StartDirection = Quaternion.identity;
    public Quaternion EndDirection = Quaternion.identity;
    [Min(0)]
    public float StartStrength;
    [Min(0)]
    public float EndStrength;

    public WindCurrentCollider ColliderPrefab;
    [HideInInspector] public List<WindCurrentCollider> editorColliders = new List<WindCurrentCollider>();

    HashSet<WindCurrentCollider> currentColliders = new HashSet<WindCurrentCollider>();

    public Vector3 Force { get; private set; }

    void OnEnable()
    {
        editorColliders = GetComponentsInChildren<WindCurrentCollider>().ToList();
    }

    void FixedUpdate()
    {
        Force = Vector3.zero;
        Debug.Log("Force");
        if (currentColliders.Count > 0)
        {
            foreach (var collider in currentColliders)
            {
                Force += collider.direction * Vector3.forward * collider.strength;
                Debug.Log(collider.direction * Vector3.forward * collider.strength);
            }
            Force /= currentColliders.Count;
        }
    }

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

    private void OnValidate()
    {
        if (points.Length != 3)
            Array.Resize(ref points, 3);
    }

    private void OnDrawGizmos()
    {
        Transform handleTransform = transform;

        Handles.color = Color.white;
        Vector3 lineStart = handleTransform.position + GetCurvePoint(0f);
        for (int i = 0; i <= steps; i++)
        {
            Vector3 lineEnd = handleTransform.position + GetCurvePoint(i / (float)steps);
            Gizmos.DrawLine(lineStart, lineEnd);
            lineStart = lineEnd;

            Handles.DrawWireDisc(handleTransform.position + GetCurvePoint(i / (float)steps), GetCurvePointDerivative(i / (float)steps), radius);
        }
    }

    public void OnPlayerEnter(WindCurrentCollider entered)
    {
        if (currentColliders.Count == 0)
        {
            WindManager.OnPlayerEnter(this);
        }
        currentColliders.Add(entered);
    }

    public void OnPlayerLeave(WindCurrentCollider left)
    {
        currentColliders.Remove(left);
        if (currentColliders.Count == 0)
        {
            WindManager.OnPlayerLeave(this);
        }
    }
}
