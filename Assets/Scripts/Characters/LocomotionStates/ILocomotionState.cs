using UnityEngine;

public interface ILocomotionState
{
    public void StartJump();
    public void StopJump();
    public void Move(Vector2 input);
    public void Run();
    public void Walk(bool walk);
    public void Fall();
    public void Slide();
    public void Ground();
    public void StartState();
    public void StartState(GameObject obstacle);
    public void Break();
    public void Push(GameObject obstacle);
    public void Interact(bool active);
}