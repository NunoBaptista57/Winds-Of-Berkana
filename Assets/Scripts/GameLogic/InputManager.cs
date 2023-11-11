using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private LevelManager _levelManager;
    private PlayerManager _playerManager;

    private void OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Play:
                _playerManager.CanMove(true);
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
    }

    private void OnEnable()
    {
        _levelManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        _levelManager.OnGameStateChanged -= OnGameStateChanged;
    }
}