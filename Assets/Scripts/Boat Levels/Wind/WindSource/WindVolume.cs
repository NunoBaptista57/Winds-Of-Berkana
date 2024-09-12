using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WindVolume : MonoBehaviour, IWindEffector
{
    [SerializeField] Quaternion WindDirection = Quaternion.identity;

    [SerializeField] float WindForce = 1;

    public Vector3 Force => WindDirection * Vector3.forward * WindForce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WindManager.OnPlayerEnter(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WindManager.OnPlayerLeave(this);
        }
    }
}
