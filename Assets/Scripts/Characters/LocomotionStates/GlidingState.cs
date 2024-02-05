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
        _glider.SetActive(true);
    }

    public void StartJump()
    {
    }

    public void StopJump()
    {
        _glider.SetActive(false);
        _characterLocomotion.ChangeState<FallingState>();
    }

    public void Move()
    {
        _characterLocomotion.Rotate(_rotationSpeed * Time.deltaTime, true);

        float acceleration = _acceleration;
        float maxSpeed = _maxSpeed;
        float deceleration = _deceleration;

        if (_walk)
        {
            acceleration /= 2;
            maxSpeed /= 2;
            deceleration /= 2;
        }

        Vector3 newVelocity = _characterLocomotion.GetNewHorizontalVelocity(_acceleration, _maxSpeed, _deceleration);
        newVelocity.y = _characterLocomotion.GetNewVerticalSpeed(_gravity, _maxFallSpeed, _gravity * 2);
        _characterLocomotion.NewVelocity += newVelocity * Time.deltaTime;
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
    }

    public void Ground()
    {
        _characterLocomotion.ChangeState<RunningState>();
        _glider.SetActive(false);
    }

    public void Tunnel()
    {
        _characterLocomotion.ChangeState<WindTunnel>();
    }

    private void Awake()
    {
        _characterLocomotion = GetComponent<CharacterLocomotion>();
    }

    public void Break()
    {
    }
}