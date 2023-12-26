using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private GameObject _content;
    private readonly LevelManager _levelManager;

    public void Pause(InputAction.CallbackContext context)
    {
        if (_content.activeSelf)
        {
            Resume();
            return;
        }
        _content.SetActive(true);
        _levelManager.UpdateGameState(GameState.Paused);
    }
    public void Resume()
    {
        _levelManager.UpdateGameState(GameState.Play);
    }

    public void Load()
    {
        _levelManager.UpdateGameState(GameState.Load);
        _levelManager.UpdateGameState(GameState.Play);
    }

    public void Save()
    {
        _levelManager.UpdateGameState(GameState.Save);
        _levelManager.UpdateGameState(GameState.Paused);
    }

    public void Restart()
    {
        _levelManager.UpdateGameState(GameState.Remake);
    }

    private void Start()
    {
        _content = transform.GetChild(0).gameObject;
    }
}