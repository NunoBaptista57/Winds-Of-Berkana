using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public bool CanMove = true;
    protected CharacterLocomotion CharacterLocomotion;
    protected CharacterAnimation CharacterAnimation;

    public void Spawn(Transform transform)
    {
        transform.SetPositionAndRotation(transform.position, transform.rotation);
    }

    public void Move(Vector2 input)
    {
        if (CanMove)
        {
            CharacterLocomotion.Input = input;
        }
    }

    public void Run()
    {
        if (CanMove)
        {
            CharacterLocomotion.Run();
        }
    }

    public void Walk(bool walk)
    {
        if (CanMove)
        {
            CharacterLocomotion.Walk(walk);
        }
    }

    // The bool states wether the jump is starting or ending
    public void Jump(bool startedJump)
    {
        if (!CanMove)
        {
            return;
        }

        if (startedJump)
        {
            CharacterLocomotion.StartJump();
        }
        else
        {
            CharacterLocomotion.StopJump();
        }
    }

    public void ChangeAnimation(CharacterAnimation.AnimationState animationState)
    {
        CharacterAnimation.ChangeAnimation(animationState);
    }

    private void Awake()
    {
        CharacterAnimation = GetComponentInChildren<CharacterAnimation>();
        CharacterLocomotion = GetComponentInChildren<CharacterLocomotion>();
    }
}