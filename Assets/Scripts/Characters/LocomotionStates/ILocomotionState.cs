public interface ILocomotionState
{
    public void StartJump();
    public void StopJump();
    public void Move();
    public void Run();
    public void Walk(bool walk);
    public void Fall();
    public void Ground();
    public void StartState();
}