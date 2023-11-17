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
        Vector3 newVelocity = _characterLocomotion.GetNewHorizontalVelocity(_acceleration, _maxSpeed, _deceleration);
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

        _characterLocomotion.CharacterController.Move(newVelocity * Time.deltaTime);
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

    public void StartState()
    {
        _jump = true;
    }

    public void Start()
    {
        _characterLocomotion = GetComponent<CharacterLocomotion>();
    }
}