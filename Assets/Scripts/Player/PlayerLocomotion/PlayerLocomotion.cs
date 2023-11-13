using System;
using UnityEngine;

public class PlayerLocomotion : CharacterLocomotion
{
    public bool CanMove = true;
    [SerializeField] private Transform _cameraPosition;

    public void OnStartJump()
    {
        State.StartJump();
    }

    public void OnStopJump()
    {
        State.StopJump();
    }

    public void OnMove(Vector2 direction)
    {
        if (CanMove)
        {
            State.Move(direction);
        }
    }

    public void OnRun()
    {
        State.Run();
    }
    private void Update()
    {
        BaseAngle = new(_cameraPosition.position.x - transform.position.x, _cameraPosition.position.z - transform.position.z);
        BaseAngle.Normalize();
    }
}