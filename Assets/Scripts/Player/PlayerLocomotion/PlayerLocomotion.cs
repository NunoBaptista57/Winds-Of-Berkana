using System;
using UnityEngine;

public class PlayerLocomotion : CharacterLocomotion
{
    public bool CanMove = true;
    public Rigidbody _rigidbody;

    public void ChangeState(LocomotionState locomotionState)
    {
        State = locomotionState;
    }

    public void StartJump()
    {
        State.StartJump();
    }

    public void StopJump()
    {
        State.StopJump();
    }

    public void Move(Vector2 direction)
    {
        State.Move(direction);
    }

    public void Run()
    {
        State.Run();
    }

    private void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }
}