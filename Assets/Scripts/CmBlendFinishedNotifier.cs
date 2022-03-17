using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using System;

public class CmBlendFinishedNotifier : MonoBehaviour
{
    CinemachineVirtualCameraBase vcamBase;
    CinemachineStateDrivenCamera parentCamera;
    [Serializable] public class BlendFinishedEvent : UnityEvent<CinemachineVirtualCameraBase> { }
    public BlendFinishedEvent OnBlendFinished;
    bool usingEvent;
    bool wasLive = false;

    void Start()
    {
        vcamBase = GetComponent<CinemachineVirtualCameraBase>();
        parentCamera = GetComponentInParent<CinemachineStateDrivenCamera>();
        ConnectToVcam(true);
        if (usingEvent)
            enabled = false;
    }

    void ConnectToVcam(bool connect)
    {
        var vcam = vcamBase as CinemachineVirtualCamera;
        if (vcam != null)
        {
            vcam.m_Transitions.m_OnCameraLive.RemoveListener(OnCameraLive);
            if (connect)
            {
                vcam.m_Transitions.m_OnCameraLive.AddListener(OnCameraLive);
                usingEvent = true;
            }
        }
        var freeLook = vcamBase as CinemachineFreeLook;
        if (freeLook != null)
        {
            freeLook.m_Transitions.m_OnCameraLive.RemoveListener(OnCameraLive);
            if (connect)
            {
                freeLook.m_Transitions.m_OnCameraLive.AddListener(OnCameraLive);
                usingEvent = true;
            }
        }
    }

    void OnCameraLive(ICinemachineCamera vcamIn, ICinemachineCamera vcamOut)
    {
        enabled = true;
    }

    void Update()
    {
        var brain = CinemachineCore.Instance.FindPotentialTargetBrain(vcamBase);
        if (usingEvent && brain == null)
            enabled = false;
        else if (brain && !brain.IsBlending && (parentCamera == null || !parentCamera.IsBlending))
        {
            bool live = brain.IsLive(vcamBase) && (parentCamera == null || parentCamera.IsLiveChild(vcamBase));
            if (live && !wasLive)
                OnBlendFinished.Invoke(vcamBase);
            wasLive = live;
            if (usingEvent)
                enabled = false;
        }
        else
        {
            wasLive = false;
        }
    }
}