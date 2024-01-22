using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Cinemachine;
using Vector3 = UnityEngine.Vector3;

public class MainTunelVento : MonoBehaviour
{
    public GameObject _player;
    private CharacterLocomotion _characterLocomotion;

    private bool overworldCamera = true;

    public float rate;
    
    [SerializeField]
    private CinemachineFreeLook playerCam;
    [SerializeField]
    private CinemachineFreeLook vcam2;

    void Start(){
        _characterLocomotion = _player.GetComponentInChildren<CharacterLocomotion>();
    }

    void OnTriggerEnter(Collider target){
        if (target.gameObject.CompareTag("Player"))
        {
            SwitchPriority();
        }
    }
    void OnTriggerStay(Collider target)
    {
        if (target.gameObject.CompareTag("Player"))
        {
            _characterLocomotion.Tunnel();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SwitchPriority();
        }
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
