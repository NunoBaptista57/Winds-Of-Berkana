using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PushingState : MonoBehaviour, ILocomotionState
{
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxSpeed;
    private CharacterLocomotion _characterLocomotion;
    private GameObject _object;

    public void Break() {}

    public void Fall()
    {
        RemoveObstacle();
        _characterLocomotion.ChangeState<FallingState>();
    }

    public void Ground() {}

    // TODO: change input method
    public void Move(Vector2 input)
    {
        Vector3 bodyProjection = _characterLocomotion.Body.transform.position.HorizontalProjection();
        Vector2 bodyDirection = new (bodyProjection.x, bodyProjection.z);
        if (input != Vector2.zero && System.Math.Abs(Vector2.SignedAngle(input, bodyDirection)) < 45f)
        {
            _characterLocomotion.ChangeInputVelocity(bodyDirection, _acceleration, _maxSpeed, _acceleration);
        }
        else if (input == Vector2.zero)
        {
            _characterLocomotion.ChangeInputVelocity(Vector2.zero, _acceleration, _maxSpeed, _acceleration);
        }
        else
        {
            RemoveObstacle();
            _characterLocomotion.ChangeState<RunningState>();
        }
    }

    public void Push(GameObject obstacle) {}

    public void Run() {}

    public void Slide()
    {
        RemoveObstacle();
        _characterLocomotion.ChangeState<SlidingState>();
    }

    public void StartJump()
    {
        RemoveObstacle();
        _characterLocomotion.ChangeState<JumpingState>();
    }

    public void StartState() {}

    public void StartState(GameObject obstacle)
    {
        _object = obstacle;
        obstacle.transform.SetParent(_characterLocomotion.Body);
        _characterLocomotion.ChangeImediateInputVelocity(Vector3.zero);
    }

    public void StopJump() {}

    public void Walk(bool walk) {}

    private void Awake()
    {
        _characterLocomotion = GetComponent<CharacterLocomotion>();
    }

    private void RemoveObstacle()
    {
        _object.transform.SetParent(null);
        _object = null;
    }
}