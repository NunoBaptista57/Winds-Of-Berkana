using UnityEngine;

public class WalkingState : MonoBehaviour, ILocomotionState
{
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _deceleration = 5f;
    [SerializeField] private float _rotationSpeed = 10f;

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
        _characterLocomotion.Rotate(_rotationSpeed);
        Vector3 newVelocity = _characterLocomotion.GetNewHorizontalVelocity(_acceleration, _maxSpeed, _deceleration);
        newVelocity.y -= 0.1f; // So that the CharaterController detects the ground
        _characterLocomotion.CharacterController.Move(newVelocity * Time.deltaTime);
    }

    public void Run()
    {

    }

    public void Walk(bool walk)
    {
        if (!walk)
        {
            _characterLocomotion.ChangeState<RunningState>();
        }
    }

    public void Fall()
    {
        GetComponent<FallingState>().CanStopJump = false;
        _characterLocomotion.ChangeState<FallingState>();
    }

    public void Ground()
    {

    }

    public void StartState()
    {

    }

    private void Start()
    {
        _characterLocomotion = GetComponent<CharacterLocomotion>();
    }
}