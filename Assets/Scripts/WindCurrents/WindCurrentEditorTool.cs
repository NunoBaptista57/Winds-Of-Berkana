using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[EditorTool("Wind Current Direction Tool", typeof(WindCurrent))]
public class WindSourceEditorTool : EditorTool
{
    public override void OnToolGUI(EditorWindow window)
    {
        WindCurrent t = target as WindCurrent;
        if (t != null)
        {
            var so = new SerializedObject(t);
            var startPos = t.transform.TransformPoint(t.points[0]);
            var startProp = so.FindProperty("StartDirection");
            if (Event.current.type == EventType.Repaint)
                Handles.ArrowHandleCap(0, startPos, startProp.quaternionValue, HandleUtility.GetHandleSize(startPos), EventType.Repaint);
            var endPos = t.transform.TransformPoint(t.points[2]);
            var endProp = so.FindProperty("EndDirection");
            if (Event.current.type == EventType.Repaint)
                Handles.ArrowHandleCap(0, endPos, endProp.quaternionValue, HandleUtility.GetHandleSize(endPos), EventType.Repaint);
            EditorGUI.BeginChangeCheck();
            Quaternion startRot = Handles.RotationHandle(startProp.quaternionValue, startPos);
            Quaternion endRot = Handles.RotationHandle(endProp.quaternionValue, endPos);
            if (EditorGUI.EndChangeCheck())
            {
                startProp.quaternionValue = startRot;
                endProp.quaternionValue = endRot;
                for (int i = 0; i <= t.steps; i++)
                {
                    var collider = t.editorColliders[i];
                    collider.Radius = t.radius;
                    collider.direction = Quaternion.Slerp(t.StartDirection, t.EndDirection, i / (float)t.steps);
                    collider.strength = Mathf.Lerp(t.StartStrength, t.EndStrength, i / (float)t.steps);
                }
                so.ApplyModifiedProperties();
            }
        }
    }
}