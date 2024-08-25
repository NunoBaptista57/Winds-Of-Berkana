using System.Collections;
using UnityEngine;

public class PushingState : MonoBehaviour, ILocomotionState
{
    private CharacterLocomotion _characterLocomotion;
    private GameObject _object;

    public void Break() {}

    public void Fall()
    {
        _characterLocomotion.ChangeState<FallingState>();
    }

    public void Ground() {}

    public void Move(Vector2 input)
    {
        
    }

    public void Run() {}

    public void Slide()
    {
        _characterLocomotion.ChangeState<SlidingState>();
    }

    public void StartJump()
    {
        _characterLocomotion.ChangeState<JumpingState>();
    }

    public void StartState()
    {

    }

    public void StopJump() {}

    public void Walk(bool walk) {}

    private void Awake()
    {
        _characterLocomotion = GetComponent<CharacterLocomotion>();
    }
}