using UnityEngine;

public interface ILocomotionState
{
    public void StartJump();
    public void StopJump();
    public Vector3 Move();
    public void Run();
    public void Fall();
    public void Ground();
}