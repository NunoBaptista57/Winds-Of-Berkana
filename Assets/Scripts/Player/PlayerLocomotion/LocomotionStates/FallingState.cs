using System;
using System.IO;
using UnityEngine;

public class FallingState : MonoBehaviour, ILocomotionState
{
    [SerializeField] private float _gravity = 3f;
    [SerializeField] private float _stoppingJumpForce = 0.5f;
    [SerializeField] private float _maxFallSpeed = 10f;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _deceleration = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [HideInInspector] public bool CanStopJump = false;
    private bool _glide = false;
    private bool _jump = false;
    private CharacterLocomotion _characterLocomotion;

    public void StartJump()
    {
        _glide = true;
    }

    public void StopJump()
    {
        _jump = true;
    }

    public Vector3 Move()
    {
        _characterLocomotion.Rotate(_rotationSpeed);
        Vector3 newVelocity = _characterLocomotion.GetNewHorizontalVelocity(_acceleration * 10, _maxSpeed, _deceleration * 10);
        newVelocity.y = _characterLocomotion.GetNewVerticalSpeed(_gravity, _maxFallSpeed, _gravity);

        if (CanStopJump && _jump)
        {
            _jump = false;
            newVelocity.y = _characterLocomotion.DecelerateJump(_stoppingJumpForce, newVelocity.y);
        }

        return newVelocity;
    }

    public void Run()
    {

    }

    public void Fall()
    {
    }

    public void Ground()
    {
        _characterLocomotion.ChangeState<RunningState>();
    }

    private void Start()
    {
        _characterLocomotion = GetComponent<CharacterLocomotion>();
    }
}