using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public bool CanMove = true;
    public bool CanMoveCamera = true;
    private PlayerInput _playerInput;
    private CharacterAnimation _playerAnimation;
    private CharacterLocomotion _playerLocomotion;
    [SerializeField] private float _deadzone = 0.2f;
    [SerializeField] private float _walkDeadzone = 0.5f;
    [SerializeField] private Transform _cameraPosition;

    public void Jump(InputAction.CallbackContext context)
    {
        if (!CanMove)
        {
            return;
        }
        if (context.started)
        {
            _playerLocomotion.StartJump();
        }
        else if (context.canceled)
        {
            _playerLocomotion.StopJump();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!CanMove)
        {
            return;
        }
        Vector2 input = context.ReadValue<Vector2>();
        if (input.magnitude < _deadzone)
        {
            input = Vector2.zero;
        }
        else if (input.magnitude <= _walkDeadzone)
        {
            input = input.normalized * 0.5f;
        }

        _playerLocomotion.Input = input;
    }

    public void Walk(InputAction.CallbackContext context)
    {
        if (!CanMove)
        {
            return;
        }
        if (context.started)
        {
            _playerLocomotion.Walk(true);
        }
        else if (context.canceled)
        {
            _playerLocomotion.Walk(false);
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (!CanMove)
        {
            return;
        }
        _playerLocomotion.Run();
    }

    private void Update()
    {
        if (!CanMoveCamera)
        {
            return;
        }
        _playerLocomotion.BaseRotation = _cameraPosition.rotation.eulerAngles.y;
    }

    private void Start()
    {
        _playerLocomotion = GetComponent<CharacterLocomotion>();
    }

    public void ChangeAnimation(CharacterAnimation.AnimationState animationState)
    {
        _playerAnimation.ChangeAnimation(animationState);
    }

    private void Awake()
    {
        _playerInput = GetComponentInChildren<PlayerInput>();
        _playerAnimation = GetComponentInChildren<CharacterAnimation>();
        _playerLocomotion = GetComponentInChildren<CharacterLocomotion>();
    }
}