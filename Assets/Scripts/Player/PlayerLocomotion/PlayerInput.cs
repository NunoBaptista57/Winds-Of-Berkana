using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerInput : MonoBehaviour
{
    public bool CanMove = true;
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
        _characterLocomotion.Input = context.ReadValue<Vector2>();
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