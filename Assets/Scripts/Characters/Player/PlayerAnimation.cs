using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [HideInInspector] public Animator Animator;
    private CharacterController _characterController;


    public void ChangeAnimation(AnimationState animationState)
    {
        Animator.Play(animationState.ToString());
    }

    private void Awake()
    {
        _characterController = GetComponentInParent<CharacterController>();
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