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

    public void Move(Vector2 input)
    {
        if (input != Vector2.zero)
        {
            _characterLocomotion.accessAudioManager().RunningSound();
        }
        else
        {
            _characterLocomotion.accessAudioManager().StopSFX();
        }
        _characterLocomotion.Rotate(input, _rotationSpeed, canDo180: true);
        _characterLocomotion.ChangeInputVelocity(input, _acceleration, _maxSpeed, _deceleration);
    }

    public void Run()
    {

    }

    public void Fall()
    {
        _characterLocomotion.ChangeImediateFallVelocity(0f);
        _characterLocomotion.ChangeState<FallingState>();
    }

    public void Ground()
    {
    }

    public void Tunnel() { }

    public void Walk(bool walk)
    {
        _walk = walk;
    }

    public void StartState()
    {
        _characterLocomotion.ChangeAnimationState(CharacterAnimation.AnimationState.running);
        _characterLocomotion.ChangeImediateFallVelocity(10f);
    }

    private void Awake()
    {
        _characterLocomotion = GetComponent<CharacterLocomotion>();
    }

    public void Break()
    {
    }
    
    public void Slide() 
    {
        _characterLocomotion.ChangeState<SlidingState>();
        _characterLocomotion.ChangeImediateFallVelocity(0f);
    }
}