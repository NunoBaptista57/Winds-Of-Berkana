using System;
using UnityEngine;

public class LocomotionState : MonoBehaviour
{
    public float MaxSpeed = 10f;
    public float Acceleration = 1f;
    public float Deceleration = 1f;
    public float RotationSpeed = 30f;
    public CharacterLocomotion Locomotion;

    public virtual void StartJump()
    {
        Debug.Log("LocomotionState: StartJump not implemented.");
    }

    public virtual void StopJump()
    {
        Debug.Log("LocomotionState: StopJump not implemented.");
    }

    public virtual void Move(Vector2 direction)
    {
        Debug.Log("LocomotionState: Move not implemented.");
    }

    public virtual void Run()
    {
        Debug.Log("LocomotionState: Run not implemented.");
    }

    private void Start()
    {
        Locomotion = GetComponent<CharacterLocomotion>();
    }
}