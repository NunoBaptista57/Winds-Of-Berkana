using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR
[CustomEditor(typeof(WindCurrent))]
public class WindCurrentEditor : Editor
{
    public float HandleScaleFactor = 0.5f;

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
        WindCurrent current = target as WindCurrent;

        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();

        if (current.UseCustomDirections)
        {
            // Quaternion Toolbar
            EditorGUILayout.EditorToolbarForTarget(EditorGUIUtility.TrTempContent("Edit Current"), target);
        }
        if (EditorGUI.EndChangeCheck())
        {
            current.RefreshColliders();
        }
    }

    private void ShowPointHandle(WindCurrent wc, int idx, Transform handleTransform, Quaternion handleRotation)
    {
        var startMatrix = Handles.matrix;
        Vector3 point = wc.transform.position + wc.points[idx];
        EditorGUI.BeginChangeCheck();
        Handles.matrix = Matrix4x4.Scale(Vector3.one * HandleScaleFactor) * startMatrix;
        point = Handles.DoPositionHandle(point / HandleScaleFactor, handleRotation);
        Handles.matrix = startMatrix;
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(wc, "Move Point");
            EditorUtility.SetDirty(wc);
            wc.points[idx] = handleTransform.InverseTransformPoint(point * HandleScaleFactor);
            wc.RefreshColliders();
        }
    }
}
#endif