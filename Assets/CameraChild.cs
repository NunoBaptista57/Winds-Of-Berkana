using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChild : MonoBehaviour
{
    [SerializeField] Camera target;
    [SerializeField] CinemachineVirtualCamera cinemachineTarget;
    Vector3 initialPosition;
    Quaternion initialRotation;

    private void Start()
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        if (target)
        {
            initialPosition = target.transform.InverseTransformPoint(position);
            initialRotation = Quaternion.Inverse(target.transform.rotation) * rotation;
        }
        if (cinemachineTarget)
        {
            initialPosition = cinemachineTarget.transform.InverseTransformPoint(position);
            initialRotation = Quaternion.Inverse(cinemachineTarget.transform.rotation) * rotation;
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (target)
        {
            transform.position = target.transform.position + target.transform.rotation * initialPosition;
            transform.rotation = target.transform.rotation * initialRotation;
        }
        if (cinemachineTarget)
        {
            transform.position = cinemachineTarget.State.FinalPosition + cinemachineTarget.State.FinalOrientation * initialPosition;
            transform.rotation = cinemachineTarget.State.FinalOrientation * initialRotation;
        }
    }
}
