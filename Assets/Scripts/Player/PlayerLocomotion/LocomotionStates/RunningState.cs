using System;
using UnityEngine;

public class RunningState : LocomotionState
{
    public override void StartJump()
    {
        Debug.Log("LocomotionState: Current State is Parent Class.");
    }

    public override void StopJump()
    {
        Debug.Log("LocomotionState: Current State is Parent Class.");
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