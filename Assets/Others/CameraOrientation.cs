using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraOrientation : MonoBehaviour
{
    [SerializeField] Camera target;
    [SerializeField] CinemachineVirtualCamera cinemachineTarget;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = target.transform.rotation;
        if (cinemachineTarget)
        {
            transform.rotation = cinemachineTarget.State.FinalOrientation;
        }
    }
}
