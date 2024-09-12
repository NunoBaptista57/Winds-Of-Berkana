using UnityEngine;

public class GlidingState : MonoBehaviour, ILocomotionState
{
    [SerializeField] private float _gravity = 3f;
    [SerializeField] private float _maxFallSpeed = 3f;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _deceleration = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private GameObject _glider;
    private bool _walk = false;
    private CharacterLocomotion _characterLocomotion;

    public void StartState()
    {
        _characterLocomotion.ChangeAnimationState(CharacterAnimation.AnimationState.falling);
        _glider.SetActive(true);
        AudioManager audioManager = _characterLocomotion.accessAudioManager();
        if (audioManager != null)
        {
            _characterLocomotion.accessAudioManager().GlidingSound();
        }
    }

    public void StartJump()
    {
    }

    public void StopJump()
    {
        _glider.SetActive(false);
        _characterLocomotion.ChangeState<FallingState>();
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
        _walk = walk;
    }

    public void Fall()
    {
        _characterLocomotion.ChangeFallVelocity(_gravity, _maxFallSpeed, _gravity);
    }

    public void Ground()
    {
        AudioManager audioManager = _characterLocomotion.accessAudioManager();
        if (audioManager != null)
        {
            _characterLocomotion.accessAudioManager().LandingSound();
        }
        _characterLocomotion.ChangeState<RunningState>();
        _glider.SetActive(false);
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
        _glider.SetActive(false);
    }

    public void Push(GameObject obstacle) {}

    public void StartState(GameObject obstacle) {}

    public void Interact(bool active) {}
}