using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windTunnelBoat : MonoBehaviour
{
    [Header("Wind Tunnel Settings")]
    [SerializeField] public float windForce;
    [SerializeField] public Vector3 windVector;

    void Awake()
    {
        if (windVector == Vector3.zero)
        {
            windVector = transform.forward;
        }
    }
}
