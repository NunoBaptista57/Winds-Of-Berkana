using System;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [HideInInspector] public Animator Animator;

    public void ChangeAnimation(AnimationState animationState)
    {
        Animator.Play(animationState.ToString());
    }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public enum AnimationState
    {
        idle,
        walking,
        running,
        jumping,
        landing,
        falling
    }
}