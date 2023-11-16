using System;
using UnityEngine;

public class RunningState : MonoBehaviour, ILocomotionState
{
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _deceleration = 5f;
    [SerializeField] private float _rotationSpeed = 10f;

    private CharacterLocomotion _characterLocomotion;

    public void StartJump()
    {

    }

    public void StopJump()
    {

    }

    public Vector3 Move()
    {
        _characterLocomotion.Rotate(_rotationSpeed);
        Vector3 newVelocity = _characterLocomotion.GetNewHorizontalVelocity(_acceleration, _maxSpeed, _deceleration);

        return newVelocity;
    }

    public void Run()
    {

    }

    public void SetCharacterLocomotion(CharacterLocomotion characterLocomotion)
    {
        _characterLocomotion = characterLocomotion;
    }
}