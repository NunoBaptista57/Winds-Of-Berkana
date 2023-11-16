using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PlayerInput _playerInput;

    public void SetCanMove(bool canMove)
    {
        _playerInput.CanMove = canMove;
    }

    private void Start()
    {
        _playerInput = gameObject.GetComponentInChildren<PlayerInput>();
    }
}