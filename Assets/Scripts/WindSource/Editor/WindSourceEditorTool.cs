using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[EditorTool("Wind Source Direction Tool", typeof(WindSource))]
public class WindSourceEditorTool : EditorTool
{
    public override void OnToolGUI(EditorWindow window)
    {
        WindSource t = target as WindSource;
        var so = new SerializedObject(t);
        var prop = so.FindProperty("WindDirection");
        if (Event.current.type == EventType.Repaint)
            Handles.ArrowHandleCap(0, t.transform.position, prop.quaternionValue, HandleUtility.GetHandleSize(t.transform.position), EventType.Repaint);
        EditorGUI.BeginChangeCheck();
        Quaternion rot = Handles.RotationHandle(prop.quaternionValue, t.transform.position);
        if (EditorGUI.EndChangeCheck())
        {
            prop.quaternionValue = rot;
            so.ApplyModifiedProperties();
        }
    }
}