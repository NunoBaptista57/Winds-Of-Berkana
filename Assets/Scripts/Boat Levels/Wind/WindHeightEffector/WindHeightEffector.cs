using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WindHeightEffector : MonoBehaviour, IWindEffector
{
    public enum Direction
    {
        Up,
        Down,
    }

    public Vector3 Force { get; private set; }

    [SerializeField] float HeightThreshold = 0;
    [SerializeField] Direction WindDirection;
    [SerializeField] float ForceDistanceMultiplier = 1;
    [SerializeField, ReadOnlyInspector] Vector3 CurrentForce;

    bool wasInside;

    const float GIZMO_SIZE = 15;

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        var player = FindObjectOfType<PlayerBoatEntity>();
        if (player != null)
        {
            var p = player.transform.position.HorizontalProjection();
            Vector3[] pts = new Vector3[] {
                new Vector3(-GIZMO_SIZE, HeightThreshold, -GIZMO_SIZE) + p,
                new Vector3(-GIZMO_SIZE,HeightThreshold, GIZMO_SIZE) + p,
                new Vector3(GIZMO_SIZE, HeightThreshold, GIZMO_SIZE) + p,
                new Vector3(GIZMO_SIZE, HeightThreshold, -GIZMO_SIZE) + p,
                new Vector3(-GIZMO_SIZE, HeightThreshold, -GIZMO_SIZE) + p,
            };

            Handles.DrawPolyLine(pts);
        }
#endif
    }

    private void FixedUpdate()
    {
        float dist = (WindDirection == Direction.Down ? 1 : -1) * (PlayerBoatEntity.instance.transform.position.y - HeightThreshold);
        if (dist > 0)
        {
            if (!wasInside)
            {
                wasInside = true;
                WindManager.OnPlayerEnter(this);
            }
            Force = (WindDirection == Direction.Up ? Vector3.up : Vector3.down) * dist * ForceDistanceMultiplier;
        }
        else
        {
            if (wasInside)
            {
                wasInside = false;
                WindManager.OnPlayerLeave(this);
            }
            Force = Vector3.zero;
        }
        CurrentForce = Force;
    }
}
