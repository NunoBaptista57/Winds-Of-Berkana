using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class WindCurrent : MonoBehaviour, IWindEffector
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
    public bool PullTowardsCenter;
    public float PullTowardsCenterStrength;
    public float RadiusIntensify = 2;
    public bool UseCustomDirections = false;

    public WindCurrentCollider ColliderPrefab;
    List<WindCurrentCollider> editorColliders = new List<WindCurrentCollider>();

    HashSet<WindCurrentCollider> currentColliders = new HashSet<WindCurrentCollider>();

    public Vector3 Force { get; private set; }

    PlayerBoatEntity player;

    public void OnValidate()
    {
        if (points.Length != 3)
            Array.Resize(ref points, 3);
        UpdateWindDirection();
    }

    private void OnEnable()
    {
        RefreshColliders();
    }

    void FixedUpdate()
    {
        Force = Vector3.zero;
        if (PullTowardsCenter && player != null)
        {
            var linePoint = ClosestPoint(player.transform.position);
            Vector3 forceDir = linePoint - player.transform.position;
            float forceRatio = FloatUtils.Intensify(forceDir.magnitude / radius, RadiusIntensify);
            Force += (forceDir).normalized * PullTowardsCenterStrength * forceRatio;
            //Gizmos.DrawSphere(linePoint, 0.5f);
            Debug.DrawLine(player.transform.position, player.transform.position + Force.normalized * 10, Color.blue, Time.fixedDeltaTime);
        }
        if (currentColliders.Count > 0)
        {
            foreach (var collider in currentColliders)
            {
                Force += collider.direction * Vector3.forward * collider.strength;
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

#if UNITY_EDITOR
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

#endif

    public void OnPlayerEnter(WindCurrentCollider entered, PlayerBoatEntity player)
    {
        if (currentColliders.Count == 0)
        {
            WindManager.OnPlayerEnter(this);
            this.player = player;
        }
        currentColliders.Add(entered);
    }

    public void OnPlayerLeave(WindCurrentCollider left)
    {
        currentColliders.Remove(left);
        if (currentColliders.Count == 0)
        {
            WindManager.OnPlayerLeave(this);
            player = null;
        }
    }

    public void RefreshColliders()
    {
        foreach (WindCurrentCollider collider in GetComponentsInChildren<WindCurrentCollider>().ToList())
        {
            if (collider)
                DestroyImmediate(collider.gameObject);
        }
        editorColliders.Clear();
        for (int i = 0; i <= steps; i++)
        {
            float t = i / (float)steps;
            WindCurrentCollider collider = Instantiate(ColliderPrefab, transform.position + GetCurvePoint(t), Quaternion.identity, transform);
            collider.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
            collider.Radius = radius;
            collider.t = t;
            if (UseCustomDirections)
                collider.direction = Quaternion.Slerp(StartDirection, EndDirection, t);
            else
                collider.direction = Quaternion.LookRotation(-GetCurvePointDerivative(t).normalized, Vector3.up);
            collider.strength = Mathf.Lerp(StartStrength, EndStrength, t);
            editorColliders.Add(collider);
        }

        for (int i = 0; i <= steps; i++)
        {
            if (i > 0)
                editorColliders[i].previous = editorColliders[i - 1];
            if (i < steps)
                editorColliders[i].next = editorColliders[i + 1];
        }
    }

    public void UpdateWindDirection()
    {
        var colliders = GetComponentsInChildren<WindCurrentCollider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            var collider = colliders[i];
            collider.Radius = radius;
            collider.direction = Quaternion.Slerp(StartDirection, EndDirection, i / (float)steps);
            collider.strength = Mathf.Lerp(StartStrength, EndStrength, i / (float)steps);
        }
    }

    public Vector3 ClosestPoint(Vector3 position)
    {
        WindCurrentCollider start = currentColliders.Aggregate((c1, c2) => c1.t < c2.t ? c1 : c2);
        WindCurrentCollider end;
        if (currentColliders.Count > 1)
        {
            end = currentColliders.Aggregate((c1, c2) => c1.t > c2.t ? c1 : c2);
        }
        else
        {
            end = start.next != null ? start.next : start.previous;
        }
        return NearestPointOnLine(start.transform.position, end.transform.position - start.transform.position, position);
    }

    public static Vector3 NearestPointOnLine(Vector3 origin, Vector3 dir, Vector3 position)
    {
        dir.Normalize();
        var v = position - origin;
        var d = Vector3.Dot(v, dir);
        return origin + dir * d;
    }
}
