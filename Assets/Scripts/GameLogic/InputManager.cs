using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class InputManager : MonoBehaviour
{
    private LevelManager _levelManager;
    private PlayerManager _playerManager;
    private PauseMenu _pauseMenu;
    private PlayerActions _playerActions;

    private void OnGameStateChanged(GameState gameState)
    {
    }

    private void Start()
    {
        _playerActions = new();

        _levelManager = ServiceLocator.instance.GetService<LevelManager>();
        _playerManager = ServiceLocator.instance.GetService<PlayerManager>();
        _pauseMenu = ServiceLocator.instance.GetService<PauseMenu>();

        _levelManager.OnGameStateChanged += OnGameStateChanged;

        _playerActions.Character.Jump.started += _playerManager.Jump;
        _playerActions.Character.Jump.canceled += _playerManager.Jump;
        _playerActions.Character.Move.performed += _playerManager.Move;
        _playerActions.Character.Run.performed += _playerManager.Run;

        _playerActions.UI.Pause.performed += _pauseMenu.Pause;
        _playerActions.Enable();
    }

    private void OnDestroy()
    {
        _levelManager.OnGameStateChanged -= OnGameStateChanged;

        _playerActions.Character.Jump.started -= _playerManager.Jump;
        _playerActions.Character.Jump.canceled -= _playerManager.Jump;
        _playerActions.Character.Move.performed -= _playerManager.Move;
        _playerActions.Character.Run.performed -= _playerManager.Run;

        _playerActions.UI.Pause.performed -= _pauseMenu.Pause;

        _playerActions.Disable();
    }
}