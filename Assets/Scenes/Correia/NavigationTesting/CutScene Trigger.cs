using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour
{
    [Header("Optional Reference")]
    [SerializeField] CutSceneManager cutSceneManager;
    void OnTriggerEnter(Collider other)
    {
        if (cutSceneManager != null)
        {
            cutSceneManager = FindObjectOfType<CutSceneManager>();
        }
        
        if (cutSceneManager != null)
        {
            cutSceneManager.beginCutScene();
        }
        else
        {
            Debug.LogWarning("CutSceneManager not found in the scene. Make sure it's present or properly configured.");
        }
    }
}
