using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private LevelManager _levelManager;
    private PlayerManager _playerManager;
    private PauseMenu _pauseMenu;
    private readonly PlayerActions _playerActions = new();

    private void OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Play:
                _playerManager.SetCanMove(true);
                break;

            case GameState.Paused:
                _playerManager.SetCanMove(false);
                break;
        }
    }

    private void Start()
    {
        _levelManager = ServiceLocator.instance.GetService<LevelManager>();
        _playerManager = ServiceLocator.instance.GetService<PlayerManager>();
        _pauseMenu = ServiceLocator.instance.GetService<PauseMenu>();
    }

    private void OnEnable()
    {
        _levelManager.OnGameStateChanged += OnGameStateChanged;

        _playerActions.Character.Jump.started += _playerManager.Jump;
        _playerActions.Character.Jump.canceled += _playerManager.Jump;
        _playerActions.Character.Move.performed += _playerManager.Move;
        _playerActions.Character.Run.performed += _playerManager.Run;

        _playerActions.UI.Pause.performed += PauseMenu.Pause;
    }

    private void OnDisable()
    {
        _levelManager.OnGameStateChanged -= OnGameStateChanged;

        _playerActions.Character.Jump.started -= _playerManager.Jump;
        _playerActions.Character.Jump.canceled -= _playerManager.Jump;
        _playerActions.Character.Move.performed -= _playerManager.Move;
        _playerActions.Character.Run.performed -= _playerManager.Run;

        _playerActions.UI.Pause.performed -= PauseMenu.Pause;
    }
}