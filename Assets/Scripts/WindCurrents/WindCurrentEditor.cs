using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WindCurrent))]
public class WindCurrentEditor : Editor
{

    private void OnSceneGUI()
    {
        WindCurrent wc = target as WindCurrent;
        Transform handleTransform = wc.transform;
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        Handles.color = Color.grey;

        ShowPointHandle(wc, 0, handleTransform, handleRotation);
        for (int i = 1; i < wc.points.Length; i++)
        {
            ShowPointHandle(wc, i, handleTransform, handleRotation);
            Handles.DrawLine(handleTransform.position + wc.points[i - 1], handleTransform.position + wc.points[i]);
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.EditorToolbarForTarget(EditorGUIUtility.TrTempContent("Edit Current"), target);
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck())
        {
            var current = target as WindCurrent;
            foreach (WindCurrentCollider collider in current.editorColliders)
            {
                if (collider)
                    DestroyImmediate(collider.gameObject);
            }
            current.editorColliders.Clear();
            for (int i = 0; i <= current.steps; i++)
            {
                float t = i / (float)current.steps;
                WindCurrentCollider collider = Instantiate(current.ColliderPrefab, current.transform.position + current.GetCurvePoint(t), Quaternion.identity, current.transform);
                collider.gameObject.hideFlags = HideFlags.NotEditable;
                collider.Radius = current.radius;
                //collider.direction = Quaternion.Slerp(current.StartDirection, current.EndDirection, t);
                collider.direction = Quaternion.LookRotation(-current.GetCurvePointDerivative(t).normalized, Vector3.up);
                collider.strength = Mathf.Lerp(current.StartStrength, current.EndStrength, t);
                current.editorColliders.Add(collider);
            }
        }
    }

    private void ShowPointHandle(WindCurrent wc, int idx, Transform handleTransform, Quaternion handleRotation)
    {
        Vector3 point = wc.transform.position + wc.points[idx];
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(wc, "Move Point");
            EditorUtility.SetDirty(wc);
            wc.points[idx] = handleTransform.InverseTransformPoint(point);
        }
    }
}
