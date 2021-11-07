using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WindSource))]
public class WindSourceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.EditorToolbarForTarget(EditorGUIUtility.TrTempContent("Edit Collider"), target);
        DrawDefaultInspector();
    }
}