using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Rock))]
public class RockEditor : Editor
{
    Rock rock;
    Editor shapeEditor;
    Editor colorEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if(check.changed)
            {
                rock.GeneratePlanet();
            }
        }

        if(GUILayout.Button("Generate Planet"))
        {
            rock.GeneratePlanet();
        }

        DrawSettingsEditor(rock.shapeSettings, rock.OnShapeSettingsUpdated, ref rock.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(rock.colorSettings, rock.OnColorSettingsUpdated, ref rock.colorSettingsFoldout, ref colorEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldOut, ref Editor editor)
    {

        if (settings != null)
        {
            foldOut = EditorGUILayout.InspectorTitlebar(foldOut, settings);

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldOut)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }
     
    }

    private void OnEnable()
    {
        rock = (Rock)target;
    }
}
