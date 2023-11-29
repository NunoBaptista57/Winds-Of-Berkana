using System;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class RunningState : MonoBehaviour, ILocomotionState
{
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _deceleration = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    private bool _walk = false;

    private CharacterLocomotion _characterLocomotion;

    public void StartJump()
    {
        _characterLocomotion.ChangeState<JumpingState>();
    }

    public void StopJump()
    {

    }

    public void Move()
    {
        float acceleration = _acceleration;
        float maxSpeed = _maxSpeed;
        float deceleration = _deceleration;
        float rotationSpeed = _rotationSpeed;

        if (_walk)
        {
            acceleration /= 2;
            maxSpeed /= 2;
            deceleration /= 2;
        }

        Vector3 newVelocity = _characterLocomotion.GetNewHorizontalVelocity(_acceleration, _maxSpeed, _deceleration);
        if (newVelocity.magnitude <= _maxSpeed / 2)
        {
            rotationSpeed *= 4;
        }

        Vector3 localVelocity = newVelocity - _characterLocomotion.BaseVelocity;
        float horizontalSpeed = localVelocity.magnitude;
        if (_characterLocomotion.Input == Vector2.zero)
        {
            horizontalSpeed = 0;
        }

        _characterLocomotion.PlayerAnimation.Animator.SetFloat("HorizontalSpeed", horizontalSpeed);

        _characterLocomotion.Rotate(rotationSpeed);
        newVelocity.y -= 0.1f; // So that the CharaterController detects the ground
        _characterLocomotion.NewVelocity += newVelocity * Time.deltaTime;
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

    public void Walk(bool walk)
    {
        _walk = walk;
    }

    public void StartState()
    {
        _characterLocomotion.PlayerAnimation.ChangeAnimation(PlayerAnimation.AnimationState.running);
    }

    private void Start()
    {
        _characterLocomotion = GetComponent<CharacterLocomotion>();
    }
}