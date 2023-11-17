using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerInput : MonoBehaviour
{
    public bool CanMove = true;
    [SerializeField] private float _deadzone = 0.2f;
    [SerializeField] private float _walkDeadzone = 0.5f;
    [SerializeField] private Transform _cameraPosition;
    private CharacterLocomotion _characterLocomotion;

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _characterLocomotion.StartJump();
        }
        else if (context.canceled)
        {
            _characterLocomotion.StopJump();
        }
    }

    public void Move(InputAction.CallbackContext context)
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

        _characterLocomotion.Input = input;
    }

    public void Walk(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _characterLocomotion.Walk(true);
        }
        else if (context.canceled)
        {
            _characterLocomotion.Walk(false);
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        _characterLocomotion.Run();
    }

    private void Update()
    {
        _characterLocomotion.BaseRotation = _cameraPosition.rotation.eulerAngles.y;
    }

    private void Start()
    {
        _characterLocomotion = GetComponent<CharacterLocomotion>();
    }
}