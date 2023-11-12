using System;
using UnityEngine;

public class LocomotionState : MonoBehaviour
{
    public float _maxVelocity = 10f;
    public float _acceleration = 1f;
    public float _deceleration = 1f;
    public float _rotationSpeed = 0.5f;

    public void StartJump()
    {
        Debug.Log("LocomotionState: Current State is Parent Class.");
    }

    public void StopJump()
    {
        Debug.Log("LocomotionState: Current State is Parent Class.");
    }

    public void Move(Vector2 direction)
    {
        Debug.Log("LocomotionState: Current State is Parent Class.");
    }

    public void Run()
    {
        Debug.Log("LocomotionState: Current State is Parent Class.");
    }
}