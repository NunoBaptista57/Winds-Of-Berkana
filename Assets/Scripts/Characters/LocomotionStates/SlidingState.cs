using System.Collections;
using UnityEngine;

public class SlidingState : MonoBehaviour, ILocomotionState
{
    [SerializeField] private float _gravity = 3f;
    [SerializeField] private float _maxFallSpeed = 10f;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _deceleration = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    private CharacterLocomotion _characterLocomotion;

    public void StartState()
    {
        _characterLocomotion.ChangeAnimationState(CharacterAnimation.AnimationState.running);
    }

    public void StartJump()
    {
    }

    public void StopJump()
    {
    }

    public void Move(Vector2 input)
    {
        _characterLocomotion.RotateBody(input, _rotationSpeed, true);
        _characterLocomotion.ChangeInputVelocity(input, _acceleration, _maxSpeed, _deceleration, false);
    }

    public void Run()
    {

    }

    public void Walk(bool walk)
    {
    }

    public void Fall()
    {
        _characterLocomotion.ChangeState<FallingState>();
    }

    public void Ground()
    {
        _characterLocomotion.ChangeState<RunningState>();
    }

    private void Awake()
    {
        _characterLocomotion = GetComponent<CharacterLocomotion>();
    }

    public void Break() {}

    public void Slide()
    {
        _characterLocomotion.ChangeFallVelocity(_gravity, _maxFallSpeed, _gravity);
    }

    public void Push(GameObject obstacle) {}

    public void StartState(GameObject obstacle) {}

    public void Interact(bool active) {}
}