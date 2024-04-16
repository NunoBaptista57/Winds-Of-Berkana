using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Cinemachine;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.Events;

public class MainTunelVento : MonoBehaviour
{
    public UnityEvent EnterWindTunnelEvent;
    public UnityEvent ExitWindTunnelEvent;
    [SerializeField] private float rate;

    [SerializeField] private CinemachineFreeLook playerCam;
    [SerializeField] private CinemachineFreeLook vcam2;
    [SerializeField] private Transform _pointB;
    private bool overworldCamera = true;

    void OnTriggerEnter(Collider target){
        if (target.gameObject.TryGetComponent(out CharacterManager characterManager))
        {
            if (_pointB == null) 
            {
                Debug.Log("Point B not assigned");
                return;
            }
            SwitchPriority();
            EnterWindTunnelEvent.Invoke();
            StartCoroutine(MoveToPointB(characterManager));
        }
    }

    private IEnumerator MoveToPointB(CharacterManager characterManager)
    {
        characterManager.SetCanMove(false);
        Transform characterTransform = characterManager.gameObject.transform;
        while (Vector3.Distance(characterTransform.position, _pointB.position) >= 1f)
        {
            characterTransform.position = Vector3.MoveTowards(characterTransform.position, _pointB.position, rate * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        SwitchPriority();
        characterManager.SetCanMove(true);
        ExitWindTunnelEvent.Invoke();
    }

    void SwitchPriority()
    {
        if (overworldCamera)
        {
            vcam2.Priority = playerCam.Priority;
            playerCam.Priority = 0;
        }
        else
        {
            playerCam.Priority = vcam2.Priority;
            vcam2.Priority = 0;   
        }

        overworldCamera = !overworldCamera;
    }

}
