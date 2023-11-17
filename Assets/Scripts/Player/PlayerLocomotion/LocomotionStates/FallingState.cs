using UnityEngine;

public class FallingState : MonoBehaviour, ILocomotionState
{
    [SerializeField] private float _gravity = 3f;
    [SerializeField] private float _maxFallSpeed = 10f;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _deceleration = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [HideInInspector] public bool CanStopJump = false;
    private CharacterLocomotion _characterLocomotion;

    public void StartState()
    {

    }

    public void StartJump()
    {
        _characterLocomotion.ChangeState<GlidingState>();
    }

    public void StopJump()
    {

    }

    public void Move()
    {
        _characterLocomotion.Rotate(_rotationSpeed);
        Vector3 newVelocity = _characterLocomotion.GetNewHorizontalVelocity(_acceleration, _maxSpeed, _deceleration);
        newVelocity.y = _characterLocomotion.GetNewVerticalSpeed(_gravity, _maxFallSpeed, _gravity);
        _characterLocomotion.CharacterController.Move(newVelocity * Time.deltaTime);
    }

    public void Run()
    {

    }

    public void Walk(bool walk)
    {

    }

    public void Fall()
    {
    }

    public void Ground()
    {
        _characterLocomotion.ChangeState<RunningState>();
    }

    private void Start()
    {
        _characterLocomotion = GetComponent<CharacterLocomotion>();
    }
}