using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerAnimation _playerAnimation;
    private CharacterLocomotion _playerLocomotion;

    public void SetCanMove(bool canMove)
    {
        _playerInput.CanMove = canMove;
    }

    private void Awake()
    {
        _playerInput = GetComponentInChildren<PlayerInput>();
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
        _playerLocomotion = GetComponentInChildren<CharacterLocomotion>();
    }
}