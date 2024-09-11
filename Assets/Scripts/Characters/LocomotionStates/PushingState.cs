using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PushingState : MonoBehaviour, ILocomotionState
{
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _angleToExit = 60f;
    private bool _hold = false;
    private CharacterLocomotion _characterLocomotion;
    private GameObject _object;

    public void Break() {}

    public void Fall()
    {
        RemoveObstacle();
        _characterLocomotion.ChangeState<FallingState>();
    }

    public void Ground() {}

    public void Interact(bool active)
    {
        _hold = active;
        Debug.Log(active);
    }

    public void Move(Vector2 input)
    {
        Vector3 bodyProjection = _characterLocomotion.Body.transform.forward.HorizontalProjection();
        Vector2 bodyDirection = new (bodyProjection.x, bodyProjection.z);
        Vector2 targetInput = _characterLocomotion.CalculateVector(input);

        if (targetInput != Vector2.zero && ((System.Math.Abs(Vector2.SignedAngle(targetInput, bodyDirection)) < _angleToExit && !_hold) || (System.Math.Abs(Vector2.SignedAngle(targetInput, bodyDirection)) < 90f && _hold)))
        {
            _characterLocomotion.ChangeInputVelocity(bodyDirection, _acceleration, _maxSpeed, _acceleration, true);
            Debug.Log("frente");
        }
        else if (_hold && targetInput != Vector2.zero && System.Math.Abs(Vector2.SignedAngle(targetInput, bodyDirection)) >= 90)
        {
            _characterLocomotion.ChangeInputVelocity(-bodyDirection, _acceleration, _maxSpeed, _acceleration, true);
            Debug.Log("tras");
        }
        else if (targetInput == Vector2.zero)
        {
            _characterLocomotion.ChangeInputVelocity(Vector2.zero, _acceleration, _maxSpeed, _acceleration, true);
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
        Vector3 bodyProjection = _characterLocomotion.Body.transform.forward.HorizontalProjection();
        Vector2 bodyDirection = new (bodyProjection.x, bodyProjection.z);
        _characterLocomotion.ChangeImediateInputVelocity(bodyDirection);
        obstacle.transform.SetParent(_characterLocomotion.Body);
        Debug.Log(bodyDirection);
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