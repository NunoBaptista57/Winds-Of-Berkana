using System;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private LocomotionState _locomotionState;

    public void ChangeState(LocomotionState locomotionState)
    {
        _locomotionState = locomotionState;
    }

    private void Start()
    {
        _rigidbody = gameObject.GetComponent<RigidBody>();
    }
}