using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.TextCore.Text;

public class PlayerManager : CharacterManager
{
    public bool CanMoveCamera = true;
    [SerializeField] private bool _canDebug = false;
    [SerializeField] private float _deadzone = 0.2f;
    [SerializeField] private float _walkDeadzone = 0.5f;
    [SerializeField] private Transform _cameraPosition;
    private PlayerActions _playerActions;
    private PlayerDebugMode _debugMode;

    public void DebugMode(InputAction.CallbackContext context)
    {
        if (_canDebug && context.started)
        {
            _debugMode.DebugMode = !_debugMode.DebugMode;
            CanMove = !_debugMode.DebugMode;
            CharacterController.enabled = !_debugMode.DebugMode;
            Move(Vector2.zero);
            CharacterLocomotion.gameObject.SetActive(!_debugMode.DebugMode);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_debugMode.DebugMode)
            {
                _debugMode.GoUp();
                return;
            }
            Jump(true);
        }
        else if (context.canceled)
        {
            if (_debugMode.DebugMode)
            {
                _debugMode.StopGoingUp();
                return;
            }
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
        else
        {
            input = input.normalized;
        }
        if (_debugMode.DebugMode)
        {
            _debugMode.Input = input;
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

    private void Start()
    {
        _debugMode = GetComponent<PlayerDebugMode>();
    }
}