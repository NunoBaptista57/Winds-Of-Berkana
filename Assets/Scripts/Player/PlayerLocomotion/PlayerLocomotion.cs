using System;
using UnityEngine;

public class PlayerLocomotion : CharacterLocomotion
{
    public bool CanMove = true;
    [SerializeField] private Transform _cameraPosition;

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
    private void Update()
    {
        BaseAngle = new(_cameraPosition.position.x - transform.position.x, _cameraPosition.position.z - transform.position.z);
        BaseAngle.Normalize();
    }
}