using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WindVolume))]
public class WindVolumeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.EditorToolbarForTarget(EditorGUIUtility.TrTempContent("Edit Collider"), target);
        DrawDefaultInspector();
    }
}