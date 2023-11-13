using System;
using UnityEngine;

public class FallingState : LocomotionState
{
    [SerializeField] private float _stopJumpForce = 2.5f;

    public override void StartJump()
    {
        Debug.Log("LocomotionState: Current State is Parent Class.");
    }

    public override void StopJump()
    {
        Locomotion.StopJump(_stopJumpForce);
    }

    public override void Move(Vector2 direction)
    {
        Locomotion.InputDirection = direction;
    }

    public override void Run()
    {
        Debug.Log("LocomotionState: Current State is Parent Class.");
    }
}