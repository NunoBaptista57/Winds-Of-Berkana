using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class InputManager : MonoBehaviour
{
    private PlayerManager _playerManager;
    private PauseMenu _pauseMenu;
    private PlayerActions _playerActions;

    private void Awake()
    {
        _playerActions = new();

        _playerManager = ServiceLocator.Instance.GetService<PlayerManager>();
        _pauseMenu = ServiceLocator.Instance.GetService<PauseMenu>();

        _playerActions.Character.Jump.started += _playerManager.Jump;
        _playerActions.Character.Jump.canceled += _playerManager.Jump;
        _playerActions.Character.Move.performed += _playerManager.Move;
        _playerActions.Character.Run.performed += _playerManager.Run;

        _playerActions.UI.Pause.performed += _pauseMenu.Pause;
        _playerActions.Enable();
    }

    private void OnDestroy()
    {
        _playerActions.Character.Jump.started -= _playerManager.Jump;
        _playerActions.Character.Jump.canceled -= _playerManager.Jump;
        _playerActions.Character.Move.performed -= _playerManager.Move;
        _playerActions.Character.Run.performed -= _playerManager.Run;

        _playerActions.UI.Pause.performed -= _pauseMenu.Pause;

        _playerActions.Disable();
    }
}