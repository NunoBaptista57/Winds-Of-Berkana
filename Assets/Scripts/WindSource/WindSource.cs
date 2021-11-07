using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WindSource : MonoBehaviour
{
    [SerializeField] Quaternion WindDirection = Quaternion.identity;

    [SerializeField] float WindForce = 1;

    [SerializeField] float WindTorque = 1;

    Rigidbody target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.attachedRigidbody;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
        }
    }

    void FixedUpdate()
    {
        if (target)
        {
            var forceDir = WindDirection * Vector3.forward;
            target.AddForce(WindForce * forceDir, ForceMode.Acceleration);
            var torqueAxis = Vector3.Cross(target.transform.forward, forceDir).normalized;
            var directionFactor = Vector3.Angle(target.transform.forward, forceDir) / 180;
            Debug.Log(directionFactor);
            target.AddTorque(WindTorque * directionFactor * torqueAxis, ForceMode.Acceleration);
        }
    }
}
