using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainTunelVento : MonoBehaviour
{
    public GameObject _player;

    private bool overworldCamera = true;

    [SerializeField]
    private CinemachineFreeLook playerCam;
    [SerializeField]
    private CinemachineVirtualCamera vcam2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider target)
    {
        Debug.Log("Poop");
        if (target.gameObject.CompareTag("Player"))
        {
            SwitchPriority();
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
