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

    public void Move()
    {
        _characterLocomotion.Rotate(_rotationSpeed);

        float acceleration = _acceleration;
        float maxSpeed = _maxSpeed;
        float deceleration = _deceleration;

        if (_walk)
        {
            acceleration /= 2;
            maxSpeed /= 2;
            deceleration /= 2;
        }

        Vector3 newVelocity = _characterLocomotion.GetNewHorizontalVelocity(acceleration, maxSpeed, deceleration);
        newVelocity.y = _characterLocomotion.GetNewVerticalSpeed(_gravity, _gravity, _gravity);

        if (_jump)
        {
            newVelocity.y += _jumpForce;
            _jump = false;
        }
        else if (_stopJump)
        {
            if (newVelocity.y >= _characterLocomotion.BaseVelocity.y)
            {
                newVelocity.y -= _stoppingJumpForce;
                if (newVelocity.y < _characterLocomotion.BaseVelocity.y)
                {
                    newVelocity.y = _characterLocomotion.BaseVelocity.y;
                }
            }
            _stopJump = false;
            _characterLocomotion.ChangeState<FallingState>();
        }
        else if (newVelocity.y <= _characterLocomotion.BaseVelocity.y)
        {
            _characterLocomotion.ChangeState<FallingState>();
        }

        _characterLocomotion.NewVelocity += newVelocity * Time.deltaTime;
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
}