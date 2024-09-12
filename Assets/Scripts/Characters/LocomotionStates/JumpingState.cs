using UnityEngine;

public class JumpingState : MonoBehaviour, ILocomotionState
{
    [SerializeField] private float _gravity = 3f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _stoppingJumpForce = 0.5f;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _deceleration = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    private CharacterLocomotion _characterLocomotion;
    private bool _jump = false;
    private bool _stopJump = false;
    private bool _walk = false;

    public void StartJump()
    {

    }

    public void StopJump()
    {
        _stopJump = true;
    }

    public void Move(Vector2 input)
    {
        _characterLocomotion.RotateBody(input, _rotationSpeed, true);
        _characterLocomotion.ChangeInputVelocity(input, _acceleration, _maxSpeed, _deceleration, false);
        _characterLocomotion.ChangeFallVelocity(_gravity);

        if (_jump)
        {
            _characterLocomotion.AddJumpForce(_jumpForce);
            _jump = false;
        }
        else if (_stopJump)
        {
            _characterLocomotion.StopJumpForce(_stoppingJumpForce);
            _stopJump = false;
            _characterLocomotion.ChangeState<FallingState>();
        }
        else if (_characterLocomotion.FallVelocity.y <= 0f)
        {
            _characterLocomotion.ChangeState<FallingState>();
        }
    }

    public void Fall()
    {

    }

    public void Ground()
    {
    }

    public void Run()
    {

    }

    public void Walk(bool walk)
    {
        _walk = walk;
    }

    public void StartState()
    {
        _jump = true;
        // _characterLocomotion.PlayerAnimation.ChangeAnimation(PlayerAnimation.AnimationState.jumping);
    }

    public void Awake()
    {
        _characterLocomotion = GetComponent<CharacterLocomotion>();
    }

    public void Break()
    {
    }

    public void Slide()
    {
        _characterLocomotion.ChangeState<SlidingState>();
    }

    public void Push(GameObject obstacle) {}

    public void StartState(GameObject obstacle) {}

    public void Interact(bool active) {}
}