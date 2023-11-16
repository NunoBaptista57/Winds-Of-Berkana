using System;
using UnityEngine;

public class RunningState : MonoBehaviour, ILocomotionState
{
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _deceleration = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    private bool _jump = false;

    private CharacterLocomotion _characterLocomotion;

    public void StartJump()
    {
        _jump = true;
    }

    public void StopJump()
    {

    }

    public Vector3 Move()
    {
        _characterLocomotion.Rotate(_rotationSpeed);
        Vector3 newVelocity = _characterLocomotion.GetNewHorizontalVelocity(_acceleration * 10, _maxSpeed, _deceleration * 10);
        newVelocity.y -= 0.5f; // So that the CharaterController detects the ground
        if (_jump)
        {
            newVelocity.y += _jumpForce;
            _jump = false;
            GetComponent<FallingState>().CanStopJump = true;
            _characterLocomotion.ChangeState<FallingState>();
        }

        return newVelocity;
    }

    public void Run()
    {

    }

    public void Fall()
    {
        GetComponent<FallingState>().CanStopJump = false;
        _characterLocomotion.ChangeState<FallingState>();
    }

    public void Ground()
    {

    }

    private void Start()
    {
        _characterLocomotion = GetComponent<CharacterLocomotion>();
    }
}