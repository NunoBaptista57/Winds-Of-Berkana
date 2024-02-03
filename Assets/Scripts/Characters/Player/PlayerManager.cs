using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerManager : CharacterManager
{
    public bool CanMoveCamera = true;
    [SerializeField] private bool _canDebug = false;
    [SerializeField] private float _deadzone = 0.2f;
    [SerializeField] private float _walkDeadzone = 0.5f;
    [SerializeField] private Transform _cameraPosition;
    private PlayerActions _playerActions;
    private bool _debugMode = false;

    public void DebugMode(InputAction.CallbackContext context)
    {
        if (_canDebug && context.started)
        {
            _debugMode = !_debugMode;
            Debug.Log("Debug mode: " + _debugMode);
            if (_debugMode)
            {
                CharacterController.enabled = false;
                CharacterLocomotion.ChangeState<DebugState>();

            }
            else
            {
                CharacterController.enabled = false;
                CharacterLocomotion.ChangeState<RunningState>();
            }
        }
    }

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

    private void OnEnable()
    {
        _playerActions = new();

        _playerActions.Character.Jump.started += OnJump;
        _playerActions.Character.Jump.canceled += OnJump;
        _playerActions.Character.Move.performed += OnMove;
        _playerActions.Character.Debug.started += DebugMode;

        _playerActions.Enable();
    }

    private void OnDestroy()
    {
        _playerActions.Character.Jump.started -= OnJump;
        _playerActions.Character.Jump.canceled -= OnJump;
        _playerActions.Character.Move.performed -= OnMove;
        _playerActions.Character.Debug.started -= DebugMode;

        _playerActions.Disable();
    }
}