using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PlayerLocomotion _playerLocomotion;

    public void SetCanMove(bool canMove)
    {
        _playerLocomotion.CanMove = canMove;
    }

    public void Jump(InputAction.CallbackContext context)
    {
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
        _playerLocomotion.Move(context.ReadValue<Vector2>());
    }

    public void Run(InputAction.CallbackContext context)
    {
        _playerLocomotion.Run();
    }

    private void Start()
    {
        _playerLocomotion = gameObject.GetComponentInChildren<PlayerLocomotion>();
    }
}