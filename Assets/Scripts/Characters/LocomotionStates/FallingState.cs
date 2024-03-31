using System.Collections;
using UnityEngine;

public class FallingState : MonoBehaviour, ILocomotionState
{
    [SerializeField] private float _gravity = 3f;
    [SerializeField] private float _maxFallSpeed = 10f;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _deceleration = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _animationDelay = 0.1f;
    private bool _walk = false;
    private CharacterLocomotion _characterLocomotion;
    private bool _startAnimation = true;

    public void StartState()
    {
        _startAnimation = true;
        StartCoroutine(StartFall());
    }

    private IEnumerator StartFall()
    {
        float startTime = Time.time;
        while (Time.time - startTime < _animationDelay)
        {
            yield return new WaitForEndOfFrame();
        }
        if (_startAnimation)
        {
            _characterLocomotion.ChangeAnimationState(CharacterAnimation.AnimationState.falling);
        }
    }

    public void StartJump()
    {
        _characterLocomotion.ChangeState<GlidingState>();
    }

    public void StopJump()
    {

    }

    public void Move(Vector2 input)
    {
        _characterLocomotion.Rotate(input, _rotationSpeed, true);
        _characterLocomotion.ChangeInputVelocity(input, _acceleration, _maxSpeed, _deceleration);
    }

    public void Run()
    {

    }

    public void Walk(bool walk)
    {
        _walk = walk;
    }

    public void Fall()
    {
        _characterLocomotion.ChangeGravity(_gravity, _maxFallSpeed, _gravity);
    }

    public void Ground()
    {
        _startAnimation = false;
        _characterLocomotion.ChangeState<RunningState>();
    }

    public void Tunnel() { }

    private void Awake()
    {
        _characterLocomotion = GetComponent<CharacterLocomotion>();
    }

    public void Break()
    {
    }
}