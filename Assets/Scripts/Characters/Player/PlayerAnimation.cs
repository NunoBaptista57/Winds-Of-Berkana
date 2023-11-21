using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private CharacterController _characterController;


    public void ChangeAnimation(AnimationState animationState)
    {
        _animator.Play(animationState.ToString());
    }

    private void Update()
    {
        Vector2 horizontalSpeed = new(_characterController.velocity.x, _characterController.velocity.z);
        _animator.SetFloat("HorizontalSpeed", horizontalSpeed.magnitude);
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponentInParent<CharacterController>();
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