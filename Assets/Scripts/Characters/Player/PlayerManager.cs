using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerManager : CharacterManager
{
    public bool CanMoveCamera = true;
    [SerializeField] private float _deadzone = 0.2f;
    [SerializeField] private float _walkDeadzone = 0.5f;
    [SerializeField] private Transform _cameraPosition;
    private PlayerActions _playerActions;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Jump(true);
        }
        else if (context.canceled)
        {
            Jump(false);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (input.magnitude < _deadzone)
        {
            input = Vector2.zero;
        }
        else if (input.magnitude <= _walkDeadzone)
        {
            input = input.normalized * 0.5f;
        }
        Move(input);
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Walk(true);
        }
        else if (context.canceled)
        {
            Walk(false);
        }
    }

    private void Update()
    {
        if (CanMoveCamera)
        {
            CharacterLocomotion.BaseRotation = MathF.Round(_cameraPosition.rotation.eulerAngles.y, 2);
        }
    }

    private void OnEnable()
    {
        _playerActions = new();

        _playerActions.Character.Jump.started += OnJump;
        _playerActions.Character.Jump.canceled += OnJump;
        _playerActions.Character.Move.performed += OnMove;

        _playerActions.Enable();
    }

    private void OnDestroy()
    {
        _playerActions.Character.Jump.started -= OnJump;
        _playerActions.Character.Jump.canceled -= OnJump;
        _playerActions.Character.Move.performed -= OnMove;

        _playerActions.Disable();
    }
}